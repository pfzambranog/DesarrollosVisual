/*

Declare
   @PnIdMotivo           Integer          = 1,
   @PsDescripcion        Varchar(  100)   = 'Incidencias BD',
   @PsTitulo             Varchar(  100)   = 'Incidencias BD',
   @PsCuerpo             NVarchar( Max),
   @PsHtml               NVarchar( Max),
   @PsUrl                Varchar(  Max)   = Null,
   @PsPerfilCorreo       Sysname          = 'Incidencias',
   @PbPermiteCompartir   Bit              = 0,
   @PbIdEstatus          Bit              = Null,
   @PsOperacion          Varchar(   20)   = 'SU1012',
   @PnIdUsuarioAct       Smallint         = 3,
   @PsIpAct              Varchar(   30)   = Null,
   @PsMacAddressAct      Varchar(   30)   = Null,
   @PnEstatus            Integer          = 0,
   @PsMensaje            Varchar(  250)   = Null;
Begin

   Select  @PsCuerpo = '&saludo&, &Nombre& Por medio del presente se te informa que el proceso de &incidencia&, EN EL SERVIDOR &servidor& ',
           @PsHtml   = '<!DOCTYPE html>      <html lang="es">          <tbody>                  <p>&saludo&, &Nombre&                  <p style="margin:0 0 12px 0;font-size:16px;line-height:24px;font-family:Arial,sans-serif;color:#153643">                        Por medio del presente se te informa que el proceso de &incidencia&, EN EL SERVIDOR &servidor&             </tbody>      </html>'

   Execute dbo.spu_conMotivosCorreoTbl @PnIdMotivo         = @PnIdMotivo,
                                       @PsDescripcion      = @PsDescripcion,
                                       @PsTitulo           = @PsTitulo,
                                       @PsCuerpo           = @PsCuerpo,
                                       @PsHtml             = @PsHtml,
                                       @PsUrl              = @PsUrl,
                                       @PsPerfilCorreo     = @PsPerfilCorreo,
                                       @PbPermiteCompartir = @PbPermiteCompartir,
                                       @PbIdEstatus        = @PbIdEstatus,
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

Create Or Alter Procedure dbo.spu_conMotivosCorreoTbl
  (@PnIdMotivo           Integer,
   @PsDescripcion        Varchar(  100)   = Null,
   @PsTitulo             Varchar(  100)   = Null,
   @PsCuerpo             Varchar(  Max)   = Null,
   @PsHtml               NVarchar( Max)   = Null,
   @PsUrl                Varchar(  Max)   = Null,
   @PsPerfilCorreo       Sysname          = Null,
   @PbPermiteCompartir   Bit              = Null,
   @PbIdEstatus          Bit              = Null,
   @PsOperacion          Varchar(   20),
   @PnIdUsuarioAct       Integer,
   @PsIpAct              Varchar(   30)   = Null,
   @PsMacAddressAct      Varchar(   30)   = Null,
   @PnEstatus            Integer          = 0    Output,
   @PsMensaje            Varchar(  250)   = ' '  Output)
As

Declare
   @w_desc_error         Varchar( 250),
   @w_sql                Varchar( Max),
   @w_Error              Integer,
   @w_idOperacionAct     Integer,
   @w_comilla            Char(1);

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que Actualiza los Registros en la tabla conMotivosCorreoTbl.
  Creacion:       21-sep-2026.
  Version:        1.0
*/

   Set Quoted_identifier Off
   Set Nocount            On
   Set Xact_Abort         On
   Set Ansi_Nulls         On

   Select @PnEstatus       = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje       = 'Regitro Actualizado',
          @PsIpAct         = Isnull(@PsIpAct, dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMac()),
          @w_comilla       = Char(39);

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
                  And    idAutorizacion >= 3)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--
   If @PsPerfilCorreo Is Not Null
      Begin
         If Not Exists (Select Top 1 1
                        From   msdb.dbo.sysmail_account
                        Where  name = @PsPerfilCorreo)
            Begin
               Select @PnEstatus = 2040,
                      @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus)

               Set Xact_Abort Off
               Return
            End
      End

--

   If Not Exists ( Select Top 1 1
                   From   dbo.conMotivosCorreoTbl
                   Where  idMotivo    = @PnIdMotivo)
      Begin
         Select @PnEstatus = 2000,
                @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus)

         Set Xact_Abort Off
         Return
      End

--

   Set @w_sql = Concat('Update dbo.conMotivosCorreoTbl ',
                        'Set   idUsuarioAct  = ', @PnIdUsuarioAct,                   ', ',
                              'fechaAct      = ', @w_comilla, Getdate(), @w_comilla, ', ',
                              'ipAct         = ', @w_comilla, @PsIpAct,  @w_comilla, ', ',
                              'macAddressAct = ', @w_comilla, @PsMacAddressAct, @w_comilla);

   If @PsDescripcion      Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', descripcion = ', @w_comilla, @PsDescripcion, @w_comilla);
      End;

   If @PsTitulo           Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', titulo = ', @w_comilla, @PsTitulo, @w_comilla);
      End;

   If @PsCuerpo           Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', cuerpo = ', @w_comilla, @PsCuerpo, @w_comilla);
      End;

   If @PsHtml             Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', Html = ', @w_comilla, @PsHtml, @w_comilla);
      End;

   If @PsUrl              Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', Url = ', @w_comilla, @PsUrl, @w_comilla);
      End;

   If @PsPerfilCorreo     Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', perfilCorreo = ', @w_comilla, @PsPerfilCorreo, @w_comilla);
      End;

   If @PbPermiteCompartir Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', permiteCompartir = ', @PbPermiteCompartir);
      End;

   If @PbIdEstatus        Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', idEstatus  = ', @PbIdEstatus);
      End;

   Set @w_sql = Concat(@w_sql, ' Where idMotivo = ', @PnIdMotivo);

   Begin Try
      Execute (@w_sql);
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

   Set Xact_Abort Off
   Return

End
Go

Grant Execute On dbo.Spu_conMotivosCorreoTbl to Public
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que Actualiza los Registros de la tabla conMotivosCorreoTbl.',
   @w_procedimiento  Varchar( 100) = 'Spu_conMotivosCorreoTbl',
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
