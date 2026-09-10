If Exists (Select Top 1 1
           From   Sysobjects
           Where  Uid  = 1
           And    Type = 'U'
           And    Name = 'catMenusTbl')
   Begin
      Drop Table dbo.catMenusTbl
   End
Go

Create Table dbo.catMenusTbl
  (idMenu              Integer        Not Null  Identity (1, 1),
   codigoMenu          Varchar(30)    Not Null,
   descripcion         Varchar( 100)  Not Null,
   OrdenPresentacion   Smallint       Not Null,
   idEstatus           Bit            Not Null Default(1),
   idUsuarioAct        INteger        Not Null,
   fechaAct            Datetime       Not Null Default Getdate(),
   ipAct               Varchar( 30)       Null,
   macAddressAct       Varchar( 30)       Null,
Constraint catMenusPk
Primary Key (idMenu),
Index catMenusIdx01 Unique (codigoMenu),
Constraint catMenusFk01
Foreign Key (idUsuarioAct)
References dbo.segUsuariosTbl (idUsuario) on Update Cascade on Delete Cascade) 
Go 

Grant Insert, Delete, Select, Update, References On catMenusTbl to Public;

--
-- Comentarios
--

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Catálogo de Menus relacionadas a la aplicación.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl'

Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador Correlativo del Menu relacionado a la aplicacion.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idMenu'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Codigo del Menu relacionado a la aplicacion.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'codigoMenu'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Descripción del Menú' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'descripcion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Orden de la presentacíon del Menu.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'OrdenPresentacion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador del Estatus del Registro. 1.- Activo, 0.- Inactivo.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idEstatus'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Último Usuario que realizó la actualización del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idUsuarioAct'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Última Fecha de Actualización del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'fechaAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Última Dirección IP desde donde se Actualizó el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'ipAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Última Dirección Mac desde donde se Actualizó el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'catMenusTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'macAddressAct'
Go