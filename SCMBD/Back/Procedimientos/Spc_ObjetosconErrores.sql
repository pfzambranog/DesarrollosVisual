/*
Declare
   @PsOperacion             Varchar ( 20)   = 'SU1012',
   @PnIdUsuarioAct          Integer         = 3,
   @PnEstatus               Integer         = 0,
   @PsMensaje               NVarchar( Max)  = Null;

Begin
   Execute dbo.Spc_ObjetosconErrores @PsOperacion       = @PsOperacion,
                                     @PnIdUsuarioAct    = @PnIdUsuarioAct,
                                     @PnEstatus         = @PnEstatus      Output,
                                     @PsMensaje         = @PsMensaje      Output;

   Select @PnEstatus As IdError, @PsMensaje As Mensaje;
   Return;
End;
Go
*/

Create or Alter Procedure dbo.Spc_ObjetosconErrores
  (@PsOperacion              Varchar ( 20),
   @PnIdUsuarioAct           Integer,
   @PnEstatus                Integer        = 0    Output,
   @PsMensaje                NVarchar (Max) = Null Output)
With Execute AS Owner
As

Declare
   @w_desc_error             Varchar  (250),
   @w_Error                  Integer,
   @w_idOperacionAct         Integer,
   @w_registros              Integer,
   @w_secuencia              Integer,
   @w_sec                    Integer,
   @w_linea                  Integer,
   @w_sql                    NVarchar (Max),
   @w_objSchema              Sysname,
   @w_objName                Sysname,
   @w_objType                Char(2),
   @w_comilla                Char(1);

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que genera el Reporte de Objetos de la Base de datos que presentan:
                    - Errores de compilación
                    - Referencias a objetos eliminados/renombrados
                    - Dependencias no resueltas
  Creacion:       18-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32),
          @w_comilla         = Char(39),
          @w_sec             = 0,
          @w_secuencia       = 0,
          @w_linea           = 0;

   Select @PnEstatus = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct)
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

--
-- Creación de Tablas Temporales.
--

   Create Table #TempObjetos
  (secuencia    Integer         Not Null Identity(1, 1) Primary Key,
   esquema      Nvarchar(256)   Not NUll,
   nombreobjeto Sysname         Not Null,
   tipo         Char(2)         Not Null);


   Create Table #ErroresCompilacion
  (secuencia       Integer         Not Null Identity(1, 1) Primary Key,
   esquema         Sysname         Not Null,
   nombreobjeto    Sysname         Not Null,
   tipoObjeto      Varchar ( 30)   Not Null,
   MensajeError    NVarchar(MAX)       Null);

--
-- Inicio de Proceso
--

   Begin Try
      Insert Into #TempObjetos
      (esquema, nombreobjeto, tipo)
      Select Schema_name(o.schema_id), o.name, o.type
      From   sys.all_objects o
      Where  o.type          In ('P','FN','IF','TF','V')
      And    o.is_ms_shipped  = 0
      And    o.name          != 'Spc_ObjetosconErrores'
      Order By 3, 1, 2;

      Set @w_secuencia = Scope_identity();
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_linea      = Error_line(),
              @w_desc_error = Substring (Error_Message(), 1, 230);
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = Concat('Error En Linea ', @w_linea, '.: ', @w_Error, ' ',    @w_desc_error )

         Set Xact_Abort Off
         Return
      End

   While @w_sec < @w_secuencia
   Begin
      Set @w_sec = @w_sec + 1;

      Select @w_objSchema = esquema,
             @w_objName   = nombreobjeto,
             @w_objType   = tipo
      From   #TempObjetos
      Where  secuencia = @w_sec;
      If @@Rowcount = 0
         Begin
            Break;
         End;

      Begin Try
          Set @w_sql = Concat(N'Execute sp_refreshsqlmodule ', @w_comilla, QUOTENAME(@w_objSchema), '.', QUOTENAME(@w_objName) + @w_comilla, ';')
          Execute sp_executesql @w_sql;
      End Try

      Begin Catch
         Select  @w_Error      = @@Error,
                 @w_linea      = Error_line(),
                 @w_desc_error = Substring (Error_Message(), 1, 250);
      End   Catch

      If Isnull(@w_desc_error, '') <> ''
         Begin
            Insert Into #ErroresCompilacion
            (esquema, nombreobjeto, MensajeError, tipoObjeto)
            Select @w_objSchema, @w_objName, @w_desc_error,
                   Case @w_objType When 'P'
                                   Then 'Procedimiento'
                                   When 'FN'
                                   Then 'Función Escalar'
                                   When 'IF'
                                   Then 'Función Tabular'
                                   When 'TF'
                                   Then 'Función Tabular'
                                   When 'V'
                                   Then 'Vista'
                                   Else @w_objType
                   End;
         End;

   End;


   Set @PsMensaje = (Select secuencia, esquema,  nombreobjeto, tipoObjeto, MensajeError
                     From   #ErroresCompilacion
                     Order  By 1
                     For    Json Path);

   Set @w_registros = @@Rowcount
   
   If @w_registros = 0
      Begin
         Select @PnEstatus = 0,
                @PsMensaje = 'No Existen Objetos Problemas de compilación';
      End
   Else
      Begin
         Set @PnEstatus = 1;
      End

   Set Xact_Abort Off
   Return

End
Go

Grant Execute On dbo.Spc_ObjetosconErrores to Public
Go


--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que genera el Reporte de Objetos de la Base de datos que presentan problemas de compilación',
   @w_procedimiento  Varchar( 100) = 'Spc_ObjetosconErrores',
   @w_tipo           Varchar(  20) = 'Procedure';

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
                                      @level0name = N'Dbo',
                                      @level1type = @w_tipo,
                                      @level1name = @w_procedimiento;

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'Dbo',
                                        @level1type = @w_tipo,
                                        @level1name = @w_procedimiento
   End
Go
