Use adam
Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsCiudad                Char(10)        = 'Toluca',
   @PnPrecio                Decimal(19,2)   = 20,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spv_Ls_HistPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                            @PnAnio      = @PnAnio,
                                            @PnMes       = @PnMes,
                                            @PsCiudad    = @PsCiudad,
                                            @PnPrecio    = @PnPrecio,
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
           Where  Name = 'spv_Ls_HistPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.spv_Ls_HistPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spv_Ls_HistPrecioGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsCiudad                Char(10),
   @PnPrecio                Decimal(19,2),
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
   @v_Error                 Integer,
   @v_desc_error            Varchar(250);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spv_Ls_HistPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            23-jul-2026.
-- Objetivo:         Procedimiento de valida registros a ser incoporados en la tabla Ls_HistPrecioGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32),
          @v_dato          = Rtrim(Substring(@PsCiudad, 1, 10));

 -------------------------------------------------------
 -- Validar Seguridad
 -------------------------------------------------------

   Begin
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

      If IsNull(@PnPrecio, 0) <= 0
         Begin
            Select @v_desc_error = 'Error: El Precio de la Gasolina debe ser mayor a cero (0)',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250004;

            Goto Salida
         End

      If IsNull(@PnAnio, 0) Not Between 2000 And 2050
         Begin
            Select @v_desc_error = 'Error: El Parámetro Año de Proceso no es Valido. (2020-2050)',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250005;

            Goto Salida
         End

      If IsNull(@PnMes, 0) Not Between 1 And 12
         Begin
            Select @v_desc_error = 'Error: El Parámetro Mes de Proceso no es Valido. (1-12)',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

--
-- Búsqueda de codigo de Agrupación
--

      Select @v_agrupacion = Rtrim(descripcion)
      From   dbo.criterios_valores
      Where  campo = 'agrciudad'
      And    item  = 1;
      If @@Rowcount = 0
         Begin
            Select @v_desc_error = 'Error: El Parámetro de Ciudad (agrciudad) no existe en criterios_valores',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250007;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.agrupaciones_trab
                     Where  agrupacion = @v_agrupacion)
         Begin
            Select @v_desc_error = 'Error: El Código de Agrupacion de Ciudad no es Válido',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.datos_agr_trab
                     Where  agrupacion = @v_agrupacion
                     And    dato       = @v_dato)
         Begin
            Select @v_desc_error = 'Error: El Código de Ciudad seleccionado no es Válido,',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

      If Exists (Select top 1 1
                 From   dbo.Ls_HistPrecioGasolinaTbl
                 Where  compania = @PsCompania
                 And    anio     = @PnAnio
                 And    mes      = @PnMes
                 And    ciudad   = @v_dato)
         Begin
            Select @v_desc_error = 'Error: El precio de la gasolina para la ciudad ya fue cargado',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

   End;

   Set @PsMensaje = 'Registro Valido';

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go

Grant  Execute On spv_Ls_HistPrecioGasolinaTbl to Public;

--
-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Procedimiento de Validación de Registros a ser incoporados a la tabla Ls_HistPrecioGasolinaTbl.',
   @w_procedimiento  NVarchar(250) = 'spv_Ls_HistPrecioGasolinaTbl';

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
