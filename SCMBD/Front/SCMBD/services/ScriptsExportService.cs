using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SCMBD.Services
{
    public static class ScriptsExportService
    {
        /// <summary>
        /// Obtiene definición formateada de un objeto BD y la guarda en archivo .sql
        /// </summary>
        /// <param name="esquema">Ej: dbo</param>
        /// <param name="nombreObjeto">Ej: Spp_Proceso</param>
        /// <param name="tipoObjeto">Procedimiento / Función / Vista</param>
        /// <param name="cadenaConexion">Cadena de conexión</param>
        /// <param name="codOperacion">Clave de operación: SU1012</param>
        /// <param name="idUsuarioAct">ID del usuario que ejecuta</param>
        /// <returns>Ruta completa del archivo o cadena vacía si falla</returns>
        public static string ExportarDefinicionObjeto(
    string esquema,
    string nombreObjeto,
    string tipoObjeto,
    string cadenaConexion,
    string codOperacion,
    int idUsuarioAct)
        {
            try
            {
                string carpeta = ConfigurationManager.AppSettings["ScriptsDirectory"]
                              ?? @"C:\BaseDatos\SCMBD\ScriptsBD\";

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string definicion = LeerDefinicionDesdeSP(
                    esquema, nombreObjeto, codOperacion, idUsuarioAct, cadenaConexion);

                if (string.IsNullOrWhiteSpace(definicion))
                {
                    MessageBox.Show(
                        $"No se obtuvo definición para:\n{esquema}.{nombreObjeto}",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return "";
                }

                // ✅ Reemplazar CREATE → ALTER para aplicar directamente
                string definicionAlter = definicion
                    .TrimStart()
                    .Replace("Create   Procedure", "Alter   Procedure")
                    .Replace("Create Procedure", "Alter Procedure")
                    .Replace("Create   Function", "Alter   Function")
                    .Replace("Create Function", "Alter Function")
                    .Replace("Create   View", "Alter   View")
                    .Replace("Create View", "Alter View");

                string nombreArchivo = $"{nombreObjeto}.sql";
                string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("=" + new string('=', 78));
                sb.AppendLine($"-- Objeto:      {esquema}.{nombreObjeto}");
                sb.AppendLine($"-- Tipo:        {tipoObjeto}");
                sb.AppendLine($"-- Generado:    {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sb.AppendLine($"-- Usuario:     {idUsuarioAct}");
                sb.AppendLine("=" + new string('=', 78));
                sb.AppendLine();
                sb.Append(definicionAlter);
                sb.AppendLine();
                sb.AppendLine("=" + new string('=', 78));

                File.WriteAllText(rutaCompleta, sb.ToString(), Encoding.UTF8);
                return rutaCompleta;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al generar script:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        /// <summary>
        /// Abre el archivo con el programa asociado a .sql
        /// </summary>
        public static void AbrirArchivo(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo) || !File.Exists(rutaArchivo))
                return;

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = rutaArchivo,
                UseShellExecute = true
            });
        }

        #region Llamada al Procedimiento Almacenado
        private static string LeerDefinicionDesdeSP(
            string esquema,
            string nombreObjeto,
            string codOperacion,
            int idUsuarioAct,
            string cadenaConexion)
        {
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            using (SqlCommand cmd = new SqlCommand("dbo.Spc_ObtenerDefinicionObjeto", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // ✅ Parámetros de ENTRADA según estándar del proyecto
                cmd.Parameters.AddWithValue("@PsEsquema", esquema);
                cmd.Parameters.AddWithValue("@PsNombreObjeto", nombreObjeto);
                cmd.Parameters.AddWithValue("@PsOperacion", codOperacion);
                cmd.Parameters.AddWithValue("@PnIdUsuarioAct", idUsuarioAct);

                // ✅ Parámetros de SALIDA
                SqlParameter pDefinicion = new SqlParameter("@PsDefinicion", SqlDbType.NVarChar, -1)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pDefinicion);

                SqlParameter pEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pEstatus);

                SqlParameter pMensaje = new SqlParameter("@PsMensaje", SqlDbType.VarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(pMensaje);

                cn.Open();
                cmd.ExecuteNonQuery();

                int estatus = Convert.ToInt32(pEstatus.Value ?? 9999);
                string mensaje = pMensaje.Value?.ToString()?.Trim() ?? "";
                string definicion = pDefinicion.Value?.ToString() ?? "";

                // ✅ Validar resultado
                if (estatus != 0)
                {
                    MessageBox.Show(
                        $"{mensaje}\n(Estatus: {estatus})",
                        "No Disponible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return "";
                }

                return definicion;
            }
        }
        #endregion
    }
}
