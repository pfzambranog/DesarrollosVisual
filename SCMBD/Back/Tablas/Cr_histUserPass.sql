-- Use SCMBD
-- Go

If Exists (Select Top 1 1
           From   Sysobjects
           Where  Uid  = 1
           And    Type = 'U'
           And    Name = 'histUserPassTbl')
   Begin
      Drop Table dbo.histUserPassTbl
   End
Go

Create Table dbo.histUserPassTbl
(idUsuario     Integer      Not Null,
 secuencia     Smallint     Not Null,
 contrasenia   Varchar(Max) Not Null,
 fechaAct      Datetime     Not Null Default Getdate(),
 Constraint histUserPassPk
 Primary Key Clustered 
(idUsuario, secuencia),
 Constraint histUserPassFk01
 Foreign  Key (idUsuario)
 References dbo.segUsuariosTbl On Delete Cascade )
Go

--
-- Cometarios.
--

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Historico de movimientos de la actualización de la contraseña del usuarios',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'histUserPassTbl';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador Único de Seguridad de Usuario',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'histUserPassTbl',
                                   @level2type = N'Column',
                                   @level2name = N'idUsuario';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Secuencia del Historial de Contraseñas usadas.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'histUserPassTbl',
                                   @level2type = N'Column',
                                   @level2name = N'secuencia';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Contraseñas usadas por el Usuario.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'histUserPassTbl',
                                   @level2type = N'Column',
                                   @level2name = N'contrasenia';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Fecha de Actualizacion de la Contraseña del Usuario.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'histUserPassTbl',
                                   @level2type = N'Column',
                                   @level2name = N'fechaAct';
Go