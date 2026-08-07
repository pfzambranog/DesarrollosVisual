-- Use Adam
--Go

If Exists ( Select Top 1 1
            From   sysobjects
            Where  Uid  = 1
            And    Type = 'Fn'
            And    Name = 'fn_desc_agrup_dato')
   Begin
      Drop Function dbo.fn_desc_agrup_dato
   End
Go

Create Function dbo.fn_desc_agrup_dato
   (@PsAgrupacion   Char(10),
    @PsDato         Char(10))
Returns Char(150)
As

Begin

-- ***************************************************************************************************************************
--
-- Nombre físico :   fn_desc_agrup_dato
-- Autor:            Pedro Zambrano
-- Fecha:            23-jul-2026.
-- Objetivo:         Funcion que consulta la descripcion del dato de la agrupación
-- Versión:          1
--
-- ***************************************************************************************************************************


   Declare
      @v_descripcion     Varchar(150);

   Begin
      Select @v_descripcion = a.descripcion
      From   dbo.datos_agr_trab a
      Where  RTrim(a.agrupacion) = @PsAgrupacion
      And    RTrim(a.dato)       = @PsDato;
      If @@Rowcount = 0
         Begin
            Set @v_descripcion = Char(32);
         End;
   End

   Return (@v_descripcion);

End;
Go
