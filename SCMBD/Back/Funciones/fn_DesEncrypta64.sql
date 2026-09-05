--
-- Select dbo.fn_DesEncrypta64 ('SGVsbG8gQmFzZTY0')
--

-- =============================================
-- FUNCIÓN DE DESENCRIPTAR BASE64 (Corregida)
-- =============================================
Create Or Alter Function dbo.fn_DesEncrypta64
  (@PsInput    Varchar(Max))
Returns NVarchar(Max)
As
Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2026-09-03
  Descripción:    Decodifica Base64 a cadena original (Unicode).
  Version:        1.1
*/
   Declare
      @w_binario   Varbinary(Max);
   Begin
      Set @w_binario = Cast('' As Xml).value('xs:base64Binary(sql:variable("@PsInput"))', 'Varbinary(Max)');

      Return Cast(@w_binario As NVarchar(Max));
   End;

End;
Go


--
-- Comentarios
--

Declare
   @w_valor       Nvarchar(250) = 'Funcion que Desencripta String de Base 64',
   @w_objeto      NVarchar(250) = 'fn_DesEncrypta64';

If Not Exists (Select Top 1 1
               From   sys.extended_properties a
               Join   sysobjects  b
               On     b.xtype   = 'Fn'
               And    b.name    = @w_objeto
               And    b.id      = a.major_id)
   Begin
      Execute  sp_addextendedproperty @name       = N'MS_Description',
                                      @value      = @w_valor,
                                      @level0type = 'Schema',
                                      @level0name = N'dbo',
                                      @level1type = 'Function',
                                      @level1name = @w_objeto

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'dbo',
                                        @level1type = 'Function',
                                        @level1name = @w_objeto
   End
Go
