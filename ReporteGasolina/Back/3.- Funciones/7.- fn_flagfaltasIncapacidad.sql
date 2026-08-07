-- Use Adam
--Go

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'Fn'
            And    Name = 'fn_flagfaltasIncapacidad')
   Begin
      Drop Function dbo.fn_flagfaltasIncapacidad
   End
Go

Create Function dbo.fn_flagfaltasIncapacidad
  (@PsCompania     Char( 3),
   @PnAnio         Integer,
   @PnMes          Integer,
   @PsTrabajador   Char(10))
Returns Char(3)
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   fn_flagfaltasIncapacidad
-- Autor:            Pedro Zambrano
-- Fecha:            27-jul-2026.
-- Objetivo:         Funcion que indica si hay incapacidades en el periodo actual y en el anterior
-- Versión:          1
--
-- ***************************************************************************************************************************


   Declare
      @v_resultado             Char(3),
      @v_fecha_inicio          Date,
      @v_fecha_termino         Date;

   Begin
      Select @v_fecha_inicio  = Convert(Date, '01/' + Convert(Char(2), @PnMes) + '/' +
                                                      Convert(Char(4), @PnAnio), 103),
             @v_fecha_termino = dbo.fn_obten_FinMes(@v_fecha_inicio),
              @v_resultado    = '0-';

      If Exists (Select Top 1 1
                 From   dbo.Ls_faltasIncapacidadTbl
                 Where  compania               = @PsCompania
                 And    anio                   = @PnAnio
                 And    mes                    = @PnMes
                 And    trabajador             = @PsTrabajador
                 And    fecha_incidencia Between @v_fecha_inicio And @v_fecha_termino)
         Begin
            Set @v_resultado = '1-'
         End;
         
      Select @v_fecha_inicio  = DateAdd(Month, -1, @v_fecha_inicio ),
             @v_fecha_termino = dbo.fn_obten_FinMes(@v_fecha_inicio);

      If Exists (Select Top 1 1
                 From   dbo.Ls_faltasIncapacidadTbl
                 Where  compania               = @PsCompania
                 And    anio                   = @PnAnio
                 And    mes                    = @PnMes
                 And    trabajador             = @PsTrabajador
                 And    fecha_incidencia Between @v_fecha_inicio And @v_fecha_termino)
         Begin
            Set @v_resultado = Rtrim(@v_resultado) + '1';
         End;
     Else
         Begin
            Set @v_resultado = Rtrim( @v_resultado) + '0';
         End;

   End;

   Return (@v_resultado);

End;
Go
