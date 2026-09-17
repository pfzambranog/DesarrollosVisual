/*
Declare
   @PnIdMenu                 Integer          = 3,
   @PnIdOperacion            Integer          = 9,
   @PnSecuencia              Smallint         = Null,
   @PnIdEstatus              Tinyint          = Null,
   @PnIdOperacionAct         Integer          = 7,
   @PnIdUsuarioAct           Integer          = 3,
   @PsIpAct                  Varchar ( 30)    = Null,
   @PsMacAddressAct          Varchar ( 30)    = Null,
   @PnEstatus                Integer          = 0,
   @PsMensaje                Varchar (250)    = Null;

Begin
   Execute dbo.Spu_catRelMenuOperacionTbl @PnIdMenu          = @PnIdMenu,
                                          @PnIdOperacion     = @PnIdOperacion,
                                          @PnSecuencia       = @PnSecuencia,
                                          @PnIdEstatus       = @PnIdEstatus,
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
Create Or Alter Procedure dbo.Spu_catRelMenuOperacionTbl
  (@PnIdMenu                 Integer,
   @PnIdOperacion            Integer,
   @PnSecuencia              Smallint        = Null,
   @PnIdEstatus              Tinyint         = Null,
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
   @w_linea                   Integer,
   @w_sql                     Varchar( Max),
   @w_comilla                 Char(1);

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento de actualizacion de los Registros de la tabla catRelMenuOperacionTbl.
  Creacion:       15-sep-2026.
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

   If @PnIdEstatus Is Not Null
      Begin
         If Not Exists (Select Top 1 1
                        From   dbo.catGeneralesTbl
                        Where  tabla   = 'catRelMenuOperacionTbl'
                        And    columna = 'idEstatus'
                        And    valor   = @PnIdEstatus)
            Begin
               Select @PnEstatus = 8888,
                      @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

               Set Xact_Abort Off
               Return
            End
      End;

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


   Set @w_sql = Concat('Update dbo.catRelMenuOperacionTbl ',
                       'Set    fechaAct      = ', @w_comilla, Getdate(),  @w_comilla, ', ',
                              'idUsuarioAct  = ', @PnIdUsuarioAct,  ', ',
                              'ipAct         = ', @w_comilla, @PsIpAct,         @w_comilla, ', ',
                              'macAddressAct = ', @w_comilla, @PsMacAddressAct, @w_comilla);

   If @PnIdEstatus  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', idEstatus = ', @PnIdEstatus);
      End

   Begin Transaction
      If @PnSecuencia Is Not Null
         Begin
            Set @w_sql = Concat(@w_sql, ', secuencia = ', @PnSecuencia);
            Begin Try

               If Exists (Select Top 1 1
                          From   dbo.catRelMenuOperacionTbl
                          Where  idMenu       = @PnIdMenu
                          And    idOPeracion != @PnIdOperacion
                          And    secuencia    = @PnSecuencia)
                  Begin
                     Update dbo.catRelMenuOperacionTbl
                     Set    secuencia = secuencia + 1
                     Where  idMenu       = @PnIdMenu
                     And    secuencia   >= @PnSecuencia;
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

         End

      Set @w_sql = Concat(@w_sql, ' Where idMenu      = ', @PnIdMenu, ' ',
                                  ' And   idOperacion = ', @PnIdOperacion)
      Begin Try
         Execute (@w_sql);
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
   @w_valor          Varchar(1500) = 'Procedimiento de actualizacion de los Registros de la tabla catRelMenuOperacionTbl.',
   @w_procedimiento  Varchar( 100) = 'Spu_catRelMenuOperacionTbl'


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
