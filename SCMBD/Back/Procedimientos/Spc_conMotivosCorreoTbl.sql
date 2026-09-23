/*

Declare
   @PnIdMotivo           Integer          = 1,
   @PsDescripcion        Varchar(  100)   = 'Incidencias BD',
   @PsTitulo             Varchar(  100)   = 'Incidencias BD',
   @PsPerfilCorreo       Sysname          = 'Incidencias',
   @PbPermiteCompartir   Bit              = 0,
   @PbIdEstatus          Bit              = Null,
   @PsOperacion          Varchar(   20)   = 'SU1012',
   @PnIdUsuarioAct       Smallint         = 3,
   @PnEstatus            Integer          = 0,
   @PsMensaje            Varchar(  250)   = Null;
Begin

   Execute dbo.Spc_conMotivosCorreoTbl @PnIdMotivo         = @PnIdMotivo ,
                                       @PsDescripcion      = @PsDescripcion,
                                       @PsTitulo           = @PsTitulo,
                                       @PsPerfilCorreo     = @PsPerfilCorreo,
                                       @PbPermiteCompartir = @PbPermiteCompartir,
                                       @PbIdEstatus        = @PbIdEstatus,
                                       @PsOperacion        = @PsOperacion,
                                       @PnIdUsuarioAct     = @PnIdUsuarioAct,
                                       @PnEstatus          = @PnEstatus         Output,
                                       @PsMensaje          = @PsMensaje         Output;

   If @PnEstatus != 0
      Begin
         Select @PnEstatus As IdError, @PsMensaje As "Mensaje de Error";
      End

   Return;

End;
Go
*/

Create Or Alter Procedure dbo.Spc_conMotivosCorreoTbl
  (@PnIdMotivo           Integer          = Null,
   @PsDescripcion        Varchar(  100)   = Null,
   @PsTitulo             Varchar(  100)   = Null,
   @PsPerfilCorreo       Sysname          = Null,
   @PbPermiteCompartir   Bit              = Null,
   @PbIdEstatus          Bit              = Null,
   @PsOperacion          Varchar(   20),
   @PnIdUsuarioAct       Integer,
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
  Descripción:    Procedimiento que Consulta los Registros en la tabla conMotivosCorreoTbl.
  Creacion:       21-sep-2026.
  Version:        1.0
*/

   Set Quoted_identifier Off
   Set Nocount            On
   Set Xact_Abort         On
   Set Ansi_Nulls         On

   Select @PnEstatus       = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje       = Char(32),
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
                  And    idAutorizacion >= 1)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--

   Set @w_sql = Concat('Select idMotivo, descripcion, titulo,       cuerpo, ',
                              'html,     URL,         perfilCorreo, permiteCompartir, ',
                              'idEstatus, ',
                              'dbo.Fn_BuscaDescripcionGeneral(', @w_comilla, 'conMotivosCorreoTbl', @w_comilla, ', ',
                                                             @w_comilla, 'idEstatus', @w_comilla, ', idEstatus) estatus ',
                       'From  dbo.conMotivosCorreoTbl ',
                       'Where  1 = 1')

   If @PnIdMotivo Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And idMotivo = ', @PnIdMotivo);
      End

   If @PsDescripcion   Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And descripcion Like ', @w_comilla, '%', trim(@PsDescripcion), '%', @w_comilla);
      End;

   If @PsTitulo           Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And titulo Like ', @w_comilla, '%', trim(@PsTitulo), '%', @w_comilla);
      End;

   If @PsPerfilCorreo     Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And perfilCorreo = ', @w_comilla, @PsPerfilCorreo, @w_comilla);
      End;

   If @PbPermiteCompartir Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And permiteCompartir = ', @PbPermiteCompartir);
      End;

   If @PbIdEstatus        Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And idEstatus  = ', @PbIdEstatus);
      End;

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

Grant Execute On dbo.Spc_conMotivosCorreoTbl to Public
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que Consulta los Registros de la tabla conMotivosCorreoTbl.',
   @w_procedimiento  Varchar( 100) = 'Spc_conMotivosCorreoTbl',
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

