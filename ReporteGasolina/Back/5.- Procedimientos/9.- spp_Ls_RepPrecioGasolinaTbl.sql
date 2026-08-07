-- Use adam
-- Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnImprime               Bit             = 1,
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spp_Ls_RepPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                           @PnAnio      = @PnAnio,
                                           @PnMes       = @PnMes,
                                           @PsUsuario   = @PsUsuario,
                                           @PsOperacion = @PsOperacion,
                                           @PnImprime   = @PnImprime,
                                           @PnEstatus   = @PnEstatus Output,
                                           @PsMensaje   = @PsMensaje Output;

   If @PnEstatus > 0
      Begin
         Select @PnEstatus As Error, @PsMensaje As MensajeError
      End

   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spp_Ls_RepPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.spp_Ls_RepPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spp_Ls_RepPrecioGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnImprime               Bit             = 0,
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250),
--
   @v_region                Char(10),
   @v_departamento          Char(10),
   @v_zona                  Char(10),
   @v_ciudad                Char(10),
   @v_faltas                Char(10),
   @v_fecha_inicio          Date,
   @v_fecha_termino         Date,
   @v_precio                Decimal(19,2),
--
   @v_trabajador            Char( 10),
   @v_nombre                Char(100),
   @v_datoRegion            Char( 10),
   @v_datoDepart            Char( 10),
   @v_datoZona              Char( 10),
   @v_datoCiudad            Char( 10),
--
   @v_datoAntRegion         Char( 10),
   @v_datoAntDepart         Char( 10),
   @v_datoAntZona           Char( 10),
   @v_datoAntCiudad         Char( 10),
--
   @v_descRegion            Char(150),
   @v_descDepart            Char(150),
   @v_descZona              Char(150),
   @v_descCiudad            Char(150),
   @v_nss                   Char( 20),
   @v_var_trab_gas          Char( 10),
   @v_var_trab_exc          Char( 10),
   @v_var_trab_tar          Char( 10),
   @v_linea                 Integer,
   @v_pvpLitro              Decimal(19, 2),
   @v_cantLitros            Decimal(19, 2),
   @v_impGasMes             Decimal(19, 2),
   @v_impFalta              Decimal(19, 2),
   @v_diasFalta             Integer,
   @v_diasIncap             Integer,
   @v_valTrabExc            Integer,
   @v_impIncap              Decimal(19, 2),
   @v_totalDias             Integer,
   @v_totalMes              Decimal(19, 2),
   @v_netoMes               Decimal(19, 2),
   @v_tarjeta               Varchar(30);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spp_Ls_RepPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de Calculo del Reporte de Asignación de Gasolina a Trabajadores
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32),
          @v_linea         = 0,
          @v_totalDias     = 30,
          @v_fecha_inicio  = Convert(Date, '01/' + Convert(Char(2), @PnMes) + '/' +
                                                   Convert(Char(4), @PnAnio), 103),
          @v_fecha_termino = dbo.fn_obten_FinMes(@v_fecha_inicio);

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

   If Not Exists (Select top 1 1
                  From   dbo.Ls_HistPrecioGasolinaTbl
                  Where  compania = @PsCompania
                  And    anio     = @PnAnio)
      Begin
         Select @PnEstatus    = 250004,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' No existen Precios de Gasolina para el AÑO seleccionado: ' +
                                            Cast(@PnAnio As Varchar),
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.Ls_HistPrecioGasolinaTbl
                  Where  compania = @PsCompania
                  And    anio     = @PnAnio
                  And    mes      = @PnMes)
      Begin
         Select @PnEstatus    = 250005,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' No existen Precios de Gasolina para el AÑO-MES seleccionado: ' +
                                            Cast(@PnAnio As Varchar) + '-' + Cast(@PnMes As Varchar),
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de variable trabajador (Cantidad de Litros de gasolina asignado al trabajador)
--

   Select @v_var_trab_gas = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'ltrgas'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250006,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro variable trabajador "ltrgasno" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.defn_variables_ns
                  Where  compania = @PsCompania
                  And    variable = @v_var_trab_gas)
      Begin
         Select @PnEstatus    = 250007,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de la variable del trabajador "' + @v_var_trab_gas + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de número de tarjeta para deposito de asignación gasolina
--

   Select @v_var_trab_tar = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'tarjetagas'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250008,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro variable trabajador "tarjetagas" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.defn_variables_ns
                  Where  compania = @PsCompania
                  And    variable = @v_var_trab_tar)
      Begin
         Select @PnEstatus    = 250009,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de la variable del trabajador "' + @v_var_trab_tar + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de variable trabajador (Exentos de Pago de Gasolina)
