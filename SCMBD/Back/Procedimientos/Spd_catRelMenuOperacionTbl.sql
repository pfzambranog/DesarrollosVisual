/*
Declare
   @PnIdMenu                 Integer          = 1,
   @PnIdOperacion            Integer          = 7,
   @PsOperacion              Varchar( 20)     = 'SU1011',,
   @PnIdUsuarioAct           Integer          = 1,
   @PsIpAct                  Varchar ( 30)    = Null,
   @PsMacAddressAct          Varchar ( 30)    = Null,
   @PnEstatus                Integer          = 0,
   @PsMensaje                Varchar (250)    = Null;

Begin
   Execute dbo.Spd_catRelMenuOperacionTbl @PnIdMenu          = @PnIdMenu,
                                          @PnIdOperacion     = @PnIdOperacion,
                                          @PsOperacion       = @PsOperacion,
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
Create Or Alter Procedure dbo.Spd_catRelMenuOperacionTbl
  (@PnIdMenu                 Integer,
   @PnIdOperacion            Integer,
   @PsOperacion              Varchar( 20),
   @PnIdUsuarioAct           Integer,
   @PsIpAct                  Varchar ( 30)  = Null,
   @PsMacAddressAct          Varchar ( 30)  = Null,
   @PnEstatus                Integer        = 0    Output,
   @PsMensaje                Varchar (250)  = Null Output)
As

Declare
   @w_desc_error              Varchar( 250),
   @w_Error                   Integer,
   @w_idEstatus               Tinyint,
   @w_idOperacion             Integer;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento de baja de los Registros de la tabla catRelMenuOperacionTbl.
  Creacion:       15-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32),
          @PsIpAct           = Isnull(@PsIpAct, dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct   = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMAC());


   Select @PnEstatus = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct)
   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Select top 1 @w_idOperacion = idOperacion
   From   dbo.catOperacionesTbl
   Where  operacion = @PsOperacion;

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @w_idOperacion
                  And    idAutorizacion >= 4)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Not Exists (Select Top 1 1
                  From   dbo.catRelMenuOperacionTbl
                  Where  idMenu          = @PnIdMenu
                  And    idOperacion     = @PnIdOperacion)
      Begin
         Select @PnEstatus = 9973,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Begin Try
      Delete dbo.catRelMenuOperacionTbl
      Where  idMenu      = @PnIdMenu
      And    idOperacion = @PnIdOperacion;
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = Concat('Error.: ', @w_Error, ' ',  @w_desc_error )

         Set Xact_Abort Off
         Return
      End

   Set Xact_Abort Off
   Return

End
Go

Grant Execute on Spd_catRelMenuOperacionTbl to PUBLIC

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento de Baja de los Registros de la tabla catRelMenuOperacionTbl.',
   @w_procedimiento  Varchar( 100) = 'Spd_catRelMenuOperacionTbl'


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
