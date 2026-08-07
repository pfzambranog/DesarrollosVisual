using ReporteGasolina.Infrastructure;
using ReporteGasolina.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ReporteGasolina.Services
{
    public class GasolinaService
    {
        private readonly string _connectionString;

        public GasolinaService()
        {
            _connectionString =
                AppSettings.ConnectionString;
        }

        /// <summary>
        /// Valida un registro antes de ser cargado.
        /// Ejecuta spv_Ls_HistPrecioGasolinaTbl
        /// </summary>
        /// 

        public SpResult DepurarPeriodo(
    string compania,
    int anio,
    int mes,
    string usuario,
    string operacion)
        {
            SpResult resultado =
                new SpResult();

            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(
                        "spd_Ls_HistPrecioGasolinaTbl",
                        cn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@PsCompania",
                        compania);

                    cmd.Parameters.AddWithValue(
                        "@PnAnio",
                        anio);

                    cmd.Parameters.AddWithValue(
                        "@PnMes",
                        mes);

                    cmd.Parameters.AddWithValue(
                        "@PsUsuario",
                        usuario);

                    cmd.Parameters.AddWithValue(
                        "@PsOperacion",
                        operacion);

                    SqlParameter pStatus =
                        new SqlParameter(
                            "@PnEstatus",
                            SqlDbType.Int);

                    pStatus.Direction =
                        ParameterDirection.Output;

                    cmd.Parameters.Add(pStatus);

                    SqlParameter pMensaje =
                        new SqlParameter(
                            "@PsMensaje",
                            SqlDbType.VarChar,
                            250);

                    pMensaje.Direction =
                        ParameterDirection.Output;

                    cmd.Parameters.Add(pMensaje);

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado.IdError =
                        Convert.ToInt32(
                            pStatus.Value ?? 0);

                    resultado.MensajeError =
                        Convert.ToString(
                            pMensaje.Value ?? "");
                }
            }

            return resultado;
        }


        public SpResult ValidarPrecio(
            string compania,
            int anio,
            int mes,
            string ciudad,
            decimal precio,
            string usuario,
            string operacion)
        {
            SpResult resultado =
                new SpResult();

            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(
                        "spv_Ls_HistPrecioGasolinaTbl",
                        cn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@PsCompania", compania);

                    cmd.Parameters.AddWithValue(
                        "@PnAnio", anio);

                    cmd.Parameters.AddWithValue(
                        "@PnMes", mes);

                    cmd.Parameters.AddWithValue(
                        "@PsCiudad", ciudad);

                    cmd.Parameters.AddWithValue(
                        "@PnPrecio", precio);

                    cmd.Parameters.AddWithValue(
                        "@PsUsuario", usuario);

                    cmd.Parameters.AddWithValue(
                        "@PsOperacion", operacion);

                    SqlParameter pEstatus =
                        new SqlParameter(
                            "@PnEstatus",
                            SqlDbType.Int);

                    pEstatus.Direction =
                        ParameterDirection.Output;

                    cmd.Parameters.Add(pEstatus);

                    SqlParameter pMensaje =
                        new SqlParameter(
                            "@PsMensaje",
                            SqlDbType.VarChar,
                            250);

                    pMensaje.Direction =
                        ParameterDirection.Output;

                    cmd.Parameters.Add(pMensaje);

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado.IdError =
                        Convert.ToInt32(
                            pEstatus.Value ?? 0);

                    resultado.MensajeError =
                        Convert.ToString(
                            pMensaje.Value ?? "");
                }
            }

            return resultado;
        }

        /// <summary>
        /// Carga definitiva del precio.
        /// Ejecuta spa_Ls_HistPrecioGasolinaTbl
        /// </summary>
        public SpResult GuardarPrecio(
            string compania,
            int anio,
            int mes,
            string ciudad,
            decimal precio,
            string usuario,
            string operacion)
        {
            SpResult resultado =
                new SpResult();

            using (SqlConnection cn =
                new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd =
                    new SqlCommand(
                        "spa_Ls_HistPrecioGasolinaTbl",
                        cn))
                {
                    cmd.CommandType =
                        CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue(
                        "@PsCompania", compania);

                    cmd.Parameters.AddWithValue(
                        "@PnAnio", anio);

                    cmd.Parameters.AddWithValue(
                        "@PnMes", mes);

                    cmd.Parameters.AddWithValue(
                        "@PsCiudad", ciudad);

                    cmd.Parameters.AddWithValue(
                        "@PnPrecio", precio);

                    cmd.Parameters.AddWithValue(
                        "@PsUsuario", usuario);

                    cmd.Parameters.AddWithValue(
                        "@PsOperacion", operacion);

                    SqlParameter pEstatus =
                        new SqlParameter(
                            "@PnEstatus",
                            SqlDbType.Int);

                    pEstatus.Direction =
                        ParameterDirection.Output;

                    cmd.Parameters.Add(pEstatus);

                    SqlParameter pMensaje =
                        new SqlParameter(
                            "@PsMensaje",
                            SqlDbType.VarChar,
                            250);

                    pMensaje.Direction =
                        ParameterDirection.Output;

                    cmd.Parameters.Add(pMensaje);

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    resultado.IdError =
                        Convert.ToInt32(
                            pEstatus.Value ?? 0);

                    resultado.MensajeError =
                        Convert.ToString(
                            pMensaje.Value ?? "");
                }
            }

            return resultado;
        }
    }
}