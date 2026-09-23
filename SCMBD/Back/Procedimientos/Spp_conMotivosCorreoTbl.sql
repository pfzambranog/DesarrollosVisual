/*

Declare
   @PsJasonIn                Varchar(Max),
   @PsOperacion              Varchar( 20)   = 'SU1011',
   @PnIdUsuarioAct           Integer        = 3,
   @PsIpAct                  Varchar ( 30)  = Null,
   @PsMacAddressAct          Varchar ( 30)  = Null,
   @PnEstatus                Integer        = 0,
   @PsMensaje                Varchar (Max)  = Char(32);

Begin
   Set   @PsJasonIn = '[{"idMotivo":1, "descripcion":"Incidencias BD", "titulo":"Incidencias BD", "cuerpo":"", "html":"Null", "url":"", "perfilCorreo":"Incidencias BD",
                         "permiteCompartir":"", "idEstatus":""}]'

   Execute dbo.Spp_conMotivosCorreoTbl @PsJasonIn        = @PsJasonIn,
                                       @PsOperacion      = @PsOperacion,
                                       @PnIdUsuarioAct   = @PnIdUsuarioAct,
                                       @PsIpAct          = @PsIpAct,
                                       @PsMacAddressAct  = @PsMacAddressAct,
                                       @PnEstatus        = @PnEstatus Output,
                                       @PsMensaje        = @PsMensaje Output;

   If @PnEstatus != 0
      Begin
         Select @PnEstatus As Estatus, @PsMensaje As Mensaje
     End

   Return

End
Go
*/

Create or Alter Procedure Spp_conMotivosCorreoTbl
  (@PsJasonIn                Varchar (Max),
   @PsOperacion              Varchar ( 20),
   @PnIdUsuarioAct           Integer,
   @PsIpAct                  Varchar ( 30)  = Null,
   @PsMacAddressAct          Varchar ( 30)  = Null,
   @PnEstatus                Integer        = 0    Output,
   @PsMensaje                Varchar (Max)  = Null Output)
As

Declare
   @w_desc_error             Varchar( 250),
   @w_Error                  Integer,
   @w_registros              Integer,
   @w_sec                    Integer,
   @w_secuencia              Smallint,
   @w_identificador          Integer,
   @w_idOperacionAct         Integer,
   @w_linea                  Integer,
   @w_idEstatus              Tinyint,
   @w_idEstatus2             Tinyint,
--
   @w_idMotivo               Integer,
   @w_descripcion            Varchar(  100),
   @w_titulo                 Varchar( 1000),
   @w_cuerpo                 NVarchar( Max),
   @w_html                   NVarchar( Max),
   @w_URL                    Varchar( 1000),
   @w_perfilCorreo           Sysname,
   @w_permiteCompartir       Tinyint;


Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que procesa Altas / Actualizaciones de los Registros de la tabla conMotivosCorreoTbl.
  Creacion:       22-sep-2026.
  Version:        1.0
*/

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32),
          @w_sec             = 0,
          @w_linea           = 0,
          @PsIpAct           = Isnull(@PsIpAct,         dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct   = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMAC());


   Select @PnEstatus = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct)
   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Select top 1 @w_idOperacionAct = idOperacion
   From   dbo.catOperacionesTbl
   Where  operacion = @PsOperacion;

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @w_idOperacionAct
                  And    idAutorizacion >= 2)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Isjson(@PsJasonIn) != 1
      Begin
         Select @PnEstatus = 722,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

--
-- Creación de Tablas Temporales.
--

   Create Table #Temp_Json
  (secuencia           Integer        Not Null Identity(1, 1) Primary Key,
   idMotivo            Integer            Null,
   descripcion         Varchar(  100)     Null,
   titulo              Varchar( 1000)     Null,
   cuerpo              NVarchar( Max)     Null,
   html                NVarchar( Max)     Null,
   URL                 Varchar(1000)      Null,
   perfilCorreo        Sysname            Null,
   permiteCompartir    Tinyint            Null,
   idEstatus           Tinyint            Null)

   Create Table #TempError
  (sec                 Integer        Not Null  Identity(1, 1) Primary Key,
   idMotivo            Integer            Null,
   descripcion         Varchar(  100)     Null,
   titulo              Varchar( 1000)     Null,
   cuerpo              NVarchar( Max)     Null,
   html                NVarchar( Max)     Null,
   URL                 Varchar(1000)      Null,
   perfilCorreo        Sysname            Null,
   permiteCompartir    Tinyint            Null,
   idEstatus           Tinyint            Null,
   error               Integer            Null,
   mensaje             Varchar(Max)       Null)

--

   Begin Try
      Insert Into #Temp_Json
     (idMotivo, descripcion, titulo,       cuerpo,
      html,     URL,         perfilCorreo, permiteCompartir,
      idEstatus)
      Select idMotivo, descripcion, titulo,       cuerpo,
             html,     URL,         perfilCorreo, permiteCompartir,
             idEstatus
      From   Openjson(@PsJasonIn)
      With  (idMotivo            Integer           '$.idMotivo',
             descripcion         Varchar( 100)     '$.descripcion',
             titulo              Varchar(1000)     '$.titulo',
             cuerpo              NVarchar( Max)    '$.cuerpo',
             html                NVarchar( Max)    '$.html',
             URL                 Varchar(1000)     '$.URL',
             perfilCorreo        Varchar(1000)     '$.perfilCorreo',
             permiteCompartir    Sysname           '$.permiteCompartir',
             idEstatus           Tinyint           '$.idEstatus');
   
      Set @w_registros = @@Identity;
   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = Concat('Error.: ', @w_Error, '-', @w_desc_error);
         Set Xact_Abort Off
         Return
      End
      
   If @w_registros = 0
      Begin
         Select @PnEstatus = 722,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   While @w_sec < @w_registros
   Begin
      Set @w_sec = @w_sec + 1

      Select @w_idMotivo         = idMotivo,
             @w_descripcion      = descripcion,
             @w_titulo           = titulo,
             @w_cuerpo           = cuerpo,
             @w_html             = html,
             @w_URL              = URL,
             @w_perfilCorreo     = perfilCorreo,
             @w_permiteCompartir = permiteCompartir,
             @w_idEstatus        = idEstatus
      From   #Temp_Json
      Where  secuencia = @w_sec;
      If @@Rowcount = 0
         Begin
            Break
         End

      Select @PnEstatus       = 0,
             @PsMensaje       = Char(32),
             @w_identificador = 1;

