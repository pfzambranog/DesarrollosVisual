/*

Declare
   @PsDescripcion        Varchar(  100)   = 'Incidencias BD',
   @PsTitulo             Varchar(  100)   = 'Incidencias BD',
   @PsCuerpo             NVarchar( Max),
   @PsHtml               NVarchar( Max),
   @PsUrl                Varchar(  Max)   = Null,
   @PsPerfilCorreo       Sysname          = 'Incidencias',
   @PbPermiteCompartir   Bit              = 0,
   @PsOperacion          Varchar(   20)   = 'SU1012',
   @PnIdUsuarioAct       Integer          = 3,
   @PsIpAct              Varchar(   30)   = Null,
   @PsMacAddressAct      Varchar(   30)   = Null,
   @PnEstatus            Integer          = 0,
   @PsMensaje            Varchar(  250)   = Null;
Begin

   Select  @PsCuerpo = '&saludo&, &Nombre& Por medio del presente se te informa que el proceso de &incidencia&, EN EL SERVIDOR &servidor& ',
           @PsHtml   = '<!DOCTYPE html>      <html lang="es">          <tbody>                  <p>&saludo&, &Nombre&                  <p style="margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;color:#153643">                        Por medio del presente se te informa que el proceso de &incidencia&, EN EL SERVIDOR &servidor&             </tbody>      </html>'

   Execute dbo.spa_conMotivosCorreoTbl @PsDescripcion      = @PsDescripcion,
                                       @PsTitulo           = @PsTitulo,
                                       @PsCuerpo           = @PsCuerpo,
                                       @PsHtml             = @PsHtml,
                                       @PsUrl              = @PsUrl,
                                       @PsPerfilCorreo     = @PsPerfilCorreo,
                                       @PbPermiteCompartir = @PbPermiteCompartir,
                                       @PsOperacion        = @PsOperacion,
                                       @PnIdUsuarioAct     = @PnIdUsuarioAct,
                                       @PsIpAct            = @PsIpAct,
                                       @PsMacAddressAct    = @PsMacAddressAct,
                                       @PnEstatus          = @PnEstatus         Output,
                                       @PsMensaje          = @PsMensaje         Output;

   Select @PnEstatus As IdError, @PsMensaje As "Mensaje de Error";

   Return;

End;
Go
*/

Create Or Alter Procedure dbo.spa_conMotivosCorreoTbl
  (@PsDescripcion        Varchar(  100),
   @PsTitulo             Varchar(  100),
   @PsCuerpo             NVarchar( Max),
   @PsHtml               NVarchar( Max),
   @PsUrl                Varchar(  Max)   = Null,
   @PsPerfilCorreo       Sysname,
   @PbPermiteCompartir   Bit              = 0,
   @PsOperacion          Varchar(   20),
   @PnIdUsuarioAct       Integer,
   @PsIpAct              Varchar(   30)   = Null,
   @PsMacAddressAct      Varchar(   30)   = Null,
   @PnEstatus            Integer          = 0    Output,
   @PsMensaje            Varchar(  250)   = ' '  Output)
As


Declare
   @w_desc_error          Varchar( 250),
   @w_Error               Integer,
   @w_idMotivo            Integer,
   @w_idOperacionAct      Integer;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que da de Alta Registros en la tabla conMotivosCorreoTbl.
  Creacion:       21-sep-2026.
  Version:        1.0
*/

   Set Quoted_identifier Off
   Set Nocount            On
   Set Xact_Abort         On
   Set Ansi_Nulls         On

   Select @PnEstatus         = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje         = Char(32),
          @PsIpAct           = Isnull(@PsIpAct,         dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct   = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMac());

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

   If Not Exists (Select Top 1 1
                  From   msdb.dbo.sysmail_account
                  Where  name = @PsPerfilCorreo)
      Begin
         Select @PnEstatus = 2040,
                @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus)

         Set Xact_Abort Off
         Return
      End

--

   If Exists ( Select Top 1 1
               From   dbo.conMotivosCorreoTbl
               Where  descripcion = @PsDescripcion)
      Begin
         Select @PnEstatus = 2051,
                @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus)

         Set Xact_Abort Off
         Return
      End

--

   Select @w_idMotivo = Max(idMotivo)
   From   conMotivosCorreoTbl
   Set @w_idMotivo = Isnull(@w_idMotivo, 0) + 1

   Begin Try
      Insert Into  dbo.conMotivosCorreoTbl
      (idMotivo,     descripcion,  titulo,       cuerpo,
       html,         URL,          perfilCorreo, permiteCompartir,
       idUsuarioAct, ipAct,        macAddressAct)
      Select @w_idMotivo,      @PsDescripcion,   @PsTitulo,       @PsCuerpo,
             @PsHtml,          @PsUrl,           @PsPerfilCorreo, @PbPerMiteCompartir,
             @PnIdUsuarioAct,  @PsIpAct,         @PsMacAddressAct

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

   Set @PsMensaje = Cast(@w_idMotivo As Varchar)

   Set Xact_Abort Off
   Return

End
Go

Grant Execute On dbo.Spa_conMotivosCorreoTbl to Public
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que da de Alta Registros en la tabla conMotivosCorreoTbl.',
   @w_procedimiento  Varchar( 100) = 'Spa_conMotivosCorreoTbl',
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

