Use SCMBD
Go

/*
Declare
   @PsView              Sysname      = 'MenuUsuariosVw',
   @PsColumna           Sysname      = 'idUsuario',
   @PbAplica            Bit          = 0,
   @PsTipoCampo         Varchar(250) = ' ',
   @PnLogitud           Smallint     = ' ',
   @PnDecimales         Smallint     = 0,
   @PsRequerido         Char(2)      = 'No',
   @PnEstatus           Integer      = 0,
   @PsMensaje           Varchar(250) = ' ';
Begin
   Execute dbo.Spc_valCamposView @PsView      = @PsView,
                                 @PsColumna   = @PsColumna,
                                 @PbAplica    = @PbAplica      Output,
                                 @PsTipoCampo = @PsTipoCampo   Output,
                                 @PnLogitud   = @PnLogitud     Output,
                                 @PnDecimales = @PnDecimales   Output,
                                 @PsRequerido = @PsRequerido   Output,
                                 @PnEstatus   = @PnEstatus     Output,
                                 @PsMensaje   = @PsMensaje     Output;
   If @PnEstatus != 0
      Begin
         Select @PnEstatus, @PsMensaje;
      End
   Else
      Begin
         Select @PbAplica     Aplica,  @PsTipoCampo  TipoCampo, 
                @PnLogitud    Logitud, @PnDecimales  Decimales,
                @PsRequerido Requerido;
      End

   Return

End
Go

*/

Create Or Alter Procedure dbo.Spc_valCamposView
   (@PsView              Sysname,
    @PsColumna           Sysname,
    @PbAplica            Bit          = 0    Output,
    @PsTipoCampo         Varchar(250) = ' '  Output,
    @PnLogitud           Smallint     = ' '  Output,
    @PnDecimales         Smallint     = 0    Output,
    @PsRequerido         Char(2)      = 'No' Output,
    @PnEstatus           Integer      = 0    Output,
    @PsMensaje           Varchar(250) = ' '  Output)

As

Declare
    @w_longitud          Integer = 0,
    @w_system_type_id    Integer,
    @w_scale             Tinyint,
    @w_desc_error        Varchar( 250),
    @w_Error             Integer,
    @w_Precision         Smallint

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento que consulta la longitud de los campos de una tabla específica.
  Creacion:       21-sep-2026.
  Version:        1.0
*/
   Set Nocount       On
   Set Xact_Abort    On
   Set Ansi_Nulls    On

   Select @PnEstatus  = 0,
          @PsMensaje  = Null;

   Begin Try
      Select @w_system_type_id = a.system_type_id,
             @w_longitud       = a.max_length,
             @w_Precision      = a.precision,
             @w_scale          = a.scale,
             @PsTipoCampo      = Upper(Cast(b.name As Varchar(250))),
             @PsRequerido      = Case When a.Is_nullable = 0
                                      Then 'SI'
                                      Else 'NO'
                                 End
      From   sys.columns a
      Join   sys.types b on b.system_type_id = a.system_type_id
      Where  a.name   = @PsColumna
      And    Exists   ( Select Top 1 1
                        From   sys.views c
                        Where  c.object_id = a.object_id
                        And    c.name      = @PsView)
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

   If @w_system_type_id not in (34, 35, 58, 59, 60, 61, 62, 104, 189)
      Begin
         Set @PbAplica = 1
         If @w_Precision = 0
            Begin
               Set @w_Precision = @w_longitud
            End

         Select @PnLogitud    = @w_Precision,
                @PnDecimales  = @w_scale
      End


   Set Xact_Abort Off
   Return
End
Go

Grant Execute On dbo.Spc_valCamposView to Public;
Go

--
-- Comentarios
--

Declare
   @w_valor          Nvarchar(250) = 'Procedimiento que consulta la longitud de los campos de una vista específica.',
   @w_procedimiento  NVarchar(250) = 'Spc_valCamposView';

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