-- Use Adam
--Go

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'Fn'
            And    Name = 'fn_obten_faltasIncapacidad')
   Begin
      Drop Function dbo.fn_obten_faltasIncapacidad
   End
Go

Create Function dbo.fn_obten_faltasIncapacidad
  (@PsCompania     Char( 3),
   @PnAnio         Integer,
   @PnMes          Integer,
   @PsTrabajador   Char(10),
   @PnAcum         Integer)   -- 0 = Acumulado del Mes, 1 = Acumulado Antes del mes de Proceso.
Returns Integer
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   fn_obten_faltasIncapacidad
-- Autor:            Pedro Zambrano
-- Fecha:            23-jul-2026.
-- Objetivo:         Funcion que consulta La Cantidad de Faltas por incapacidad de un trabajador en un periodo.
-- Versión:          1
--
-- ***************************************************************************************************************************


   Declare
      @v_resultado             Integer,
      @v_fecha_inicio          Date,
      @v_fecha_termino         Date;
   Begin
      Select @v_fecha_inicio  = Convert(Date, '01/' + Cast(@PnMes  As Varchar) + '/' +
                                                      Cast(@PnAnio As Varchar), 103),
             @v_fecha_termino = dbo.fn_obten_FinMes(@v_fecha_inicio);   

      If @PnAcum = 1
         Begin
            Select @v_fecha_inicio  = DateAdd(Month, -1, @v_fecha_inicio),
                   @v_fecha_termino = dbo.fn_obten_FinMes(@v_fecha_inicio),
                   @v_fecha_inicio  = DateAdd(Month, -5, @v_fecha_inicio);   
         End;
         
      Select @v_resultado = Sum(dias)
      From   dbo.Ls_faltasIncapacidadTbl
      Where  compania               = @PsCompania
      And    anio                   = @PnAnio
      And    mes                    = @PnMes
      And    trabajador             = @PsTrabajador
      And    fecha_incidencia Between @v_fecha_inicio And @v_fecha_termino;

      Set @v_resultado = Isnull(@v_resultado, 0);
   End

   Return (@v_resultado);

End;
Go
