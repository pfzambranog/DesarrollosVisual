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
