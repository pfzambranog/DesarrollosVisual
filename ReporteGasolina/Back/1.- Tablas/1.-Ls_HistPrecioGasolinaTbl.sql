Use adam
Go

If Exists ( Select	Top 1 1
			From	Sysobjects
			Where	Uid = 1
			And		Type = 'U'
			And		Name = 'Ls_HistPrecioGasolinaTbl')
	Begin
	   Drop table dbo.Ls_HistPrecioGasolinaTbl;
	End
Go

Create table dbo.Ls_HistPrecioGasolinaTbl
(compania	 Char(4)	   Not Null,
 anio		 Smallint	   Not Null,
 mes		 Tinyint	   Not Null,
 ciudad		 Char(10)	   Not Null,
 precio		 Decimal(19,2) Not Null,
 usuario	 Varchar(30)   Not Null,
 fechaAct	 Datetime	   Not Null Default Getdate(),
 Constraint Ls_HistPrecioGasolinaPK
Primary Key (compania, anio, mes, ciudad),
Constraint Ls_HistPrecioGasolinaFk01
Foreign Key (compania)
References dbo.companias (compania) On Delete Cascade)
Go

Grant Select, Insert, Update, Delete, References On Ls_HistPrecioGasolinaTbl To Public
Go

--
-- Comentarios
--

Declare
   @v_existe	Bit		= 0,
   @v_table		Sysname = 'Ls_HistPrecioGasolinaTbl';

Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Tabla Histórica de Precios de Gasolina por Región',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table;

Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Código Único de Compañía',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table,
								@level2type = N'column',
								@level2name = 'compania'


Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Año de Proceso',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table,
								@level2type = N'column',
								@level2name = 'anio';

Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Mes de Proceso',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table,
								@level2type = N'column',
								@level2name = 'mes';

Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Código de Ciudad',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table,
								@level2type = N'column',
								@level2name = 'ciudad';

Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Precio de la gasolina en el período para la Ciudad',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table,
								@level2type = N'column',
								@level2name = 'precio';

Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Ult. Codigo Usuario que Actualizo el Registro',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table,
								@level2type = N'column',
								@level2name = 'usuario';

Execute sp_addExtendedproperty	@name		= N'Ms_descripcion',
								@value		= N'Ult. Fecha de Actualización del Registro',
								@level0type = N'Schema',
								@level0name = N'dbo',
								@level1type = N'Table',
								@level1name = @v_table,
								@level2type = N'column',
								@level2name = 'fechaAct';
