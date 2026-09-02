If Exists (Select Top 1 1
           From   Sysobjects
           Where  Uid  = 1
           And    Type = 'U'
           And    Name = 'catOperacionesTbl')
   Begin
      Drop Table dbo.catOperacionesTbl
   End
Go

Create Table dbo.catOperacionesTbl
  (idOperacion         Integer        Not Null    Identity(1, 1),
   operacion           Varchar(  20)  Not Null,
   descripcion         Varchar( 100)  Not Null,
   llamada             Varchar(  20)      Null,
   ruta                Varchar( 512)      Null,
   idEstatus           Bit            Not Null Default(1),
   idUsuarioAct        INteger        Not Null,
   fechaAct            Datetime       Not Null Default Getdate(),
   ipAct               Varchar( 30)       Null,
   macAddressAct       Varchar( 30)       Null,
Constraint catOperacionesPk
Primary Key (idOperacion),
Index catOperacionesIdx01 Unique (operacion),
Constraint catOperacionesFk01
Foreign Key (idUsuarioAct)
References dbo.segUsuariosTbl (idUsuario) on Update Cascade on Delete Cascade) 
Go 

Grant Insert, Delete, Select, Update, References On catOperacionesTbl to Public;

--
-- Comentarios
--

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Catálogo de Operaciones relacionadas a la aplicación.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl'

Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador Correlativo de la Operacion relacionada a la aplicacion.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idOperacion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Nombre de la operacion' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'operacion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Descripción de la Operacion' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'descripcion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Llamada a una Operacion secundaria a ejecutar.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'llamada'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ruta fisica de Alojamiento del Ejecutable.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'ruta'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador del Estatus del Registro. 1.- Activo, 0.- Inactivo.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idEstatus'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Último Usuario que realizó la actualización del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idUsuarioAct'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Última Fecha de Actualización del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'fechaAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Última Dirección IP desde donde se Actualizó el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'ipAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Última Dirección Mac desde donde se Actualizó el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'macAddressAct'
Go