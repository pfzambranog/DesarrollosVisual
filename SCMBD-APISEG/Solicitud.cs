using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SCMBD_APISEG
{
    public class Solicitud : IValidatableObject
    {
        [JsonProperty("Control")]
        [Required]
        public DatosConexion Control { get; set; }

        [JsonProperty("Destino")]
        [Required]
        public DatosDestino Destino { get; set; }

        [JsonProperty("Tipo")]
        [Range(1, 2, ErrorMessage = "Tipo debe ser 1=Consulta o 2=Ejecución")]
        public int Tipo { get; set; }

        [JsonProperty("Sql")]
        [Required(ErrorMessage = "Sentencia SQL es obligatoria")]
        public string Sql { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Control != null)
            {
                if (!int.TryParse(Control.Puerto, out int pCtrl) || pCtrl < 1 || pCtrl > 65535)
                    yield return new ValidationResult("Puerto Control fuera de rango (1-65535)", new[] { nameof(Control.Puerto) });
            }

            if (Destino != null)
            {
                if (!int.TryParse(Destino.Puerto, out int pDst) || pDst < 1 || pDst > 65535)
                    yield return new ValidationResult("Puerto Destino fuera de rango (1-65535)", new[] { nameof(Destino.Puerto) });
            }
        }
    }

    public class DatosConexion
    {
        [JsonProperty("Servidor")]
        [Required]
        public string Servidor { get; set; }

        [JsonProperty("Puerto")]
        [Required]
        public string Puerto { get; set; }

        [JsonProperty("BaseDatos")]
        [Required]
        public string BaseDatos { get; set; }

        [JsonProperty("Usuario")]
        [Required]
        public string Usuario { get; set; }

        [JsonProperty("PasswordCifrada")]
        [Required]
        public string PasswordCifrada { get; set; }
    }

    public class DatosDestino : DatosConexion
    {
        [JsonProperty("Manejador")]
        [RegularExpression(@"^(MSSQL|ORACLE|MARIADB)$",
            ErrorMessage = "Manejador debe ser: MSSQL, ORACLE o MARIADB")]
        public string Manejador { get; set; }
    }
}
