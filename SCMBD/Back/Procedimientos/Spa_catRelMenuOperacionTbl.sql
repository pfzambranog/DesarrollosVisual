/*
Declare
   @PnIdMenu                 Integer          = 1,
   @PnIdOperacion            Integer          = 7,
   @PnSecuencia              Smallint         = 0,
   @PnIdOperacionAct         Integer          = 1,
   @PnIdUsuarioAct           Integer          = 1,
   @PsIpAct                  Varchar ( 30)    = Null,
   @PsMacAddressAct          Varchar ( 30)    = Null,
   @PnEstatus                Integer          = 0,
   @PsMensaje                Varchar (250)    = Null;

Begin
   Execute dbo.Spa_catRelMenuOperacionTbl @PnIdMenu          = @PnIdMenu,
                                          @PnIdOperacion     = @PnIdOperacion,
                                          @PnSecuencia       = @PnSecuencia,
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
Create Or Alter Procedure dbo.Spa_catRelMenuOperacionTbl
  (@PnIdMenu                 Integer,
   @PnIdOperacion            Integer,
   @PnSecuencia              Smallint       = 0,
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
   @w_idEstatus               Tinyint,
   @w_secuencia               Tinyint,
   @w_linea                   Integer;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que da de alta los Registros a la tabla catRelMenuOperacionTbl.
  Creacion:       16-sep-2026.
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

--
-- Validacion del Id de menus seleccionado
--

   Select Top 1 @w_idEstatus = idEstatus
   From   dbo.catMenusTbl
   Where  idMenu = @PnIdMenu;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus = 9976,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If @w_idEstatus = 0
      Begin
         Select @PnEstatus = 9977,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--
-- Validacion del Id de Operacion seleccionado
--

   Select Top 1 @w_idEstatus = idEstatus
   From   dbo.catOperacionesTbl
   Where  idOperacion = @PnIdOperacion;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus = 9979,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If @w_idEstatus = 0
      Begin
         Select @PnEstatus = 9980,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--
-- Validacion de Operacion relacionado a Menu.
--

   If Exists (Select Top 1 1
              From   dbo.catRelMenuOperacionTbl
              Where  idOperacion     = @PnIdOperacion)
      Begin
         Select @PnEstatus = 9971,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Isnull(@PnSecuencia, 0) = 0
      Begin
         Select @w_secuencia = Max(secuencia)
         From   dbo.catRelMenuOperacionTbl
         Where  idMenu      = @PnIdMenu
         And    idOperacion = @PnIdOperacion;
         Set @w_secuencia = Isnull(@w_secuencia, 0) + 1;
      End
   Else 
      Begin
         Set @w_secuencia = @PnSecuencia;
      End   

   Begin Transaction
      Begin Try
         If Exists (Select Top 1 1
                    From   dbo.catRelMenuOperacionTbl
                    Where  idMenu      = @PnIdMenu
                    And    secuencia   = @w_secuencia)
            Begin
               Update dbo.catRelMenuOperacionTbl
               Set    secuencia = secuencia + 1
               Where  idMenu       = @PnIdMenu
               And    secuencia   >= @w_secuencia;
            End
   
      End Try
   
      Begin Catch
         Select  @w_Error      = @@Error,
                 @w_linea      = Error_line(),
                 @w_desc_error = Substring (Error_Message(), 1, 230)
      End   Catch
    
      If Isnull(@w_Error, 0) <> 0
         Begin
            Select @PnEstatus = @w_Error,
                   @PsMensaje = Concat('Error En Linea ', @w_linea, '.: ', @w_Error, ' ',    @w_desc_error )
    
            Rollback Transaction
            Set Xact_Abort Off
            Return
         End
--
--
--

      Begin Try
         Insert Into dbo.catRelMenuOperacionTbl
        (idMenu,    idOperacion, secuencia, idUsuarioAct,
         ipAct,
         macAddressAct)
         Select @PnIdMenu,    @PnIdOperacion,  @w_secuencia, @PnIdUsuarioAct,
                @PsIpAct,     @PsMacAddressAct

      End Try

      Begin Catch
         Select  @w_Error      = @@Error,
                 @w_linea      = Error_line(),
                 @w_desc_error = Substring (Error_Message(), 1, 230);
      End   Catch
   
      If Isnull(@w_Error, 0) <> 0
         Begin
            Select @PnEstatus = @w_Error,
                   @PsMensaje = Concat('Error En Linea ', @w_linea, '.: ', @w_Error, ' ',    @w_desc_error )

            Rollback Transaction   
            Set Xact_Abort Off
            Return
         End

   Commit Transaction
   
   Set Xact_Abort Off
   Return

End
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que da alta de Registros a la tabla catRelMenuOperacionTbl.',
   @w_procedimiento  Varchar( 100) = 'Spa_catRelMenuOperacionTbl'


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
