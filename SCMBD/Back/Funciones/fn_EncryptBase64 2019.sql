--
-- Select dbo.fn_EncryptBase64 ('Hello Base64')
--

CREATE OR ALTER FUNCTION dbo.fn_EncryptBase64
(
    @PsInput NVARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN

    DECLARE @Resultado VARCHAR(MAX);

    SELECT @Resultado =
        CAST(N'' AS XML).value(
            'xs:base64Binary(sql:column("BinData"))',
            'VARCHAR(MAX)'
        )
    FROM
    (
        SELECT CONVERT(VARBINARY(MAX), @PsInput) AS BinData
    ) D;

    RETURN @Resultado;

END;
GO
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
