using ClosedXML.Excel;
using ReporteGasolina.Models;
using System;
using System.Windows.Forms;

namespace ReporteGasolina.Services
{
    public class ExcelGasolinaService
    {
        public CargaPrecioGasolinaResult LeerArchivo(
            string archivo)
        {
            if (string.IsNullOrWhiteSpace(archivo))
            {
                throw new ArgumentException(
                    "Debe especificar el archivo Excel.");
            }

            CargaPrecioGasolinaResult resultado =
                new CargaPrecioGasolinaResult();


            using (XLWorkbook workbook =
                   new XLWorkbook(archivo))
            {
                IXLWorksheet ws =
                    workbook.Worksheet(1);

                string mesTexto =
                    ws.Cell("B3")
                      .GetValue<string>()
                      .Trim()
                      .ToUpper();

                if (string.IsNullOrWhiteSpace(mesTexto))
                {
                    throw new Exception(
                        "La celda B3 no contiene el mes.");
                }

                resultado.Mes =
                    ConvertirMes(mesTexto);

                string anioTexto =
                    ws.Cell("B4")
                      .GetValue<string>()
                      .Trim();

                if (!int.TryParse(
                        anioTexto,
                        out int anio))
                {
                    throw new Exception(
                        $"El valor del año no es válido. B4=[{anioTexto}]");
                }

                resultado.Anio = anio;

                int fila = 5;

                while (!string.IsNullOrWhiteSpace(
                    ws.Cell(fila, 1)
                      .GetValue<string>()))
                {
                    string ciudad =
                        ws.Cell(fila, 1)
                          .GetValue<string>()
                          .Trim();

                    // Regla heredada VB6
                    if (ciudad.Length > 10)
                    {
                        ciudad =
                            ciudad.Substring(0, 10);
                    }

                    string precioTexto =
                        ws.Cell(fila, 2)
                          .GetValue<string>()
                          .Trim();

                    if (!decimal.TryParse(
                            precioTexto,
                            out decimal precio))
                    {
                        throw new Exception(
                            $"Precio inválido en fila {fila}. " +
                            $"Ciudad=[{ciudad}] " +
                            $"Valor=[{precioTexto}]");
                    }

                    resultado.Registros.Add(
                        new PrecioGasolinaModel
                        {
                            Ciudad = ciudad,
                            Precio = Math.Round(precio, 2),
                            Mensaje = string.Empty,
                            FilaExcel = fila
                        });

                    fila++;
                }

                if (resultado.Registros.Count == 0)
                {
                    throw new Exception(
                        "El archivo no contiene registros para procesar.");
                }
            }

            return resultado;
        }

        private int ConvertirMes(string mes)
        {
            switch (mes)
            {
                case "ENERO":
                    return 1;

                case "FEBRERO":
                    return 2;

                case "MARZO":
                    return 3;

                case "ABRIL":
                    return 4;

                case "MAYO":
                    return 5;

                case "JUNIO":
                    return 6;

                case "JULIO":
                    return 7;

                case "AGOSTO":
                    return 8;

                case "SEPTIEMBRE":
                    return 9;

                case "OCTUBRE":
                    return 10;

                case "NOVIEMBRE":
                    return 11;

                case "DICIEMBRE":
                    return 12;

                default:
                    throw new Exception(
                        $"Mes inválido: {mes}");
            }
        }
    }
}