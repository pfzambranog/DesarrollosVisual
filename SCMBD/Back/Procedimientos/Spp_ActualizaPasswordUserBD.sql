/*
Declare
    @PsIdUsuarioBD           Sysname        = 'AlbertoM',
    @PsPasswordActual        Sysname        = 'Periodo2025!',
    @PsPasswordNueva         Sysname        = 'QQBsAGIAZQByADIAMAAyADYAIQA=',
    @PsBaseDatos             Sysname        = Null,
    @PnIdOperacion           Integer        = 5,
    @PnIdUsuarioAct          Integer        = 1,
    @PnEstatus               Integer        = 0,
    @PsMensaje               Varchar( 250)  = Null;

Begin
   Set @PsPasswordNueva = dbo.fn_EncryptBase64 (@PsPasswordNueva);
   Execute dbo.Spp_ActualizaPasswordUserBD @PsIdUsuarioBD     = @PsIdUsuarioBD,
                                           @PsPasswordActual  = @PsPasswordActual,
                                           @PsPasswordNueva   = @PsPasswordNueva,
                                           @PsBaseDatos       = @PsBaseDatos,
                                           @PnIdOperacion     = @PnIdOperacion,
                                           @PnIdUsuarioAct    = @PnIdUsuarioAct,
                                           @PnEstatus         = @PnEstatus      Output,
                                           @PsMensaje         = @PsMensaje      Output;

   Select @PnEstatus As IdError, @PsMensaje As Mensaje;
   Return;
End;
Go
*/


Create Or Alter Procedure dbo.Spp_ActualizaPasswordUserBD
  (@PsIdUsuarioBD           Sysname,
   @PsPasswordActual        Sysname,
   @PsPasswordNueva         Sysname,
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
    @w_sql                     NVarchar(1500),
    @w_comilla                 Char(1),
    @w_password_actual         Varchar(Max),
    @w_password_nueva          Varchar(Max),
    @w_idUsuario               Integer,
    @w_login_existe            Bit,
    @w_contrasena_valida       Bit;
Begin
-- ==========================================================================================================
-- Autor:          Pedro Zambrano
-- Fecha:          2026-09-03
-- Descripción:    Procedimiento que actualiza la contraseña de un usuario, validando la contraseña actual.
-- Version:        1.0
-- ==========================================================================================================

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus   = 0,
          @PsMensaje   = Char(32),
          @w_comilla   = Char(39),
          @PsBaseDatos = Isnull(@PsBaseDatos, db_name()),
          @w_idUsuario = dbo.Fn_BuscaIdUsuario (@PsIdUsuarioBD);

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
-- Validación de Nueva Contraseña.
--


   Set @PnEstatus = dbo.Fn_ValidaReglasContrasena(@w_idUsuario, @PsPasswordNueva, 1);
   If @PnEstatus > 0
      Begin
         Set @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End 
   
   -- =============================================
   -- DESENCRIPTAR CONTRASEÑAS
   -- =============================================

   Begin Try
      Select @w_password_actual = dbo.fn_DesEncrypta64(Cast(@PsPasswordActual As Varchar(Max))),
             @w_password_nueva  = dbo.fn_DesEncrypta64(Cast(@PsPasswordNueva  As Varchar(Max)));
   End Try
   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring(Error_Message(), 1, 230)
   End Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error;
         Set Xact_Abort Off
         Return
      End

   -- ==================================================
   -- VERIFICAR QUE EL LOGIN EXISTA EN EL SERVIDOR
   -- ==================================================

   If Not Exists (Select Top 1 1
                  From   master.sys.sql_logins
                  Where  type = 'S'
                  And    name = @PsIdUsuarioBD)
      Begin
         Select @PnEstatus = 5001,
                @PsMensaje = 'Error.: El usuario ' + @PsIdUsuarioBD + ' no existe en el servidor.';
         Set Xact_Abort Off
         Return
      End

   -- =========================================================
   -- VALIDAR QUE LA CONTRASEÑA ACTUAL SEA CORRECTA
   -- =========================================================


   Begin Try
      Set @w_sql = 'Declare @w_existe Bit ' +
                   'Select @w_existe = 1 ' +
                   'From   master.sys.sql_logins '           +
                   'Where  name = '     + @w_comilla + @PsIdUsuarioBD     + @w_comilla + ' '   +
                   'And    Pwdcompare(' + @w_comilla + @w_password_actual + @w_comilla + ', password_hash) = 1 ';

      Execute Sp_ExecuteSQL @w_sql;
      If @@Rowcount = 0
         Begin
            Select @PnEstatus = 5002,
                   @PsMensaje = 'Error.: La contraseña actual no es correcta.';
            Set Xact_Abort Off
            Return
         End

   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring(Error_Message(), 1, 230);

      Select @PnEstatus = Isnull(@w_Error, 9999),
             @PsMensaje = 'Error validando contraseña: ' + Rtrim(Ltrim(@w_desc_error));

      Set Xact_Abort Off
      Return
   End Catch

   -- =============================================
   -- ACTUALIZAR CONTRASEÑA
   -- =============================================

      Set @w_sql = 'Alter Login '     + Quotename(@PsIdUsuarioBD) + ' ' +
                   'With Password = ' + @w_comilla + @w_password_nueva + @w_comilla + ' ' +
                   'Unlock ';
   Begin Try
      Execute (@w_sql);
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring(Error_Message(), 1, 230);
   End Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error;
         Set Xact_Abort Off
         Return
      End


--
-- Alta En el Historico de Contraseñas
--
  
   Execute dbo.Spa_histUserPassTbl @PnIdUsuario       = @w_idUsuario,
                                   @PsContrasenia     = @PsPasswordNueva,
                                   @PnEstatus         = @PnEstatus Output,
                                   @PsMensaje         = @PsMensaje Output;

   If @PnEstatus != 0
      Begin
         Set Xact_Abort Off
         Return
      End

   Select @PnEstatus = 0,
          @PsMensaje = 'Ok.: Contraseña actualizada satisfactoriamente.';

   Set Xact_Abort Off
   Return
   
End
Go

-- =============================================
-- COMENTARIO / PROPIEDAD EXTENDIDA
-- =============================================
Declare
   @w_valor          Varchar(1500) = 'Procedimiento que actualiza la contraseña de un usuario validando primero la contraseña actual.',
   @w_procedimiento  Varchar( 100) = 'Spp_ActualizaPasswordUserBD'

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