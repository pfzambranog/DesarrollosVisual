using System;

namespace ReporteGasolina.Models
{
    /// <summary>
    /// Representa un registro de precio de gasolina
    /// leído desde Excel y procesado por el sistema.
    /// </summary>
    [Serializable]
    public sealed class PrecioGasolinaModel
    {
        private string _ciudad = string.Empty;
        private decimal _precio;

        /// <summary>
        /// Ciudad (máximo 10 caracteres).
        /// </summary>
        public string Ciudad
        {
            get => _ciudad;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _ciudad = string.Empty;
                    return;
                }

                value = value.Trim();

                _ciudad = value.Length > 10
                    ? value.Substring(0, 10)
                    : value;
            }
        }

        /// <summary>
        /// Precio de gasolina.
        /// </summary>
        public decimal Precio
        {
            get => _precio;
            set => _precio = Math.Round(value, 2);
        }

        /// <summary>
        /// Mensaje devuelto por los procedimientos.
        /// </summary>
        public string Mensaje { get; set; } = string.Empty;

        /// <summary>
        /// Número de fila dentro del Excel.
        /// </summary>
        public int FilaExcel { get; set; }

        /// <summary>
        /// Determina si el registro es válido.
        /// </summary>
        public bool EsValido =>
            string.Equals(
                Mensaje,
                "Registro Valido",
                StringComparison.OrdinalIgnoreCase);

        public PrecioGasolinaModel()
        {
        }

        public PrecioGasolinaModel(
            string ciudad,
            decimal precio)
        {
            Ciudad = ciudad;
            Precio = precio;
        }

        public override string ToString()
        {
            return $"{Ciudad} - {Precio:N2}";
        }
    }
}