Datos Generales.

Base de datos: Microsoft SQL Server 2008 R2 (RTM) - 10.50.1600.1 (X64)   Apr  2 2010 15:48:46   Copyright (c) Microsoft Corporation  Standard Edition (64-bit) on Windows NT 6.1 <X64> (Build 7601: Service Pack 1) 
Servidor: 192.168.1.24 Puerto 1433.
          Windows 7

Ambiente de desarrollo: Windows 11, Ip 192.168.1.20

Funcionalidad:

 Actualizar los precios de la Gasolina por año, mes y zona mediante la lectura de un archivo excel 
 Generar un reporte en excel de los precios de la Gasolina cargados en la base de datos.
 Generar un reporte con el detalle de la asignación mensual de la asignacion del importe de gasolina a empleados.
    

Este es el back desarrollado para el Reporte Control de pago de Gasolina.




Tablas

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

-- Use adam
-- Go

If Exists ( Select  Top 1 1
            From    Sysobjects
            Where   Uid = 1
            And     Type = 'U'
            And     Name = 'Ls_RepPrecioGasolinaTbl')
    Begin
       Drop table dbo.Ls_RepPrecioGasolinaTbl;
    End
Go

Create table dbo.Ls_RepPrecioGasolinaTbl
(compania     Char(4)        Not Null,
 anio         Smallint       Not Null,
 mes          Tinyint        Not Null,
 region       Char(10)       Not Null,
 departamento Char(10)       Not Null,
 zona         Char(10)       Not Null,
 ciudad       Char(10)       Not Null,
 secuencia    Integer        Not Null,
 trabajador   Char(10)       Not Null Default Char(32),
 nombre       Varchar(100)   Not Null Default Char(32),
 Nss          Varchar( 20)   Not Null Default Char(32),
 descRegion   Varchar(100)   Not Null Default Char(32),
 descDepart   Varchar(100)   Not Null Default Char(32),
 desczona     Varchar(100)   Not Null Default Char(32),
 descCiudad   Varchar(100)   Not Null Default Char(32),
 pvpLitro     Varchar( 20)   Not Null Default Char(32),
 cantLitros   Varchar( 20)   Not Null Default Char(32),
 impGasMes    Varchar( 20)   Not Null Default Char(32),
 diasFalta    Varchar( 20)   Not Null Default Char(32),
 impFalta     Varchar( 20)   Not Null Default Char(32),
 diasIncap    Varchar( 20)   Not Null Default Char(32),
 impIncap     Varchar( 20)   Not Null Default Char(32),
 totalDias    Varchar( 20)   Not Null Default Char(32),
 totalMes     Varchar( 20)   Not Null Default Char(32),
 netoMes      Varchar( 20)   Not Null Default Char(32),
 tarjeta      Varchar( 30)   Not Null Default Char(32),
 tipoLinea    Char(1)        Not Null Default 'D',
 usuario      Varchar( 30)   Not Null,
 fechaAct     Datetime       Not Null Default Getdate(),
 Constraint Ls_RepPrecioGasolinaPK
Primary Key (compania, anio, mes, region, departamento, zona, ciudad, secuencia),
Constraint Ls_RepPrecioGasolinaFk01
Foreign Key (compania)
References dbo.companias (compania) On Delete Cascade)
Go

Grant Select, Insert, Update, Delete, References On Ls_RepPrecioGasolinaTbl To Public
Go

--
-- Comentarios
--

Declare
   @v_existe    Bit     = 0,
   @v_table     Sysname = 'Ls_RepPrecioGasolinaTbl';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Tabla Trabajo para el Reporte de Precios de Gasolina por Región',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table;

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código Único de Compañía',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'compania'

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Año de Proceso',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'anio';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Mes de Proceso',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'mes';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Secuencia de Presentación de la linea de informacion',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'secuencia';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Región',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'region';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Departamento',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = 'departamento';


Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Zona',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'zona';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Ciudad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'ciudad';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'trabajador';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre del Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'nombre';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre de la Región Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'descRegion';
                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre del Departamento Relacionado al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'descDepart';
                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre de la Zona Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'desczona';
                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre de la Ciudad Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'descCiudad';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Precio del Litro de Gasolina por Ciudad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'PVPLitro';
                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Cantidad de Litros de Gasolina por Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'CantLitros';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Total Calculo de Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'impGasMes';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Dias Falta Aplicable a la Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'diasFalta';
                               
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Importe por Falta Aplicable a la Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'impFalta';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Dias Incapacidad Aplicable a la Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'diasIncap';
                               
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Importe por Incapacidad Aplicable a la Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'impIncap';

                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Importe Bruto de Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'totalDias';
                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Importe Neto de Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'netoMes';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Tarjeta donde se deposita la Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'tarjeta'; 
                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Tipo de Linea del Reporte T = Titulo, D = Detalle',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'tipoLinea';
                                
Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Ult. Codigo Usuario que Actualizo el Registro',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'usuario';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Ult. Fecha de Actualización del Registro',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = 'fechaAct';


-- Use adam
-- Go

If Exists ( Select  Top 1 1
            From    Sysobjects
            Where   Uid = 1
            And     Type = 'U'
            And     Name = 'Ls_FaltasGasolinaTbl')
    Begin
       Drop table dbo.Ls_FaltasGasolinaTbl;
    End
Go

Create table dbo.Ls_FaltasGasolinaTbl
(compania         Char(4)        Not Null,
 anio             Smallint       Not Null,
 mes              Tinyint        Not Null,
 descRegion       Varchar(100)   Not Null Default Char(32),
 depZona          Varchar( 25)   Not Null Default Char(32),
 descCiudad       Varchar(100)   Not Null Default Char(32),
 trabajador       Char(10)       Not Null,
 nombre           Varchar(100)   Not Null Default Char(32),
 incidencia_kp    Smallint       Not Null,
 concepto_ns      Smallint       Not Null,
 fecha_Incidencia Date           Not Null,
 secuencia        Smallint       Not Null,
 dias             Integer        Not Null,
 usuario          Varchar( 30)   Not Null,
 fechaAct         Datetime       Not Null Default Getdate(),
 Constraint Ls_FaltasGasolinaPK
Primary Key (compania, anio, mes, trabajador, incidencia_kp, fecha_Incidencia, secuencia),
Constraint Ls_FaltasGasolinaFk01
Foreign Key (compania)
References dbo.companias (compania) On Delete Cascade,
Constraint Ls_FaltasGasolinaFk02
Foreign Key (compania, trabajador)
References dbo.trabajadores_grales (compania, trabajador) On Delete Cascade,
Constraint Ls_FaltasGasolinaFk03
Foreign Key (compania, incidencia_kp)
References dbo.incidencias_kp_def (compania, incidencia_kp) On Delete Cascade)
Go

Grant Select, Insert, Update, Delete, References On Ls_FaltasGasolinaTbl To Public
Go

--
-- Comentarios
--

