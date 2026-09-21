/*
Declare
   @PsObjeto        Sysname      = 'catCaracterEspecTbl',
   @PsOperacion     Varchar( 20) = 'REPOBJBD02',
   @PnIdUsuarioAct  Integer      = 3,
   @PnEstatus       Integer      = 0,
   @PsMensaje       Varchar(250) = ' ';
Begin
   Execute dbo.Spc_Lista_Det_Tablas @PsObjeto        = @PsObjeto,
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

Create Or Alter Procedure dbo.Spc_Lista_Det_Tablas
  (@PsObjeto        Sysname      = Null,
   @PsOperacion     Varchar( 20),
   @PnIdUsuarioAct  Integer,
   @PnEstatus       Integer      = 0   Output,
   @PsMensaje       Varchar(250) = ' ' Output)
With Execute AS Owner
As

Declare
   @w_Error             Integer,
   @w_idOperacionAct    Integer,
   @w_desc_error        Varchar( 250),
   @w_tabla             Sysname,
   @w_columna           Sysname,
   @w_idTabla           Integer,
   @w_column_id         Integer,
   @w_column_idx        Integer,
   @w_column_Fk         Integer,
   @w_TipoCampo         Varchar(250),
   @w_Longitud          Integer,
   @w_Decimales         Integer,
   @w_requerido         Char(2),
   @w_aplica            Bit,
   @w_descripcion       NVarchar(1500);

Declare
  C_tablas Cursor For
  Select   Name, id
  from     Sysobjects
  Where    Uid     = 1
  And      Type    = 'U'
  And      Name    = Case When @PsObjeto Is Null
                          Then Name
                          Else @PsObjeto
                     End
  Order    By 1

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que Consulta la definición de Tablas en la base de datos.
  Creacion:       19-sep-2026.
  Version:        1.0
*/

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus         = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje         = Char(32),
          @w_descripcion     = Char(32);

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

   Create table #Tmp_Objectos
   (Orden       SmallInt     Not Null,
    BaseDatos   Sysname          Null Default ' ',
    Objeto      Sysname          Null Default ' ',
    Tabla       Sysname          Null Default ' ',
    idcolumna   Integer          Null,
    Columna     Sysname          Null Default ' ',
    Tipo        Varchar(250)     Null Default ' ',
    Longitud    Varchar(20)      Null Default ' ',
    Decimales   Varchar(20)      Null Default ' ',
    Requerido   Char(2)          Null Default ' ',
    Llave_Prim  Varchar(5)       Null Default ' ',
    Llave_For   Varchar(5)       Null Default ' ',
    Descripcion NVarchar(1500))

   Insert Into #Tmp_Objectos
   (Orden, BaseDatos, Descripcion)
   Select 0, db_Name(), (Select Cast(Value As NVarchar(1550))
      From   fn_listextendedproperty('MS_Description', Null, Null, Null, Null,
              Null, Null))

   Open  C_tablas
   While @@Fetch_status < 1
   Begin
      Fetch C_tablas Into @w_tabla, @w_idTabla
      If @@Fetch_status <> 0
         Begin
            Break
         End

      Set @w_descripcion = ''
      Select @w_descripcion = Cast(Value As NVarchar(1550))
      From   fn_listextendedproperty('MS_Description', 'SCHEMA', 'dbo', 'table', @w_tabla,
              Null, Null)

      Insert Into #Tmp_Objectos
      (Orden, Objeto, tabla, Descripcion)
      Values (1, @w_tabla, @w_tabla, @w_descripcion)

      Declare
         C_columnas Cursor For
           Select Name, Column_id
           From   sys.columns
           Where  Object_id = @w_idTabla
           Order  By Column_id

      Begin
         Open   C_columnas
         While  @@Fetch_Status < 1
         Begin
            Fetch C_columnas Into @w_columna, @w_column_id
            If @@Fetch_status <> 0
               Begin
                  Break
               End

            Select @w_column_idx = b.key_ordinal
            From   sys.indexes i
            Join   sys.index_columns  b
            On     i.object_id                         = b.object_id
            And    i.index_id                          = b.index_id
            And    Col_name(b.object_id, b.column_id)  = @w_columna
            And    b.column_id                         = @w_column_id
            Where  i.object_id                         = @w_idTabla
            And    i.is_primary_key                    = 1
            If @@Rowcount = 0
               Begin
                  Set  @w_column_idx  = 0
               End

            Select @w_column_fk = 1
            From   sys.tables a
            Join   sys.sysforeignkeys b
            On     b.fkeyid    = a.object_id
            Join   sys.columns c
            On     c.object_id = b.rkeyid
            And    c.Name      = @w_columna
            Where  a.object_id = @w_idTabla
            If @@Rowcount = 0
               Begin
                  Set @w_column_fk = 0
               End

            Select @w_column_idx = Isnull(@w_column_idx, 0),
                   @w_column_fk  = Isnull(@w_column_fk,  0)

            Set @w_descripcion = ''
            Select @w_descripcion = Cast(Value As NVarchar(1550))
            From   fn_listextendedproperty('MS_Description', 'Schema', 'dbo', 'table', @w_tabla,
                    'Column', @w_columna)

            Execute Spc_valida_longitud @w_Tabla,     @w_Columna,
                                        @w_Aplica     Output,     @w_TipoCampo Output, @w_Longitud  Output,
                                        @w_Decimales  Output,     @w_requerido Output, @PnEstatus   Output,
                                        @PsMensaje    Output

            If @PnEstatus = 0
               Begin
                  Begin Try
                     Insert Into #Tmp_Objectos
                    (Orden,  Objeto, idcolumna, Columna,  Tipo, Longitud, Decimales,
                     Requerido, Llave_Prim, Llave_For, Descripcion)
                     Select  2, @w_tabla, @w_column_id, @w_columna, @w_TipoCampo,
                            Cast(@w_longitud As Varchar(20)),
                            Case When @w_decimales = 0
                                 Then ' '
                                 Else Cast(@w_decimales As Varchar)
                            End, @w_requerido,
                            Case When @w_column_idx = 0
                                 Then ' '
                                 Else Cast(@w_column_idx As Varchar)
                            End,
                            Case When @w_column_fk  = 0
                                 Then ' '
                                 Else 'Si'
                            End, @w_descripcion
                  End Try

                  Begin Catch
                     Select  @w_Error      = @@Error,
                             @w_desc_error = Substring (Error_Message(), 1, 230)
                  End   Catch

                  If Isnull(@w_Error, 0) <> 0
                     Begin
                        Select @PnEstatus = @w_Error,
                               @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error
                        Select @PsMensaje

                        Set Xact_Abort Off
                        Return
                     End
               End
            Else
               Begin
                  Select @PnEstatus, @PsMensaje
               End
         End
         Close      C_columnas
         Deallocate C_columnas
      End

   End
   Close      C_tablas
   Deallocate C_tablas

   Select BaseDatos "Base de Datos", Tabla, Columna, Upper(tipo) "Tipo de Dato", longitud + Case When Decimales != ' '
                                                           Then ', ' + Decimales
                                                           Else ' '
                                                       End Longitud,
          Requerido, Llave_Prim "llaveprimaria", Descripcion "Descripción"
   From   #Tmp_Objectos
   Order By Objeto, idcolumna


End
Go

Grant Execute On Spc_Lista_Det_Tablas to Public
Go

-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Procedimiento que Consulta las Tablas declaradas en la base de datos.',
   @w_procedimiento  NVarchar(250) = 'Spc_Lista_Det_Tablas';

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
