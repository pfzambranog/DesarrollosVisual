/*
Declare
   @PsOperacion             Varchar ( 20)   = 'SU1012',
   @PnIdUsuarioAct          Integer         = 3,
   @PnEstatus               Integer         = 0,
   @PsMensaje               NVarchar( Max)  = Null;

Begin
   Execute dbo.Spc_ObjetosReferenciasRotas @PsOperacion       = @PsOperacion,
                                           @PnIdUsuarioAct    = @PnIdUsuarioAct,
                                           @PnEstatus         = @PnEstatus      Output,
                                           @PsMensaje         = @PsMensaje      Output;

   Select @PnEstatus As IdError, @PsMensaje As Mensaje;
   Return;
End;
Go
*/

Create or Alter Procedure dbo.Spc_ObjetosReferenciasRotas
  (@PsOperacion              Varchar ( 20),
   @PnIdUsuarioAct           Integer,
   @PnEstatus                Integer        = 0    Output,
   @PsMensaje                NVarchar (Max)  = Null Output)
With Execute AS Owner
As

Declare
   @w_desc_error             Varchar( 250),
   @w_Error                  Integer,
   @w_idOperacionAct         Integer,
   @w_registros              Integer;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que genera el Reporte de Objetos con Errores / Dependencias Rotas.
  Creacion:       18-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus         = 0,
          @PsMensaje         = Char(32);

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

--

   Set @PsMensaje = (Select Schema_name(o.schema_id) Esquema,
                            o.name                   NombreObjeto,
                            Case o.type When 'P'
                                        Then 'Procedimiento'
                                        When 'FN'
                                        Then 'Función Escalar'
                                        When 'IF'
                                        Then 'Función Tabular'
                                        When 'TF'
                                        Then 'Función Tabular'
                                        When 'V'
                                        Then 'Vista'
                                        Else o.type
                            End TipoObjeto,
                            d.referenced_entity_name    ObjetoReferenciado,
                            'No existe o fue modificado' DetalleError
                     From   sys.sql_expression_dependencies d
                     Join   sys.all_objects o
                     On     o.object_id    = d.referencing_id
                     Where  d.is_ambiguous = 0
                     And    d.referenced_id IS NULL
                     And    o.type IN ('P','FN','IF','TF','V')
                     Order  By TipoObjeto, Esquema, NombreObjeto
                     For    Json Path);

   Set @w_registros = @@Rowcount

   If @w_registros = 0
      Begin
         Select @PnEstatus = 0,
                @PsMensaje = 'No Existen Objetos con Dependencias Rotas';
      End
   Else
      Begin
         Set @PnEstatus = 1;
      End

   Set Xact_Abort Off
   Return

End
Go

Grant Execute On dbo.Spc_ObjetosReferenciasRotas to Public
Go


--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento que genera el Reporte de Objetos con Errores / Dependencias Rotas.',
   @w_procedimiento  Varchar( 100) = 'Spc_ObjetosReferenciasRotas',
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

