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
   Set   @PsJasonIn = '[{"codRegla":"Test01","nombreRegla":"Regla de Prueba","descripcion":"Test Reglas de Prueba", "esRequerido":0, "valorMinimo":1, "idEstatus":1}]'

   Execute dbo.Spp_segReglasContrasenaTbl @PsJasonIn        = @PsJasonIn,
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

Create or Alter Procedure Spp_segReglasContrasenaTbl
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
   @w_idEstatus              Tinyint,
   @w_idEstatus2             Tinyint,
   @w_codRegla               Varchar( 10),
   @w_nombreRegla            Varchar(100),
   @w_descripcion            Varchar(500),
   @w_esRequerido            Bit,
   @w_valorMinimo            Integer;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que procesa Altas / Actualizaciones de los Registros de la tabla segReglasContrasenaTbl.
  Creacion:       16-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32),
          @w_sec             = 0,
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

   Create Table #Temp_Json
  (sec                 Integer      Not Null Identity(1, 1) Primary Key,
   codRegla            Varchar( 10)     Null,
   nombreRegla         Varchar(100)     Null,
   descripcion         Varchar(500)     Null,
   esRequerido         Bit              Null,
   valorMinimo         Integer          Null,
   idEstatus           Tinyint          Null)

   Create Table #TempError
  (sec                 Integer      Not Null  Primary Key,
   codRegla            Varchar( 10)     Null,
   nombreRegla         Varchar(100)     Null,
   descripcion         Varchar(500)     Null,
   esRequerido         Bit              Null,
   valorMinimo         Integer          Null,
   idEstatus           Tinyint          Null,
   error               Integer          Null,
   mensaje             Varchar(Max)     Null)

   Insert Into #Temp_Json
  (codRegla, nombreRegla, descripcion, esRequerido, valorMinimo, idEstatus)
   Select codRegla, nombreRegla, descripcion, esRequerido, valorMinimo, idEstatus
   From   Openjson(@PsJasonIn)
   With  (codRegla        Varchar( 10)      '$.codRegla',
          nombreRegla     Varchar(100)      '$.nombreRegla',
          descripcion     Varchar(500)      '$.descripcion',
          esRequerido     Bit               '$.esRequerido',
          valorMinimo     Integer           '$.valorMinimo',
          idEstatus       Tinyint           '$.idEstatus');

   Set @w_registros = @@Identity;

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

      Select @w_codRegla    = codRegla,
             @w_nombreRegla = nombreRegla,
             @w_descripcion = descripcion,
             @w_esRequerido = esRequerido,
             @w_valorMinimo = valorMinimo,
             @w_idEstatus   = idEstatus
      From   #Temp_Json
      Where  sec = @w_sec;
      If @@Rowcount = 0
         Begin
            Break
         End

      Select @PnEstatus       = 0,
             @PsMensaje       = Char(32),
             @w_identificador = 1;

--
-- Validación de Id esRequerido proveniente del Json.
--

      If @w_esRequerido Is Not Null
         Begin
            If Not Exists (Select Top 1 1
                           From   dbo.catGeneralesTbl
                           Where  tabla   = 'segReglasContrasenaTbl'
                           And    columna = 'esRequerido'
                           And    valor   = @w_esRequerido)
               Begin
                  Select @PnEstatus = 5017,
                         @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

                  Insert Into #TempError
                 (sec,         codRegla,    nombreRegla, descripcion,
                  esRequerido, valorMinimo, idEstatus,   error,
                  mensaje)
                  Select @w_sec,          @w_codRegla,    @w_nombreRegla, @w_descripcion,
                         @w_esRequerido,  @w_valorMinimo, @w_idEstatus,   @PnEstatus,
                         @PsMensaje;
      
                  Goto Proximo;
            End
      End
      
--
-- Validación de Id Estatus proveniente del Json.
--

      If @w_idEstatus Is Not Null
         Begin
            If Not Exists ( Select top 1 1
                            From   dbo.catGeneralesTbl
                            Where  tabla   = 'segReglasContrasenaTbl'
                            And    columna = 'idEstatus'
                            And    valor   = @w_idEstatus)
               Begin
                  Select @PnEstatus = 8888,
                         @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);
      
                  Insert Into #TempError
                 (sec,         codRegla,    nombreRegla, descripcion,
                  esRequerido, valorMinimo, idEstatus,   error,
                  mensaje)
                  Select @w_sec,          @w_codRegla,    @w_nombreRegla, @w_descripcion,
                         @w_esRequerido,  @w_valorMinimo, @w_idEstatus,   @PnEstatus,
                         @PsMensaje;
      
                  Goto Proximo;
               End;
         End;
      
--

      If Exists (Select Top 1 1
                 From   dbo.segReglasContrasenaTbl
                 Where  codRegla  = @w_codRegla)
         Begin
            Set @w_identificador = 2;
         End;

      If @w_identificador = 1
         Begin
            Execute dbo.Spa_segReglasContrasenaTbl @PsCodRegla       = @w_codRegla,
                                                   @PsNombreRegla    = @w_nombreRegla,
                                                   @PsDescripcion    = @w_descripcion,
                                                   @PbEsRequerido    = @w_esRequerido,
                                                   @PnValorMinimo    = @w_valorMinimo,
                                                   @PnIdOperacionAct = @w_idOperacionAct,
                                                   @PnIdUsuarioAct   = @PnIdUsuarioAct,
                                                   @PsIpAct          = @PsIpAct,
                                                   @PsMacAddressAct  = @PsMacAddressAct,
                                                   @PnEstatus        = @PnEstatus Output,
                                                   @PsMensaje        = @PsMensaje Output;
         End
      Else
         Begin
            Execute dbo.Spu_segReglasContrasenaTbl @PsCodRegla       = @w_codRegla,
                                                   @PsNombreRegla    = @w_nombreRegla,
                                                   @PsDescripcion    = @w_descripcion,
                                                   @PbEsRequerido    = @w_esRequerido,
                                                   @PnValorMinimo    = @w_valorMinimo,
                                                   @PbIdEstatus      = @w_idEstatus,
                                                   @PnIdOperacionAct = @w_idOperacionAct,
                                                   @PnIdUsuarioAct   = @PnIdUsuarioAct,
                                                   @PsIpAct          = @PsIpAct,
                                                   @PsMacAddressAct  = @PsMacAddressAct,
                                                   @PnEstatus        = @PnEstatus Output,
                                                   @PsMensaje        = @PsMensaje Output;
         End

      If @PnEstatus != 0
         Begin
            Insert Into #TempError
           (sec,         codRegla,    nombreRegla, descripcion,
            esRequerido, valorMinimo, idEstatus,   error,
            mensaje)
            Select @w_sec,          @w_codRegla,    @w_nombreRegla, @w_descripcion,
                   @w_esRequerido,  @w_valorMinimo, @w_idEstatus,   @PnEstatus,
                   @PsMensaje;
        End

Proximo:

   End;

   If Exists ( Select Top 1 1
               From   #TempError)
      Begin
         Select @PnEstatus = 1,
                @PsMensaje = (Select sec,         codRegla,    nombreRegla, descripcion,
                                     esRequerido, valorMinimo, idEstatus,   error,
                                     mensaje
                              From   #TempError
                              For    Json Path);
      End

   Return;

End
Go

Grant Execute on Spp_segReglasContrasenaTbl to public;

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que procesa altas / Actualizaciones de los Registros de la tabla segReglasContrasenaTbl.',
   @w_procedimiento  Varchar( 100) = 'Spp_segReglasContrasenaTbl',
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
