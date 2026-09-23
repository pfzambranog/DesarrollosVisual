/*

Declare
   @PnIdMotivo           Integer          = 1,
   @PsOperacion          Varchar(   20)   = 'SU1012',
   @PnIdUsuarioAct       Smallint         = 3,
   @PnEstatus            Integer          = 0,
   @PsMensaje            Varchar(  250)   = Null;
Begin

   Execute dbo.spd_conMotivosCorreoTbl @PnIdMotivo         = @PnIdMotivo ,
                                       @PsOperacion        = @PsOperacion,
                                       @PnIdUsuarioAct     = @PnIdUsuarioAct,
                                       @PnEstatus          = @PnEstatus         Output,
                                       @PsMensaje          = @PsMensaje         Output;

   Select @PnEstatus As IdError, @PsMensaje As "Mensaje";

   Return;

End;
Go
*/

Create Or Alter Procedure dbo.spd_conMotivosCorreoTbl
  (@PnIdMotivo           Integer,
   @PsOperacion          Varchar(   20),
   @PnIdUsuarioAct       Integer,
   @PnEstatus            Integer          = 0    Output,
   @PsMensaje            Varchar(  250)   = ' '  Output)

As

Declare
   @w_desc_error         Varchar( 250),
   @w_Error              Integer,
   @w_idOperacionAct     Integer;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento queda de Baja los Registros en la tabla conMotivosCorreoTbl.
  Creacion:       21-sep-2026.
  Version:        1.0
*/

   Set Quoted_identifier Off
   Set Nocount            On
   Set Xact_Abort         On
   Set Ansi_Nulls         On

   Select @PnEstatus      = dbo.Fn_ValidaUsuarioAdmin(@PnIdUsuarioAct),
          @PsMensaje      = 'Regitro Eliminado';

   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus)

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
                  And    idAutorizacion  = 4)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End


   If Not Exists ( Select Top 1 1
                   From   dbo.conMotivosCorreoTbl
                   Where  idMotivo    = @PnIdMotivo)
      Begin
         Select @PnEstatus = 2000,
                @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus)

         Set Xact_Abort Off
         Return
      End;

   Execute dbo.Spc_ReferenciasTablsFk @PsTable            = 'conMotivosCorreoTbl' ,
                                      @PsValor            = @PnIdMotivo,
                                      @PnEstatus          = @PnEstatus         Output,
                                      @PsMensaje          = @PsMensaje         Output;
   If @PnEstatus != 0                                   
      Begin
         Set @PnEstatus = 547

         Set Xact_Abort Off
         Return
      End;

   Begin Try
      Delete conMotivosCorreoTbl
      Where  idMotivo      = @PnIdMotivo;
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = Concat('Error.: ', @w_Error, ' ', @w_desc_error);

         Set Xact_Abort Off
         Return
      End

   Set Xact_Abort Off
   Return

End
Go

Grant Execute On dbo.Spd_conMotivosCorreoTbl to Public
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento queda de Baja los Registros en la tabla conMotivosCorreoTbl.',
   @w_procedimiento  Varchar( 100) = 'Spd_conMotivosCorreoTbl',
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
