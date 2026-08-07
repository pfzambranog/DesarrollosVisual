-- Use Adam
--Go

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'Fn'
            And    Name = 'fn_obten_FinMes')
   Begin
      Drop Function dbo.fn_obten_FinMes
   End
Go

Create Function dbo.fn_obten_FinMes
  (@PdFecha     Date)
Returns Date
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   fn_obten_FinMes
-- Autor:            Pedro Zambrano
-- Fecha:            27-jul-2026.
-- Objetivo:         Funcion que Calcula la fecha de fin de mes
-- Versión:          1
--
-- ***************************************************************************************************************************


   Declare
      @v_fecha_termino         Date;

   Begin
      Select @v_fecha_termino  = DateAdd(dd, -1, DateAdd(mm, 1, 
                                 Convert(Date, '01/' + Cast(DatePart(mm,   @PdFecha) As Varchar) + '/' +
                                 Cast(DatePart(yyyy, @PdFecha) As Varchar), 103)));
   End;
   
   Return (@v_fecha_termino);

End;
Go

