/*
Declare
   @PnIdUsuario              Integer          = 1,
   @PnIdOperacion            Integer          = 7,
   @PnIdAutorizacion         Tinyint          = 4,
   @PnIdEstatus              Tinyint          = 0,
   @PnIdOperacionAct         Integer          = 1,
   @PnIdUsuarioAct           Integer          = 1,
   @PsIpAct                  Varchar ( 30)    = Null,
   @PsMacAddressAct          Varchar ( 30)    = Null,
   @PnEstatus                Integer          = 0,
   @PsMensaje                Varchar (250)    = Null;

Begin
   Execute dbo.Spu_segAutOperacionesTbl    @PnIdUsuario       = @PnIdUsuario,
                                           @PnIdOperacion     = @PnIdOperacion,
                                           @PnIdAutorizacion  = @PnIdAutorizacion,
                                           @PnIdEstatus       = @PnIdEstatus,
                                           @PnIdOperacionAct  = @PnIdOperacion,
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

Create Or Alter Procedure dbo.Spu_segAutOperacionesTbl
  (@PnIdUsuario              Integer,
   @PnIdOperacion            Integer,
   @PnIdAutorizacion         Tinyint         = Null,
   @PnIdEstatus              Tinyint         = Null,
   @PnIdOperacionAct         Integer,
   @PnIdUsuarioAct           Integer,
   @PsIpAct                  Varchar ( 30)   = Null,
   @PsMacAddressAct          Varchar ( 30)   = Null,
   @PnEstatus                Integer         = 0    Output,
   @PsMensaje                Varchar (250)   = Null Output)
As

Declare
   @w_desc_error              Varchar( 250),
   @w_sql                     Varchar( Max),
   @w_PasswordActual          Varchar( Max),
   @w_Error                   Integer,
   @w_registros               Integer,
   @w_fechaAct                Datetime,
   @w_comilla                 Char(1),
   @w_claveUsuario            Varchar( 50),
   @w_nombre                  Varchar(400);

Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2024-06-12
  Descripción:    Procedimiento que actualiza los Registros a la tabla segAutOperacionesTbl.
  Creacion:       11-sep-2026.
  Version:        1.0
*/

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32),
          @w_comilla         = Char(39),
          @PsIpAct           = Isnull(@PsIpAct, dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct   = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMAC());

   Select @PnEstatus = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct)
   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--
-- Validacion de Password
--


   If @PnIdEstatus Is Not Null
      Begin
         If Not Exists (Select Top 1 1
                        From   dbo.catGeneralesTbl
                        Where  tabla   = 'segAutOperacionesTbl'
                        And    columna = 'idEstatus'
                        And    valor   = @PnIdEstatus)
            Begin
               Select @PnEstatus = 8888,
                      @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

               Set Xact_Abort Off
               Return
           End
      End

   If @PnIdAutorizacion Is Not Null
      Begin
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
      End

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @PnIdOperacionAct
                  And    idAutorizacion >= 3)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End


   Set @w_sql = Concat('Update dbo.segAutOperacionesTbl ',
                       'Set    fechaAct      = ', @w_comilla, Getdate(),  @w_comilla, ', ',
                              'idUsuarioAct  = ', @PnIdUsuarioAct,  ', ',
                              'ipAct         = ', @w_comilla, @PsIpAct,         @w_comilla, ', ',
                              'macAddressAct = ', @w_comilla, @PsMacAddressAct, @w_comilla);



   If @PnIdAutorizacion    Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', idAutorizacion  = ', @PnIdAutorizacion);
      End

   If @PnIdEstatus  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', idEstatus = ', @w_comilla, @PnIdEstatus, @w_comilla);
      End

   Set @w_sql = Concat(@w_sql, ' Where idUsuario = ',   @PnIdUsuario,
                               ' And   idOperacion = ', @PnIdOperacion)
   Begin Try
      Execute (@w_sql);
      Set @w_registros = Isnull(@w_registros, 0) + 1
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

Grant Execute on Spp_ActualizaPasswordUserBD to Public;

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que actualiza los Registros a la tabla segAutOperacionesTbl.',
   @w_procedimiento  Varchar( 100) = 'Spu_segAutOperacionesTbl'


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
