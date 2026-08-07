-- Use Adam
--Go

If Exists ( Select Top 1 1
			From   sysobjects
			Where  Uid	= 1
			And	   Type = 'Fn'
			And	   Name = 'fn_obten_nss')
   Begin
	  Drop Function dbo.fn_obten_nss
   End
Go

Create Function dbo.fn_obten_nss
   (@PsTrabajador	Char(10))
Returns Char(150)
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :	 fn_obten_nss
-- Autor:			 Pedro Zambrano
-- Fecha:			 23-jul-2026.
-- Objetivo:		 Funcion que consulta el numero de seguridad social del trabajador
-- Versión:			 1
--
-- ***************************************************************************************************************************


   Declare
	  @v_nss	 Varchar(20);

   Begin
	  Select @v_nss = Isnull(reg_seguro_social, Char(32))
	  From	 dbo.trabajadores
	  Where	 trabajador = @PsTrabajador;
	  If @@Rowcount = 0
		 Begin
			Set @v_nss = Char(32);
		 End;
   End

   Return (@v_nss);

End;
Go
