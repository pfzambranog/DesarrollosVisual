-- Use Adam
--Go

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'Fn'
            And    Name = 'fn_obten_valor_vartrab')
   Begin
      Drop Function dbo.fn_obten_valor_vartrab
   End
Go

Create Function dbo.fn_obten_valor_vartrab
  (@PsCompania     Char( 3),
   @PsTrabajador   Char(10),
   @PsVartrab      Char(10))
Returns Decimal(19, 6)
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   fn_obten_valor_vartrab
-- Autor:            Pedro Zambrano
-- Fecha:            23-jul-2026.
-- Objetivo:         Funcion que consulta el valor de la variable del trabajador
-- Versión:          1
--
-- ***************************************************************************************************************************


   Declare
      @v_resultado   Decimal(19, 6),
      @v_secuencia   Integer;

   Begin
      Set @v_secuencia = Substring(@PsVartrab, 9, 2)

      Select @v_resultado = Isnull(variable_trabajador, 0)
      From   dbo.variables_trabajador
      Where  compania   = @PsCompania
      And    trabajador = @PsTrabajador
      And    secuencia  = @v_secuencia;
      If @@Rowcount = 0
         Begin
            Set @v_resultado = 0;
         End;
   End

   Return (@v_resultado);

End;
Go