Declare
   @v_existe    Bit     = 0,
   @v_table     Sysname = 'Ls_FaltasGasolinaTbl';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Tabla Trabajo de Incidencias por faltas para el Reporte de Gasolina.',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table;

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código Único de Compañía',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'compania'

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Año de Proceso',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'anio';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Mes de Proceso',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'mes';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre de la Región Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'descRegion';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código del Departamento y la Zona  Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'depZona';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre de la Ciudad Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'descCiudad';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'trabajador';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre del Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'nombre';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Incidencia de la Falta',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'incidencia_kp';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Concepto de Nómina Relacionada a la Incidencia por Falta',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'concepto_ns';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Fecha de la Incidencia por Falta',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'fecha_Incidencia';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Secuencia de la Incidencia por Falta',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'secuencia';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Cantidad de Dias de Falta',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'dias';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Ult. Codigo Usuario que Actualizo el Registro',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'usuario';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Ult. Fecha de Actualización del Registro',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = 'fechaAct';

-- Use adam
-- Go

If Exists ( Select  Top 1 1
            From    Sysobjects
            Where   Uid = 1
            And     Type = 'U'
            And     Name = 'Ls_FaltasIncapacidadTbl')
    Begin
       Drop table dbo.Ls_FaltasIncapacidadTbl;
    End
Go

Create table dbo.Ls_FaltasIncapacidadTbl
(compania         Char(4)        Not Null,
 anio             Smallint       Not Null,
 mes              Tinyint        Not Null,
 descRegion       Varchar(100)   Not Null Default Char(32),
 depZona          Varchar( 25)   Not Null Default Char(32),
 descCiudad       Varchar(100)   Not Null Default Char(32),
 trabajador       Char(10)       Not Null,
 nombre           Varchar(100)   Not Null Default Char(32),
 incidencia_kp    Smallint       Not Null,
 concepto_ns      Smallint       Not Null,
 fecha_Incidencia Date           Not Null,
 secuencia        Smallint       Not Null,
 dias             Integer        Not Null,
 usuario          Varchar( 30)   Not Null,
 fechaAct         Datetime       Not Null Default Getdate(),
 Constraint Ls_FaltasIncapacidadPK
Primary Key (compania, anio, mes, trabajador, incidencia_kp, fecha_Incidencia, secuencia),
Constraint Ls_FaltasIncapacidadFk01
Foreign Key (compania)
References dbo.companias (compania) On Delete Cascade,
Constraint Ls_FaltasIncapacidadFk02
Foreign Key (compania, trabajador)
References dbo.trabajadores_grales (compania, trabajador) On Delete Cascade,
Constraint Ls_FaltasIncapacidadFk03
Foreign Key (compania, incidencia_kp)
References dbo.incidencias_kp_def (compania, incidencia_kp) On Delete Cascade)
Go

Grant Select, Insert, Update, Delete, References On Ls_FaltasIncapacidadTbl To Public
Go

--
-- Comentarios
--

Declare
   @v_existe    Bit     = 0,
   @v_table     Sysname = 'Ls_FaltasIncapacidadTbl';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Tabla Trabajo de Incidencias por Incapacidad para el Reporte de Gasolina.',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table;

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código Único de Compañía',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'compania'

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Año de Proceso',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'anio';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Mes de Proceso',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'mes';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre de la Región Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'descRegion';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código del Departamento y la Zona  Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'depZona';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre de la Ciudad Relacionada al Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'descCiudad';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'trabajador';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Nombre del Trabajador',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'nombre';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Código de Incidencia por Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'incidencia_kp';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Concepto de Nómina Relacionada a la Incidencia por Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'concepto_ns';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Fecha de la Incidencia por Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'fecha_Incidencia';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Secuencia de la Incidencia por Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'secuencia';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Cantidad de Dias de Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'dias';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Ult. Codigo Usuario que Actualizo el Registro',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'usuario';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Ult. Fecha de Actualización del Registro',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = 'fechaAct';


--
-- Datos de Parametrización
--

Use adam
Go

--
-- Objetivo.: Generación de Operación Reporte de Gasolina.
-- Fecha:     01/06/2026
-- Version:   1
-- Programador: Pedro Zambrano.
--

Declare
   @v_operacion   Char(8),
   @v_secuencia   Integer;

Begin
   Set @v_operacion = 'FPLS001';

   Delete aut_operaciones
   Where  operacion = @v_operacion;

   Delete parametros_oper
   Where  operacion = @v_operacion;

   Delete rel_oper_menus
   Where  ejecuta_operacion = @v_operacion;

   Delete rel_oper_modulos
   Where  operacion = @v_operacion;

   Delete operaciones
   Where  operacion = @v_operacion;


   Insert Into operaciones
   (operacion, descripcion_ope, icono, tipo_operacion, ejecuta_llamada, parametros, ruta, tipo_Ejecucion)
   Select @v_operacion, 'Generacion Reporte Gasolina', 1, 2, 'RepGas.exe', 1, '', 0;

   Insert into parametros_oper
   (operacion, secuencia, tipo_campo, parametro)
   Select @v_operacion, 1, 5, 'DSN'
   Union
   Select @v_operacion, 2, 3, 'User'
   Union
   Select @v_operacion, 3, 4, 'Password'
   Union
   Select @v_operacion, 4, 2, 'Compania';

  Insert Into rel_oper_modulos
  (operacion, modulo)
   Select @v_operacion, 'NS';

--
  Select @v_secuencia = max(secuencia)
  From   rel_oper_menus
  Where  modulo = 'NS'
  And    Menu   = 20;
  
  Insert Into rel_oper_menus
 (modulo,       menu, secuencia, tipo_llamada,
  ejecuta_menu, ejecuta_operacion)
  Select 'NS', 20, Isnull(@v_secuencia, 0) + 1, 2, 
          null,   @v_operacion;

   Insert Into aut_operaciones
  (usuario, operacion, nivel_seguridad)
   Select 'ADAM', @v_operacion, 4;
   
   Return;

End;
Go

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
   
   Return

End;
Go

--
-- Funciones
--

-- Use Adam
--Go

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'Fn'
            And    Name = 'fn_desc_agrup_dato')
   Begin
      Drop Function dbo.fn_desc_agrup_dato
   End
Go

Create Function dbo.fn_desc_agrup_dato
   (@PsAgrupacion   Char(10),
    @PsDato         Char(10))
Returns Char(150)
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   fn_desc_agrup_dato
-- Autor:            Pedro Zambrano
-- Fecha:            23-jul-2026.
-- Objetivo:         Funcion que consulta la descripcion del dato de la agrupación
-- Versión:          1
--
-- ***************************************************************************************************************************


   Declare
      @v_descripcion     Varchar(150);

   Begin
      Select @v_descripcion = a.descripcion
      From   dbo.datos_agr_trab a
      Where  RTrim(a.agrupacion) = @PsAgrupacion
      And    RTrim(a.dato)       = @PsDato;
      If @@Rowcount = 0
         Begin
            Set @v_descripcion = Char(32);
         End;
   End

   Return (@v_descripcion);

