using ReporteGasolina.Infrastructure;
using ReporteGasolina.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ReporteGasolina.Services
{
    public class ReporteGasolinaService
    {
        private readonly string _connectionString;

        public ReporteGasolinaService()
        {
            _connectionString =
                AppSettings.ConnectionString;
        }


        public string ObtenerDirectorioSalida()
        {
            SqlHelper sql =
                new SqlHelper(
                    _connectionString);

            object resultado =
                sql.ExecuteScalar(
                    @"Select RTRIM(descripcion)
              From dbo.criterios_valores
              Where campo = 'dirsalgas'
              And item = 1",
                    CommandType.Text);

            return Convert.ToString(resultado ?? "");
        }

        public DataTable ObtenerAltas(string compania, int anio, int mes)
        {
            SqlHelper sql =
                new SqlHelper(
                    _connectionString);

            return sql.Execute(
        @"DECLARE @FechaInicio DATE;
          DECLARE @FechaFinMes DATE;

        SET @FechaInicio = CONVERT(DATE, '01/' + RIGHT('00' + CAST(@mes AS VARCHAR(2)),2) + '/' +
                                  CAST(@anio AS VARCHAR(4)),  103);

        SET @FechaFinMes = dbo.fn_obten_FinMes(@FechaInicio);

SELECT
    RTRIM(
        dbo.fn_desc_agrup_dato(
            'REG',
            a.Region))                AS Region,

    RTRIM(a.departamento)
        + ' - ' +
    RTRIM(a.zona)                    AS DepZona,

    a.trabajador,

    RTRIM(a.nombre)                  AS Nombre,

    a.nss,

    RTRIM(a.descCiudad)              AS Ciudad,

    a.pvpLitro                       AS PVPCiudad,

    a.cantLitros                     AS Litros,

    CAST(
        b.fecha_ingreso
        AS DATE)                     AS Alta,

    DATEDIFF(
        DAY,
        b.fecha_ingreso,
        dbo.fn_obten_FinMes(
            b.fecha_ingreso)) + 1    AS Dias,

    a.netoMes                        AS Pagar,

    'PAGAR EN: '
        + RIGHT('00'
        + CAST(@mes AS VARCHAR(2)),2)
        + '-'
        + CAST(@anio AS VARCHAR(4))
                                    AS Observaciones,

    a.tarjeta
FROM dbo.Ls_RepPrecioGasolinaTbl a
INNER JOIN dbo.trabajadores_grales b
       ON b.compania   = a.compania
      AND b.trabajador = a.trabajador

WHERE a.compania = @compania
AND   a.anio     = @anio
AND   a.mes      = @mes

AND CAST(b.fecha_ingreso AS DATE)
    BETWEEN
        @FechaInicio
    AND @FechaFinMes

AND a.trabajador <> ''
ORDER BY a.trabajador
",
                CommandType.Text,
                new SqlParameter(
                    "@compania",
                    compania),

                new SqlParameter(
                    "@anio",
                    anio),

                new SqlParameter(
                    "@mes",
                    mes));
        }

        public DataTable ObtenerFaltas(string compania, int anio, int mes)
        {
            SqlHelper sql =
                new SqlHelper(
                    _connectionString);

            return sql.Execute(
        @"DECLARE @FechaInicio DATE;
          DECLARE @FechaFinMes DATE;

        SET @FechaInicio = CONVERT(DATE, '01/' + RIGHT('00' + CAST(@mes AS VARCHAR(2)),2) + '/' +
                                  CAST(@anio AS VARCHAR(4)),  103);

        SET @FechaFinMes = dbo.fn_obten_FinMes(@FechaInicio);

        Select Rtrim(a.descRegion) as Region, 
               Rtrim(depZona) As depZona, a.trabajador, RTrim(a.nombre) as Nombre, 
               NSS, Rtrim(a.descCiudad) as Ciudad,
               a.incidencia_kp as concepto, b.descripcion_kp as descripcion, 
               a.fecha_incidencia as fecha,
               c.cantLitros as Litros,  a.dias as Dias,
               Round((Convert(Decimal(19, 2), Replace(c.impGasMes, ',', '')) / 30.00) * dias, 0, 1) As Pagar,
               'PAGAR EN: ' + RIGHT('00' + CAST(@mes AS VARCHAR(2)), 2)
                            + '-' + CAST(@anio AS VARCHAR(4)) AS Observaciones, tarjeta
        From   dbo.Ls_FaltasGasolinaTbl a
        Join   dbo.incidencias          b 
        On     b.incidencia_kp = a.incidencia_kp
        Join   dbo.Ls_RepPrecioGasolinaTbl c
        On     c.compania      = a.compania
        And    c.anio          = a.anio 
        And    c.mes           = a.mes
        And    c.trabajador    = a.trabajador
        And    c.tipoLinea     = 'D'
        And    Isnumeric(c.netoMes) = 1 
        Where  a.compania = @compania
        And    a.anio     = @anio
        And    a.mes      = @mes
        Order  By a.trabajador, a.fecha_incidencia",
                CommandType.Text,
                new SqlParameter(
                    "@compania",
                    compania),

                new SqlParameter(
                    "@anio",
                    anio),

                new SqlParameter(
                    "@mes",
                    mes));
        }

        public SpResult ProcesarReporte(
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
                        "spp_Ls_RepPrecioGasolinaTbl",
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

                    cmd.Parameters.AddWithValue(
                        "@PnImprime",
                        0);

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

        //
        public DataTable ObtenerIncapacidades(string compania, int anio, int mes)
        {
            SqlHelper sql =
                new SqlHelper(
                    _connectionString);

            return sql.Execute(
        @"Select a.descRegion Region, a.depZona DepZona,  a.trabajador, a.nombre, b.Nss,
                a.descCiudad Ciudad, a.fechaInicio Baja, a.fechaTermino Alta,  a.dias DiasIncap,
                b.cantLitros Litros, a.diasDet DiasDetalle, a.pago Pagar,a.Observaciones,
                a.Tarjeta
         From   dbo.Ls_RepFaltasIncapacidadTbl a
         Join   dbo.Ls_RepPrecioGasolinaTbl b
         On     b.compania    = a.compania
         And    b.anio        = a.anio
         And    b.mes         = a.mes
         And    b.trabajador  = a.trabajador
         And    b.tipoLinea   = 'D'
         And    b.compania    = @compania
         And    b.anio        = @anio
         And    b.mes         = @mes
         Where  a.descRegion != ''
         And    a.dias != 0
         Order by Region, a.trabajador",
                CommandType.Text,
                new SqlParameter(
                    "@compania",
                    compania),

                new SqlParameter(
                    "@anio",
                    anio),

                new SqlParameter(
                    "@mes",
                    mes));
        }

//

        public DataTable ObtenerReporte(
            string compania,
            int anio,
            int mes)
        {
            SqlHelper sql =
                new SqlHelper(
                    _connectionString);

            return sql.Execute(
                @"SELECT descRegion AS Region,
                         CASE WHEN tipoLinea = 'T'
                              THEN RTRIM(departamento)   + ' - '  + RTRIM(zona)
                              ELSE ''
                         END AS DepZona,

                         trabajador, nombre,     nss,          descCiudad AS Ciudad,
                         pvpLitro,   cantLitros, impGasMes,    diasFalta,   impFalta,
                         diasIncap,  impIncap,   totalDias,    totalMes,    netoMes,
                         tarjeta
                 FROM Ls_RepPrecioGasolinaTbl
                 WHERE compania = @compania
                 AND anio = @anio
                 AND mes = @mes
                 ORDER BY secuencia",
                CommandType.Text,
                new SqlParameter("@compania",  compania),
                new SqlParameter("@anio",      anio),
                new SqlParameter("@mes",       mes));
        }
    }
}
