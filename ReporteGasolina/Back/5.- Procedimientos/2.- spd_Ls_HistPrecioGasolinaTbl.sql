/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsCiudad                Char(10)        = 'Toluca',
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null;
Begin
   Execute dbo.spd_Ls_HistPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                            @PnAnio      = @PnAnio,
                                            @PnMes       = @PnMes,
                                            @PsCiudad    = @PsCiudad,
                                            @PsUsuario   = @PsUsuario,
                                            @PsOperacion = @PsOperacion,
                                            @PnEstatus   = @PnEstatus Output,
                                            @PsMensaje   = @PsMensaje Output;

   Select @PnEstatus As IdError, @PsMensaje As MensajeError
   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spd_Ls_HistPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.spd_Ls_HistPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spd_Ls_HistPrecioGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsCiudad                Char(10)        = Null,
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_agrupacion            Char(10),
   @v_dato                  Char(10),
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_error                 Integer,
   @v_desc_error            Varchar(250),
   @v_registros             Integer,
   @v_linea                 Integer;

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spd_Ls_HistPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de baja de registros a la tabla Ls_HistPrecioGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @v_dato = Rtrim(Substring(@PsCiudad, 1, 10)),
          @PnEstatus  = 0,
          @PsMensaje  = Char(32);

  -------------------------------------------------------
  -- Validar Seguridad
  -------------------------------------------------------

   Begin
      If IsNull(@PnAnio, 0) = 0 Or
         IsNull(@PnMes,  0) = 0
         Begin
            Goto Salida
         End

      If Not Exists (Select Top 1 1
                     From   master.dbo.usuario_base
                     Where  Usuario         = @PsUsuario )
         Begin
            Select @v_desc_error = 'Error: El Usuario no esta Registrado como usuario ADAM',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250001

            Goto Salida
         End

      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Operaciones
                     Where  Usuario         = @PsUsuario
                     And    Operacion       = @PsOperacion
                     And    Nivel_seguridad > 1)
         Begin
            Select @v_desc_error = 'Error: No tiene Autorizacion de Operacion',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250002;

            Goto Salida

         End

      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Companias
                     Where  Usuario        = @PsUsuario
                     And    Compania       = @PsCompania)
         Begin
            Select @v_desc_error = 'Error: No tiene Autorizacion para la compania',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250003;

            Goto Salida
         End


      Begin Try
         If @v_dato Is Not Null
            Begin
               If Not Exists (Select top 1 1
                              From   dbo.Ls_HistPrecioGasolinaTbl
                              Where  compania = @PsCompania
                              And    anio     = @PnAnio
                              And    mes      = @PnMes
                              And    ciudad   = @v_dato)
                  Begin
                      Select @v_desc_error = 'Error: El Registro a Eliminar No es Valido',
                             @PsMensaje    = @v_desc_error,
                             @PnEstatus    = 250007;

                      Goto Salida
                  End

               Delete dbo.Ls_HistPrecioGasolinaTbl
               Where  compania = @PsCompania
               And    anio     = @PnAnio
               And    mes      = @PnMes
               And    ciudad   = @v_dato;
               Set @v_registros = @@Rowcount
            End
         Else
            Begin
               Delete dbo.Ls_HistPrecioGasolinaTbl
               Where  compania = @PsCompania
               And    anio     = @PnAnio
               And    mes      = @PnMes;
               Set @v_registros = @@Rowcount
            End
      End Try

      Begin Catch
         Select  @v_Error      = @@Error,
                 @v_linea      = error_line(),
                 @v_desc_error = Substring (Error_Message(), 1, 230) + ' En linea: ' + Cast(@v_linea as Varchar)
      End   Catch

      If IsNull(@v_Error, 0) <> 0
         Begin
            Select @PnEstatus = @v_error,
                   @PsMensaje = 'Error.: ' + @v_desc_error;

         End;

   End;

   Set @PsMensaje = Cast(@v_registros As Varchar) + ' Registros Eliminados.';

Salida:

   Set Xact_Abort    Off
   Return;
End;
Go

Grant  Execute On spd_Ls_HistPrecioGasolinaTbl to Public;

--
-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Procedimiento de Baja de Registros a la tabla Ls_HistPrecioGasolinaTbl.',
   @w_procedimiento  NVarchar(250) = 'spd_Ls_HistPrecioGasolinaTbl';

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

