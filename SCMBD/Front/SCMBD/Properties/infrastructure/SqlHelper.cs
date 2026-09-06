using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SCMBD.Infrastructure
{
    public class SqlHelper
    {
        private readonly string _connectionString;

        public SqlHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region DataTable

        public DataTable Execute(
            string sql,
            CommandType commandType,
            params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            Stopwatch sw = Stopwatch.StartNew();

            try
            {
                Logger.Debug(
                    "SqlHelper",
                    $"SQL INICIO. CommandType={commandType}, Command={sql}");

                using (SqlConnection cn =
                    new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd =
                        new SqlCommand(sql, cn))
                    {
                        cmd.CommandType = commandType;

                        if (parameters != null &&
                            parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);

                            foreach (SqlParameter p in parameters)
                            {
                                Logger.Debug(
                                    "SqlHelper",
                                    $"PARAM {p.ParameterName}={p.Value}");
                            }
                        }

                        using (SqlDataAdapter da =
                            new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                sw.Stop();

                Logger.Debug(
                    "SqlHelper",
                    $"SQL FIN. Command={sql}");

                Logger.Debug(
                    "SqlHelper",
                    $"REGISTROS={dt.Rows.Count}");

                Logger.Debug(
                    "SqlHelper",
                    $"TIEMPO_MS={sw.ElapsedMilliseconds}");

                return dt;
            }
            catch (Exception ex)
            {
                sw.Stop();

                Logger.Debug(
                    "SqlHelper",
                    $"SQL ERROR. Command={sql}");

                Logger.Debug(
                    "SqlHelper",
                    $"MENSAJE={ex.Message}");

                Logger.Debug(
                    "SqlHelper",
                    $"TIEMPO_MS={sw.ElapsedMilliseconds}");

                throw;
            }
        }

        #endregion DataTable

        #region Scalar

        public object ExecuteScalar(
            string sql,
            CommandType commandType,
            params SqlParameter[] parameters)
        {
            Stopwatch sw = Stopwatch.StartNew();

            try
            {
                Logger.Debug(
                    "SqlHelper",
                    $"SQL INICIO. CommandType={commandType}, Command={sql}");

                using (SqlConnection cn =
                    new SqlConnection(_connectionString))
                {
                    cn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(sql, cn))
                    {
                        cmd.CommandType = commandType;

                        if (parameters != null &&
                            parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);

                            foreach (SqlParameter p in parameters)
                            {
                                Logger.Debug(
                                    "SqlHelper",
                                    $"PARAM {p.ParameterName}={p.Value}");
                            }
                        }

                        object result =
                            cmd.ExecuteScalar();

                        sw.Stop();

                        Logger.Debug(
                            "SqlHelper",
                            $"SQL FIN. Command={sql}");

                        Logger.Debug(
                            "SqlHelper",
                            $"RESULTADO={result}");

                        Logger.Debug(
                            "SqlHelper",
                            $"TIEMPO_MS={sw.ElapsedMilliseconds}");

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();

                Logger.Debug(
                    "SqlHelper",
                    $"SQL ERROR. Command={sql}");

                Logger.Debug(
                    "SqlHelper",
                    $"MENSAJE={ex.Message}");

                Logger.Debug(
                    "SqlHelper",
                    $"TIEMPO_MS={sw.ElapsedMilliseconds}");

                throw;
            }
        }

        #endregion Scalar

        #region NonQuery

        public int ExecuteNonQuery(
            string sql,
            CommandType commandType,
            params SqlParameter[] parameters)
        {
            Stopwatch sw = Stopwatch.StartNew();

            try
            {
                Logger.Debug(
                    "SqlHelper",
                    $"SQL INICIO. CommandType={commandType}, Command={sql}");

                using (SqlConnection cn =
                    new SqlConnection(_connectionString))
                {
                    cn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(sql, cn))
                    {
                        cmd.CommandType = commandType;

                        if (parameters != null &&
                            parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);

                            foreach (SqlParameter p in parameters)
                            {
                                Logger.Debug(
                                    "SqlHelper",
                                    $"PARAM {p.ParameterName}={p.Value}");
                            }
                        }

                        int rowsAffected =
                            cmd.ExecuteNonQuery();

                        sw.Stop();

                        Logger.Debug(
                            "SqlHelper",
                            $"SQL FIN. Command={sql}");

                        Logger.Debug(
                            "SqlHelper",
                            $"ROWS_AFFECTED={rowsAffected}");

                        Logger.Debug(
                            "SqlHelper",
                            $"TIEMPO_MS={sw.ElapsedMilliseconds}");

                        return rowsAffected;
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();

                Logger.Debug(
                    "SqlHelper",
                    $"SQL ERROR. Command={sql}");

                Logger.Debug(
                    "SqlHelper",
                    $"MENSAJE={ex.Message}");

                Logger.Debug(
                    "SqlHelper",
                    $"TIEMPO_MS={sw.ElapsedMilliseconds}");

                throw;
            }
        }

        #endregion NonQuery
    }
}
