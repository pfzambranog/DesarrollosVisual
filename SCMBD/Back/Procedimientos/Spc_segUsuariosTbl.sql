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
   Execute dbo.Spc_segUsuariosTbl @PnIdUsuario       = @PnIdUsuario,
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

    If  @PnEstatus != 0
        Begin
           Select @PnEstatus, @PsMensaje;
        End

    Return;
End;
Go

*/
Create Or Alter Procedure dbo.Spc_segUsuariosTbl
  (@PnIdUsuario              Integer         = Null,
   @PsClaveUsuario           Varchar (50)    = Null,
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
   @w_registros               Integer,
   @w_fechaAct                Datetime,
   @w_comilla                 Char(1);

Begin
/*
  Autor:          Pedro Zambrano
  Fecha:          2024-06-12
  Descripción:    Procedimiento que consulta los Registros a la tabla segUsuariosTbl.
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

   If Not Exists (Select Top 1 1
                  From   dbo.segAutOperacionesTbl
                  Where  idUsuario       = @PnIdUsuarioAct
                  And    idOperacion     = @PnIdOperacion
                  And    idAutorizacion >= 1)
      Begin
         Select @PnEstatus = 9985,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus);

         Set Xact_Abort Off
         Return
      End

   Set @w_sql = Concat('Select  a.idUsuario,       a.claveUsuario, a.idTipoUsuario, a.primerApellido, ',
                               'a.segundoApellido, a.nombres,      a.correo,        a.idEstatus, ',
                               'dbo.Fn_BuscaDescripcionGeneral(', @w_comilla, 'catGeneralesTbl', @w_comilla, ',',
                                                                  @w_comilla, 'idEstatus',       @w_comilla, ',',
                                                                  'a.idEstatus) estatus, ',
                               'dbo.Fn_BuscaNombreUsuario(a.idUsuarioAct, 1) NombreUsuarioAct, ',
                               'a.ipAct, a.macAddressAct ',
                       'From    dbo.segUsuariosTbl a ',
                       'Where   1 = 1')

   If @PnIdUsuario  Is Not Null
       Begin
         Set @w_sql = Concat(@w_sql, ' And  idUsuario = ', @PnIdUsuario);
      End

   If @PsClaveUsuario  Is Not Null
      Begin
          Set @w_sql = Concat(@w_sql, ' And claveUsuario Like ', @w_comilla, '%', trim(@PsClaveUsuario), '%', @w_comilla);
      End

   If @PnIdTipoUsuario   Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And  idTipoUsuario = ', @PnIdTipoUsuario);
      End

   If @PsNombres        Is Not Null
      Begin
         Set @w_sql = Concat(@w_sql, ' And nombres Like ', @w_comilla, '%', trim(@PsNombres), '%', @w_comilla);
      End

   If @PsPrimerApellido  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, '  And primerApellido Like ', @w_comilla, '%', trim( @PsPrimerApellido), '%', @w_comilla);
      End

   If @PsSegundoApellido Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, '  And segundoApellido Like ', @w_comilla, '%', trim( @PsSegundoApellido), '%', @w_comilla);
      End

   If @PsCorreo          Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, '  And  correo Like ', @w_comilla, '%', trim( @PsCorreo), '%', @w_comilla);
      End

   If @PnIdEstatus  Is Not Null
      Begin
        Set @w_sql = Concat(@w_sql, ' And idEstatus = ', @w_comilla, @PnIdEstatus, @w_comilla);
      End

   Set @w_sql = Concat(@w_sql, ' Order By a.idUsuario')

   Begin Try

      Execute (@w_sql);
      Set @w_registros = @@Rowcount

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

   If @w_registros = 0
      Begin
         Select @PnEstatus = 99,
                @PsMensaje = 'Error.: ' + Dbo.Fn_Busca_MensajeError(@PnEstatus)

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
   @w_valor          Varchar(1500) = 'Procedimiento que consulta los Registros a la tabla segUsuariosTbl.',
   @w_procedimiento  Varchar( 100) = 'Spc_segUsuariosTbl'


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
