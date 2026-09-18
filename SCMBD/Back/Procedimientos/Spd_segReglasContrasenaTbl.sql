/*
Declare
   @PsCodRegla            Varchar( 10)  = 'Test01',
   @PnIdOperacionAct      Integer       = 8,
   @PnIdUsuarioAct        Integer       = 3,
   @PnEstatus             Integer       = 0,
   @PsMensaje             Varchar(Max)  = Null;
Begin
   Execute dbo.Spd_segReglasContrasenaTbl @PsCodRegla       = @PsCodRegla,
                                          @PnIdOperacionAct = @PnIdOperacionAct,
                                          @PnIdUsuarioAct   = @PnIdUsuarioAct,
                                          @PnEstatus        = @PnEstatus Output,
                                          @PsMensaje        = @PsMensaje Output;

   Select @PnEstatus IdError, @PsMensaje Error;

   Return;

End;
Go
*/

Create Or Alter Procedure dbo.Spd_segReglasContrasenaTbl
   @PsCodRegla            Varchar( 10),
   @PnIdOperacionAct      Integer,
   @PnIdUsuarioAct        Integer,
   @PnEstatus             Integer      = 0     Output,
   @PsMensaje             Varchar(Max) = Null  Output
As

Declare
   @w_desc_error          Varchar( 250),
   @w_Error               Integer,
   @w_linea               Integer;

Begin
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On
   
-- =================================================================
-- Autor:       Pedro Zambrano
-- Fecha:       2026-09-17
-- Descripción: Procedimiento de Baja de Reglas de Contraseñas 
-- Operación:   SU1012
-- =================================================================


   Select @PnEstatus       = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje       = Char(32);

   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);
         Set Xact_Abort Off
         Return
      End
      
   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @PnIdOperacionAct
                  And    idAutorizacion >= 4)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);
         Set Xact_Abort Off
         Return
      End
      
   If Not Exists (Select Top 1 1
                  From   dbo.segReglasContrasenaTbl
                  Where  codRegla = @PsCodRegla)
      Begin
         Select @PnEstatus = 5017,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Begin Try
      Delete dbo.segReglasContrasenaTbl
      Where  codRegla = @PsCodRegla;
   End Try
   
   Begin Catch
      Select  @w_Error      = @@Error,
              @w_linea      = Error_line(),
              @w_desc_error = Substring(Error_Message(), 1, 230);
   End Catch
   
   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = Concat('Error En Linea ', @w_linea, '.: ', @w_Error, ' ', @w_desc_error);
         Set Xact_Abort Off
         Return
      End
      
   Set Xact_Abort Off
   Return
End
Go

Grant Execute On Spd_segReglasContrasenaTbl to Public 
Go


--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que da baja a registros de segReglasContrasenaTbl.',
   @w_procedimiento  Varchar( 100) = 'Spd_segReglasContrasenaTbl';
   
If Not Exists (Select Top 1 1
               From   sys.extended_properties a
               Join   sysobjects  b
               On     b.xtype   = 'P'
               And    b.name    = @w_procedimiento
               And    b.id      = a.major_id)
   Begin
      Execute sp_addextendedproperty @name       = N'MS_Description',
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