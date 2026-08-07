Use adam
Go

--
-- Objetivo.: Generación de Operación Reporte de Gasolina.
-- Fecha:     01/06/2026
-- Version:   1
-- Programador: Pedro Zambrano.
--

Declare
   @v_operacion   Char(8),
   @v_secuencia   Integer;

Begin
   Set @v_operacion = 'FPLS001';

   Delete aut_operaciones
   Where  operacion = @v_operacion;

   Delete parametros_oper
   Where  operacion = @v_operacion;

   Delete rel_oper_menus
   Where  ejecuta_operacion = @v_operacion;

   Delete rel_oper_modulos
   Where  operacion = @v_operacion;

   Delete operaciones
   Where  operacion = @v_operacion;


   Insert Into operaciones
   (operacion, descripcion_ope, icono, tipo_operacion, ejecuta_llamada, parametros, ruta, tipo_Ejecucion)
   Select @v_operacion, 'Generacion Reporte Gasolina', 1, 2, 'RepGas.exe', 1, '', 0;

   Insert into parametros_oper
   (operacion, secuencia, tipo_campo, parametro)
   Select @v_operacion, 1, 5, 'DSN'
   Union
   Select @v_operacion, 2, 3, 'User'
   Union
   Select @v_operacion, 3, 4, 'Password'
   Union
   Select @v_operacion, 4, 2, 'Compania';

  Insert Into rel_oper_modulos
  (operacion, modulo)
   Select @v_operacion, 'NS';

--
  Select @v_secuencia = max(secuencia)
  From   rel_oper_menus
  Where  modulo = 'NS'
  And    Menu   = 20;
  
  Insert Into rel_oper_menus
 (modulo,       menu, secuencia, tipo_llamada,
  ejecuta_menu, ejecuta_operacion)
  Select 'NS', 20, Isnull(@v_secuencia, 0) + 1, 2, 
          null,   @v_operacion;

   Insert Into aut_operaciones
  (usuario, operacion, nivel_seguridad)
   Select 'ADAM', @v_operacion, 4;
   
   Return;

End;
Go
