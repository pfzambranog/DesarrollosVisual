Use SCMBD
Go

If Exists (Select Top 1 1
           From   Sysobjects
           Where  Uid  = 1
           And    Type = 'U'
           And    Name = 'conMotivosCorreoTbl')
   Begin
      Drop Table dbo.conMotivosCorreoTbl
   End
Go


Create Table dbo.conMotivosCorreoTbl
   (idMotivo         Integer        Not Null,
	descripcion      Varchar( 100)  Not Null,
	titulo           Varchar( 1000) Not Null,
	cuerpo           NVarchar( Max) Not Null Default Char(32),
	html             NVarchar( Max)     Null,
	URL              Varchar(1000)      Null,
    perfilCorreo     Sysname        Not Null,
	permiteCompartir Bit            Not Null Default 0,
    idEstatus        Bit            Not Null Default 1,
	idUsuarioAct     Integer        Not Null,
	fechaAct         Datetime       Not Null Default Getdate(),
	ipAct            Varchar(30)        Null Default Char(32),
	macAddressAct    Varchar(30)        Null Default Char(32),
 Constraint conMotivosCorreoPk
 Primary Key Clustered (idMotivo))
 ON [PRIMARY]
Go

--
-- Comentarios.
--

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Catálogo de Configuración de Motivos de Emisión de Correos.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl'
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Código de Motivo de Emisión de Correo',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'idMotivo'
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description', 
                                   @value      = N'Decripción Motivo de Emisión de Correo',
                                   @level0type = N'Schema',@level0name=N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'descripcion'
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Titulo del Correo',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo', 
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'titulo'
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Texto del Correo',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'cuerpo'
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description', 
                                   @value      = N'Texto del Correo  en Formato HTML',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'html';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Dirección URL de Apoyo para Envío de Notificaciones',
                                   @level0type = N'Schema',@level0name=N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'URL'
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador del Correo de SYSMAIL.',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'perfilCorreo';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Bandera que Indica si el Correo puede ser Compartido', 
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'permiteCompartir';

Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Indicador de Estatus del Registro. 0 = No Habilitado, 1 = Operativo', 
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'idEstatus';

Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Identificador del Último Usuario que Actualizó el Registro',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'idUsuarioAct';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Ultima fecha de actualiazción del Registro',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'fechaAct';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Última Dirección IP desde Donde se Actualizo el Registro',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'ipAct';
Go

Execute sys.sp_addextendedproperty @name       = N'MS_Description',
                                   @value      = N'Última Dirección MAC desde Donde se Actualizo el Registro',
                                   @level0type = N'Schema',
                                   @level0name = N'dbo',
                                   @level1type = N'Table',
                                   @level1name = N'conMotivosCorreoTbl',
                                   @level2type = N'Column',
                                   @level2name = N'macAddressAct';
Go





