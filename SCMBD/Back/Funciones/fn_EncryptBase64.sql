--
-- Select dbo.fn_EncryptBase64 ('Hello Base64')
--

Create Or Alter Function dbo.fn_EncryptBase64
  (@PsInput    NVarchar(Max))
Returns Varchar(Max)
As
Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2024-06-12 / Actualizada 2026-09-03
  Descripción:    Convierte cadena a Base64 (Unicode).
  Version:        1.0
*/
   Declare
      @o_salida    Varchar(Max),
      @w_Binary    Varbinary(Max);

   Set @w_Binary = Cast(@PsInput As Varbinary(Max));

   Set @o_salida = Cast('' As Xml).value('xs:base64Binary(sql:variable("@w_Binary"))', 'Varchar(Max)');

   Return @o_salida;
End;
Go

--
-- Comentarios
--

Declare
   @w_valor       Nvarchar(250) = 'Funcion que Encripta Cadena de Stting a Base 64',
   @w_objeto      NVarchar(250) = 'fn_EncryptBase64';

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
