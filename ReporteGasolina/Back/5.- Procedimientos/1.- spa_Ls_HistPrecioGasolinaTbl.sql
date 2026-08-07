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
   Execute dbo.spa_Ls_HistPrecioGasolinaTbl @PsCompania  = @PsCompania,
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
           Where  Name = 'Spa_Ls_HistPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.Spa_Ls_HistPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spa_Ls_HistPrecioGasolinaTbl
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
-- Nombre físico :   spa_Ls_HistPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de alta de registros a la tabla Ls_HistPrecioGasolinaTbl
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
            Select @PnEstatus    = 250001,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Usuario no esta Registrado como usuario ADAM',
                   @PsMensaje    = @v_desc_error
                   
            Goto Salida
         End
                     
      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Operaciones
                     Where  Usuario         = @PsUsuario
                     And    Operacion       = @PsOperacion
                     And    Nivel_seguridad > 1)
         Begin
            Select @PnEstatus    = 250002,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' No tiene Autorizacion de Operacion',
                   @PsMensaje    = @v_desc_error;
                   
            Goto Salida

         End


      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Companias
                     Where  Usuario        = @PsUsuario
                     And    Compania       = @PsCompania)
         Begin
            Select @PnEstatus    = 250003,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             '  No tiene Autorizacion para la compania',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If IsNull(@PnPrecio, 0) <= 0
         Begin
            Select @PnEstatus    = 250004,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Precio de la Gasolina debe ser mayor a cero (0)',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If IsNull(@PnAnio, 0) Not Between 2000 And 2050
         Begin
            Select @PnEstatus    = 250005,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Parámetro Año de Proceso no es Valido. (2020-2050)',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If IsNull(@PnMes, 0) Not Between 1 And 12
         Begin
            Select @PnEstatus    = 250006,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             '  El Parámetro Mes de Proceso no es Valido. (1-12)',
                   @PsMensaje    = @v_desc_error;

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
            Select @PnEstatus    = 250007,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Parámetro de Ciudad (agrciudad) no existe en criterios_valores',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.agrupaciones_trab
                     Where  agrupacion = @v_agrupacion)
         Begin
            Select @PnEstatus    = 250008,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Código de Agrupacion de Ciudad no es Válido',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.datos_agr_trab
                     Where  agrupacion = @v_agrupacion
                     And    dato       = @v_dato)
         Begin
            Select @PnEstatus    = 250009,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Código de Ciudad seleccionado no es Válido,',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If Exists (Select top 1 1
                 From   dbo.Ls_HistPrecioGasolinaTbl
                 Where  compania = @PsCompania
                 And    anio     = @PnAnio
                 And    mes      = @PnMes
                 And    ciudad   = @v_dato)
         Begin
            Select @PnEstatus    = 250010,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El precio de la gasolina para la ciudad ya fue cargado',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      Begin Try
         Insert Into dbo.Ls_HistPrecioGasolinaTbl
         (compania, anio, mes, ciudad, usuario, precio)
         Values (@PsCompania, @PnAnio, @PnMes, @v_dato, @PsUsuario, @PnPrecio)
      End   Try

      Begin Catch
         Select  @v_Error      = @@Error,
                 @v_desc_error = Substring (Error_Message(), 1, 230)
      End   Catch

      If IsNull(@v_Error, 0) <> 0
         Begin
            Select @PnEstatus = @v_error,
                   @PsMensaje = 'Error.: ' + @v_desc_error;

         End;

   End;

   Set @PsMensaje = 'Registro Guardado';

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go


Grant  Execute On Spa_Ls_HistPrecioGasolinaTbl to Public;
