/*

Declare
   @PsInput                  Varchar (Max)  = 'ClaveEncriptada',
   @PsOutputEncrip           NVarchar(Max)  = Null,
   @PnEstatus                Integer        = 0,
   @PsMensaje                Varchar (Max)  = Null;

Begin
   Execute dbo.Sp_EncryptaCadena    @PsInput         = @PsInput,
                                    @PsOutputEncrip  = @PsOutputEncrip Output,
                                    @PnEstatus       = @PnEstatus     Output,
                                    @PsMensaje       = @PsMensaje     Output;

   If @PnEstatus != 0
      Begin
         Select @PnEstatus IdError, @PsMensaje MensajeError
      End
   Else
      Begin
         Select @PsOutputEncrip Password_Cifrado;
     End

   Return

End;
Go
*/

Create Or Alter Procedure dbo.Sp_EncryptaCadena
  (@PsInput                  NVarchar(Max),
   @PsOutputEncrip           Varchar(Max)  = Null Output,
   @PnEstatus                Integer        = 0    Output,
   @PsMensaje                Varchar (Max)  = Null Output)
As
Declare
   @w_desc_error             Varchar( 250),
   @w_Error                  Integer,
   @w_ClaveAbierta           Bit,
   @w_PassBin                Varbinary(Max);

Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2026-09-24
  Descripción:    Cifra cadena y devuelve en Cadena Encriptada.
  Notas:          Abre y cierra la clave internamente
  Versión:        1.0
*/

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @w_ClaveAbierta    = 0,
          @PsMensaje         = Char(32),
          @PsOutputEncrip    = Char(32);
          

   Begin Try
      If Exists (Select top 1 1
                 From   sys.openkeys
                 Where  key_name = 'ClaveAES_API_SCMBD')
         Begin
            Set @w_ClaveAbierta = 1;
         End;
      Else
         Begin
            Open Symmetric Key ClaveAES_API_SCMBD
            Decryption By Certificate Cert_AES_SCMBD;
         End;
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = Concat('Error.: ', @w_Error, '-', @w_desc_error);

         Set Xact_Abort Off
         Return
      End

--
-- Inicio de Proceso.
--

   Set @w_PassBin = Encryptbykey(Key_guid('ClaveAES_API_SCMBD'), @PsInput);


   Set @PsOutputEncrip = Cast(N'' AS XML).value(
                        'xs:base64Binary(sql:variable("@w_PassBin"))',
                        'Varchar(Max)');
  
  If @w_ClaveAbierta = 0
     Begin
        Begin Try
           Close Symmetric Key ClaveAES_API_SCMBD;
       End Try

       Begin Catch
          Select  @w_Error      = @@Error,
                  @w_desc_error = Substring (Error_Message(), 1, 230)
       End   Catch

       If Isnull(@w_Error, 0) <> 0
          Begin
             Select @PnEstatus = @w_Error,
                    @PsMensaje = Concat('Error en Cierre.: ', @w_Error, '-', @w_desc_error);

             Set Xact_Abort Off
             Return
          End

     End

   Set Xact_Abort Off
   Return;

End;
Go

