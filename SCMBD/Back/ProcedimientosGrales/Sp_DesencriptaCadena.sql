/*

Declare
   @PsInputBase64            Varchar (Max)  = 'ADNu60fMgUCmJAf5JsqrmwIAAABFOErpPICuyrli6SLn+4lGE18Ma/itPymDRcecrKA0glkBarfr7gv4ZLwpHe1phKbFSYsPhWuZbSYX7xktjRQK',
   @PsOutputClaro            NVarchar(Max)  = Null,
   @PnEstatus                Integer        = 0,
   @PsMensaje                Varchar (Max)  = Null;

Begin
   Execute dbo.Sp_DesencriptaCadena @PsInputBase64 = @PsInputBase64,
                                    @PsOutputClaro = @PsOutputClaro Output,
                                    @PnEstatus     = @PnEstatus     Output,
                                    @PsMensaje     = @PsMensaje     Output;

   If @PnEstatus != 0
      Begin
         Select @PnEstatus IdError, @PsMensaje MensajeError
      End
   Else
      Begin
         Select @PsOutputClaro Password_Cifrado;
     End

   Return

End;
Go
*/

Create Or Alter Procedure dbo.Sp_DesencriptaCadena
  (@PsInputBase64            Varchar (Max),
   @PsOutputClaro            NVarchar(Max)  = Null Output,
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
  Fecha:          2024-00-24
  Descripción:    Convierte cadena string a cadena encriptada mediante "Clave Maestra de Base de Datos".
  Versión:        1.1 — Corregido: Base64 + tipo retorno
*/

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @w_ClaveAbierta    = 0,
          @PsMensaje         = Char(32),
          @PsOutputClaro     = Char(32);

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

   Set @w_PassBin = Cast(N'' AS XML).value(
                    'xs:base64Binary(sql:variable("@PsInputBase64"))',
                    'Varbinary(Max)');

  -- Descifrar

  Set @PsOutputClaro = Cast(Decryptbykey(@w_PassBin) AS NVarchar(Max));

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

