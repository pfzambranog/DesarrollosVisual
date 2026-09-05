--
-- Select dbo.Fn_BuscaClaveUsuario (1)
--

Create Or Alter Function dbo.Fn_BuscaClaveUsuario
  (@PnIdUsuario      Integer)
Returns Varchar(260)
As

Begin
-- =============================================
-- Autor:          Pedro Zambrano
-- Fecha:          2026-09-03
-- Descripción:    Consulta el Código del Usuario Seleccionado.
-- Uso:            Select dbo.Fn_BuscaClaveUsuario(@PnIdUsuario)
-- Version:        1.0
-- =============================================

   Declare
      @o_salida           Varchar(260);

   Begin
      Select Top 1 @o_salida = claveUsuario
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
   @w_valor          Nvarchar(250) = 'Función que Consulta el Código del Usuario Seleccionado.',
   @w_procedimiento  NVarchar(250) = 'Fn_BuscaClaveUsuario';

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