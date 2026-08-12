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
   Execute dbo.spp_Ls_RepFaltasIncapacidadTbl @PsCompania  = @PsCompania,
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
           Where  Name = 'spp_Ls_RepFaltasIncapacidadTbl')
   Begin
      Drop Procedure dbo.spp_Ls_RepFaltasIncapacidadTbl;
   End
Go

Create Procedure dbo.spp_Ls_RepFaltasIncapacidadTbl
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
   @v_linea                 Integer,
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250),
--
   @v_secuencia             Integer,
   @v_sec                   Integer,
   @v_diasPer               Integer,
   @v_totDias               Integer,
   @v_trabajador            Char(10),
   @v_ciclo                 Varchar(  7),
   @v_dias                  Varchar(100),
   @v_diasDet               Varchar( 15),
   @v_mesDesc               Varchar(  3),
   @v_fecha_inicio          Date,
   @v_fecha_termino         Date,
   @v_fechaProcIni          Date,
   @v_fechaProcFin          Date,
   @v_mes                   Tinyint,
   @v_anio                  Smallint;

Begin

-- ********************************************************************************************************************************
--
-- Nombre físico :   spp_Ls_RepFaltasIncapacidadTbl
-- Autor:            Pedro Zambrano
-- Fecha:            10-ago-2026.
-- Objetivo:         Procedimiento de Calculo del Reporte de Incapacidades Aplicables a la Asignación de Gasolina a Trabajadores
-- Versión:          1
--
-- ********************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32),
          @v_linea         = 0,
          @v_sec           = 0,
          @v_totDias       = 0,
          @v_fecha_inicio  = Convert(Date, '01/' + Convert(Char(2), @PnMes) + '/' +
                                                   Convert(Char(4), @PnAnio), 103),
          @v_fecha_termino = dbo.fn_obten_FinMes(@v_fecha_inicio);

   Set @v_fecha_inicio = DateAdd(Month, -7, @v_fecha_inicio);

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

   Execute dbo.spd_Ls_RepFaltasIncapacidadTbl @PsCompania  = @PsCompania,
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