End;
Go

-- Use Adam
--Go

If Exists ( Select Top 1 1
			From   sysobjects
			Where  Uid	= 1
			And	   Type = 'Fn'
			And	   Name = 'fn_obten_nss')
   Begin
	  Drop Function dbo.fn_obten_nss
   End
Go

Create Function dbo.fn_obten_nss
   (@PsTrabajador	Char(10))
Returns Char(150)
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :	 fn_obten_nss
-- Autor:			 Pedro Zambrano
-- Fecha:			 23-jul-2026.
-- Objetivo:		 Funcion que consulta el numero de seguridad social del trabajador
-- Versión:			 1
--
-- ***************************************************************************************************************************


   Declare
	  @v_nss	 Varchar(20);

   Begin
	  Select @v_nss = Isnull(reg_seguro_social, Char(32))
	  From	 dbo.trabajadores
	  Where	 trabajador = @PsTrabajador;
	  If @@Rowcount = 0
		 Begin
			Set @v_nss = Char(32);
		 End;
   End

   Return (@v_nss);

End;
Go

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

Triggers 

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

Procedimientos

Use adam
Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsCiudad                Char(10)        = 'Toluca',
   @PnPrecio                Decimal(19,2)   = 20,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spa_Ls_HistPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                            @PnAnio      = @PnAnio,
                                            @PnMes       = @PnMes,
                                            @PsCiudad    = @PsCiudad,
                                            @PnPrecio    = @PnPrecio,
                                            @PsUsuario   = @PsUsuario,
                                            @PsOperacion = @PsOperacion,
                                            @PnEstatus   = @PnEstatus Output,
                                            @PsMensaje   = @PsMensaje Output;
                                            
   Select @PnEstatus As Error, @PsMensaje As MensajeError

   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'Spa_Ls_HistPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.Spa_Ls_HistPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spa_Ls_HistPrecioGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsCiudad                Char(10),
   @PnPrecio                Decimal(19,2),
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_agrupacion            Char(10),
   @v_dato                  Char(10),
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spa_Ls_HistPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de alta de registros a la tabla Ls_HistPrecioGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32),
          @v_dato          = Rtrim(Substring(@PsCiudad, 1, 10));

 -------------------------------------------------------
 -- Validar Seguridad
 -------------------------------------------------------

   Begin
      If Not Exists (Select Top 1 1
                     From   master.dbo.usuario_base
                     Where  Usuario         = @PsUsuario )
         Begin
            Select @PnEstatus    = 250001,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Usuario no esta Registrado como usuario ADAM',
                   @PsMensaje    = @v_desc_error
                   
            Goto Salida
         End
                     
      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Operaciones
                     Where  Usuario         = @PsUsuario
                     And    Operacion       = @PsOperacion
                     And    Nivel_seguridad > 1)
         Begin
            Select @PnEstatus    = 250002,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' No tiene Autorizacion de Operacion',
                   @PsMensaje    = @v_desc_error;
                   
            Goto Salida

         End


      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Companias
                     Where  Usuario        = @PsUsuario
                     And    Compania       = @PsCompania)
         Begin
            Select @PnEstatus    = 250003,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             '  No tiene Autorizacion para la compania',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If IsNull(@PnPrecio, 0) <= 0
         Begin
            Select @PnEstatus    = 250004,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Precio de la Gasolina debe ser mayor a cero (0)',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If IsNull(@PnAnio, 0) Not Between 2000 And 2050
         Begin
            Select @PnEstatus    = 250005,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Parámetro Año de Proceso no es Valido. (2020-2050)',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If IsNull(@PnMes, 0) Not Between 1 And 12
         Begin
            Select @PnEstatus    = 250006,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             '  El Parámetro Mes de Proceso no es Valido. (1-12)',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

--
-- Búsqueda de codigo de Agrupación
--

      Select @v_agrupacion = Rtrim(descripcion)
      From   dbo.criterios_valores
      Where  campo = 'agrciudad'
      And    item  = 1;
      If @@Rowcount = 0
         Begin
            Select @PnEstatus    = 250007,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Parámetro de Ciudad (agrciudad) no existe en criterios_valores',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.agrupaciones_trab
                     Where  agrupacion = @v_agrupacion)
         Begin
            Select @PnEstatus    = 250008,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Código de Agrupacion de Ciudad no es Válido',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.datos_agr_trab
                     Where  agrupacion = @v_agrupacion
                     And    dato       = @v_dato)
         Begin
            Select @PnEstatus    = 250009,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El Código de Ciudad seleccionado no es Válido,',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      If Exists (Select top 1 1
                 From   dbo.Ls_HistPrecioGasolinaTbl
                 Where  compania = @PsCompania
                 And    anio     = @PnAnio
                 And    mes      = @PnMes
                 And    ciudad   = @v_dato)
         Begin
            Select @PnEstatus    = 250010,
                   @v_desc_error = 'Error: ' + Cast(@PnEstatus as Varchar) + 
                                             ' El precio de la gasolina para la ciudad ya fue cargado',
                   @PsMensaje    = @v_desc_error;

            Goto Salida
         End

      Begin Try
         Insert Into dbo.Ls_HistPrecioGasolinaTbl
         (compania, anio, mes, ciudad, usuario, precio)
         Values (@PsCompania, @PnAnio, @PnMes, @v_dato, @PsUsuario, @PnPrecio)
      End   Try

      Begin Catch
         Select  @v_Error      = @@Error,
                 @v_desc_error = Substring (Error_Message(), 1, 230)
      End   Catch

      If IsNull(@v_Error, 0) <> 0
         Begin
            Select @PnEstatus = @v_error,
                   @PsMensaje = 'Error.: ' + @v_desc_error;

         End;

   End;

   Set @PsMensaje = 'Registro Guardado';

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go


