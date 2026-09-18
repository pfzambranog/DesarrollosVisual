/*
Declare
   @PsCodRegla            Varchar( 10)  = 'Test01',
   @PsNombreRegla         Varchar(100)  = Null,
   @PsDescripcion         Varchar(500)  = 'Reglas de Prueba',
   @PbEsRequerido         Bit           = 0,
   @PnValorMinimo         Integer       = 2,
   @PbIdEstatus           Bit           = Null,
   @PnIdOperacionAct      Integer       = 8,
   @PnIdUsuarioAct        Integer       = 3,
   @PsIpAct               Varchar(30)   = Null,
   @PsMacAddressAct       Varchar(30)   = Null,
   @PnEstatus             Integer       = 0,
   @PsMensaje             Varchar(Max)  = Null;
Begin
   Execute dbo.Spu_segReglasContrasenaTbl @PsCodRegla       = @PsCodRegla,
                                          @PsNombreRegla    = @PsNombreRegla,
                                          @PsDescripcion    = @PsDescripcion,
                                          @PbEsRequerido    = @PbEsRequerido,
                                          @PnValorMinimo    = @PnValorMinimo,
                                          @PbIdEstatus      = @PbIdEstatus,
                                          @PnIdOperacionAct = @PnIdOperacionAct,
                                          @PnIdUsuarioAct   = @PnIdUsuarioAct,
                                          @PsIpAct          = @PsIpAct,
                                          @PsMacAddressAct  = @PsMacAddressAct,
                                          @PnEstatus        = @PnEstatus Output,
                                          @PsMensaje        = @PsMensaje Output;

   Select @PnEstatus IdError, @PsMensaje Error;

   Return;

End;
Go
*/

Create Or Alter Procedure dbo.Spu_segReglasContrasenaTbl
   @PsCodRegla            Varchar( 10),
   @PsNombreRegla         Varchar(100) = Null,
   @PsDescripcion         Varchar(500) = Null,
   @PbEsRequerido         Bit          = Null,
   @PnValorMinimo         Integer      = Null,
   @PbIdEstatus           Bit          = Null,
   @PnIdOperacionAct      Integer,
   @PnIdUsuarioAct        Integer,
   @PsIpAct               Varchar(30)  = Null,
   @PsMacAddressAct       Varchar(30)  = Null,
   @PnEstatus             Integer      = 0     Output,
   @PsMensaje             Varchar(Max) = Null  Output
As
Declare
   @w_desc_error          Varchar( 250),
   @w_sql                 Varchar( Max),
   @w_comilla             Char(1),
   @w_Error               Integer,
   @w_linea               Integer;
Begin
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

-- =================================================================
-- Autor:       Pedro Zambrano
-- Fecha:       2026-09-17
-- Descripción: Procedimiento de Actualización de Reglas de Contraseñas
-- Operación:   SU1012
-- =================================================================

   Select @PnEstatus       = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje       = Char(32),
          @w_comilla       = Char(39),
          @PsIpAct         = Isnull(@PsIpAct, dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMAC());

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

   If @PbEsRequerido Is Not Null
      Begin
         If Not Exists (Select Top 1 1
                        From   dbo.catGeneralesTbl
                        Where  tabla   = 'segReglasContrasenaTbl'
                        And    columna = 'esRequerido'
                        And    valor   = @PbEsRequerido)
            Begin
               Select @PnEstatus = 5017,
                      @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);
               Set Xact_Abort Off
               Return
            End
      End

   If @PbIdEstatus Is Not Null
      Begin
         If Not Exists (Select Top 1 1
                        From   dbo.catGeneralesTbl
                        Where  tabla   = 'segReglasContrasenaTbl'
                        And    columna = 'idEstatus'
                        And    valor   = @PbIdEstatus)
            Begin
               Select @PnEstatus = 8888,
                      @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);
               Set Xact_Abort Off
               Return
            End
      End

   If Not Exists (Select Top 1 1
                  From   dbo.segReglasContrasenaTbl
                  Where  codRegla = @PsCodRegla)
      Begin
         Select @PnEstatus = 5015,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);;
         Set Xact_Abort Off
         Return
      End

   Set @w_sql = Concat('Update dbo.segReglasContrasenaTbl ',
                       'Set    fechaAct      = ', @w_comilla, Getdate(),  @w_comilla, ', ',
                              'idUsuarioAct  = ', @PnIdUsuarioAct,  ', ',
                              'ipAct         = ', @w_comilla, @PsIpAct,         @w_comilla, ', ',
                              'macAddressAct = ', @w_comilla, @PsMacAddressAct, @w_comilla);

   If @PsNombreRegla, Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', nombreRegla = ', @w_comilla, @PsNombreRegla, @w_comilla);
      End
      
   If @PsDescripcion        Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', descripcion = ', @w_comilla, @PsDescripcion, @w_comilla);
      End

   If @PbEsRequerido  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', esRequerido = ', @PbEsRequerido);
      End

   If @PnValorMinimo  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', valorMinimo = ', @PnValorMinimo);
      End


   If @PbIdEstatus  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', idEstatus = ', @PbIdEstatus);
      End

   Set @w_sql = Concat(@w_sql, ' Where codRegla = ', @w_comilla + @PsCodRegla + @w_comilla)

   Begin Try
      Execute (@w_sql);
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

--
-- Comentarios.
--
Declare
   @w_valor          Varchar(1500) = 'Procedimiento que actualiza registros de la tabla segReglasContrasenaTbl.',
   @w_procedimiento  Varchar( 100) = 'Spu_segReglasContrasenaTbl'
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