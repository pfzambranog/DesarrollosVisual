/*
Declare
   @PnIdUsuario              Integer          = 2,
   @PsClaveUsuario           Varchar (50)     = 'AlbertoM@empresa.com',
   @PnIdTipoUsuario          Integer          = 1,
   @PsPrimerApellido         Varchar (100)    = 'Mendez',
   @PsSegundoApellido        Varchar (100)    = 'Pelado',
   @PsNombres                Varchar (100)    = 'Alberto Jose',
   @PsCorreo                 Varchar (260)    = 'AlbertoM@empresa',
   @PnIdEstatus              Tinyint          = 1,
   @PnIdOperacion            Integer          = 4,
   @PnIdUsuarioAct           Integer          = 1,
   @PsIpAct                  Varchar ( 30),
   @PsMacAddressAct          Varchar ( 30),
   @PnEstatus                Integer        = 0,
   @PsMensaje                Varchar (250)  = Null;

Begin
   Execute dbo.Spu_segUsuariosTbl @PnIdUsuario       = @PnIdUsuario,
                                  @PnIdTipoUsuario   = @PnIdTipoUsuario,
                                  @PsPrimerApellido  = @PsPrimerApellido,
                                  @PsSegundoApellido = @PsSegundoApellido,
                                  @PsNombres         = @PsNombres,
                                  @PsCorreo          = @PsCorreo,
                                  @PnIdEstatus       = @PnIdEstatus,
                                  @PnIdOperacion     = @PnIdOperacion,
                                  @PnIdUsuarioAct    = @PnIdUsuarioAct,
                                  @PsIpAct           = @PsIpAct,
                                  @PsMacAddressAct   = @PsMacAddressAct,
                                  @PnEstatus         = @PnEstatus Output,
                                  @PsMensaje         = @PsMensaje Output;

    Select @PnEstatus, @PsMensaje;
    Return;
End;
Go

*/
Create Or Alter Procedure dbo.Spu_segUsuariosTbl
  (@PnIdUsuario              Integer,
   @PnIdTipoUsuario          Integer         = Null,
   @PsPrimerApellido         Varchar (100)   = Null,
   @PsSegundoApellido        Varchar (100)   = Null,
   @PsNombres                Varchar (160)   = Null,
   @PsCorreo                 Varchar (260)   = Null,
   @PnIdEstatus              Tinyint         = Null,
   @PnIdOperacion            Integer,
   @PnIdUsuarioAct           Integer,
   @PsIpAct                  Varchar ( 30)   = Null,
   @PsMacAddressAct          Varchar ( 30)   = Null,
   @PnEstatus                Integer         = 0    Output,
   @PsMensaje                Varchar (250)   = Null Output)
As

Declare
   @w_desc_error              Varchar( 250),
   @w_sql                     Varchar( Max),
   @w_Error                   Integer,
   @w_fechaAct                Datetime,
   @w_comilla                 Char(1),
   @w_nombre                  Varchar(400);;

Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2024-06-12
  Descripción:    Procedimiento que actualiza los Registros a la tabla segUsuariosTbl.
  Creacion:       02-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32),
          @w_comilla         = Char(39),
          @PsIpAct           = Isnull(@PsIpAct, dbo.Fn_BuscaDireccionIP()),
          @PsMacAddressAct   = Isnull(@PsMacAddressAct, dbo.Fn_Busca_DireccionMAC());


   Select @PnEstatus = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct)
   If @PnEstatus != 0
      Begin
         Set @PsMensaje = Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If @PnIdTipoUsuario Is Not Null
      Begin
         If Not Exists (Select Top 1 1
                        From   dbo.catGeneralesTbl
                        Where  tabla   = 'segUsuariosTbl'
                        And    columna = 'idTipoUsuario'
                        And    valor   = @PnIdTipoUsuario)
            Begin
               Select @PnEstatus = 2050,
                      @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

               Set Xact_Abort Off
               Return
            End
     End


   If @PnIdEstatus Is Not Null
      Begin
         If Not Exists (Select Top 1 1
                        From   dbo.catGeneralesTbl
                        Where  tabla   = 'segUsuariosTbl'
                        And    columna = 'idEstatus'
                        And    valor   = @PnIdEstatus)
            Begin
               Select @PnEstatus = 8888,
                      @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

               Set Xact_Abort Off
               Return
           End
      End

   If @PsCorreo Is Not Null
      Begin
         If dbo.Fn_ValidaCorreo(@PsCorreo) = 0
            Begin
               Select @PnEstatus = 3010,
                      @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

               Set Xact_Abort Off
               Return
            End
     End

   If Not Exists (Select Top 1 1
                  From   dbo.segUsuariosTbl
                  Where  idUsuario    = @PnIdUsuario)
      Begin
         Select @PnEstatus = 10002,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @PnIdOperacion
                  And    idAutorizacion >= 3)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Set @w_sql = Concat('Update dbo.segUsuariosTbl ',
                       'Set    fechaAct      = ', @w_comilla, Getdate(),  @w_comilla, ', ',
                              'idUsuarioAct  = ', @PnIdUsuarioAct,  ', ',
                              'ipAct         = ', @w_comilla, @PsIpAct,         @w_comilla, ', ',
                              'macAddressAct = ', @w_comilla, @PsMacAddressAct, @w_comilla);

   If @PnIdTipoUsuario   Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', idTipoUsuario = ', @PnIdTipoUsuario);
      End

   If @PsNombres        Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ', nombres = ', @w_comilla, @PsNombres, @w_comilla);
      End

   If @PsPrimerApellido  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', primerApellido = ', @w_comilla, @PsPrimerApellido, @w_comilla);
      End

   If @PsSegundoApellido Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', segundoApellido = ', @w_comilla, @PsSegundoApellido, @w_comilla);
      End

   If @PsCorreo          Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', correo = ', @w_comilla, @PsCorreo, @w_comilla);
      End

   If @PnIdEstatus  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ', idEstatus = ', @w_comilla, @PnIdEstatus, @w_comilla);
      End

   Set @w_sql = Concat(@w_sql, ' Where idUsuario = ', @PnIdUsuario)

   Begin Try

      Execute (@w_sql);

   End Try

   Begin Catch
      Select  @w_Error      = @@Error,
              @w_desc_error = Substring (Error_Message(), 1, 230)
   End   Catch

   If Isnull(@w_Error, 0) <> 0
      Begin
         Select @PnEstatus = @w_Error,
                @PsMensaje = Concat('Error.: ', @w_Error, ' ',  @w_desc_error )

         Set Xact_Abort Off
         Return
      End


   Set Xact_Abort Off
   Return

End
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que actualiza los Registros a la tabla segUsuariosTbl.',
   @w_procedimiento  Varchar( 100) = 'Spu_segUsuariosTbl'


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
                                      @level1type = 'Procedure',
                                      @level1name = @w_procedimiento;

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'Dbo',
                                        @level1type = 'Procedure',
                                        @level1name = @w_procedimiento
   End
Go
