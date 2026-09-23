/*

Declare
   @PsTable              Sysname          = 'conMotivosCorreoTbl',
   @PsValor              NVarchar(100)    = 1,
   @PnEstatus            Integer          = 0,
   @PsMensaje            Varchar(  250)   = Null;
Begin

   Execute dbo.Spc_ReferenciasTablsFk @PsTable            = @PsTable ,
                                      @PsValor            = @PsValor,
                                      @PnEstatus          = @PnEstatus         Output,
                                      @PsMensaje          = @PsMensaje         Output;

   Select @PnEstatus As IdError, @PsMensaje As "Mensaje";

   Return;

End;
Go
*/

Create Or Alter Procedure dbo.Spc_ReferenciasTablsFk
  (@PsTable              Sysname,
   @PsValor              NVarchar(100),
   @PnEstatus            Integer          = 0    Output,
   @PsMensaje            Varchar(  250)   = ' '  Output)
With Execute AS Owner
As

Declare
   @w_desc_error         Varchar( 250),
   @w_Error              Integer,
   @w_sql                NVarchar(1500),
   @w_param              NVarchar( 750),
   @w_comilla            Char(1),
   @w_secuencia          Integer,
   @w_sec                Integer,
   @w_registros          Integer,
   @w_TablaHija          Sysname,
   @w_ColumnaFK          Sysname;

Begin
/*
  Autor:          Pedro Zambrano
  Descripción:    Procedimiento Consulta las Referencias de FK de una Tabla.
  Creacion:       21-sep-2026.
  Version:        1.0
*/

   Set Quoted_identifier Off
   Set Nocount            On
   Set Xact_Abort         On
   Set Ansi_Nulls         On

   Select @PnEstatus      = 0,
          @PsMensaje      = Char(32),
          @w_comilla      = Char(39),
          @w_sec          = 0;

--
-- Creación de Tabla Temporal.
--

   Create Table #TempTablesFKTble
  (secuencia    Integer Not Null Identity(1, 1) Primary Key,
   TablaHija	Sysname Not Null,
   ColumnaFK	Sysname Not Null)

   Begin Try
      Insert Into #TempTablesFKTble
     (TablaHija, ColumnaFK)
      Select Object_name(fk.parent_object_id),
             Col_name(fkc.parent_object_id, fkc.parent_column_id)
      From   sys.foreign_keys         fk
      Join   sys.foreign_key_columns  fkc 
      On     fkc.constraint_object_id = fk.object_id
      And    Object_name(fk.referenced_object_id) = @PsTable;
      Set @w_secuencia = Scope_Identity();
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

   While @w_sec  < @w_secuencia
   Begin
      Set @w_sec = @w_sec + 1;

      Select @w_TablaHija = TablaHija,
             @w_ColumnaFK = ColumnaFK
      From   #TempTablesFKTble
      Where  secuencia = @w_sec;
      If @@Rowcount = 0
         Begin
            Break
         End
         
      Select @w_sql       = Concat('Select @o_salida = Count(1) ',
                                   'From  ', @w_TablaHija, ' ',
                                   'Where ', @w_ColumnaFK, ' = ', @w_comilla, @PsValor, @w_comilla),
             @w_param     = '@o_salida  Integer Output',
             @w_registros = 0;    

      Execute Sp_ExecuteSQL @w_sql, @w_param, @o_salida = @w_registros Output
    
      If Isnull(@w_registros, 0) = 0
         Begin
            Goto Siguiente;
         End

     If @PsMensaje = Char(32)
        Begin
           Select @PnEstatus = 1,
                  @PsMensaje =  Concat('Tablas Referencias Con Valores.: ', @w_TablaHija);
        End
     Else
        Begin
           Set @PsMensaje =  Concat(Trim(@PsMensaje), ', ',@w_TablaHija);
        End
        
Siguiente:

   End
   
   Set Xact_Abort Off
   Return

End
Go

Grant Execute On dbo.Spc_ReferenciasTablsFk to Public
Go

--
-- Comentarios.
--

Declare
   @w_valor          Varchar(1500) = 'Procedimiento Consulta las Referencias de FK de una Tabla.',
   @w_procedimiento  Varchar( 100) = 'Spc_ReferenciasTablsFk',
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