--
-- Validación de Id Estatus proveniente del Json.
--

      If @w_idEstatus Is Not Null
         Begin
            If Not Exists ( Select top 1 1
                            From   dbo.catGeneralesTbl
                            Where  tabla   = 'conMotivosCorreoTbl'
                            And    columna = 'idEstatus'
                            And    valor   = @w_idEstatus)
               Begin
                  Select @PnEstatus = 8888,
                         @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);

                  Insert Into #TempError
                 (idMotivo,    descripcion, titulo,       cuerpo,
                  html,        URL,         perfilCorreo, permiteCompartir,
                  idEstatus,   error,       mensaje)
                  Select @w_idMotivo,  @w_descripcion, @w_titulo,       @w_cuerpo,
                         @w_html,      @w_URL,         @w_perfilCorreo, @w_permiteCompartir,
                         @w_idEstatus, @PnEstatus,
                         @PsMensaje;

                  Goto Proximo;
               End;
         End;

--

      If Exists (Select Top 1 1
                 From   dbo.conMotivosCorreoTbl
                 Where  idMotivo  = @w_idMotivo)
         Begin
            Set @w_identificador = 2;
         End;
      Else
          Begin
            Set @w_identificador = 1;
         End;

      If @w_identificador = 1
         Begin
            Execute dbo.spa_conMotivosCorreoTbl @PsDescripcion      = @w_Descripcion,
                                                @PsTitulo           = @w_titulo,
                                                @PsCuerpo           = @w_cuerpo,
                                                @PsHtml             = @w_html,
                                                @PsUrl              = @w_url,
                                                @PsPerfilCorreo     = @w_perfilCorreo,
                                                @PbPermiteCompartir = @w_permiteCompartir,
                                                @PsOperacion        = @PsOperacion,
                                                @PnIdUsuarioAct     = @PnIdUsuarioAct,
                                                @PsIpAct            = @PsIpAct,
                                                @PsMacAddressAct    = @PsMacAddressAct,
                                                @PnEstatus          = @PnEstatus         Output,
                                                @PsMensaje          = @PsMensaje         Output;
         End
      Else
         Begin
            Execute dbo.spu_conMotivosCorreoTbl @PnIdMotivo         = @w_idMotivo,
                                                @PsDescripcion      = @w_descripcion,
                                                @PsTitulo           = @w_titulo,
                                                @PsCuerpo           = @w_cuerpo,
                                                @PsHtml             = @w_html,
                                                @PsUrl              = @w_url,
                                                @PsPerfilCorreo     = @w_perfilCorreo,
                                                @PbPermiteCompartir = @w_permiteCompartir,
                                                @PbIdEstatus        = @w_idEstatus,
                                                @PsOperacion        = @PsOperacion,
                                                @PnIdUsuarioAct     = @PnIdUsuarioAct,
                                                @PsIpAct            = @PsIpAct,
                                                @PsMacAddressAct    = @PsMacAddressAct,
                                                @PnEstatus          = @PnEstatus         Output,
                                                @PsMensaje          = @PsMensaje         Output;
         End

      If @PnEstatus != 0
         Begin
            Insert Into #TempError
           (idMotivo,    descripcion, titulo,       cuerpo,
            html,        URL,         perfilCorreo, permiteCompartir,
            idEstatus,   error,       mensaje)
            Select @w_idMotivo,  @w_descripcion, @w_titulo,       @w_cuerpo,
                   @w_html,      @w_URL,         @w_perfilCorreo, @w_permiteCompartir,
                   @w_idEstatus, @PnEstatus,
                   @PsMensaje;
        End

Proximo:

   End;

   If Exists ( Select Top 1 1
               From   #TempError)
      Begin
         Select @PnEstatus = 1,
                @PsMensaje = (Select idMotivo,    descripcion, titulo,       cuerpo,
                                     html,        URL,         perfilCorreo, permiteCompartir,
                                     idEstatus,   error,       mensaje
                              From   #TempError
                              Order  By sec
                              For    Json Path);
      End

   Set Xact_Abort Off
   Return;

End
Go

Grant Execute on Spp_conMotivosCorreoTbl to public;

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que procesa altas / Actualizaciones de los Registros de la tabla conMotivosCorreoTbl.',
   @w_procedimiento  Varchar( 100) = 'Spp_conMotivosCorreoTbl',
   @w_tipo           Varchar(  20) = 'Procedure';

If Not Exists (Select Top 1 1
               From   sys.extended_properties a
               Join   sysobjects  b
               On     b.xtype   = 'P'
               And    b.name    = @w_procedimiento
               And    b.id      = a.major_id)

   Begin
      Execute  sp_addextendedproperty @name       = N'MS_Description',
                                      @value      = @w_valor,
                                      @level0type = 'Schema',
                                      @level0name = N'Dbo',
                                      @level1type = @w_tipo,
                                      @level1name = @w_procedimiento;

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'Dbo',
                                        @level1type = @w_tipo,
                                        @level1name = @w_procedimiento
   End
Go
