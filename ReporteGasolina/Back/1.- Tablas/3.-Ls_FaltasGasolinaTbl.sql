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
