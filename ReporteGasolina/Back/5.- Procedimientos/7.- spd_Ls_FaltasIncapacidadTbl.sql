-- Use adam
-- Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spd_Ls_FaltasIncapacidadTbl    @PsCompania  = @PsCompania,
                                              @PnAnio      = @PnAnio,
                                              @PnMes       = @PnMes,
                                              @PsUsuario   = @PsUsuario,
                                              @PsOperacion = @PsOperacion,
                                              @PnEstatus   = @PnEstatus Output,
                                              @PsMensaje   = @PsMensaje Output;

   Select @PnEstatus As Error, @PsMensaje As MensajeError

   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spd_Ls_FaltasIncapacidadTbl')
   Begin
      Drop Procedure dbo.spd_Ls_FaltasIncapacidadTbl;
   End
Go

Create Procedure dbo.spd_Ls_FaltasIncapacidadTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spd_Ls_FaltasIncapacidadTbl
-- Autor:            Pedro Zambrano
-- Fecha:            24-jul-2026.
-- Objetivo:         Procedimiento de baja a la tabla Ls_FaltasIncapacidadTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32);

 -------------------------------------------------------
 -- Validar Seguridad
 -------------------------------------------------------

   If Not Exists (Select Top 1 1
                  From   master.dbo.usuario_base
                  Where  Usuario         = @PsUsuario )
      Begin
         Select @PnEstatus    = 250001,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Usuario "' + @PsUsuario + '" no esta Registrado como usuario ADAM',
                @PsMensaje    = @v_desc_error;


         Goto Salida
      End

   If Not Exists (Select Top 1 1
                  From   dbo.Aut_Operaciones
                  Where  Usuario         = @PsUsuario
                  And    Operacion       = @PsOperacion
                  And    Nivel_seguridad > 1)
      Begin
         Select @PnEstatus    = 250002,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Usuario No tiene Autorizacion sobre la Operacion ' + @PsOperacion,
                @PsMensaje    = @v_desc_error;

         Goto Salida

      End

   If Not Exists (Select Top 1 1
                  From   dbo.Aut_Companias
                  Where  Usuario        = @PsUsuario
                  And    Compania       = @PsCompania)
      Begin
         Select @PnEstatus    = 250003,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Usuario no tiene Autorizacion sobre la compania ' + @PsCompania,
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   Begin Try
      Delete dbo.Ls_FaltasIncapacidadTbl
      Where  compania = @PsCompania
      And    Anio     = @PnAnio
      And    Mes      = @PnMes;
   End   Try

   Begin Catch
      Select  @v_Error      = @@Error,
              @v_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If IsNull(@v_Error, 0) <> 0
      Begin
         Select @PnEstatus = @v_error,
                @PsMensaje = 'Error.: ' + @v_desc_error;
         Goto Salida;

     End;

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go

Grant  Execute On spd_Ls_FaltasIncapacidadTbl to Public;

--
-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Procedimiento de Baja de Registros a la tabla Ls_FaltasIncapacidadTbl.',
   @w_procedimiento  NVarchar(250) = 'spd_Ls_FaltasIncapacidadTbl';

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
                                      @level0name = N'dbo',
                                      @level1type = 'Procedure',
                                      @level1name = @w_procedimiento

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'dbo',
                                        @level1type = 'Procedure',
                                        @level1name = @w_procedimiento
   End
Go
