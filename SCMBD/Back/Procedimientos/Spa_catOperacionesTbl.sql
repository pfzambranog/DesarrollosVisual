/*
Declare
   @PsOperacion              Varchar ( 20)    = 'SU1011',
   @PsDescripcion            Varchar (100)    = 'Asignacion de Permisos la Relacion Usuarios - Operaciones',
   @PsLlamada                Varchar ( 20)    = Null,
   @PsRutas                  Varchar (512)    = Null,
   @PnIdOperacion            Integer          = 5,
   @PnIdUsuarioAct           Integer          = 1,
   @PsIpAct                  Varchar ( 30)    = Null,
   @PsMacAddressAct          Varchar ( 30)    = Null,
   @PnEstatus                Integer          = 0,
   @PsMensaje                Varchar (250)    = Null;

Begin
   Execute dbo.Spa_catOperacionesTbl @PsOperacion       = @PsOperacion,
                                     @PsDescripcion     = @PsDescripcion,
                                     @PsLlamada         = @PsLlamada,
                                     @PsRutas           = @PsRutas,
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
Create Or Alter Procedure dbo.Spa_catOperacionesTbl
  (@PsOperacion              Varchar ( 20),
   @PsDescripcion            Varchar (100),
   @PsLlamada                Varchar ( 20),
   @PsRutas                  Varchar (512),
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
  Descripción:    Procedimiento que da de alta los Registros a la tabla catOperacionesTbl.
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
                  And    idOperacion     = @PnIdOperacion
                  And    idAutorizacion >= 2)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Begin Try
      Insert Into dbo.catOperacionesTbl
     (operacion,    descripcion, llamada, ruta,
      idUsuarioAct, ipAct,   macAddressAct)
      Select @PsOperacion,    @PsDescripcion, @PsLlamada,       @PsRutas,
             @PnIdUsuarioAct, @PsIpAct,       @PsMacAddressAct

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
   @w_valor          Varchar(1500) = 'Procedimiento que da alta de Registros a la tabla catOperacionesTbl.',
   @w_procedimiento  Varchar( 100) = 'Spa_catOperacionesTbl'


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
