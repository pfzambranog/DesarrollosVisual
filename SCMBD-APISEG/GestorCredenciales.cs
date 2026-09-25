using System;
using System.Data;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

namespace SCMBD_APISEG
{
    internal static class GestorCredenciales
    {
        // 🔐 CLAVE MAESTRA — 32 bytes EXACTOS para AES-256
        // NO modificar estos valores — es tu llave de protección
        private static readonly byte[] ClaveMaestra = new byte[]
        {
            0x53, 0x43, 0x4D, 0x42, 0x44, 0x2D, 0x41, 0x50,
            0x49, 0x53, 0x45, 0x47, 0x2D, 0x32, 0x30, 0x32,
            0x34, 0x2D, 0x43, 0x4C, 0x41, 0x56, 0x45, 0x2D,
            0x53, 0x45, 0x47, 0x55, 0x52, 0x41, 0x31, 0x32
        };

        /// <summary>
        /// Descifra localmente la credencial de control usando la clave maestra
        /// </summary>
        private static string DescifrarLocal(string valorCifrado)
        {
            if (string.IsNullOrWhiteSpace(valorCifrado))
                throw new ArgumentNullException(nameof(valorCifrado), "La credencial cifrada no puede estar vacía");

            byte[] datos = Convert.FromBase64String(valorCifrado);

            using (var aes = Aes.Create())
            {
                aes.Key = ClaveMaestra;

                // Los primeros 16 bytes son el vector de inicialización (IV)
                byte[] iv = new byte[16];
                Array.Copy(datos, 0, iv, 0, 16);
                aes.IV = iv;

                // El resto es el contenido cifrado
                byte[] cifrado = new byte[datos.Length - 16];
                Array.Copy(datos, 16, cifrado, 0, cifrado.Length);

                using (var ms = new System.IO.MemoryStream(cifrado))
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var sr = new System.IO.StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Conecta a la BD de control y descifra la credencial de destino
        /// </summary>
        public static string DescifrarDestino(DatosConexion bdControl, string valorCifradoDestino)
        {
            if (bdControl == null)
                throw new ArgumentNullException(nameof(bdControl));
            if (string.IsNullOrWhiteSpace(valorCifradoDestino))
                throw new ArgumentNullException(nameof(valorCifradoDestino));

            // Desciframos la contraseña de control primero
            string passControlPlana = DescifrarLocal(bdControl.PasswordCifrada);

            // Construimos cadena de conexión a la BD de control
            string cadenaControl =
                $"Server={bdControl.Servidor},{bdControl.Puerto};" +
                $"Database={bdControl.BaseDatos};" +
                $"User Id={bdControl.Usuario};" +
                $"Password={passControlPlana};" +
                "TrustServerCertificate=True;" +
                "Connection Timeout=30;";

            // Conectamos y llamamos al SP para descifrar la credencial de destino
            using (var conn = new SqlConnection(cadenaControl))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "dbo.Sp_DesencriptaCadena";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30;

                    var pEntrada = cmd.CreateParameter();
                    pEntrada.ParameterName = "@PsInputBase64";
                    pEntrada.Value = valorCifradoDestino;
                    cmd.Parameters.Add(pEntrada);

                    var pSalida = cmd.CreateParameter();
                    pSalida.ParameterName = "@PsOutputClaro";
                    pSalida.DbType = DbType.String;
                    pSalida.Size = 4000;
                    pSalida.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pSalida);

                    var pEstatus = cmd.CreateParameter();
                    pEstatus.ParameterName = "@PnEstatus";
                    pEstatus.DbType = DbType.Int32;
                    pEstatus.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pEstatus);

                    var pMensaje = cmd.CreateParameter();
                    pMensaje.ParameterName = "@PsMensaje";
                    pMensaje.DbType = DbType.String;
                    pMensaje.Size = 4000;
                    pMensaje.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pMensaje);

                    cmd.ExecuteNonQuery();

                    int estatus = pEstatus.Value as int? ?? 1;
                    string mensaje = pMensaje.Value?.ToString() ?? "Error desconocido";

                    if (estatus != 0)
                        throw new InvalidOperationException($"Descifrado fallido: {mensaje}");

                    return pSalida.Value?.ToString() ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Genera cadena cifrada — usar UNA VEZ para obtener credenciales para el JSON
        /// </summary>
        public static string CifrarLocal(string textoPlano)
        {
            if (string.IsNullOrWhiteSpace(textoPlano))
                throw new ArgumentNullException(nameof(textoPlano));

            using (var aes = Aes.Create())
            {
                aes.Key = ClaveMaestra;
                aes.GenerateIV(); // Genera IV aleatorio para cada cifrado

                using (var ms = new System.IO.MemoryStream())
                {
                    // Escribimos el IV al inicio del resultado
                    ms.Write(aes.IV, 0, 16);

                    // Ciframos el texto
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var sw = new System.IO.StreamWriter(cs))
                    {
                        sw.Write(textoPlano);
                        sw.Flush();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
}
