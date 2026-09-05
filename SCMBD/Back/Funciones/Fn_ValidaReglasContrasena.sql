--
-- Select dbo.Fn_ValidaReglasContrasena(1, 'SGVsbG8gQmFzZTY0')
--

Create Or Alter Function dbo.Fn_ValidaReglasContrasena
  (@PnIdUsuario       Integer,
   @PsContrasenia     Varchar(Max),
   @PbValidaUsuario   Bit)

Returns Integer
As

Begin
-- =============================================
-- Autor:          Pedro Zambrano
-- Fecha:          2026-09-03
-- Descripción:    Valida contraseña según reglas en segReglasContrasenaTbl.
--                 Devuelve 0 = Válida / Código Error = Regla incumplida
-- Uso:            Select dbo.Fn_ValidaReglasContrasena(@Usuario, @Contrasenia)
-- Version:        1.0
-- =============================================

   Declare
      @w_CodError        Integer,
      @w_valorMinimo     Integer,
      @w_pos             Integer,
      @w_len             Integer,
      @w_flag            Integer,
      @w_maxSec          Integer,
      @w_secuencia       Integer,
      @w_contrasenia     Varchar(Max),
      @w_contraseniaAnt  Varchar(Max),
      @w_claveUsuario    Varchar(260);

   Begin
      Select @w_len          = 0,
             @w_CodError     = 0,
             @w_pos          = 0,
             @w_flag         = 0,
             @w_valorMinimo  = 0,
             @w_contrasenia  = dbo.fn_DesEncrypta64(@PsContrasenia),
             @w_len          = len(@w_contrasenia),
             @w_claveUsuario = dbo.Fn_BuscaClaveUsuario(@PnIdUsuario);

    -- =============================================
    -- 0. Validación del Usuario.
    -- =============================================

      If @PbValidaUsuario = 1
         Begin
            Set @w_CodError = dbo.Fn_ValidaUsuario(@PnIdUsuario)
            If @w_CodError != 0
               Begin
                  Return @w_CodError;
               End
         End

    -- =============================================
    -- 0. Maxima Secuencia del Historico.
    -- =============================================

      Select @w_maxSec = Max(Secuencia)
      From   dbo.histUserPassTbl
      Where  idUsuario = @PnIdUsuario;
      Set @w_maxSec  = Isnull(@w_maxSec, 0)

    -- =============================================
    -- 1. Se Valida la Contraseña
    -- =============================================

      Set @w_CodError = 0;

      If Not Exists (Select Top 1 1
                     From   dbo.segReglasContrasenaTbl
                     Where  codRegla    = 'VAL001'
                     And    esRequerido = 1
                     And    valorMinimo = 1
                     And    IdEstatus   = 1)
         Begin
            Set    @w_CodError = 0;
            Return @w_CodError;
         End;

    -- ==================================================
    -- 2. LONGITUD MÍNIMA  (si regla LEN001 está activa)
    -- ==================================================

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'LEN001'
      And    esRequerido = 1
      And    IdEstatus   = 1;
      If Isnull(@w_valorMinimo, 0) > 0
         Begin
           If @w_len < @w_valorMinimo
              Begin
                 Set    @w_CodError = 5003;
                 Return @w_CodError;
              End;
         End;

    -- ======================================================================
    -- 9. Diferencia Pass Anterior con actual  (si regla DIF001 está activa)
    -- ======================================================================

      Set @w_valorMinimo = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'DIF001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            Select @w_contraseniaAnt = contrasenia
            From   dbo.histUserPassTbl
            Where  idUsuario = @PnIdUsuario
            And    secuencia = @w_maxSec;
            If @@Rowcount > 0
               Begin
                  If @w_contraseniaAnt = @PsContrasenia
                     Begin
                        Set    @w_CodError = 5010;
                        Return @w_CodError;
                     End;
              End
        End

    -- ====================================================
    -- 3. Incluir MAYÚSCULAS (si regla MUS001 está activa)
    -- ====================================================

      Select @w_valorMinimo = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'MUS001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            While @w_pos < @w_len
            Begin
               Set @w_pos = @w_pos + 1;
               If Ascii(Substring(@w_contrasenia, @w_pos, 1)) Between 65 And 90
                  Begin
                     Select @w_flag = 1,
                            @w_pos  = @w_len;
                  End

            End

            If @w_flag = 0
               Begin
                  Set @w_CodError = 5004;
                  Return @w_CodError;
               End;
         End

    -- =======================================================
    -- 4. Incluir MINÚSCULAS  (si regla MIN001 está activa)
    -- =======================================================

      Select @w_valorMinimo = 0,
             @w_flag        = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'MIN001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            While @w_pos < @w_len
            Begin
               Set @w_pos = @w_pos + 1;
               If Ascii(Substring(@w_contrasenia, @w_pos, 1)) Between 97 And 122
                  Begin
                     Select @w_flag = 1,
                            @w_pos  = @w_len;
                  End

            End

            If @w_flag = 0
               Begin
                  Set    @w_CodError = 5005;
                  Return @w_CodError;
               End;
        End


    -- ================================================
    -- 5. Inlcuir NÚMERO (si regla NUM001 está activa)
    -- ================================================

      Select @w_valorMinimo = 0,
             @w_flag        = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'NUM001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            While @w_pos < @w_len
            Begin
               Set @w_pos = @w_pos + 1;
               If Substring(@w_contrasenia, @w_pos, 1) Between '0' And '9'
                  Begin
                     Select @w_flag = 1,
                            @w_pos  = @w_len;
                  End

            End

            If @w_flag = 0
               Begin
                  Set    @w_CodError = 5006;
                  Return @w_CodError;
               End;
         End


    -- ============================================================
    -- 6. Incluir CARÁCTER ESPECIAL (si regla ESP001 está activa)
    -- ============================================================

      Select @w_valorMinimo = 0,
             @w_flag        = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla = 'ESP001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            If @w_contrasenia Not Like '%[^A-Za-z0-9]%'
               Begin
                  Set @w_CodError = 5007;
                  Return @w_CodError;
               End;
         End;

    -- =========================================================
    -- 7. SIN NOMBRE DE USUARIO (si regla VAL001 está activa)
    -- ==========================================================

      Select @w_valorMinimo = 0,
             @w_flag        = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'USR001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            If CharIndex(@w_claveUsuario, @w_contrasenia) > 0
               Begin
                   Set @w_CodError = 5008;
                   Return @w_CodError;
               End;
         End;

    -- ==========================================================
    -- 8. SIN ESPACIOS EN BLANCO (si regla BLA001 está activa)
    -- ==========================================================

      Select @w_valorMinimo = 0,
             @w_flag        = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'BLA001'
      And    esRequerido = 1
      And    IdEstatus   = 1;
      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            If CharIndex(Char(32), @w_contrasenia) > 0
               Begin
                  Set @w_CodError = 5011;
                  Return @w_CodError;
               End;
          End;

    -- ==========================================================
    -- 9. Historial de contraseñas (si regla BLA001 está activa)
    -- ==========================================================

      Select @w_valorMinimo = 0,
             @w_flag        = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'HIS001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
           If Isnull(@w_maxSec, 0) > 0
              Begin
                 While @w_pos < @w_valorMinimo
                 Begin
                    Select @w_pos       = @w_pos + 1,
                           @w_secuencia = 0;

                    Select @w_flag = Case When @PsContrasenia = contrasenia
                                          Then 1
                                          Else 0
                                     End,
                           @w_secuencia = secuencia
                    From   dbo.histUserPassTbl a
                    Where  idUsuario = @PnIdUsuario
                    And    Secuencia = (Select Max(Secuencia)
                                        From   dbo.histUserPassTbl
                                        Where  idUsuario  = a.idUsuario
                                        And    secuencia <= @w_maxSec)
                    If Isnull(@w_flag, 0) = 1
                       Begin
                          Set @w_CodError = 5009;
                          Return @w_CodError;
                       End;

                    Set @w_maxSec = @w_secuencia - 1;

                 End
              End
         End

     -- =========================================================================
    -- 10. SIN 3 CARACTERES CONSECUTIVOS IGUALES (si regla CON001 está activa)
    -- ==========================================================================

      Select @w_valorMinimo = 0,
             @w_flag        = 0,
             @w_pos         = 0;

      Select Top 1 @w_valorMinimo = valorMinimo
      From   dbo.segReglasContrasenaTbl
      Where  codRegla    = 'CON001'
      And    esRequerido = 1
      And    IdEstatus   = 1;

      If Isnull(@w_valorMinimo, 0) > 0
         Begin
            While @w_pos < @w_Len
            Begin
               Set @w_pos = @w_pos + 1;

               If Substring(@w_contrasenia, @w_pos, 1)     = Substring(@w_contrasenia, @w_pos + 1, 1) And
                  Substring(@w_contrasenia, @w_pos + 1, 1) = Substring(@w_contrasenia, @w_pos + 2, 1)
               Begin
                  Set @w_flag = 1; -- ? Encontrado 3 iguales seguidos
                  Break;
               End

               Set @w_pos = @w_pos + 1;
            End
      End

      If @w_flag = 1
         Begin
            Set @w_CodError = 5012;
            Return @w_CodError;
         End

   End

   Return @w_CodError;

End
Go