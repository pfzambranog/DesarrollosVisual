namespace ReporteGasolina.Models
{
    public sealed class SpResult
    {
        public int IdError { get; set; }

        public string MensajeError { get; set; } = string.Empty;

        public bool EsCorrecto => IdError == 0;
    }
}