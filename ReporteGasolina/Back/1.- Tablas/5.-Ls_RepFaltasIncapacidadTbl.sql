-- Use adam
-- Go

If Exists ( Select  Top 1 1
            From    Sysobjects
            Where   Uid = 1
            And     Type = 'U'
            And     Name = 'Ls_RepFaltasIncapacidadTbl')
    Begin
       Drop table dbo.Ls_RepFaltasIncapacidadTbl;
    End
Go

Create table dbo.Ls_RepFaltasIncapacidadTbl
 (compania         Char(4)        Not Null,
  anio             Smallint       Not Null,
  mes              Tinyint        Not Null,
  descRegion       Varchar(100)   Not Null Default Char(32),
  depZona          Varchar( 25)   Not Null Default Char(32),
  descCiudad       Varchar(100)   Not Null Default Char(32),
  trabajador       Char(10)       Not Null,
  nombre           Varchar(100)   Not Null Default Char(32),
  fechaInicio      Date           Not Null,
  fechaTermino     Varchar( 10)   Not Null Default Char(32),
  dias             Integer        Not Null Default 0,
  diasDet          Varchar(100)   Not Null Default Char(32),
  pago             Decimal(19, 2) Not Null Default 0,
  observaciones    Varchar(100)   Not Null Default Char(32),
  tarjeta          Varchar( 30)   Not Null Default Char(32),
  usuario          Varchar( 30)   Not Null,
  fechaAct         Datetime       Not Null Default Getdate(),
 Constraint Ls_RepFaltasIncapacidadPK
Primary Key (compania, anio, mes, trabajador, fechaInicio),
Constraint Ls_RepFaltasIncapacidadFk01
Foreign Key (compania)
References dbo.companias (compania) On Delete Cascade,
Constraint Ls_RepFaltasIncapacidadFk02
Foreign Key (compania, trabajador)
References dbo.trabajadores_grales (compania, trabajador) On Delete Cascade)
Go

Grant Select, Insert, Update, Delete, References On Ls_RepFaltasIncapacidadTbl To Public
Go

--
-- Comentarios
--

Declare
   @v_existe    Bit     = 0,
   @v_table     Sysname = 'Ls_RepFaltasIncapacidadTbl';

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
                                @value      = N'Fecha Inicio Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'fechaInicio';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Fecha Termino Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'fechaTermino';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Cantidad de Días de Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'dias';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Detalle de Días de Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'diasDet';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Importe de los Dias de Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'pago';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Observaciones sobre el pago de la Incapacidad',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'observaciones';

Execute sp_addExtendedproperty  @name       = N'Ms_descripcion',
                                @value      = N'Tarjeta donde se deposita la Asignación por Gasolina del Mes',
                                @level0type = N'Schema',
                                @level0name = N'dbo',
                                @level1type = N'Table',
                                @level1name = @v_table,
                                @level2type = N'column',
                                @level2name = N'tarjeta';

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
