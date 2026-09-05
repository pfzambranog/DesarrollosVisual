/*

Declare
   @PsIdUsuarioBD           Sysname        = 'Usuario1',
   @PsPasswordBD            Sysname        = 'SGVsbG8gQmFzZTY0',
   @PsBaseDatos             Sysname        = Null,
   @PnIdOperacion           Integer        = 4,
   @PnIdUsuarioAct          Integer        = 1,
   @PnEstatus               Integer        = 0,
   @PsMensaje               Varchar( 250)  = Null;

Begin
   Execute dbo.Spp_GeneraUserBD @PsIdUsuarioBD  = @PsIdUsuarioBD,
                                @PsPasswordBD   = @PsPasswordBD,
                                @PsBaseDatos    = @PsBaseDatos,
                                @PnIdOperacion  = @PnIdOperacion,
                                @PnIdUsuarioAct = @PnIdUsuarioAct,
                                @PnEstatus      = @PnEstatus  Output,
                                @PsMensaje      = @PsMensaje  Output;

   Select @PnEstatus As IdError, @PsMensaje As Error;
   Return;

End;
Go

*/

Create Or Alter Procedure dbo.Spp_GeneraUserBD
  (@PsIdUsuarioBD           Sysname,
   @PsPasswordBD            Sysname,
   @PsBaseDatos             Sysname,
   @PnIdOperacion           Integer,
   @PnIdUsuarioAct          Integer,
   @PnEstatus               Integer        = 0     Output,
   @PsMensaje               Varchar( 250)  = Null  Output)
With Execute AS Owner
As

Declare
    @w_desc_error              Varchar( 250),
    @w_Error                   Integer,
    @w_IdEstatus               Tinyint,
    @w_sql                     NVarchar(1500),
    @w_param                   NVarchar( 750),
    @w_comilla                 Char(1),
    @w_registros               Smallint,
    @w_password                Varchar(Max);

Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2024-06-12
  Descripción:    Procedimiento que da de alta a usuarios en la Base de Datos.
  Creacion:       03-sep-2026.
  Version:        1.0
*/

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On
   Set Ansi_Warnings On
   Set Ansi_Padding  On

   Select @PnEstatus   = 0,
          @PsMensaje   = Char(32),
          @w_comilla   = Char(39),
          @PsBaseDatos = Isnull(@PsBaseDatos, db_name());

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @PnIdOperacion
                  And    idAutorizacion  = 4)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--
--
--

   Begin Try
      Select @w_password = dbo.fn_DesEncrypta64 (Cast(@PsPasswordBD As Varchar(Max)));
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error

         Set Xact_Abort Off
         Return
      End

--
-- Inicio de Proceso.
--

   Select @w_idEstatus = is_disabled
   From   master.sys.sql_logins
   Where  type = 'S'
   And    name = @PsIdUsuarioBD
   If @@Rowcount = 0
      Begin
         Set @w_sql = 'Create Login ' + @PsIdUsuarioBD + ' With  Password = ' + @w_comilla + @w_password + @w_comilla + ', ' +
                      'Default_database = ' + @PsBaseDatos                                                               + ', ' +
                      'Default_Language = ' + @@LANGUAGE                                                                 + ', ' +
                      'Check_Expiration = Off '                                                                          + ', ' +
                      'Check_Policy     = Off'
         Begin Try
            Execute (@w_sql)
         End   Try

         Begin Catch
            Select  @w_Error      = @@Error,
                    @w_desc_error = Substring (Error_Message(), 1, 230)
         End   Catch

         If Isnull(@w_Error, 0) <> 0
            Begin
               Select @PnEstatus = @w_Error,
                      @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error

               Set Xact_Abort Off
               Return
            End
      End

   Select @w_sql   = 'Select @o_idEstatus = Count(1) '                           +
                     'From   ' + @PsBaseDatos + '.sys.sysusers '                 +
                     'Where  name = ' + @w_comilla + @PsIdUsuarioBD + @w_comilla,
          @w_param = '@o_idEstatus  Tinyint Output'

   Begin Try
      Execute Sp_executeSQL @w_sql, @w_param, @o_idEstatus = @w_IdEstatus Output
   End   Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error
         Set Xact_Abort Off
         Return
      End


   If Isnull(@w_IdEstatus, 0) = 0
      Begin
         Set @w_sql = 'Use '         + @PsBaseDatos   + '; ' + Char(13) +
                      'Create User ' + @PsIdUsuarioBD + ' For Login ' + @PsIdUsuarioBD + ' ' +
                      'With Default_schema = ' + @PsIdUsuarioBD
         Begin Try
            Execute (@w_sql)
         End   Try

         Begin Catch
            Select  @w_Error      = @@Error,
                    @w_desc_error = Substring (Error_Message(), 1, 230)
         End   Catch

         If Isnull(@w_Error, 0) <> 0
            Begin
               Select @PnEstatus = @w_Error,
                      @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error
               Set Xact_Abort Off
               Return
            End
      End

   Set @w_sql =  'Use '         + @PsBaseDatos   + '; ' + Char(13) +
                'Execute sp_addrolemember ' + @w_comilla + 'db_datareader'  + @w_comilla + ', '              +
                                              @w_comilla + @PsIdUsuarioBD   + @w_comilla + '; ' + Char(13)   +
                'Execute sp_addrolemember ' + @w_comilla + 'db_datawriter'  + @w_comilla + ', '              +
                                              @w_comilla + @PsIdUsuarioBD   + @w_comilla +'; '  + Char(13)   +
                'Execute sp_addrolemember ' + @w_comilla + 'db_accessadmin' + @w_comilla + ', '              +
                                              @w_comilla + @PsIdUsuarioBD   + @w_comilla +';'
   Begin Try
      Execute (@w_sql)
   End   Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error
         Set Xact_Abort Off
         Return
      End

   Set Xact_Abort Off
   Return

End
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que da alta Genera Usarios en la Base de Datos.',
   @w_procedimiento  Varchar( 100) = 'Spp_GeneraUserBD'


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
                                      @level1type = 'Procedure',
                                      @level1name = @w_procedimiento;

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'Dbo',
                                        @level1type = 'Procedure',
                                        @level1name = @w_procedimiento
   End
Go
