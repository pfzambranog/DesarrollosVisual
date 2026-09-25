using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace SCMBD_APISEG
{
    internal static class Procesador
    {
        public static XElement Procesar(Solicitud solicitud)
        {
            var raiz = new XElement("Resultados");
            int estatus = 0;
            string mensaje = "Operación ejecutada correctamente";

            try
            {
                Validator.ValidateObject(solicitud, new ValidationContext(solicitud), true);

                // ✅ Descifrar destino usando la BD de control
                string passDestinoPlana = GestorCredenciales.DescifrarDestino(
                    solicitud.Control,
                    solicitud.Destino.PasswordCifrada);

                using (var conn = ConexionFactory.Crear(solicitud.Destino, passDestinoPlana))
                {
                    conn.Open();

                    if (solicitud.Tipo == 1)
                        ProcesarConsulta(conn, solicitud.Sql, raiz);
                    else if (solicitud.Tipo == 2)
                        ProcesarEjecucion(conn, solicitud.Sql, out estatus, out mensaje);
                }
            }
            catch (Exception ex)
            {
                estatus = 1;
                mensaje = $"{ex.GetType().Name}: {ex.Message}";
            }

            raiz.Add(new XElement("Ejecucion",
                new XElement("PnEstatus", estatus),
                new XElement("PsMensaje", mensaje)));

            return raiz;
        }

        private static void ProcesarConsulta(IDbConnection conn, string sql, XElement raiz)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandTimeout = 300;

                using (var lector = cmd.ExecuteReader())
                {
                    var cols = new XElement("Columnas");
                    for (int i = 0; i < lector.FieldCount; i++)
                    {
                        cols.Add(new XElement("Columna",
                            new XAttribute("nombre", lector.GetName(i)),
                            new XAttribute("tipo", lector.GetFieldType(i)?.FullName ?? "System.Object"),
                            new XAttribute("longitud", 0)));
                    }

                    var filas = new XElement("Filas");
                    while (lector.Read())
                    {
                        var fila = new XElement("Fila");
                        for (int i = 0; i < lector.FieldCount; i++)
                        {
                            var valor = lector.IsDBNull(i) ? "" : lector[i]?.ToString() ?? "";
                            fila.Add(new XElement(lector.GetName(i), valor));
                        }
                        filas.Add(fila);
                    }

                    raiz.Add(new XElement("Consulta", cols, filas));
                }
            }
        }

        private static void ProcesarEjecucion(IDbConnection conn, string sql, out int estatus, out string mensaje)
        {
            estatus = 0;
            mensaje = "Ejecución completada";

            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = trans;
                        cmd.CommandText = sql;
                        cmd.CommandTimeout = 300;
                        cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
    }
}
