using System.Configuration;

namespace ReporteGasolina.Infrastructure
{
    public static class AppSettings
    {
        /// <summary>
        /// Compañía configurada en App.config
        /// </summary>
        public static string Compania =>
            ConfigurationManager.AppSettings["Compania"];

        /// <summary>
        /// Operación de seguridad
        /// </summary>
        public static string Operacion =>
            ConfigurationManager.AppSettings["Operacion"];

        /// <summary>
        /// Usuario de la aplicación
        /// </summary>
        public static string Usuario =>
            ConfigurationManager.AppSettings["Usuario"];

        /// <summary>
        /// Cadena de conexión a SQL Server
        /// </summary>
        public static string ConnectionString =>
            ConfigurationManager
                .ConnectionStrings["AdamDb"]
                .ConnectionString;


    }
}