Grant  Execute On Spa_Ls_HistPrecioGasolinaTbl to Public;

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsCiudad                Char(10)        = 'Toluca',
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null;
Begin
   Execute dbo.spd_Ls_HistPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                            @PnAnio      = @PnAnio,
                                            @PnMes       = @PnMes,
                                            @PsCiudad    = @PsCiudad,
                                            @PsUsuario   = @PsUsuario,
                                            @PsOperacion = @PsOperacion,
                                            @PnEstatus   = @PnEstatus Output,
                                            @PsMensaje   = @PsMensaje Output;

   Select @PnEstatus As IdError, @PsMensaje As MensajeError
   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spd_Ls_HistPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.spd_Ls_HistPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spd_Ls_HistPrecioGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsCiudad                Char(10)        = Null,
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_agrupacion            Char(10),
   @v_dato                  Char(10),
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_error                 Integer,
   @v_desc_error            Varchar(250),
   @v_registros             Integer,
   @v_linea                 Integer;

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spd_Ls_HistPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de baja de registros a la tabla Ls_HistPrecioGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off
   
   Select @v_dato = Rtrim(Substring(@PsCiudad, 1, 10)),
          @PnEstatus  = 0,
          @PsMensaje  = Char(32);

  -------------------------------------------------------
  -- Validar Seguridad
  -------------------------------------------------------

   Begin
      If IsNull(@PnAnio, 0) = 0 Or
         IsNull(@PnMes,  0) = 0
         Begin
            Goto Salida
         End
         
      If Not Exists (Select Top 1 1
                     From   master.dbo.usuario_base
                     Where  Usuario         = @PsUsuario )
         Begin
            Select @v_desc_error = 'Error: El Usuario no esta Registrado como usuario ADAM',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250001

            Goto Salida
         End
                     
      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Operaciones
                     Where  Usuario         = @PsUsuario
                     And    Operacion       = @PsOperacion
                     And    Nivel_seguridad > 1)
         Begin
            Select @v_desc_error = 'Error: No tiene Autorizacion de Operacion',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250002;

            Goto Salida

         End

      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Companias
                     Where  Usuario        = @PsUsuario
                     And    Compania       = @PsCompania)
         Begin
            Select @v_desc_error = 'Error: No tiene Autorizacion para la compania',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250003;

            Goto Salida
         End


      Begin Try
         If @v_dato Is Not Null
            Begin
               If Not Exists (Select top 1 1
                              From   dbo.Ls_HistPrecioGasolinaTbl
                              Where  compania = @PsCompania
                              And    anio     = @PnAnio
                              And    mes      = @PnMes
                              And    ciudad   = @v_dato)
                  Begin
                      Select @v_desc_error = 'Error: El Registro a Eliminar No es Valido',
                             @PsMensaje    = @v_desc_error,
                             @PnEstatus    = 250007;
   
                      Goto Salida
                  End
         
               Delete dbo.Ls_HistPrecioGasolinaTbl
               Where  compania = @PsCompania
               And    anio     = @PnAnio
               And    mes      = @PnMes
               And    ciudad   = @v_dato;
               Set @v_registros = @@Rowcount
            End
         Else
            Begin
               Delete dbo.Ls_HistPrecioGasolinaTbl
               Where  compania = @PsCompania
               And    anio     = @PnAnio
               And    mes      = @PnMes;
               Set @v_registros = @@Rowcount
            End
      End Try

      Begin Catch
         Select  @v_Error      = @@Error,
                 @v_linea      = error_line(),
                 @v_desc_error = Substring (Error_Message(), 1, 230) + ' En linea: ' + Cast(@v_linea as Varchar)
      End   Catch

      If IsNull(@v_Error, 0) <> 0
         Begin
            Select @PnEstatus = @v_error,
                   @PsMensaje = 'Error.: ' + @v_desc_error;

         End;
    
   End;

   Set @PsMensaje = Cast(@v_registros As Varchar) + ' Registros Eliminados.';
   
Salida:

   Set Xact_Abort    Off
   Return;
End;
Go


Grant  Execute On spd_Ls_HistPrecioGasolinaTbl to Public;

Use adam
Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsCiudad                Char(10)        = 'Toluca',
   @PnPrecio                Decimal(19,2)   = 20,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spv_Ls_HistPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                            @PnAnio      = @PnAnio,
                                            @PnMes       = @PnMes,
                                            @PsCiudad    = @PsCiudad,
                                            @PnPrecio    = @PnPrecio,
                                            @PsUsuario   = @PsUsuario,
                                            @PsOperacion = @PsOperacion,
                                            @PnEstatus   = @PnEstatus Output,
                                            @PsMensaje   = @PsMensaje Output;
                                            
   Select @PnEstatus As Error, @PsMensaje As MensajeError

   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spv_Ls_HistPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.spv_Ls_HistPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spv_Ls_HistPrecioGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsCiudad                Char(10),
   @PnPrecio                Decimal(19,2),
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_agrupacion            Char(10),
   @v_dato                  Char(10),
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spv_Ls_HistPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            23-jul-2026.
-- Objetivo:         Procedimiento de valida registros a ser incoporados en la tabla Ls_HistPrecioGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32),
          @v_dato          = Rtrim(Substring(@PsCiudad, 1, 10));

 -------------------------------------------------------
 -- Validar Seguridad
 -------------------------------------------------------

   Begin
      If Not Exists (Select Top 1 1
                     From   master.dbo.usuario_base
                     Where  Usuario         = @PsUsuario )
         Begin
            Select @v_desc_error = 'Error: El Usuario no esta Registrado como usuario ADAM',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250001

            Goto Salida
         End
                     
      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Operaciones
                     Where  Usuario         = @PsUsuario
                     And    Operacion       = @PsOperacion
                     And    Nivel_seguridad > 1)
         Begin
            Select @v_desc_error = 'Error: No tiene Autorizacion de Operacion',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250002;

            Goto Salida

         End


      If Not Exists (Select Top 1 1
                     From   dbo.Aut_Companias
                     Where  Usuario        = @PsUsuario
                     And    Compania       = @PsCompania)
         Begin
            Select @v_desc_error = 'Error: No tiene Autorizacion para la compania',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250003;

            Goto Salida
         End

      If IsNull(@PnPrecio, 0) <= 0
         Begin
            Select @v_desc_error = 'Error: El Precio de la Gasolina debe ser mayor a cero (0)',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250004;

            Goto Salida
         End

      If IsNull(@PnAnio, 0) Not Between 2000 And 2050
         Begin
            Select @v_desc_error = 'Error: El Parámetro Año de Proceso no es Valido. (2020-2050)',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250005;

            Goto Salida
         End

      If IsNull(@PnMes, 0) Not Between 1 And 12
         Begin
            Select @v_desc_error = 'Error: El Parámetro Mes de Proceso no es Valido. (1-12)',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

--
-- Búsqueda de codigo de Agrupación
--

      Select @v_agrupacion = Rtrim(descripcion)
      From   dbo.criterios_valores
      Where  campo = 'agrciudad'
      And    item  = 1;
      If @@Rowcount = 0
         Begin
            Select @v_desc_error = 'Error: El Parámetro de Ciudad (agrciudad) no existe en criterios_valores',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250007;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.agrupaciones_trab
                     Where  agrupacion = @v_agrupacion)
         Begin
            Select @v_desc_error = 'Error: El Código de Agrupacion de Ciudad no es Válido',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

      If Not Exists (Select top 1 1
                     From   dbo.datos_agr_trab
                     Where  agrupacion = @v_agrupacion
                     And    dato       = @v_dato)
         Begin
            Select @v_desc_error = 'Error: El Código de Ciudad seleccionado no es Válido,',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

      If Exists (Select top 1 1
                 From   dbo.Ls_HistPrecioGasolinaTbl
                 Where  compania = @PsCompania
                 And    anio     = @PnAnio
                 And    mes      = @PnMes
                 And    ciudad   = @v_dato)
         Begin
            Select @v_desc_error = 'Error: El precio de la gasolina para la ciudad ya fue cargado',
                   @PsMensaje    = @v_desc_error,
                   @PnEstatus    = 250006;

            Goto Salida
         End

   End;

   Set @PsMensaje = 'Registro Valido';

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go


