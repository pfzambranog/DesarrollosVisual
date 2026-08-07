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
   Execute dbo.spp_Ls_FaltasGasolinaTbl @PsCompania  = @PsCompania,
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

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spp_Ls_FaltasGasolinaTbl')
   Begin
      Drop Procedure dbo.spp_Ls_FaltasGasolinaTbl;
   End
Go

Create Procedure dbo.spp_Ls_FaltasGasolinaTbl
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
   @v_faltas                Char(10),
   @v_fecha_inicio          Date,
   @v_fecha_termino         Date;
--

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spp_Ls_FaltasGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de alta de Registros por Falta en la tabla Ls_FaltasGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32),
          @v_fecha_inicio  = Dateadd(month, -1,
                                     Convert(Date, '01/' + Cast(@PnMes  As Varchar) + '/' +
                                                           Cast(@PnAnio As Varchar), 103)),
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
                                          ' No existen Precios de Gasolina para el AÑO seleccionado: ' + @PnAnio,
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
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' Código de Agrupacion de Conceptos  "' + @v_faltas + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--

   Execute dbo.spd_Ls_FaltasGasolinaTbl @PsCompania  = @PsCompania,
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
     Insert Into Ls_FaltasGasolinaTbl
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
     And    c.agr_conceptos = @v_faltas
     And    c.concepto      = b.concepto_genera
     where  a.compania      = @PsCompania
     And    a.fecha_incidencia Between @v_fecha_inicio And @v_fecha_termino;

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


Grant  Execute On spp_Ls_FaltasGasolinaTbl to Public;