--

   Select @v_var_trab_exc = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'excgas'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250010,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          '  El Parámetro variable trabajador "excgas" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.defn_variables_ns
                  Where  compania = @PsCompania
                  And    variable = @v_var_trab_exc)
      Begin
         Select @PnEstatus    = 250011,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de la variable del trabajador "' + @v_var_trab_exc + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de Agrupaciones de Trabajadores Region
--

   Select @v_region = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrregion'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250012,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Region (agrregion) no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_region)
      Begin
         Select @PnEstatus    = 250013,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Región "' + @v_region + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de Agrupaciones de Trabajadores Departamento
--

   Select @v_departamento = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrdep'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250014,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Departamento "agrdep" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_departamento)
      Begin
         Select @PnEstatus    = 250015,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Departamento "' + @v_departamento + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de Agrupación Zona
--

   Select @v_zona = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrzona'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250016,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Zona "agrzona" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_zona)
      Begin
         Select @PnEstatus    = 250017,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Zona "' + @v_zona + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End


--
-- Búsqueda de codigo de Agrupación Ciudad
--

   Select @v_ciudad = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrciudad'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250018,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Ciudad "agrciudad" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_ciudad)
      Begin
         Select @PnEstatus    = 250019,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Ciudad "' + @v_ciudad + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de Agrupación Conceptos Faltas
--

   Select @v_faltas = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrfalta'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250020,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Agr Conceptos por Faltas "agrfalta" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.rel_conceptos_agr
                  Where  compania      = @PsCompania
                  And    agr_conceptos = @v_faltas)
      Begin
         Select @PnEstatus    = 250021,
                @v_desc_error ='Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' Código de Agrupacion de Conceptos  "' + @v_faltas + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--

   Execute dbo.spd_Ls_RepPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                           @PnAnio      = @PnAnio,
                                           @PnMes       = @PnMes,
                                           @PsUsuario   = @PsUsuario,
                                           @PsOperacion = @PsOperacion,
                                           @PnEstatus   = @PnEstatus Output,
                                           @PsMensaje   = @PsMensaje Output;

   If @PnEstatus != 0
      Begin
         Goto Salida
      End

   Execute dbo.spp_Ls_FaltasGasolinaTbl @PsCompania  = @PsCompania,
                                        @PnAnio      = @PnAnio,
                                        @PnMes       = @PnMes,
                                        @PsUsuario   = @PsUsuario,
                                        @PsOperacion = @PsOperacion,
                                        @PnImprime   = @PnImprime,
                                        @PnEstatus   = @PnEstatus Output,
                                        @PsMensaje   = @PsMensaje Output;
   If @PnEstatus != 0
      Begin
         Goto Salida
      End

   Execute dbo.spp_Ls_FaltasIncapacidadTbl @PsCompania  = @PsCompania,
                                           @PnAnio      = @PnAnio,
                                           @PnMes       = @PnMes,
                                           @PsUsuario   = @PsUsuario,
                                           @PsOperacion = @PsOperacion,
                                           @PnImprime   = @PnImprime,
                                           @PnEstatus   = @PnEstatus Output,
                                           @PsMensaje   = @PsMensaje Output;
   If @PnEstatus != 0
      Begin
         Goto Salida
      End

   Declare
      C_Detalle Cursor For
      Select a.trabajador, Replace(b.nombre, '/', ' ') nombre,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_region), Char(32)) region,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_departamento), Char(32)) departamento,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_zona), Char(32)) zona,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_ciudad), Char(32)) ciudad
      From   dbo.trabajadores_grales a
      Join   dbo.trabajadores        b
      On     b.trabajador     = a.trabajador
      Where  a.compania       = @PsCompania
      And    a.fecha_ingreso <= @v_fecha_termino
      And    a.sit_trabajador = 1
      Order  By 3, 4, 5, 6, 1;

   Begin
      Open C_Detalle
      While @@Fetch_status < 1
      Begin
         Fetch C_Detalle Into @v_trabajador,  @v_nombre,     @v_datoRegion, @v_datoDepart,
                              @v_datoZona,    @v_datoCiudad;
         If @@Fetch_status <> 0
            Begin
               Break
            End

         If @v_datoRegion = Char(32) Or
            @v_datoDepart = Char(32) Or
            @v_datoZona   = Char(32)
            Begin
               goto Proximo
            End

         Begin
            Select @v_pvpLitro = precio
            From   dbo.Ls_HistPrecioGasolinaTbl
            Where  compania = @PsCompania
            And    Anio     = @PnAnio
            And    Mes      = @PnMes
            And    ciudad   = @v_datoCiudad;
            If @@Rowcount = 0
               Begin
                  Set @v_pvpLitro = 0
               End
         End;

         Select @v_diasFalta = 0,
                @v_impFalta  = 0,
                @v_diasIncap = 0,
                @v_impIncap  = 0,
                @v_totalDias = 30;

         Select @v_nss           = dbo.fn_obten_nss(@v_trabajador),
                @v_descCiudad    = dbo.fn_desc_agrup_dato(@v_ciudad, @v_datoCiudad),
                @v_cantLitros    = dbo.fn_obten_valor_vartrab(@PsCompania,   @v_Trabajador,   @v_var_trab_gas),
                @v_tarjeta       = dbo.fn_obten_valor_vartrab(@PsCompania,   @v_Trabajador,   @v_var_trab_tar),
                @v_diasFalta     = dbo.fn_obten_faltasGasolina( @PsCompania, @PnAnio,         @PnMes, @v_trabajador),
                @v_impGasMes     = @v_pvpLitro * @v_cantLitros

         If @v_cantLitros = 0
            Begin
               goto Proximo
            End

         Set @v_linea = @v_linea + 1;

         If @v_datoRegion != Isnull(@v_datoAntRegion, 'X1') Or
            @v_datoDepart != Isnull(@v_datoAntDepart, 'X1') Or
            @v_datoZona   != Isnull(@v_datoAntZona,   'X1')
            Begin
               Select @v_descRegion    = dbo.fn_desc_agrup_dato(@v_region,       @v_datoRegion),
                      @v_descDepart    = dbo.fn_desc_agrup_dato(@v_departamento, @v_datoDepart),
                      @v_descZona      = dbo.fn_desc_agrup_dato(@v_zona,         @v_datoZona),
                      @v_datoAntRegion = @v_datoRegion,
                      @v_datoAntDepart = @v_datoDepart,
                      @v_datoAntZona   = @v_datoZona;

               If @v_linea > 1
                  Begin
                     Insert Into dbo.Ls_RepPrecioGasolinaTbl
                    (compania,   anio,       mes,       region,     departamento,
                     zona,       ciudad,     secuencia, trabajador, nombre,
                     descRegion, descDepart, desczona,  descCiudad, tipoLinea,
                     usuario)
                     Select @PsCompania,   @PnAnio,       @PnMes,      @v_datoRegion, @v_datoDepart,
                            @v_datoZona,   @v_datoCiudad, @v_linea,    Char(32),      Char(32),
                            Char(32),      Char(32),      Char(32),    Char(32), 'S',
                            @PsUsuario
                     Set @v_linea = @v_linea + 1;

                  End

               Insert Into dbo.Ls_RepPrecioGasolinaTbl
              (compania,   anio,       mes,       region,     departamento,
               zona,       ciudad,     secuencia, trabajador, nombre,
               descRegion, descDepart, desczona,  tipoLinea,
               usuario)
               Select @PsCompania,   @PnAnio,       @PnMes,      @v_datoRegion, @v_datoDepart,
                      @v_datoZona,   @v_datoCiudad, @v_linea,    Char(32),      RTrim(@v_datoDepart) + ' - ' + @v_descDepart,
                      @v_descRegion, @v_descDepart, @v_descZona, 'T',
                      @PsUsuario
               Set @v_linea = @v_linea + 1;
            End