Grant  Execute On spv_Ls_HistPrecioGasolinaTbl to Public;

-- Use adam
-- Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spd_Ls_RepPrecioGasolinaTbl @PsCompania  = @PsCompania,
                                           @PnAnio      = @PnAnio,
                                           @PnMes       = @PnMes,
                                           @PsUsuario   = @PsUsuario,
                                           @PsOperacion = @PsOperacion,
                                           @PnEstatus   = @PnEstatus Output,
                                           @PsMensaje   = @PsMensaje Output;

   Select @PnEstatus As Error, @PsMensaje As MensajeError

   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spd_Ls_RepPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.spd_Ls_RepPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spd_Ls_RepPrecioGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spd_Ls_RepPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de baja a la tabla Ls_RepPrecioGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32);

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

   Begin Try
      Delete dbo.Ls_RepPrecioGasolinaTbl
      Where  compania = @PsCompania
      And    Anio     = @PnAnio
      And    Mes      = @PnMes;
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


Grant  Execute On spd_Ls_RepPrecioGasolinaTbl to Public;

-- Use adam
-- Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spd_Ls_FaltasGasolinaTbl    @PsCompania  = @PsCompania,
                                           @PnAnio      = @PnAnio,
                                           @PnMes       = @PnMes,
                                           @PsUsuario   = @PsUsuario,
                                           @PsOperacion = @PsOperacion,
                                           @PnEstatus   = @PnEstatus Output,
                                           @PsMensaje   = @PsMensaje Output;

   Select @PnEstatus As Error, @PsMensaje As MensajeError

   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spd_Ls_FaltasGasolinaTbl')
   Begin
      Drop Procedure dbo.spd_Ls_FaltasGasolinaTbl;
   End
Go

Create Procedure dbo.spd_Ls_FaltasGasolinaTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spd_Ls_FaltasGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de baja a la tabla Ls_FaltasGasolinaTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32);
          
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

   Begin Try
      Delete dbo.Ls_FaltasGasolinaTbl
      Where  compania = @PsCompania
      And    Anio     = @PnAnio
      And    Mes      = @PnMes;
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


Grant  Execute On spd_Ls_FaltasGasolinaTbl to Public;

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

-- Use adam
-- Go

/*

Declare
   @PsCompania              Char(4)         = 'LS',
   @PnAnio                  Smallint        = 2026,
   @PnMes                   Tinyint         = 7,
   @PsUsuario               Char(20)        = 'adam',
   @PsOperacion             Char(10)        = 'FPLS001',
   @PnEstatus               Integer         = Null,
   @PsMensaje               Varchar( 250)   = Null

Begin
   Execute dbo.spd_Ls_FaltasIncapacidadTbl    @PsCompania  = @PsCompania,
                                              @PnAnio      = @PnAnio,
                                              @PnMes       = @PnMes,
                                              @PsUsuario   = @PsUsuario,
                                              @PsOperacion = @PsOperacion,
                                              @PnEstatus   = @PnEstatus Output,
                                              @PsMensaje   = @PsMensaje Output;

   Select @PnEstatus As Error, @PsMensaje As MensajeError

   Return;

End;
Go

*/

If Exists (Select Top 1 1
           From   sys.procedures
           Where  Name = 'spd_Ls_FaltasIncapacidadTbl')
   Begin
      Drop Procedure dbo.spd_Ls_FaltasIncapacidadTbl;
   End
Go

Create Procedure dbo.spd_Ls_FaltasIncapacidadTbl
  (@PsCompania              Char(04),
   @PnAnio                  Smallint,
   @PnMes                   Tinyint,
   @PsUsuario               Char(20),
   @PsOperacion             Char(10),
   @PnEstatus               Integer         = Null Output,
   @PsMensaje               Varchar( 250)   = Null Output)

As

Declare
   @v_mensaje               Varchar(250),
   @v_existe                Bit,
   @v_Error                 Integer,
   @v_desc_error            Varchar(250);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spd_Ls_FaltasIncapacidadTbl
-- Autor:            Pedro Zambrano
-- Fecha:            24-jul-2026.
-- Objetivo:         Procedimiento de baja a la tabla Ls_FaltasIncapacidadTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32);

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

   Begin Try
      Delete dbo.Ls_FaltasIncapacidadTbl
      Where  compania = @PsCompania
      And    Anio     = @PnAnio
      And    Mes      = @PnMes;
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


Grant  Execute On spd_Ls_FaltasIncapacidadTbl to Public;

-- Use Adam
--Go

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
   Execute dbo.spp_Ls_FaltasIncapacidadTbl @PsCompania  = @PsCompania,
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

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'P'
            And    Name = 'spp_Ls_FaltasIncapacidadTbl')
   Begin
      Drop Procedure dbo.spp_Ls_FaltasIncapacidadTbl
   End
Go

Create Procedure dbo.spp_Ls_FaltasIncapacidadTbl
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
   @v_incap                 Char(10),
   @v_fecha_inicio          Date,
   @v_fecha_termino         Date,
   @v_fecha_inicio_kp       Date,
   @v_fecha_termino_kp      Date,
   @v_x                     Bit,
   @v_registros             Integer;

--

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spp_Ls_FaltasIncapacidadTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de alta de Registros por Falta en la tabla Ls_FaltasIncapacidadTbl
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus          = 0,
          @PsMensaje          = Char(32),
          @v_registros        = 0,
          @v_x                = 0,
          @v_fecha_inicio     = Convert(Date, '01/' + Convert(Char(2),  @PnMes) + '/' +
                                                      Convert(Char(4), @PnAnio), 103),
          @v_fecha_termino    = dbo.fn_obten_FinMes(@v_fecha_inicio),
          @v_fecha_inicio_kp  = Dateadd(month, -1, @v_fecha_inicio);

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

   Select @v_incap = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrincap'
   And  item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 25005,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Agr Conceptos por Faltas "agrincap" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.rel_conceptos_agr
                  Where  compania      = @PsCompania
                  And    agr_conceptos = @v_incap)
      Begin
         Select @PnEstatus    = 250006,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' Código de Agrupacion de Conceptos  "' + @v_incap + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Creación de Tablas Temporales
--

   Create Table #TempTrab
  (trabajador   Char(10) Not Null Primary Key,
   idEstatus    bit      Not Null Default 1)

