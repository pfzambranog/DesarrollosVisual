  
/*  
  
Procedimiento que ingresa valores de la tabla de Catalogos Mensajes de Errores.  
  
Declare  
   @PnIdError            Varchar(  90)   = Null,  
   @PsMensaje            Varchar( 250)   = Null,  
   @PnIdUsuarioAct       Integer         = 1,  
   @PsIpAct              Varchar(30)     = Null,  
   @PsMacAddressAct      Varchar(30)     = Null,  
   @PnEstatus            Integer         = 0,  
   @PsMensajeR           Varchar( 250)   = ' '  
  
Begin  
   Execute  dbo.Spa_catMensajesErroresTbl @PnIdError       = @PnIdError,  
                                          @PsMensaje       = @PsMensaje,  
                                          @PnIdUsuarioAct  = @PnIdUsuarioAct,  
                                          @PsIpAct         = @PsIpAct,  
                                          @PsMacAddressAct = @PsMacAddressAct,  
                                          @PnEstatus       = @PnEstatus Output,  
                                          @PsMensajeR      = @PsMensajeR Output  
  
   If @PnEstatus != 0  
      Begin  
         Select @PnEstatus, @PsMensajeR  
      End  
  
   Return  
  
End  
Go  
  
*/  
  
Create   Procedure dbo.Spa_catMensajesErroresTbl  
    (@PnIdError         Integer,  
     @PsMensaje         Varchar( 250),  
     @PnIdUsuarioAct    Integer,  
     @PsIpAct           Varchar(30)   = Null,  
     @PsMacAddressAct   Varchar(30)   = Null,  
     @PnEstatus         Integer       = 0   Output,  
     @PsMensajeR        Varchar( 250) = ' ' Output)  
As  
Declare  
    @w_sql               Varchar(Max),  
    @w_desc_error        Varchar( 250),  
    @w_Error             Integer,  
    @w_comilla           Char(1),  
    @w_registros         Integer,  
    @w_fechaAct          Datetime,  
    @w_ipAct             Varchar(30),  
    @w_macAddressAct     Varchar(30);  
  
Begin  
/*  
Objetivo: Dar de Alta al Catálogo de Errores.  
Version:  1  
*/  
  
   Set Nocount       On  
   Set Xact_Abort    On  
   Set Ansi_Nulls    On  
  
   Select @PnEstatus       = 0,  
          @PsMensajeR      = Null,  
          @w_comilla       = Char(39),  
          @w_fechaAct      = Getdate(),  
          @w_ipAct         = Isnull(@PsipAct,            dbo.Fn_BuscaDireccionIP()),  
          @w_macAddressAct = Isnull(@w_macAddressAct, dbo.Fn_Busca_DireccionMac());  
  
--  
  
   Set @PnEstatus = dbo.Fn_ValidaUsuario(@PnIdUsuarioAct)  
  
   If @PnEstatus <> 0  
      Begin  
         Set @PsMensajeR =  dbo.Fn_Busca_MensajeError(@PnEstatus)  
  
         Set Xact_Abort Off  
         Return  
      End  
--  
  
   If Isnull(@PsMensaje, '') = ''  
      Begin  
  
         Select @PnEstatus  = 8103,  
                @PsMensajeR = dbo.Fn_Busca_MensajeError(@PnEstatus)  
  
         Set Xact_Abort Off  
         Return  
      End  
  
   If Exists ( Select top 1 1  
               From   dbo.catMensajesErroresTbl  
               Where  idError = @PnIdError)  
      Begin  
         Select @PnEstatus  = 2601,  
                @PsMensajeR = dbo.Fn_Busca_MensajeError(@PnEstatus)  
  
         Set Xact_Abort Off  
         Return  
      End  
        
   If Exists ( Select top 1 1  
               From   dbo.catMensajesErroresTbl  
               Where  mensaje = @PsMensaje)  
      Begin  
         Select @PnEstatus  = 2601,  
                @PsMensajeR = dbo.Fn_Busca_MensajeError(@PnEstatus)  
  
         Set Xact_Abort Off  
         Return  
      End  
       
  
   Set @w_sql = Concat('Select ', @PnIdError,      ', ',  @w_comilla, @PsMensaje,  @w_comilla , ', ',  
                                  @PnIdUsuarioAct, ', ',  @w_comilla, @w_fechaAct, @w_comilla,  ', ',  
                                  @w_comilla, @w_ipAct, @w_comilla, ', ', @w_comilla, @w_macAddressAct, @w_comilla )  
  
   Begin Try  
      Insert Into dbo.catMensajesErroresTbl  
      (idError, mensaje, idUsuarioAct, fechaAct,  
       ipAct,   macAddressAct)  
      Execute (@w_sql)  
  
      Set  @w_registros = @@Rowcount  
   End Try  
  
   Begin Catch  
      Select  @w_Error      = @@Error,  
              @w_desc_error = Substring (Error_Message(), 1, 230)  
   End   Catch  
  
   If Isnull(@w_Error, 0) <> 0  
      Begin  
         Select @PnEstatus  = @w_Error,  
                @PsMensajeR = 'Error.: ' + Rtrim(Ltrim(Cast(@w_Error As Varchar))) + ' ' + @w_desc_error  
         Set Xact_Abort Off  
         Return  
      End  
  
   If @w_registros  = 0  
      Begin  
         Select @PnEstatus  = 6204,  
                @PsMensajeR = dbo.Fn_Busca_MensajeError(@PnEstatus)  
  
         Set Xact_Abort Off  
         Return  
      End  
  
   Set Xact_Abort Off  
   Return  
End  
Go
