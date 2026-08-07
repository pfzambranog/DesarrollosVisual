-- Use Adam
--Go

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
   Execute dbo.spp_Ls_FaltasIncapacidadTbl @PsCompania  = @PsCompania,
                                           @PnAnio      = @PnAnio,
                                           @PnMes       = @PnMes,
                                           @PsUsuario   = @PsUsuario,
                                           @PsOperacion = @PsOperacion,
                                           @PnImprime   = @PnImprime,
                                           @PnEstatus   = @PnEstatus Output,
                                           @PsMensaje   = @PsMensaje Output;

   Select @PnEstatus As Error, @PsMensaje As MensajeError

   Return;

End;
Go

*/

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'P'
            And    Name = 'spp_Ls_FaltasIncapacidadTbl')
   Begin
      Drop Procedure dbo.spp_Ls_FaltasIncapacidadTbl
   End
Go

Create Procedure dbo.spp_Ls_FaltasIncapacidadTbl
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
   @v_incap                 Char(10),
   @v_fecha_inicio          Date,
   @v_fecha_termino         Date,
   @v_fecha_inicio_kp       Date,
   @v_fecha_termino_kp      Date,
   @v_x                     Bit,
   @v_registros             Integer;

--

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spp_Ls_FaltasIncapacidadTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de alta de Registros por Falta en la tabla Ls_FaltasIncapacidadTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus          = 0,
          @PsMensaje          = Char(32),
          @v_registros        = 0,
          @v_x                = 0,
          @v_fecha_inicio     = Convert(Date, '01/' + Convert(Char(2),  @PnMes) + '/' +
                                                      Convert(Char(4), @PnAnio), 103),
          @v_fecha_termino    = dbo.fn_obten_FinMes(@v_fecha_inicio),
          @v_fecha_inicio_kp  = Dateadd(month, -1, @v_fecha_inicio);

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
                                          ' No existen Precios de Gasolina para el AÑO seleccionado: ' + @PnAnio,
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End


--
-- Búsqueda de codigo de Agrupación Conceptos Faltas
--

   Select @v_incap = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrincap'
   And  item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 25005,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Agr Conceptos por Faltas "agrincap" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.rel_conceptos_agr
                  Where  compania      = @PsCompania
                  And    agr_conceptos = @v_incap)
      Begin
         Select @PnEstatus    = 250006,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' Código de Agrupacion de Conceptos  "' + @v_incap + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Creación de Tablas Temporales
--

   Create Table #TempTrab
  (trabajador   Char(10) Not Null Primary Key,
   idEstatus    bit      Not Null Default 1)

--

   Execute dbo.spd_Ls_FaltasIncapacidadTbl @PsCompania  = @PsCompania,
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

  Begin Try
     Insert Into dbo.Ls_FaltasIncapacidadTbl
    (compania,    anio,             mes,        trabajador, incidencia_kp,
     concepto_ns, fecha_Incidencia, secuencia,  dias,       usuario)
     Select a.compania,        @PnAnio,            @PnMes,      a.trabajador,  a.incidencia_kp,
            b.concepto_genera, a.fecha_incidencia, a.secuencia, a.variable_01, @PsUsuario
     From   dbo.incidencias_KP a
     Join   dbo.incidencias_kp_def b
     On     b.compania      = a.compania
     And    b.incidencia_kp = a.incidencia_kp
     Join   dbo.rel_conceptos_agr c
     On     c.compania      = b.compania
     And    c.agr_conceptos = @v_incap
     And    c.concepto      = b.concepto_genera
     Where  a.compania      = @PsCompania
     And    a.fecha_incidencia Between @v_fecha_inicio_kp And @v_fecha_termino;

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

   Insert Into #TempTrab
   (trabajador)
   Select Distinct trabajador
   From   dbo.Ls_FaltasIncapacidadTbl
   Where  compania   = @PsCompania
   And    anio       = @PnAnio
   And    Mes        = @PnMes;

  While @v_x = 0
  Begin
     Select @v_fecha_inicio_kp  = DateAdd(Month, -1, @v_fecha_inicio_kp),
            @v_fecha_termino_kp = dbo.fn_obten_FinMes(@v_fecha_inicio_kp);

     Insert Into dbo.Ls_FaltasIncapacidadTbl
    (compania,    anio,             mes,        trabajador, incidencia_kp,
     concepto_ns, fecha_Incidencia, secuencia,  dias,       usuario)
     Select a.compania,        @PnAnio,            @PnMes,      a.trabajador,  a.incidencia_kp,
            b.concepto_genera, a.fecha_incidencia, a.secuencia, a.variable_01, @PsUsuario
     From   dbo.incidencias_KP a
     Join   dbo.incidencias_kp_def b
     On     b.compania               = a.compania
     And    b.incidencia_kp          = a.incidencia_kp
     Join   dbo.rel_conceptos_agr c
     On     c.compania               = b.compania
     And    c.agr_conceptos          = @v_incap
     And    c.concepto               = b.concepto_genera
     Join   #TempTrab d
     On     d.trabajador             = a.trabajador
     And    d.idEstatus              = 1
     Where a.compania               = @PsCompania
     And    a.fecha_incidencia Between @v_fecha_inicio_kp And @v_fecha_termino_kp;
     Set @v_registros = @@Rowcount

     If @v_registros = 0
        Begin
           Set @v_x = 1

           Update #TempTrab
           Set    idEstatus = 0
           From   #TempTrab a
           Where  idEstatus = 1;

           goto siguiente;
        End

    Update #TempTrab
    Set    idEstatus = 0
    From   #TempTrab a
    Where  idEstatus = 1
    And    Not Exists ( Select top 1 1
                        From   dbo.Ls_FaltasIncapacidadTbl
                        Where  compania                 = @PsCompania
                        And    anio                     = @PnAnio
                        And    Mes                      = @PnMes
                        And    trabajador               = a.trabajador
                        And    fecha_incidencia   Between @v_fecha_inicio And @v_fecha_termino)
siguiente:

  End

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go