--

   Execute dbo.spd_Ls_FaltasIncapacidadTbl @PsCompania  = @PsCompania,
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
     Insert Into dbo.Ls_FaltasIncapacidadTbl
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
     And    c.agr_conceptos = @v_incap
     And    c.concepto      = b.concepto_genera
     Where  a.compania      = @PsCompania
     And    a.fecha_incidencia Between @v_fecha_inicio_kp And @v_fecha_termino;

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

   Insert Into #TempTrab
   (trabajador)
   Select Distinct trabajador
   From   dbo.Ls_FaltasIncapacidadTbl
   Where  compania   = @PsCompania
   And    anio       = @PnAnio
   And    Mes        = @PnMes;

  While @v_x = 0
  Begin
     Select @v_fecha_inicio_kp  = DateAdd(Month, -1, @v_fecha_inicio_kp),
            @v_fecha_termino_kp = dbo.fn_obten_FinMes(@v_fecha_inicio_kp);

     Insert Into dbo.Ls_FaltasIncapacidadTbl
    (compania,    anio,             mes,        trabajador, incidencia_kp,
     concepto_ns, fecha_Incidencia, secuencia,  dias,       usuario)
     Select a.compania,        @PnAnio,            @PnMes,      a.trabajador,  a.incidencia_kp,
            b.concepto_genera, a.fecha_incidencia, a.secuencia, a.variable_01, @PsUsuario
     From   dbo.incidencias_KP a
     Join   dbo.incidencias_kp_def b
     On     b.compania               = a.compania
     And    b.incidencia_kp          = a.incidencia_kp
     Join   dbo.rel_conceptos_agr c
     On     c.compania               = b.compania
     And    c.agr_conceptos          = @v_incap
     And    c.concepto               = b.concepto_genera
     Join   #TempTrab d
     On     d.trabajador             = a.trabajador
     And    d.idEstatus              = 1
     Where a.compania               = @PsCompania
     And    a.fecha_incidencia Between @v_fecha_inicio_kp And @v_fecha_termino_kp;
     Set @v_registros = @@Rowcount

     If @v_registros = 0
        Begin
           Set @v_x = 1

           Update #TempTrab
           Set    idEstatus = 0
           From   #TempTrab a
           Where  idEstatus = 1;

           goto siguiente;
        End

    Update #TempTrab
    Set    idEstatus = 0
    From   #TempTrab a
    Where  idEstatus = 1
    And    Not Exists ( Select top 1 1
                        From   dbo.Ls_FaltasIncapacidadTbl
                        Where  compania                 = @PsCompania
                        And    anio                     = @PnAnio
                        And    Mes                      = @PnMes
                        And    trabajador               = a.trabajador
                        And    fecha_incidencia   Between @v_fecha_inicio And @v_fecha_termino)
siguiente:

  End

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go


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
   Execute dbo.spp_Ls_RepPrecioGasolinaTbl @PsCompania  = @PsCompania,
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
           Where  Name = 'spp_Ls_RepPrecioGasolinaTbl')
   Begin
      Drop Procedure dbo.spp_Ls_RepPrecioGasolinaTbl;
   End
Go

Create Procedure dbo.spp_Ls_RepPrecioGasolinaTbl
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
   @v_region                Char(10),
   @v_departamento          Char(10),
   @v_zona                  Char(10),
   @v_ciudad                Char(10),
   @v_faltas                Char(10),
   @v_fecha_inicio          Date,
   @v_fecha_termino         Date,
   @v_precio                Decimal(19,2),
--
   @v_trabajador            Char( 10),
   @v_nombre                Char(100),
   @v_datoRegion            Char( 10),
   @v_datoDepart            Char( 10),
   @v_datoZona              Char( 10),
   @v_datoCiudad            Char( 10),
--
   @v_datoAntRegion         Char( 10),
   @v_datoAntDepart         Char( 10),
   @v_datoAntZona           Char( 10),
   @v_datoAntCiudad         Char( 10),
--
   @v_descRegion            Char(150),
   @v_descDepart            Char(150),
   @v_descZona              Char(150),
   @v_descCiudad            Char(150),
   @v_nss                   Char( 20),
   @v_var_trab_gas          Char( 10),
   @v_var_trab_exc          Char( 10),
   @v_var_trab_tar          Char( 10),
   @v_linea                 Integer,
   @v_pvpLitro              Decimal(19, 2),
   @v_cantLitros            Decimal(19, 2),
   @v_impGasMes             Decimal(19, 2),
   @v_impFalta              Decimal(19, 2),
   @v_diasFalta             Integer,
   @v_diasIncap             Integer,
   @v_valTrabExc            Integer,
   @v_impIncap              Decimal(19, 2),
   @v_totalDias             Integer,
   @v_totalMes              Decimal(19, 2),
   @v_netoMes               Decimal(19, 2),
   @v_tarjeta               Varchar(30);

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   spp_Ls_RepPrecioGasolinaTbl
-- Autor:            Pedro Zambrano
-- Fecha:            22-jul-2026.
-- Objetivo:         Procedimiento de Calculo del Reporte de Asignación de Gasolina a Trabajadores
-- Versión:          1
--
-- ***************************************************************************************************************************

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus       = 0,
          @PsMensaje       = Char(32),
          @v_linea         = 0,
          @v_totalDias     = 30,
          @v_fecha_inicio  = Convert(Date, '01/' + Convert(Char(2), @PnMes) + '/' +
                                                   Convert(Char(4), @PnAnio), 103),
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
-- Búsqueda de codigo de variable trabajador (Cantidad de Litros de gasolina asignado al trabajador)
--

   Select @v_var_trab_gas = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'ltrgas'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250006,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro variable trabajador "ltrgasno" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.defn_variables_ns
                  Where  compania = @PsCompania
                  And    variable = @v_var_trab_gas)
      Begin
         Select @PnEstatus    = 250007,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de la variable del trabajador "' + @v_var_trab_gas + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de número de tarjeta para deposito de asignación gasolina
--

   Select @v_var_trab_tar = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'tarjetagas'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250008,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro variable trabajador "tarjetagas" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.defn_variables_ns
                  Where  compania = @PsCompania
                  And    variable = @v_var_trab_tar)
      Begin
         Select @PnEstatus    = 250009,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de la variable del trabajador "' + @v_var_trab_tar + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de variable trabajador (Exentos de Pago de Gasolina)
--

   Select @v_var_trab_exc = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'excgas'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250010,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          '  El Parámetro variable trabajador "excgas" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.defn_variables_ns
                  Where  compania = @PsCompania
                  And    variable = @v_var_trab_exc)
      Begin
         Select @PnEstatus    = 250011,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de la variable del trabajador "' + @v_var_trab_exc + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de Agrupaciones de Trabajadores Region
--

   Select @v_region = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrregion'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250012,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Region (agrregion) no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_region)
      Begin
         Select @PnEstatus    = 250013,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Región "' + @v_region + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de Agrupaciones de Trabajadores Departamento
--

   Select @v_departamento = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrdep'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250014,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Departamento "agrdep" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_departamento)
      Begin
         Select @PnEstatus    = 250015,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Departamento "' + @v_departamento + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--
