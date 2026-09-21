Use SCMBD
Go

/*

*/

Create Or Alter Procedure dbo.Spc_Lista_Det_Triggers
  (@PsObjeto        Sysname      = Null,
   @PsOperacion     Varchar( 20),
   @PnIdUsuarioAct  Integer,
   @PnEstatus       Integer      = 0    Output,
   @PsMensaje       Varchar(250) = ' '  Output)
With Execute AS Owner
As

Declare
   @w_Error             Integer,
   @w_idOperacionAct    Integer,
   @w_desc_error        Varchar( 250);

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que Consulta la definición de Vistas en la base de datos.
  Creacion:       19-sep-2026.
  Version:        1.0
*/

   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    Off

   Select @PnEstatus         = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct),
          @PsMensaje         = Char(32);

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

--

   Begin Try
       Select db_name() [Base de Datos],
           OBJECT_NAME(so.parent_obj) [Nombre Tabla],
           so.name                    [Nombre Trigger],
           USER_NAME(so.uid)           Propietario,
           s.name                      Esquema,
       	   Case When OBJECTPROPERTY(id, 'ExecIsTriggerDisabled')  = 0 Then 'Si' Else 'No' End Habilitado,
           Case When OBJECTPROPERTY(id, 'ExecIsInsteadOfTrigger') = 1 Then 'Si' Else 'No' End InsteadOf,
           Case When OBJECTPROPERTY(id, 'ExecIsAfterTrigger')     = 1 Then 'Si' Else 'No' End "After",
       	   Case When OBJECTPROPERTY(id, 'ExecIsInsertTrigger')    = 1 Then 'Si' Else 'No' End "Insert",
           Case When OBJECTPROPERTY(id, 'ExecIsUpdateTrigger')    = 1 Then 'Si' Else 'No' End "Update",
           Case When OBJECTPROPERTY(id, 'ExecIsDeleteTrigger')    = 1 Then 'Si' Else 'No' End "Delete"
       From  sysobjects  so
       Join  sysusers    su
       On    so.uid        = su.uid
       Join  sys.tables  t
       On so.parent_obj = t.object_id
       Join  sys.schemas s
       On t.schema_id   = s.schema_id
       WHERE so.type = 'TR'
  	   And   OBJECT_NAME(so.parent_obj)  = Iif(@PsObjeto  Is Null, OBJECT_NAME(so.parent_obj),  @PsObjeto)

    End Try

	Begin Catch
       Select  @w_Error      = @@Error,
               @w_desc_error = Substring (Error_Message(), 1, 230)
    End   Catch

    If Isnull(@w_Error, 0) <> 0
       Begin
          Select @PnEstatus = @w_Error,
                 @PsMensaje = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error

          Set Xact_Abort Off
          Return
       End


End
Go

-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Procedimiento que Consulta los disparadores declarados en la base de datos.',
   @w_procedimiento  NVarchar(250) = 'Spc_Lista_Det_Triggers';

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
                                      @level0name = N'dbo',
                                      @level1type = 'Procedure',
                                      @level1name = @w_procedimiento

   End
Else
   Begin
      Execute sp_updateextendedproperty @name       = 'MS_Description',
                                        @value      = @w_valor,
                                        @level0type = 'Schema',
                                        @level0name = N'dbo',
                                        @level1type = 'Procedure',
                                        @level1name = @w_procedimiento
   End
Go