--

   If Object_id('tempdb..#TempRepFaltas') Is Not Null
      Begin
         Drop Table #TempRepFaltas
      End

   Create Table #TempRepFaltas
  (secuencia    Integer      Not Null Identity (1, 1),
   trabajador   Char(10)     Not Null,
   ciclo        Varchar( 8)  Not Null,
   dias         Integer      Not Null,
   Constraint TempRepFaltasPk
   Primary Key (secuencia));

   Insert Into #TempRepFaltas
  (trabajador,  ciclo, dias)
   Select trabajador,   Cast(DatePart(yyyy, a.fecha_incidencia) As Varchar) + '-' + Substring(Convert(Char(10), a.fecha_incidencia, 103), 4, 2),
          Sum(dias)
   From   dbo.Ls_faltasIncapacidadTbl a
   Where  compania               = @PsCompania
   And    anio                   = @PnAnio
   And    mes                    = @PnMes
   And    fecha_incidencia Between @v_fecha_inicio And @v_fecha_termino
   Group  By trabajador, Cast(DatePart(yyyy, a.fecha_incidencia) As Varchar) + '-' + Substring(Convert(Char(10), a.fecha_incidencia, 103), 4, 2)
   Order  by trabajador, 2;
   Set @v_secuencia = @@Identity;

   While @v_secuencia > @v_sec
   Begin
      Set @v_sec = @v_sec + 1

      Select @v_trabajador = trabajador,
             @v_ciclo      = ciclo,
             @v_diasPer    = dias
      From   #TempRepFaltas
      Where  secuencia     = @v_sec;
      If @@Rowcount = 0
         Begin
            Select @v_sec
            Break
         End;

      Select @v_anio = Cast(Substring(@v_ciclo, 1, 4) As Smallint),
             @v_mes  = Cast(Substring(@v_ciclo, 6, 2) As Tinyint);

      Select top 1 @v_mesDesc = Substring(Rtrim(descripcion), 1, 3)
      From   dbo.criterios_valores
      Where  campo = 'meses'
      And    item  = @v_mes;
      If  @@Rowcount = 0
          Begin
             Set @v_mesDesc = Char(32);
          End;

      Begin Try
         Select @v_fechaProcIni =  Convert(Date, '01/' + Convert(Char(2), @v_mes) + '/' +
                                                         Convert(Char(4), @v_anio), 103),
                @v_fechaProcFin = dbo.fn_obten_FinMes(@v_fechaProcIni);

      End Try

      Begin Catch
         Select  @v_Error      = @@Error,
                 @v_desc_error = Substring (Error_Message(), 1, 230),
                 @v_linea      = Error_line();
      End   Catch

      If IsNull(@v_Error, 0) <> 0
         Begin
            Select @PnEstatus = @v_error,
                   @PsMensaje = 'Error.: ' +  Cast(@v_error As Varchar) + ': ' + @v_desc_error + ' En Linea: ' + Cast(@v_linea As Varchar);
            Goto Salida;

        End;

     Set @v_diasDet = Cast(Isnull(@v_diasPer, 0) As Varchar) + ' ' + Rtrim(@v_mesDesc);

     If Not Exists (Select Top 1 1
                    From   dbo.Ls_RepFaltasIncapacidadTbl
                    Where  compania               = @PsCompania
                    And    anio                   = @PnAnio
                    And    mes                    = @PnMes
                    And    trabajador             = @v_trabajador)
        Begin
           Begin Try
              Insert Into dbo.Ls_RepFaltasIncapacidadTbl
             (compania,    anio,        mes, trabajador,  fechaInicio, fechaTermino,
              dias,        diasDet,     usuario)
              Select compania,    @PnAnio,     @PnMes,  trabajador,  Min(fecha_incidencia)  fechaInicio,
                     Convert(Char(10), Max(fecha_incidencia), 103) fechaTermino,
                     @v_diasPer,       Ltrim(Rtrim(@v_diasDet)),              @PsUsuario
              From   dbo.Ls_faltasIncapacidadTbl a
              Where  compania               = @PsCompania
              And    anio                   = @PnAnio
              And    mes                    = @PnMes
              And    trabajador             = @v_trabajador
              And    fecha_incidencia Between @v_fecha_inicio And @v_fecha_termino
              Group  By compania,    descRegion,   depZona, descCiudad,
                         trabajador,  nombre;
           End Try

           Begin Catch
              Select  @v_Error      = @@Error,
                      @v_desc_error = Substring (Error_Message(), 1, 230),
                      @v_linea      = Error_line();
           End   Catch

           If IsNull(@v_Error, 0) <> 0
              Begin
                 Select @PnEstatus = @v_error,
                        @PsMensaje = 'Error.: ' +  Cast(@v_error As Varchar) + ': ' + @v_desc_error + ' En Linea: ' + Cast(@v_linea As Varchar);
                 Goto Salida;

             End;
        End

     Else
        Begin
           Begin Try
              Update  dbo.Ls_RepFaltasIncapacidadTbl
              Set     dias    = dias + @v_diasPer,
                      diasDet = Ltrim(diasDet) + '+' +  Ltrim(Rtrim(@v_diasDet))
              Where   compania               = @PsCompania
              And     anio                   = @PnAnio
              And     mes                    = @PnMes
              And     trabajador             = @v_trabajador;
           End Try

           Begin Catch
              Select  @v_Error      = @@Error,
                      @v_desc_error = Substring (Error_Message(), 1, 230),
                      @v_linea      = Error_line();
           End   Catch

           If IsNull(@v_Error, 0) <> 0
              Begin
                 Select @PnEstatus = @v_error,
                        @PsMensaje = 'Error.: ' +  Cast(@v_error As Varchar) + ': ' + @v_desc_error + ' En Linea: ' + Cast(@v_linea As Varchar);
                 Goto Salida;

             End;
        End;

   End

--

    Update  dbo.Ls_RepFaltasIncapacidadTbl
    Set     Observaciones = 'PAGAR EN: ' + Replace(Convert(Varchar(5), Cast(@PnMes  As Money), 1), '.00', '')  + '-' +
                                           Replace(Convert(Varchar(8), Cast(@PnAnio As Money), 1), '.00', '')
    Where   compania               = @PsCompania
    And     anio                   = @PnAnio
    And     mes                    = @PnMes
    And     Substring(dbo.fn_flagfaltasIncapacidad(Compania, Anio, Mes, trabajador), 1, 1) = '0'

   If @PnImprime = 1
      Begin
         Select trabajador, Convert(Char(1), fechaInicio, 103) fechaInicio, fechaTermino, diasDet, dias,
                dbo.fn_flagfaltasIncapacidad(@PsCompania, @PnAnio, @PnMes, trabajador) Flag
         From   dbo.Ls_RepFaltasIncapacidadTbl
         Where  compania = @PsCompania
         And    Anio     = @PnAnio
         And    Mes      = @PnMes;
      End;

Salida:

   If Object_id('tempdb..#TempRepFaltas') Is Not Null
      Begin
         Drop Table #TempRepFaltas
      End

   Set Xact_Abort    Off
   Return;

End;
Go

Grant  Execute On spp_Ls_RepFaltasIncapacidadTbl to Public;

--
-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Procedimiento de Calculo del Reporte de Incapacidades Aplicables a la Asignación de Gasolina a Trabajadores.',
   @w_procedimiento  NVarchar(250) = 'spp_Ls_RepFaltasIncapacidadTbl';

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