-- Búsqueda de codigo de Agrupación Zona
--

   Select @v_zona = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrzona'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250016,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Zona "agrzona" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_zona)
      Begin
         Select @PnEstatus    = 250017,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Zona "' + @v_zona + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End


--
-- Búsqueda de codigo de Agrupación Ciudad
--

   Select @v_ciudad = Rtrim(descripcion)
   From   dbo.criterios_valores
   Where  campo = 'agrciudad'
   And    item  = 1;
   If @@Rowcount = 0
      Begin
         Select @PnEstatus    = 250018,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Parámetro de Ciudad "agrciudad" no existe en criterios_valores',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

   If Not Exists (Select top 1 1
                  From   dbo.agrupaciones_trab
                  Where  agrupacion = @v_ciudad)
      Begin
         Select @PnEstatus    = 250019,
                @v_desc_error = 'Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' El Código de Agrupacion de Ciudad "' + @v_ciudad + '" no es Válido',
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
                @v_desc_error ='Error: ' + Cast(@PnEstatus As Varchar) +
                                          ' Código de Agrupacion de Conceptos  "' + @v_faltas + '" no es Válido',
                @PsMensaje    = @v_desc_error;

         Goto Salida
      End

--

   Execute dbo.spd_Ls_RepPrecioGasolinaTbl @PsCompania  = @PsCompania,
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

   Execute dbo.spp_Ls_FaltasGasolinaTbl @PsCompania  = @PsCompania,
                                        @PnAnio      = @PnAnio,
                                        @PnMes       = @PnMes,
                                        @PsUsuario   = @PsUsuario,
                                        @PsOperacion = @PsOperacion,
                                        @PnImprime   = @PnImprime,
                                        @PnEstatus   = @PnEstatus Output,
                                        @PsMensaje   = @PsMensaje Output;
   If @PnEstatus != 0
      Begin
         Goto Salida
      End

   Execute dbo.spp_Ls_FaltasIncapacidadTbl @PsCompania  = @PsCompania,
                                           @PnAnio      = @PnAnio,
                                           @PnMes       = @PnMes,
                                           @PsUsuario   = @PsUsuario,
                                           @PsOperacion = @PsOperacion,
                                           @PnImprime   = @PnImprime,
                                           @PnEstatus   = @PnEstatus Output,
                                           @PsMensaje   = @PsMensaje Output;
   If @PnEstatus != 0
      Begin
         Goto Salida
      End

   Declare
      C_Detalle Cursor For
      Select a.trabajador, Replace(b.nombre, '/', ' ') nombre,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_region), Char(32)) region,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_departamento), Char(32)) departamento,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_zona), Char(32)) zona,
             Isnull((Select Top 1 dato
                     From   dbo.rel_trab_agr
                     Where  compania   = a.compania
                     And    trabajador = a.trabajador
                     And    agrupacion = @v_ciudad), Char(32)) ciudad
      From   dbo.trabajadores_grales a
      Join   dbo.trabajadores        b
      On     b.trabajador     = a.trabajador
      Where  a.compania       = @PsCompania
      And    a.fecha_ingreso <= @v_fecha_termino
      And    a.sit_trabajador = 1
      Order  By 3, 4, 5, 6, 1;

   Begin
      Open C_Detalle
      While @@Fetch_status < 1
      Begin
         Fetch C_Detalle Into @v_trabajador,  @v_nombre,     @v_datoRegion, @v_datoDepart,
                              @v_datoZona,    @v_datoCiudad;
         If @@Fetch_status <> 0
            Begin
               Break
            End

         If @v_datoRegion = Char(32) Or
            @v_datoDepart = Char(32) Or
            @v_datoZona   = Char(32)
            Begin
               goto Proximo
            End

         Begin
            Select @v_pvpLitro = precio
            From   dbo.Ls_HistPrecioGasolinaTbl
            Where  compania = @PsCompania
            And    Anio     = @PnAnio
            And    Mes      = @PnMes
            And    ciudad   = @v_datoCiudad;
            If @@Rowcount = 0
               Begin
                  Set @v_pvpLitro = 0
               End
         End;

         Select @v_diasFalta = 0,
                @v_impFalta  = 0,
                @v_diasIncap = 0,
                @v_impIncap  = 0,
                @v_totalDias = 30;

         Select @v_nss           = dbo.fn_obten_nss(@v_trabajador),
                @v_descCiudad    = dbo.fn_desc_agrup_dato(@v_ciudad, @v_datoCiudad),
                @v_cantLitros    = dbo.fn_obten_valor_vartrab(@PsCompania,   @v_Trabajador,   @v_var_trab_gas),
                @v_tarjeta       = dbo.fn_obten_valor_vartrab(@PsCompania,   @v_Trabajador,   @v_var_trab_tar),
                @v_diasFalta     = dbo.fn_obten_faltasGasolina( @PsCompania, @PnAnio,         @PnMes, @v_trabajador),
                @v_impGasMes     = @v_pvpLitro * @v_cantLitros

         If @v_cantLitros = 0
            Begin
               goto Proximo
            End

         Set @v_linea = @v_linea + 1;

         If @v_datoRegion != Isnull(@v_datoAntRegion, 'X1') Or
            @v_datoDepart != Isnull(@v_datoAntDepart, 'X1') Or
            @v_datoZona   != Isnull(@v_datoAntZona,   'X1')
            Begin
               Select @v_descRegion    = dbo.fn_desc_agrup_dato(@v_region,       @v_datoRegion),
                      @v_descDepart    = dbo.fn_desc_agrup_dato(@v_departamento, @v_datoDepart),
                      @v_descZona      = dbo.fn_desc_agrup_dato(@v_zona,         @v_datoZona),
                      @v_datoAntRegion = @v_datoRegion,
                      @v_datoAntDepart = @v_datoDepart,
                      @v_datoAntZona   = @v_datoZona;

               If @v_linea > 1
                  Begin
                     Insert Into dbo.Ls_RepPrecioGasolinaTbl
                    (compania,   anio,       mes,       region,     departamento,
                     zona,       ciudad,     secuencia, trabajador, nombre,
                     descRegion, descDepart, desczona,  descCiudad, tipoLinea,
                     usuario)
                     Select @PsCompania,   @PnAnio,       @PnMes,      @v_datoRegion, @v_datoDepart,
                            @v_datoZona,   @v_datoCiudad, @v_linea,    Char(32),      Char(32),
                            Char(32),      Char(32),      Char(32),    Char(32), 'S',
                            @PsUsuario
                     Set @v_linea = @v_linea + 1;

                  End

               Insert Into dbo.Ls_RepPrecioGasolinaTbl
              (compania,   anio,       mes,       region,     departamento,
               zona,       ciudad,     secuencia, trabajador, nombre,
               descRegion, descDepart, desczona,  tipoLinea,
               usuario)
               Select @PsCompania,   @PnAnio,       @PnMes,      @v_datoRegion, @v_datoDepart,
                      @v_datoZona,   @v_datoCiudad, @v_linea,    Char(32),      RTrim(@v_datoDepart) + ' - ' + @v_descDepart,
                      @v_descRegion, @v_descDepart, @v_descZona, 'T',
                      @PsUsuario
               Set @v_linea = @v_linea + 1;
            End

