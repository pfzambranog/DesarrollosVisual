Use Adam
Go

--
-- Diparador:	  TrinsLs_HistPrecioGasolinaTbl
-- Objetivo:	  Disparador de Alta y Actualización relacionado a la Entidad Ls_HistPrecioGasolinaTbl
-- Fecha:		  25-Jul-2026
-- Version:		  1
--
-- Programador:	  Pedro Zambrano
--

If Exists ( Select Top 1 1
            From   sys.triggers
            Where  Name = 'TrinsLs_HistPrecioGasolinaTbl')
   Begin
      Drop Trigger dbo.TrinsLs_HistPrecioGasolinaTbl
   End
go

Create Trigger dbo.TrinsLs_HistPrecioGasolinaTbl
On	   dbo.Ls_HistPrecioGasolinaTbl
After  Insert, Update
As

Declare
   @v_compania			Char ( 4),
   @v_anio				Smallint,
   @v_mes				Tinyint,
   @v_ciudad			Char (10),
   @v_precio			Decimal(19,2),
   @v_usuario			Varchar(  30),
   @v_fechaAct			Datetime,
--
   @v_desc_error		Varchar(400),
   @v_agrupacion		Char(10);

Begin

   Select @v_compania  = compania,
		  @v_anio	   = anio,
		  @v_mes	   = mes,
		  @v_ciudad	   = ciudad,
		  @v_precio	   = precio,
		  @v_usuario   = usuario,
		  @v_fechaAct  = fechaAct
   From	  Inserted a;

--

-------------------------------------------------------
 -- Validar Seguridad
 -------------------------------------------------------

   Begin
	  If Not Exists (Select Top 1 1
					 From	master.dbo.usuario_base
					 Where	Usuario			= @v_Usuario )
		 Begin
			Set @v_desc_error = 'Error: El Usuario no esta Registrado como usuario ADAM';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 End

	  If Not Exists (Select Top 1 1
					 From	dbo.Aut_Operaciones
					 Where	Usuario			= @v_Usuario
					 And	Operacion		= 'FPLS001'
					 And	Nivel_seguridad > 1)
		 Begin
			Set @v_desc_error = 'Error: No tiene Autorizacion de Operacion';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return

		 End

	  If Not Exists (Select Top 1 1
					 From	dbo.Aut_Companias
					 Where	Usuario		   = @v_Usuario
					 And	Compania	   = @v_Compania)
		 Begin
			Set @v_desc_error = 'Error: No tiene Autorizacion para la compania';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 End

	  If IsNull(@v_Precio, 0) <= 0
		 Begin
			Select @v_desc_error = 'Error: El Precio de la Gasolina debe ser mayor a cero (0)';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 End

	  If IsNull(@v_Anio, 0) Not Between 2000 And 2050
		 Begin
			Set @v_desc_error = 'Error: El Parámetro Año de Proceso no es Valido. (2020-2050)';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 End

	  If IsNull(@v_Mes, 0) Not Between 1 And 12
		 Begin
			Set @v_desc_error = 'Error: El Parámetro Mes de Proceso no es Valido. (1-12)';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 End

--
-- Búsqueda de codigo de Agrupación
--

	  Select @v_agrupacion = Rtrim(descripcion)
	  From	 dbo.criterios_valores
	  Where	 campo = 'agrciudad'
	  And	 item  = 1;
	  If @@Rowcount = 0
		 Begin
			Set @v_desc_error = 'Error: El Parámetro de Ciudad (agrciudad) no existe en criterios_valores';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 End

	  If Not Exists (Select top 1 1
					 From	dbo.agrupaciones_trab
					 Where	agrupacion = @v_agrupacion)
		 Begin
			Set @v_desc_error = 'Error: El Código de Agrupacion de Ciudad no es Válido';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 
		 End

	  If Not Exists (Select top 1 1
					 From	dbo.datos_agr_trab
					 Where	agrupacion = @v_agrupacion
					 And	dato	   = @v_ciudad)
		 Begin
			Select @v_desc_error = 'Error: El Código de Ciudad seleccionado no es Válido,';

			Raiserror (@v_desc_error, 16, 1)
			Rollback Transaction
			Return
		 End

   End;

   Return

End
Go
