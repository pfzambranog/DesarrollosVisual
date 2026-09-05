-- =============================================
-- Autor:          Pedro Zambrano
-- Fecha:          2026-09-03
-- Descripción:    Tabla de Reglas de Validación de Contraseñas
-- Version:        1.0
-- =============================================
If Exists (Select Top 1 1
           From   Sysobjects
           Where  Uid  = 1
           And    Type = 'U'
           And    Name = 'segReglasContrasenaTbl')
   Begin
      Drop Table dbo.segReglasContrasenaTbl
   End
Go

Create Table dbo.segReglasContrasenaTbl
  (idRegla             Integer         Not Null Identity(1,1),
   codRegla            Varchar(10)     Not Null,
   nombreRegla         Varchar(100)    Not Null,
   descripcion         Varchar(500)    Not Null,
   esRequerido         Bit             Not Null Default 1,
   valorMinimo         Integer             Null Default 0,
   idEstatus           Bit             Not Null Default 1,
   idUsuarioAct        INteger         Not Null,
   fechaAct            Datetime        Not Null Default Getdate(),
   ipAct               Varchar( 30)        Null,
   macAddressAct       Varchar( 30)        Null,
Constraint segReglasContrasenaPk
Primary Key Clustered (idRegla),
Index   segReglasContrasenaIdex01 Unique (codRegla)
)
Go

Grant Insert, Delete, Select, Update, References On segReglasContrasenaTbl to Public;

--
-- Comentarios
--

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Catalogo de Reglas de Validación de Contraseñas.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl'

Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador Unico de la secuencia de las reglas.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idRegla'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Código corto único de la regla.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'codRegla'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Nombre Descriptivo de la regla.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'nombreRegla'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Descripción Detallada de la regla.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'descripcion'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = '1 = Obligatoria • 0 = Opcional.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'esRequerido'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Valor Minimo de la Regla cuando es Requerida.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'valorMinimo'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Identificador del Estatus del Registro. 1.- Activo, 0.- Inactivo.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idEstatus'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultimo Usuario que realizo la actualizacion del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'idUsuarioAct'
Go


Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultima Fecha de Actualizacion del Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'fechaAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultima Direccion IP desde donde se Actualizó el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'ipAct'
Go

Execute sys.sp_addextendedproperty @name       = 'MS_Description', 
                                   @value      = 'Ultima Direcci?n Mac desde donde se Actualizo el Registro.' , 
                                   @level0type = 'Schema',
                                   @level0name = 'dbo', 
                                   @level1type = 'Table',
                                   @level1name = 'segReglasContrasenaTbl', 
                                   @level2type = 'Column',
                                   @level2name = 'macAddressAct'
Go


--
-- Llenado de la tabla.
--

Begin
   Insert Into dbo.segReglasContrasenaTbl
  (codRegla, nombreRegla, descripcion, esRequerido, valorMinimo,  idUsuarioAct,
   ipAct,    macAddressAct)
   Values
       ('VAL001', 'Validación de Contraseña', 'La contraseña se Valida. 0 = No, 1 = SI',                                0, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('LEN001', 'Longitud mínima',          'La contraseña debe contener una cantidad mínima de caracteres',          1, 8, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('MUS001', 'Contener mayúsculas',      'Debe incluir al menos una letra en mayúscula (A-Z)',                     1, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('MIN001', 'Contener minúsculas',      'Debe incluir al menos una letra en minúscula (a-z)',                     1, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('NUM001', 'Contener números',         'Debe incluir al menos un dígito numérico (0-9)',                         1, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('ESP001', 'Caracteres especiales',    'Debe incluir al menos un carácter especial: !@#$%^&*()_+-=[]{}|;:,.<>?', 1, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('USR001', 'Sin nombre de usuario',    'La contraseña no debe contener el nombre del usuario',                   0, 0, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('HIS001', 'Historial de contraseñas', 'No reutilizar las últimas contraseñas utilizadas',                       1, 5, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('DIF001', 'Diferencia con actual',    'La nueva contraseña debe ser totalmente distinta a la actual',           1, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('BLA001', 'Sin espacios en blanco',   'No se permiten espacios al inicio, en medio ni al final',                1, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
       ('CON001', 'Sin caracteres repetidos', 'Limitar repetición consecutiva del mismo carácter',                      1, 1, 1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac())

End
Go

--
-- Actualización de Mensajes de errores
--
Begin
   Delete dbo.catMensajesErroresTbl
   Where idError Between 5003 And 5012;
   
   Insert Into dbo.catMensajesErroresTbl
  (idError, mensaje, idUsuarioAct, ipAct, macAddressAct)
   Values
    (5003, 'La contraseña debe tener al menos 8 caracteres.',             1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5004, 'La contraseña debe contener al menos una letra mayúscula.',   1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5005, 'La contraseña debe contener al menos una letra minúscula.',   1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5006, 'La contraseña debe contener al menos un número.' ,            1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5007, 'La contraseña debe contener al menos un carácter especial.',  1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5008, 'La contraseña no debe contener su nombre de usuario.',        1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5009, 'No puede reutilizar las últimas 5 contraseñas.',              1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5010, 'La nueva contraseña debe ser diferente a la actual.',         1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5011, 'La contraseña no debe contener espacios en blanco.',          1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac()),
    (5012, 'No se permiten más de 2 caracteres iguales seguidos.',        1, dbo.Fn_BuscaDireccionIP(), dbo.Fn_Busca_DireccionMac());
End
Go

-- =============================================
-- CONSULTA DE VERIFICACIÓN
-- =============================================
Select 
    idRegla,
    codRegla, nombreRegla, descripcion, esRequerido, valorMinimo,  idUsuarioAct,
    ipAct,    macAddressAct
From dbo.segReglasContrasenaTbl
Order By idRegla
Go

