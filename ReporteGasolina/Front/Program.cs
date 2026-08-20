using System;
using System.Windows.Forms;

namespace ReporteGasolina
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var login = new FrmConexion())
            {
                var dr = login.ShowDialog();
                if (dr != DialogResult.OK)
                {
                    // Si el usuario cierra o no valida, salir de la aplicación
                    return;
                }

                // Arrancar formulario principal con credenciales provistas
                Application.Run(new FrmReporteGasolina(login.SelectedUsuario, login.SelectedCompania));
            }
        }
    }
}
