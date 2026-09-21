If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'V'
            And    Name = 'MenuUsuariosVw')
   Begin
      Drop View dbo.MenuUsuariosVw
   End
Go

Create View dbo.MenuUsuariosVw
As
Select a.idUsuario, a.claveUsuario, d.idMenu, d.descripcion menu,
       0 idOperacion, Char(32)  claveOperacion, d.descripcion operacion, 0 idAutorizacion, Char(32) llamada,
       d.OrdenPresentacion, 0 secuencia
From   dbo.segUsuariosTbl a
Join   dbo.segAutOPeracionesTbl b
On     a.idUsuario = b.idUsuario
Join   dbo.catOperacionesTbl c
On     b.idOperacion = c.idOperacion
Join   dbo.catMenusTbl d
On     d.idEstatus      = 1
Join   dbo.catRelMenuOperacionTbl e
On     e.idMenu      = d.idMenu
And    e.idOperacion = c.idOperacion
Where  a.idEstatus = 1
And    b.idEstatus = a.idEstatus
Union
Select a.idUsuario, a.claveUsuario, d.idMenu,  d.descripcion menu,
       c.idOperacion, c.operacion claveOperacion, Replicate(Char(32), 10) + c.descripcion operacion, b.idAutorizacion, Isnull(llamada, Char(32)),
       d.OrdenPresentacion, e.secuencia
From   dbo.segUsuariosTbl a
Join   dbo.segAutOPeracionesTbl b
On     a.idUsuario = b.idUsuario
Join   dbo.catOperacionesTbl c
On     b.idOperacion = c.idOperacion
Join   dbo.catMenusTbl d
On     d.idEstatus      = 1
Join   dbo.catRelMenuOperacionTbl e
On     e.idMenu      = d.idMenu
And    e.idOperacion = c.idOperacion
Where  a.idEstatus = 1
And    b.idEstatus = a.idEstatus;
Go

Grant Select on MenuUsuariosVw to public;

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Vista para la presentacion del menu de la aplicacion.',
   @w_nombre         Sysname       = 'MenuUsuariosVw',
   @w_xType          Char(2)       = 'V',
   @w_tipo           Varchar(128)  = 'VIEW';


Execute  sp_addextendedproperty @name       = N'MS_Description',
                                @value      = @w_valor,
                                @level0type = 'Schema',
                                @level0name = N'Dbo',
                                @level1type = @w_tipo,
                                @level1name = @w_nombre;

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador Único de Usuario de la Aplicación.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'idUsuario';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Clave Única de Usuario de la Aplicación.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'claveUsuario';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador Único del Menú Relacionado a la Aplicación.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'idMenu';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Descripción del Menú Relacionado a la Aplicación.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'menu';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador Único de la Operación Relacionado al Menú.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'idOperacion';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Código Único de la Operación Relacionado al Menú.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'claveOperacion';


Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Descripción de la Operación Relacionado al Menú.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'operacion';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador del Nivel de Autorizacion del Usuario sobre la Operación Relacionado al Menú.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'idAutorizacion';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador de la Pantalla Relacionada a la Operación.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'llamada';

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Orden de Presentación del Menú.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'OrdenPresentacion';
Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Orden de Presentación de la Operación en el Menú.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = @w_tipo,
                                   @level1name = @w_nombre,
                                   @level2type = N'Column',
                                   @level2name = N'secuencia';
Go
