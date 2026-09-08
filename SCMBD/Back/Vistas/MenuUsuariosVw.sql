Create or Alter View MenuUsuariosVw
As
Select a.idUsuario, a.claveUsuario, d.idMenu, d.descripcion menu,
       0 idOperacion, Char(32)  claveOperacion, d.descripcion operacion, 0 idAutorizacion, Char(32) llamada
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
       c.idOperacion, c.operacion claveOperacion, Replicate(Char(32), 10) + c.descripcion operacion, b.idAutorizacion, Isnull(llamada, Char(32))
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
   @w_tipo           Varchar(128)  = 'View';


If Not Exists (Select Top 1 1
               From   sys.extended_properties a
               Join   sysobjects  b
               On     b.xtype   = @w_xType
               And    b.name    = @w_nombre
               And    b.id      = a.major_id)

   Begin
      Execute  sp_addextendedproperty @name       = N'MS_Description',
                                      @value      = @w_valor,
                                      @level0type = 'Schema',
                                      @level0name = N'Dbo',
                                      @level1type = @w_tipo,
                                      @level1name = @w_nombre

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'Dbo',
                                        @level1type = @w_tipo,
                                        @level1name = @w_nombre
   End
Go




