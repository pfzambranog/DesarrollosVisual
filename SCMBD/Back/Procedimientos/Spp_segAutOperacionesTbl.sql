/*

Declare
   @PsJasonIn                Varchar(Max),
   @PnIdOperacion            Integer        = 9,
   @PnIdUsuarioAct           Integer        = 3,
   @PsIpAct                  Varchar ( 30)  = Null,
   @PsMacAddressAct          Varchar ( 30)  = Null,
   @PnEstatus                Integer        = 0,
   @PsMensaje                Varchar (Max)  = Char(32);

Begin
   Set   @PsJasonIn = '[{
                           "Identificador": 1,
                           "IdUsuario": 0,
                           "IdOperacion": 7,
                           "IdAutorizacion": 0,
                           "IdEstatus": 0
                        },
                        {
                           "Identificador": 2,
                           "IdUsuario": 0,
                           "IdOperacion": 7,
                           "IdAutorizacion": 1,
                           "IdEstatus": 0
                        }
                       ]'

   Execute dbo.Spp_segAutOperacionesTbl @PsJasonIn        = @PsJasonIn,
                                        @PnIdOperacion    = @PnIdOperacion,
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

Create or Alter Procedure Spp_segAutOperacionesTbl
  (@PsJasonIn                Varchar(Max),
   @PnIdOperacion            Integer        = 9,
   @PnIdUsuarioAct           Integer        = 3,
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
   @w_identificador          Integer,
   @w_idusuario              Integer,
   @w_idOperacion            Integer,
   @w_idAutorizacion         Tinyint,
   @w_idEstatus              Tinyint;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que procesa Altas / Actualizaciones de los Registros de la tabla segAutOperacionesTbl.
  Creacion:       11-sep-2026.
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

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOPeracionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOPeracion     = @PnIdOPeracion
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
  (secuencia           Integer    Not Null Identity(1, 1) Primary Key,
   identificador       Integer        Null,
   idusuario           Integer        Null,
   idOperacion         Integer        Null,
   idAutorizacion      Tinyint        Null,
   idEstatus           Tinyint        Null)

   Create Table #TempError
  (secuencia           Integer    Not Null  Primary Key,
   identificador       Integer        Null,
   idusuario           Integer        Null,
   idOperacion         Integer        Null,
   idAutorizacion      Tinyint        Null,
   idEstatus           Tinyint        Null,
   error               Integer        Null,
   mensaje             Varchar(Max)   Null)

   Insert Into #Temp_Json
  (Identificador, Idusuario, IdOperacion, IdAutorizacion, IdEstatus)
   Select Identificador, IdUsuario, IdOperacion,  Isnull(IdAutorizacion, 0), Isnull(IdEstatus, 0)
   From   Openjson(@PsJasonIn)
   With  (Identificador       Integer      '$.Identificador',
          IdUsuario           Integer      '$.IdUsuario',
          IdOperacion         Integer      '$.IdOperacion',
          IdAutorizacion      Tinyint      '$.IdAutorizacion',
          IdEstatus           Tinyint      '$.idusuario');

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

      Select @w_identificador  = identificador,
             @w_idusuario      = idusuario,
             @w_idOperacion    = idOperacion,
             @w_idAutorizacion = idAutorizacion,
             @w_idEstatus      = idEstatus
      From    #Temp_Json
      Where   secuencia = @w_sec;
      If @@Rowcount = 0
         Begin
            Break
         End

      Select @PnEstatus = 0,
             @PsMensaje = Char(32);

      If @w_identificador = 1
         Begin
            Execute dbo.Spa_segAutOperacionesTbl @PnIdUsuario       = @w_idusuario,
                                                 @PnIdOperacion     = @w_idOperacion,
                                                 @PnIdAutorizacion  = @w_idAutorizacion,
                                                 @PnIdOperacionAct  = @PnIdOperacion,
                                                 @PnIdUsuarioAct    = @PnIdUsuarioAct,
                                                 @PsIpAct           = @PsIpAct,
                                                 @PsMacAddressAct   = @PsMacAddressAct,
                                                 @PnEstatus         = @PnEstatus Output,
                                                 @PsMensaje         = @PsMensaje Output;
         End
      Else
         Begin
            Execute dbo.Spu_segAutOperacionesTbl @PnIdUsuario       = @w_idusuario,
                                                 @PnIdOperacion     = @w_idOperacion,
                                                 @PnIdAutorizacion  = @w_idAutorizacion,
                                                 @PnIdEstatus       = @w_idEstatus,
                                                 @PnIdOperacionAct  = @PnIdOperacion,
                                                 @PnIdUsuarioAct    = @PnIdUsuarioAct,
                                                 @PsIpAct           = @PsIpAct,
                                                 @PsMacAddressAct   = @PsMacAddressAct,
                                                 @PnEstatus         = @PnEstatus Output,
                                                 @PsMensaje         = @PsMensaje Output;

         End

            If @PnEstatus != 0
               Begin
                  Insert Into #TempError
                 (secuencia,       identificador,  idusuario,  idOperacion,
                  idAutorizacion,  idEstatus,      error,      mensaje)
                  Select @w_sec,            @w_identificador, @w_idusuario, @w_idOperacion,
                         @w_idAutorizacion, @w_idEstatus,     @PnEstatus,   @PsMensaje
              End


   End
   
   If Exists ( Select Top 1 1
               From   #TempError)
      Begin
         Select @PnEstatus = 1,
                @PsMensaje = (Select secuencia,       identificador,  idusuario,  idOperacion,
                                     idAutorizacion,  idEstatus,      error,      mensaje
                              From   #TempError
                              For    Json Path);
      End
     
   Return;

End
Go

Grant Execute on Spa_segAutOperacionesTbl to public;

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que procesa altas / Actualizaciones de los Registros de la tabla segAutOperacionesTbl.',
   @w_procedimiento  Varchar( 100) = 'Spp_segAutOperacionesTbl',
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
