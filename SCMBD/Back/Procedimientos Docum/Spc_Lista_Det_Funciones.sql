Use SCMBD
Go

/*
Declare
   @PsObjeto        Sysname      = Null,
   @PsOperacion     Varchar( 20) = 'REPOBJBD02',
   @PnIdUsuarioAct  Integer      = 3,
   @PnEstatus       Integer      = 0,
   @PsMensaje       Varchar(250) = ' ';
Begin
   Execute dbo.Spc_Lista_Det_Funciones @PsObjeto       = @PsObjeto,
                                       @PsOperacion    = @PsOperacion,
                                       @PnIdUsuarioAct = @PnIdUsuarioAct,
                                       @PnEstatus      = @PnEstatus Output,
                                       @PsMensaje      = @PsMensaje Output;
   If @PnEstatus != 0
      Begin
         Select @PnEstatus, @PsMensaje;
      End

   Return

End
Go
*/

Create Or Alter Procedure dbo.Spc_Lista_Det_Funciones
  (@PsObjeto        Sysname      = Null,
   @PsOperacion     Varchar( 20),
   @PnIdUsuarioAct  Integer,
   @PnEstatus       Integer      = 0    Output,
   @PsMensaje       Varchar(250) = ' '  Output)
With Execute AS Owner
As
Declare
   @w_Error             Integer,
   @w_idOperacionAct    Integer,
   @w_desc_error        Varchar( 250)
Begin
 /*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que Consulta la definición de los Parámetros de las Funciones de la base de datos.
  Creacion:       21-sep-2026.
  Version:        1.1 — Nombre función una sola vez
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   -- ? Validaciones iguales
   Select @PnEstatus         = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje         = Char(32);
   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);
         Set Xact_Abort Off
         Return
      End

   Select top 1 @w_idOperacionAct = idOperacion
   From   dbo.catOperacionesTbl
   Where  operacion = @PsOperacion;

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @w_idOperacionAct
                  And    idAutorizacion >= 2)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);
         Set Xact_Abort Off
         Return
      End

   -- ? Tabla temporal para controlar repetición
   Create Table #TempFunciones(
    object_id              Integer    Not Null Identity(1, 1) Primary Key,
    Esquema                Varchar(128),
    NombreFuncion          Varchar(150),
    TipoFuncion            Varchar(10),
    DescripcionTipo        Varchar(60),
    IdParametro            Integer,
    NombreParametro        Varchar(100),
    TipoDato               Varchar(30),
    Longitud               Varchar(50),
    TipoParametroFn        Varchar(10));

   Begin Try
      Insert into #TempFunciones
      Select SCHEMA_NAME(so.SCHEMA_ID),
             Trim(so.name),
             so.type,
             so.Type_Desc,
             p.parameter_id,
             p.name,
             Upper(TYPE_NAME(p.user_type_id)),
             Case When p.system_type_id Not In(34, 35, 58, 61, 189)
                  Then Case When p.max_length > 0 And p.precision = 0
                            Then Cast(p.max_length As Varchar)
                            When p.max_length < 0 And p.precision = 0
                            Then 'Max'
                            When p.precision  > 0 And p.scale = 0
                            Then Cast(p.precision As Varchar)
                            When p.precision  > 0 And p.scale > 0
                            Then Concat(p.precision, ', ', p.scale)
                       End
                  Else Cast(p.max_length As Varchar)
             End,
             Case When p.is_output = 1 Then 'OUT' Else 'IN' End
      From   sys.objects    so
      Join   sys.parameters p
      On     so.OBJECT_ID = p.OBJECT_ID
      Where  so.TYPE IN ('FN','TF','IF')
      And    so.name Like IIF(@PsObjeto Is Null, so.name, '%' + @PsObjeto + '%')
      Order By 1, 2, p.parameter_id;
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring(Error_Message(), 1, 230)
   End Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error
         Set Xact_Abort Off
         Return
      End


      -- ? Borrar nombre en filas siguientes a la primera de cada función

   Update t
   Set    NombreFuncion = Char(32)
   From   #TempFunciones t
   Where  IdParametro > 0;

      -- ? Resultado final
   Select Esquema,
          NombreFuncion,
          TipoFuncion,
          DescripcionTipo,
          IdParametro,
          NombreParametro,
          TipoDato,
          Longitud,
          TipoParametroFn
   From   #TempFunciones
   Order By object_id;

   Set Xact_Abort Off
   Return
End
Go

Grant Execute on Spc_Lista_Det_Funciones to Public;
Go

-- Comentarios
Declare
   @w_valor          Nvarchar(250) = 'Procedimiento que Consulta los parámetros de las Funciones declarados en la base de datos.',
   @w_procedimiento  NVarchar(250) = 'Spc_Lista_Det_Funciones';
   
If Not Exists (Select Top 1 1
               From   sys.extended_properties a
               Join   sysobjects  b
               On     b.xtype   = 'P'
               And    b.name    = @w_procedimiento
               And    b.id      = a.major_id)
   Begin
      Execute  sp_addextendedproperty @name       = N'MS_Description',
                                      @value      = @w_valor,
                                      @level0type = 'Schema',
                                      @level0name = N'dbo',
                                      @level1type = 'Procedure',
                                      @level1name = @w_procedimiento
   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'dbo',
                                        @level1type = 'Procedure',
                                        @level1name = @w_procedimiento
   End
Go