/*
Declare
   @PnIdUsuario              Integer        = 2,
   @PsPassword               Varchar (Max)  = 'TmV2YVBhc3N3b3Jk',
   @PnEstatus                Integer        = 0,
   @PsMensaje                Varchar (250)  = Null;

Begin
   Execute dbo.Spa_histUserPassTbl @PnIdUsuario       = @PnIdUsuario,
                                   @PsContrasenia     = @PsPassword,
                                   @PnEstatus         = @PnEstatus Output,
                                   @PsMensaje         = @PsMensaje Output;

    Select @PnEstatus, @PsMensaje;
    Return;
End;
Go
*/


Create Or Alter Procedure dbo.spa_histUserPassTbl
  (@PnIdUsuario              Integer,
   @PsContrasenia            Varchar(Max),
   @PnEstatus                Integer        = 0    Output,
   @PsMensaje                Varchar (250)  = Null Output)

As

Declare
   @w_desc_error            Varchar( 250),
   @w_Error                 Integer,
   @w_secuencia             Smallint;

Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2024-06-12
  Descripción:    Procedimiento que da de alta los Registros a la tabla segUsuariosTbl.
  Creacion:       03-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = dbo.Fn_ValidaUsuario(@PnIdUsuario),
          @PsMensaje         = Char(32)

   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Select  @w_secuencia = Max(secuencia)
   From    dbo.histUserPassTbl
   Where   IdUsuario = @PnIdUsuario
   Set @w_secuencia = Isnull(@w_secuencia, 0) + 1;

   Begin Try
      Insert  Into dbo.histUserPassTbl
      (idUsuario, secuencia, contrasenia)
      Select @PnIdUsuario, @w_secuencia, @PsContrasenia;
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
   @w_valor          Varchar(1500) = 'Procedimiento que da alta de Registros a la tabla histUserPassTbl.',
   @w_procedimiento  Varchar( 100) = 'spa_histUserPassTbl'


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