--
-- Linea de detalle.
--

         If @v_diasFalta > 30
            Begin
               Set @v_diasFalta = 30
            End

         If @v_diasFalta > 0
            Begin
               Select @v_totalDias = @v_totalDias - @v_diasFalta,
                      @v_impFalta  = (@v_impGasMes / 30) * @v_diasFalta;
            End

         Set @v_totalMes = @v_impGasMes - Isnull(@v_impFalta, 0);

         If @v_tarjeta = '0.000000'
            Begin
               Set @v_tarjeta = Char(32);
            End
         Else
            Begin
               Set @v_tarjeta = Substring(Replace(@v_tarjeta, '.', ''), 1, 16)
            End

         If dbo.fn_flagfaltasIncapacidad(@PsCompania, @PnAnio, @PnMes, @v_trabajador) = '0-1'
            Begin
               Select @v_diasIncap = dbo.fn_obten_faltasIncapacidad(@PsCompania, @PnAnio, @PnMes, @v_trabajador, 1),
                      @v_impIncap  = (@v_impGasMes / 30) * Isnull(@v_diasIncap, 0);
            End
         Else
            Begin
               Select @v_diasIncap = 0,
                      @v_impIncap  = 0;
            End

         Select @v_totalMes   = @v_totalMes   + Isnull(@v_impIncap , 0),
                @v_totalDias  = @v_totalDias  + @v_diasIncap,
                @v_valTrabExc = dbo.fn_obten_valor_vartrab(@PsCompania, @v_trabajador, @v_var_trab_exc)

         Insert Into dbo.Ls_RepPrecioGasolinaTbl
        (compania,  anio,       mes,        region,     departamento,
         zona,      ciudad,     secuencia,  trabajador, nombre,
         Nss,       descCiudad, cantLitros, pvpLitro,   impGasMes,
         diasFalta, impFalta,   diasIncap,  impIncap,
         totalDias, totalMes,   netoMes,    tarjeta,    usuario)
         Select @PsCompania, @PnAnio,       @PnMes,        @v_datoRegion, @v_datoDepart,
                @v_datoZona, @v_datoCiudad, @v_linea,      @v_trabajador, @v_nombre,
                @v_nss,      @v_descCiudad,
                Convert(Varchar(20), Cast(@v_cantLitros As Money), 1),
                Convert(Varchar(20), Cast(@v_pvpLitro   As Money), 1) pvpLitro,
                Convert(Varchar(20), Cast(@v_impGasMes  As Money), 1) impGasMes,
                Case When Isnull(@v_diasFalta,  0) = 0
                     Then Char(32)
                     Else Replace(Convert(Varchar(20), Cast(@v_diasFalta As Money), 1), '.00', '')
                End  diasFalta,
                Case When Isnull(@v_impFalta,  0) = 0
                     Then Char(32)
                     Else Convert(Varchar(20), Cast(@v_impFalta As Money), 1)
                End  impFalta,
                Case When @v_diasIncap = 0
                     Then Char(32)
                     Else Replace(Convert(Varchar(20), Cast(@v_diasIncap As Money), 1), '.00','')
                End  diasIncap,
                Case When @v_impIncap = 0
                     Then Char(32)
                     Else Convert(Varchar(20), Cast(@v_impIncap As Money), 1)
                End impIncap,
                Replace(Convert(Varchar(20), Cast(Isnull(@v_totalDias, 0) As Money), 1), '.00', '') totalDias,
                Convert(Varchar(20), Cast(Isnull(@v_totalMes,  0) As Money), 1) totalMes,
                Case When Substring(dbo.fn_flagfaltasIncapacidad(@PsCompania, @PnAnio, @PnMes, @v_trabajador), 1, 1) = '1'
                     Then 'INCAPACITADO'
                     When  @v_valTrabExc > 0
                     Then 'EXENTO'
                     Else Convert(Varchar(20), Cast(Isnull(Round(@v_totalMes, 0, 0), 0)As Money), 1)
                End  netoMes,
                @v_tarjeta tarjeta,
                @PsUsuario
