Create Or Alter Function dbo.Fn_BuscaNombreUsuario
  (@PnIdUsuario      Integer,
   @PnPresentacion   Tinyint)   -- 1 = Apellidos Nombres. 2 = Nombres Apellidos.
Returns Varchar(400)
As

Begin
   Declare
      @o_salida           Varchar(400)

   Begin
      Select @o_salida = Case When @PnPresentacion = 1
                              Then Trim(Concat(primerApellido, ' ', segundoApellido, ' ', nombres))
                              When @PnPresentacion = 2
                              Then Trim(Concat(nombres, primerApellido, ' ', segundoApellido))
                              Else Concat('Presentacion ', @PnPresentacion, '  no Impoementada')
                         End
      From   dbo.segUsuariosTbl
      Where  idUsuario  = @PnIdUsuario;
      If @@Rowcount = 0
         Begin
            Set @o_salida = 'Usuario No Registrado '
         End
   End

   Set @o_salida = Isnull(@o_salida, ' ')

   Return(@o_salida)

End
Go

--
-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Función que Busca y Concatena el nombre completo del usuario',
   @w_procedimiento  NVarchar(250) = 'Fn_BuscaNombreUsuario';

If Not Exists (Select Top 1 1
               From   sys.extended_properties a
               Join   sysobjects  b
               On     b.xtype   = 'Fn'
               And    b.name    = @w_procedimiento
               And    b.id      = a.major_id)
   Begin
      Execute  sp_addextendedproperty @name       = N'MS_Description',
                                      @value      = @w_valor,
                                      @level0type = 'Schema',
                                      @level0name = N'dbo',
                                      @level1type = 'Function',
                                      @level1name = @w_procedimiento

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'dbo',
                                        @level1type = 'Function',
                                        @level1name = @w_procedimiento
   End
Go