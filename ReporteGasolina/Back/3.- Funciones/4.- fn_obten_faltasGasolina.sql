-- Use Adam
--Go

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'Fn'
            And    Name = 'fn_obten_faltasGasolina')
   Begin
      Drop Function dbo.fn_obten_faltasGasolina
   End
Go

Create Function dbo.fn_obten_faltasGasolina
  (@PsCompania     Char( 3),
   @PnAnio         Integer,
   @PnMes          Integer,
   @PsTrabajador   Char(10))
Returns Integer
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   fn_obten_faltasGasolina
-- Autor:            Pedro Zambrano
-- Fecha:            23-jul-2026.
-- Objetivo:         Funcion que consulta La Cantidad de Faltas de un trabajador en un periodo.
-- Versión:          1
--
-- ***************************************************************************************************************************


   Declare
      @v_resultado   Integer;

   Begin
      Select @v_resultado = Sum(dias)
      From   dbo.Ls_FaltasGasolinaTbl
      Where  compania   = @PsCompania
      And    anio       = @PnAnio
      And    mes        = @PnMes
      And    trabajador = @PsTrabajador;
      Set @v_resultado = Isnull(@v_resultado, 0);
   End

   Return (@v_resultado);

End;
Go
