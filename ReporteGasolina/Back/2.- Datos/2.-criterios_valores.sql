-- Use adam
-- Go

--
-- Objetivo.: Generación de Criterios de filtros para las agrupaciones de trabajadores y Conceptos.
-- Fecha:     22/07/2026
-- Version:   1
-- Programador: Pedro Zambrano.
--

Declare
   @v_campo        Char(20),
   @v_agrupacion   Char(10);

Begin
--
-- Agrupación Ciudad.
--

   Select @v_campo       = 'agrciudad',
          @v_agrupacion  = 'ciu';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- Agrupación Region.
--

   Select @v_campo      = 'agrregion',
          @v_agrupacion = 'REG';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- Agrupación Departamento.
--

   Select @v_campo      = 'agrdep',
          @v_agrupacion = 'DEPART';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- Agrupación Zona.
--

   Select @v_campo      = 'agrzona',
          @v_agrupacion = 'ZONA';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- Agrupación de conceptos de Faltas.
--

   Select @v_campo      = 'agrfalta',
          @v_agrupacion = 'DLC';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- Agrupación de conceptos de Incapacidades.
--

   Select @v_campo      = 'agrincap',
          @v_agrupacion = 'INC';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- variables trabajador Litros gasolina.
--

   Select @v_campo      = 'ltrgas',
          @v_agrupacion = 'var_tra_20';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- variables trabajador Excentos pago gasolina.
--

   Select @v_campo      = 'excgas',
          @v_agrupacion = 'var_tra_31';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- variable trabajador para deposito pago gasolina.
--

   Select @v_campo      = 'pagogas',
          @v_agrupacion = 'rel_trab_ins_dep';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--

   Select @v_campo      = 'insdepgas',
          @v_agrupacion = '98';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- Número de Tarjeta de Pago Asignación Gasolina
--

   Select @v_campo      = 'tarjetagas',
          @v_agrupacion = 'var_tra_21';

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, @v_agrupacion)

--
-- Directorio Ubicación del reporte de salida
--

   Set @v_campo      = 'dirsalgas'

   If Exists (Select top 1 1
              From   dbo.criterios_valores
              Where  campo = @v_campo)
      Begin
         Delete dbo.criterios_valores
         Where  campo = @v_campo;
      End

   Insert Into dbo.criterios_valores
   (campo, item, descripcion)
   Values (@v_campo, 1, 'C:\TempAdam\');

   
   Return

End;
Go