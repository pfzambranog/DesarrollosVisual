using System.Collections.Generic;

namespace ReporteGasolina.Models
{
    public sealed class CargaPrecioGasolinaResult
    {
        public int Mes { get; set; }

        public int Anio { get; set; }

        public List<PrecioGasolinaModel> Registros { get; set; }
            = new List<PrecioGasolinaModel>();

        public bool TieneErrores =>
            Registros.Exists(x => !x.EsValido);
    }
}
