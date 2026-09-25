using System;
using System.Data;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Oracle.ManagedDataAccess.Client;

namespace SCMBD_APISEG
{
    internal static class ConexionFactory
    {
        public static IDbConnection Crear(DatosDestino destino, string passwordPlana)
        {
            string manejador = (destino.Manejador ?? "").ToUpperInvariant();

            switch (manejador)
            {
                case "MSSQL":
                    return new SqlConnection(
                        $"Server={destino.Servidor},{destino.Puerto};" +
                        $"Database={destino.BaseDatos};" +
                        $"User Id={destino.Usuario};" +
                        $"Password={passwordPlana};" +
                        "TrustServerCertificate=True;Connection Timeout=30;");

                case "ORACLE":
                    return new OracleConnection(
                        $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)" +
                        $"(HOST={destino.Servidor})(PORT={destino.Puerto}))" +
                        $"(CONNECT_DATA=(SERVICE_NAME={destino.BaseDatos})));" +
                        $"User Id={destino.Usuario};Password={passwordPlana};");

                case "MARIADB":
                    return new MySqlConnection(
                        $"Server={destino.Servidor};Port={destino.Puerto};" +
                        $"Database={destino.BaseDatos};" +
                        $"Uid={destino.Usuario};Pwd={passwordPlana};Connection Timeout=30;");

                default:
                    throw new NotSupportedException($"Motor no soportado: {destino.Manejador}");
            }
        }
    }
}