--
-- Linea de detalle.
--

         If @v_diasFalta > 30
            Begin
               Set @v_diasFalta = 30
            End

         If @v_diasFalta > 0
            Begin
               Select @v_totalDias = @v_totalDias - @v_diasFalta,
                      @v_impFalta  = (@v_impGasMes / 30) * @v_diasFalta;
            End

         Set @v_totalMes = @v_impGasMes - Isnull(@v_impFalta, 0);

         If @v_tarjeta = '0.000000'
            Begin
               Set @v_tarjeta = Char(32);
            End
         Else
            Begin
               Set @v_tarjeta = Substring(Replace(@v_tarjeta, '.', ''), 1, 16)
            End

         If dbo.fn_flagfaltasIncapacidad(@PsCompania, @PnAnio, @PnMes, @v_trabajador) = '0-1'
            Begin
               Select @v_diasIncap = dbo.fn_obten_faltasIncapacidad(@PsCompania, @PnAnio, @PnMes, @v_trabajador, 1),
                      @v_impIncap  = (@v_impGasMes / 30) * Isnull(@v_diasIncap, 0);
            End
         Else
            Begin
               Select @v_diasIncap = 0,
                      @v_impIncap  = 0;
            End

         Select @v_totalMes   = @v_totalMes   + Isnull(@v_impIncap , 0),
                @v_totalDias  = @v_totalDias  + @v_diasIncap,
                @v_valTrabExc = dbo.fn_obten_valor_vartrab(@PsCompania, @v_trabajador, @v_var_trab_exc)

         Insert Into dbo.Ls_RepPrecioGasolinaTbl
        (compania,  anio,       mes,        region,     departamento,
         zona,      ciudad,     secuencia,  trabajador, nombre,
         Nss,       descCiudad, cantLitros, pvpLitro,   impGasMes,
         diasFalta, impFalta,   diasIncap,  impIncap,
         totalDias, totalMes,   netoMes,    tarjeta,    usuario)
         Select @PsCompania, @PnAnio,       @PnMes,        @v_datoRegion, @v_datoDepart,
                @v_datoZona, @v_datoCiudad, @v_linea,      @v_trabajador, @v_nombre,
                @v_nss,      @v_descCiudad,
                Convert(Varchar(20), Cast(@v_cantLitros As Money), 1),
                Convert(Varchar(20), Cast(@v_pvpLitro   As Money), 1) pvpLitro,
                Convert(Varchar(20), Cast(@v_impGasMes  As Money), 1) impGasMes,
                Case When Isnull(@v_diasFalta,  0) = 0
                     Then Char(32)
                     Else Replace(Convert(Varchar(20), Cast(@v_diasFalta As Money), 1), '.00', '')
                End  diasFalta,
                Case When Isnull(@v_impFalta,  0) = 0
                     Then Char(32)
                     Else Convert(Varchar(20), Cast(@v_impFalta As Money), 1)
                End  impFalta,
                Case When @v_diasIncap = 0
                     Then Char(32)
                     Else Replace(Convert(Varchar(20), Cast(@v_diasIncap As Money), 1), '.00','')
                End  diasIncap,
                Case When @v_impIncap = 0
                     Then Char(32)
                     Else Convert(Varchar(20), Cast(@v_impIncap As Money), 1)
                End impIncap,
                Replace(Convert(Varchar(20), Cast(Isnull(@v_totalDias, 0) As Money), 1), '.00', '') totalDias,
                Convert(Varchar(20), Cast(Isnull(@v_totalMes,  0) As Money), 1) totalMes,
                Case When Substring(dbo.fn_flagfaltasIncapacidad(@PsCompania, @PnAnio, @PnMes, @v_trabajador), 1, 1) = '1'
                     Then 'INCAPACITADO'
                     When  @v_valTrabExc > 0
                     Then 'EXENTO'
                     Else Convert(Varchar(20), Cast(Isnull(Round(@v_totalMes, 0, 0), 0)As Money), 1)
                End  netoMes,
                @v_tarjeta tarjeta,
                @PsUsuario
Proximo:

      End
      Close      C_Detalle
      Deallocate C_Detalle
   End

   Begin Try
      Update dbo.Ls_FaltasIncapacidadTbl
      Set    descRegion = dbo.fn_desc_agrup_dato(@v_region, b.Region),
             depZona    = Rtrim(b.departamento) + ' - ' + Rtrim(b.Zona),
             descCiudad = b.descCiudad,
             nombre     = b.nombre
      From   dbo.Ls_FaltasIncapacidadTbl a
      Join   dbo.Ls_RepPrecioGasolinaTbl b
      On     b.compania   = a.compania
      And    b.anio       = a.anio
      And    b.mes        = a.mes
      And    b.trabajador = a.trabajador
      Where  a.compania = @PsCompania
      And    a.Anio     = @PnAnio
      And    a.Mes      = @PnMes;
   End Try

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

   Begin Try
      Update dbo.Ls_FaltasGasolinaTbl
      Set    descRegion = dbo.fn_desc_agrup_dato(@v_region, b.Region),
             depZona    = Rtrim(b.departamento) + ' - ' + Rtrim(b.Zona),
             descCiudad = b.descCiudad,
             nombre     = b.nombre
      From   dbo.Ls_FaltasGasolinaTbl    a
      Join   dbo.Ls_RepPrecioGasolinaTbl b
      On     b.compania   = a.compania
      And    b.anio       = a.anio
      And    b.mes        = a.mes
      And    b.trabajador = a.trabajador
      Where  a.compania   = @PsCompania
      And    a.Anio       = @PnAnio
      And    a.Mes        = @PnMes
      And    b.tipoLinea  = 'D';
   End Try

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

   If @PnImprime = 1
      Begin
         Select descRegion region, Case When tipoLinea = 'T'
                                        Then Rtrim(departamento) + ' - ' + Rtrim(Zona)
                                        Else Char(32)
                                   End 'DEPTO Y ZONA',
                trabajador, nombre, NSS,   descCiudad Ciudad,
                pvpLitro 'PVP Ciudad',   cantLitros Litros,    impGasMes 'GAS. MENS.',
                diasFalta 'Dias Falta', impFalta 'Importe Faltas',
                diasIncap 'Dias Incap', impIncap 'Importe Incap',
                totalDias, totalMes,   netoMes,    tarjeta
         From   dbo.Ls_RepPrecioGasolinaTbl
         Where  compania = @PsCompania
         And    Anio     = @PnAnio
         And    Mes      = @PnMes;
      End;

Salida:

   Set Xact_Abort    Off
   Return;

End;
Go


Grant  Execute On spp_Ls_RepPrecioGasolinaTbl to Public;

