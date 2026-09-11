/*
Declare
   @PnIdUsuario              Integer          = 1,
   @PnIdOperacion            Integer          = 7,
   @PnIdAutorizacion         Tinyint          = 4,
   @PnIdOperacionAct         Integer          = 1,
   @PnIdUsuarioAct           Integer          = 1,
   @PsIpAct                  Varchar ( 30)    = Null,
   @PsMacAddressAct          Varchar ( 30)    = Null,
   @PnEstatus                Integer          = 0,
   @PsMensaje                Varchar (250)    = Null;

Begin
   Execute dbo.Spa_segAutOperacionesTbl @PnIdUsuario       = @PnIdUsuario,
                                        @PnIdOperacion     = @PnIdOperacion,
                                        @PnIdAutorizacion  = @PnIdAutorizacion,
                                        @PnIdOperacionAct  = @PnIdOperacionAct,
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
Create Or Alter Procedure dbo.Spa_segAutOperacionesTbl
  (@PnIdUsuario              Integer,
   @PnIdOperacion            Integer,
   @PnIdAutorizacion         Tinyint,
   @PnIdOperacionAct         Integer,
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
  Descripción:    Procedimiento que da de alta los Registros a la tabla segAutOperacionesTbl.
  Creacion:       02-sep-2026.
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

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @PnIdOperacionAct
                  And    idAutorizacion >= 2)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End


   If Exists (Select Top 1 1
              From   dbo.segAutOperacionesTbl
              Where  idUsuario       = @PnIdUsuario
              And    idOperacion     = @PnIdOperacion)
      Begin
         Select @PnEstatus = 706,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Not Exists (Select Top 1 1
                  From   dbo.catGeneralesTbl
                  Where  tabla           = 'segAutOperacionesTbl'
                  And    columna         = 'idAutorizacion'
                  And    valor           = @PnIdAutorizacion)
      Begin
         Select @PnEstatus = 707,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End
      

   Begin Try
      Insert Into dbo.segAutOperacionesTbl
     (idUsuario,    idOperacion, idAutorizacion,
      idUsuarioAct, ipAct,       macAddressAct)
      Select @PnIdUsuario,    @PnIdOperacion,     @PnIdAutorizacion,
             @PnIdUsuarioAct, @PsIpAct,          @PsMacAddressAct

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

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que da alta de Registros a la tabla segAutOperacionesTbl.',
   @w_procedimiento  Varchar( 100) = 'Spa_segAutOperacionesTbl'


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
