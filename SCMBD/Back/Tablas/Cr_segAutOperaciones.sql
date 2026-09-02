If Exists (Select Top 1 1
           From   Sysobjects
           Where  Uid  = 1
           And    Type = 'U'
           And    Name = 'segAutOperacionesTbl')
   Begin
      Drop Table dbo.segAutOperacionesTbl
   End
Go

Create Table dbo.segAutOperacionesTbl
  (idRelacion          Integer        Not Null  Identity(1, 1),
   idUsuario           Integer        Not Null,
   idOperacion         Integer        Not Null,
   idAutotizacion      Tinyint        Not Null  Default 1,
   idEstatus           Bit            Not Null  Default 1,
   idUsuarioAct        INteger        Not Null,
   fechaAct            Datetime       Not Null  Default Getdate(),
   ipAct               Varchar( 30)       Null,
   macAddressAct       Varchar( 30)       Null,
Constraint segAutOperacionesPk
Primary Key (idRelacion),
Index segAutOperacionesIdx01 Unique (idUsuario, IdOperacion),
Constraint segAutOperacionesFk01
Foreign Key (idUsuarioAct)
References dbo.segUsuariosTbl (idUsuario) on Delete Cascade,
Constraint segAutOperacionesFk02
Foreign Key (idOperacion) 
References dbo.catOperacionesTbl (idOperacion),
Constraint segAutOperacionesCh01
Check ()) 
Go 

Grant Insert, Delete, Select, Update, References On segAutOperacionesTbl to Public;

--
-- Comentarios
--

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Catalogo de Relacion Usuarios-Operacion.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl'

Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador Correlativo de la Ralación Usuario - Operacion.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idRelacion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador de la operacion Relacionada al Usuario.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idOperacion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador del Usuario Relacionado a la Operacion.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idUsuario'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador del Estatus del Registro. 1.- Activo, 0.- Inactivo.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idEstatus'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultimo Usuario que realizo la actualizacion del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idUsuarioAct'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultima Fecha de Actualizacion del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'fechaAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultima Direccion IP desde donde se Actualizó el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'ipAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultima Direcci�n Mac desde donde se Actualizo el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segAutOperacionesTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'macAddressAct'
Go