Proximo:

      End
      Close      C_Detalle
      Deallocate C_Detalle
   End

   Begin Try
      Update dbo.Ls_FaltasIncapacidadTbl
      Set    descRegion = dbo.fn_desc_agrup_dato(@v_region, b.Region),
             depZona    = Rtrim(b.departamento) + ' - ' + Rtrim(b.Zona),
             descCiudad = b.descCiudad,
             nombre     = b.nombre
      From   dbo.Ls_FaltasIncapacidadTbl a
      Join   dbo.Ls_RepPrecioGasolinaTbl b
      On     b.compania   = a.compania
      And    b.anio       = a.anio
      And    b.mes        = a.mes
      And    b.trabajador = a.trabajador
      Where  a.compania = @PsCompania
      And    a.Anio     = @PnAnio
      And    a.Mes      = @PnMes;
   End Try

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

   Begin Try
      Update dbo.Ls_FaltasGasolinaTbl
      Set    descRegion = dbo.fn_desc_agrup_dato(@v_region, b.Region),
             depZona    = Rtrim(b.departamento) + ' - ' + Rtrim(b.Zona),
             descCiudad = b.descCiudad,
             nombre     = b.nombre
      From   dbo.Ls_FaltasGasolinaTbl    a
      Join   dbo.Ls_RepPrecioGasolinaTbl b
      On     b.compania   = a.compania
      And    b.anio       = a.anio
      And    b.mes        = a.mes
      And    b.trabajador = a.trabajador
      Where  a.compania   = @PsCompania
      And    a.Anio       = @PnAnio
      And    a.Mes        = @PnMes
      And    b.tipoLinea  = 'D';
   End Try

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

   If @PnImprime = 1
      Begin
         Select descRegion region, Case When tipoLinea = 'T'
                                        Then Rtrim(departamento) + ' - ' + Rtrim(Zona)
                                        Else Char(32)
                                   End 'DEPTO Y ZONA',
                trabajador, nombre, NSS,   descCiudad Ciudad,
                pvpLitro 'PVP Ciudad',   cantLitros Litros,    impGasMes 'GAS. MENS.',
                diasFalta 'Dias Falta', impFalta 'Importe Faltas',
                diasIncap 'Dias Incap', impIncap 'Importe Incap',
                totalDias, totalMes,   netoMes,    tarjeta
         From   dbo.Ls_RepPrecioGasolinaTbl
         Where  compania = @PsCompania
         And    Anio     = @PnAnio
         And    Mes      = @PnMes;
      End;

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go


Grant  Execute On spp_Ls_RepPrecioGasolinaTbl to Public;
