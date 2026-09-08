/*
Declare
   @PnIdUsuario              Integer          = 3,
   @PsClaveUsuario           Varchar ( 50)    = 'AlbertoM',
   @PsPassword               Varchar (Max)    = '*************',
   @PnIdTipoUsuario          Integer          = 0,
   @PsPrimerApellido         Varchar (100)    = 'Mendez',
   @PsSegundoApellido        Varchar (100)    = 'Pelado',
   @PsNombres                Varchar (100)    = 'Alberto Jose',
   @PsCorreo                 Varchar (260)    = 'AlbertoM@empresa.com',
   @PnIdOperacion            Integer          = 4,
   @PnIdUsuarioAct           Integer          = 1,
   @PsIpAct                  Varchar ( 30),
   @PsMacAddressAct          Varchar ( 30),
   @PnEstatus                Integer        = 0,
   @PsMensaje                Varchar (250)  = Null;

Begin

   Set @PsPassword = dbo.fn_EncryptBase64 (@PsPassword );
   
   Execute dbo.Spa_segUsuariosTbl @PnIdUsuario       = @PnIdUsuario,
                                  @PsClaveUsuario    = @PsClaveUsuario,
                                  @PsPassword        = @PsPassword,
                                  @PnIdTipoUsuario   = @PnIdTipoUsuario,
                                  @PsPrimerApellido  = @PsPrimerApellido,
                                  @PsSegundoApellido = @PsSegundoApellido,
                                  @PsNombres         = @PsNombres,
                                  @PsCorreo          = @PsCorreo,
                                  @PnIdOperacion     = @PnIdOperacion,
                                  @PnIdUsuarioAct    = @PnIdUsuarioAct,
                                  @PsIpAct           = @PsIpAct,
                                  @PsMacAddressAct   = @PsMacAddressAct,
                                  @PnEstatus         = @PnEstatus Output,
                                  @PsMensaje         = @PsMensaje Output;

    Select @PnEstatus, @PsMensaje;
    Return;
End;
Go

*/
Create Or Alter Procedure dbo.Spa_segUsuariosTbl
  (@PnIdUsuario              Integer,
   @PsClaveUsuario           Varchar ( 50),
   @PsPassword               Varchar (Max),
   @PnIdTipoUsuario          Integer,
   @PsPrimerApellido         Varchar (100),
   @PsSegundoApellido        Varchar (100),
   @PsNombres                Varchar (160),
   @PsCorreo                 Varchar (260),
   @PnIdOperacion            Integer,
   @PnIdUsuarioAct           Integer,
   @PsIpAct                  Varchar ( 30)  = Null,
   @PsMacAddressAct          Varchar ( 30)  = Null,
   @PnEstatus                Integer        = 0    Output,
   @PsMensaje                Varchar (250)  = Null Output)
As

Declare
   @w_desc_error              Varchar( 250),
   @w_Error                   Integer,
   @w_fechaAct                Datetime,
   @w_idEstatus               Bit;

Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2024-06-12
  Descripción:    Procedimiento que da de alta los Registros a la tabla segUsuariosTbl.
  Creacion:       02-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32),
          @PsIpAct           = Isnull(@PsIpAct,         dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct   = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMAC());


   Select @PnEstatus = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct)
   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Not Exists (Select Top 1 1
                  From   dbo.catGeneralesTbl
                  Where  tabla   = 'segUsuariosTbl'
                  And    columna = 'idTipoUsuario'
                  And    valor   = @PnIdTipoUsuario)
      Begin
         Select @PnEstatus = 2050,
                @PsMensaje   = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If dbo.Fn_ValidaCorreo(@PsCorreo) = 0
      Begin
         Select @PnEstatus = 3010,
                @PsMensaje   = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Exists (Select Top 1 1
              From   dbo.segUsuariosTbl
              Where  claveUsuario = @PsClaveUsuario
              Or     idUsuario    = @PnIdUsuario)
      Begin
         Select @PnEstatus = 10009,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @PnIdOperacion
                  And    idAutorizacion >= 2)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If @PsPassword Is Null
      Begin
         Select @PnEstatus = 10007,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--
-- Validacion de Password
--

   Set @PnEstatus = dbo.Fn_ValidaReglasContrasena(@PnIdUsuario, @PsPassword, 0);
   If @PnEstatus > 0
      Begin
         Set @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End  

--
-- Alta de Usuario en la Aplicación
--

   Begin Transaction
      Begin Try
         Insert Into dbo.segUsuariosTbl
         (idUsuario,       claveUsuario,    idTipoUsuario, primerApellido,
          segundoApellido, nombres,         correo, idUsuarioAct,
          ipAct,           macAddressAct)
         Select @PnIdUsuario,       @PsClaveUsuario,  @PnIdTipoUsuario, @PsPrimerApellido,
                @PsSegundoApellido, @PsNombres,       @PsCorreo,        @PnIdUsuarioAct,
                @PsIpAct,           @PsMacAddressAct
   
      End Try
   
      Begin Catch
         Select  @w_Error      = @@Error,
                 @w_desc_error = Substring (Error_Message(), 1, 230)
      End   Catch
   
      If Isnull(@w_Error, 0) <> 0
         Begin
            Select @PnEstatus = @w_Error,
                   @PsMensaje = Concat('Error.: ', @w_Error, ' ',  @w_desc_error )

            Rollback Transaction
            Set Xact_Abort Off
            Return
         End
      
--
-- Generación de Usuario en Base de Datos.
--

      Execute dbo.Spp_GeneraUserBD @PsIdUsuarioBD  = @PsClaveUsuario,
                                   @PsPasswordBD   = @PsPassword,
                                   @PsBaseDatos    = Null,
                                   @PnIdOperacion  = @PnIdOperacion,
                                   @PnIdUsuarioAct = @PnIdUsuarioAct,
                                   @PnEstatus      = @PnEstatus  Output,
                                   @PsMensaje      = @PsMensaje  Output;
   
   
      If @PnEstatus != 0
         Begin
            Rollback TRansaction
            Set Xact_Abort Off
            Return
         End

--
-- Alta En el Historico de Contraseñas
--
  
      Execute dbo.Spa_histUserPassTbl @PnIdUsuario       = @PnIdUsuario,
                                      @PsContrasenia     = @PsPassword,
                                      @PnEstatus         = @PnEstatus Output,
                                      @PsMensaje         = @PsMensaje Output;

      If @PnEstatus != 0
         Begin
            Rollback TRansaction
            Set Xact_Abort Off
            Return
         End

   Commit Transaction
   Set Xact_Abort Off
   Return

End
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que da alta de Registros a la tabla segUsuariosTbl.',
   @w_procedimiento  Varchar( 100) = 'Spa_segUsuariosTbl'


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
