using System;
using System.Data;
using System.Data.SqlClient;

namespace ReporteGasolina.Infrastructure
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
                    }

                    using (SqlDataAdapter da =
                        new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        #endregion

        #region Scalar

        public object ExecuteScalar(
            string sql,
            CommandType commandType,
            params SqlParameter[] parameters)
        {
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
                    }

                    return cmd.ExecuteScalar();
                }
            }
        }

        #endregion

        #region NonQuery

        public int ExecuteNonQuery(
            string sql,
            CommandType commandType,
            params SqlParameter[] parameters)
        {
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
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}