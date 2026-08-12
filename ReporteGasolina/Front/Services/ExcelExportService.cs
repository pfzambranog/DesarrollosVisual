using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using DocumentFormat.OpenXml.ExtendedProperties;
using ReporteGasolina.Infrastructure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace ReporteGasolina.Services
{
    public class ExcelExportService
    {


        public void ExportarPreciosGasolina(
            string archivo,
            DataGridView grid,
            int mes,
            int anio,
            string operacion,
            string usuario)
        {
            using (XLWorkbook workbook =
                new XLWorkbook())
            {
                var worksheet =
                    workbook.Worksheets.Add("Gasolina");

                AgregarLogo(worksheet, AppSettings.Compania);

                ConstruirEncabezado(
                    worksheet,
                    mes,
                    anio,
                    operacion,
                    usuario);

                ConstruirDetalle(
                    worksheet,
                    grid);

                workbook.SaveAs(archivo);
            }
        }

        private void AgregarLogo(IXLWorksheet ws, string ciaCodigo)
        {
            string nombreLogo = $"logo_{ciaCodigo.Trim().ToLower()}";

            Image logo = Properties.Resources.ResourceManager
                                .GetObject(nombreLogo) as Image;



            if (logo == null)
            {
                MessageBox.Show(
                    $"No existe el recurso: {nombreLogo}");
                return;
            }

            using (var ms = new MemoryStream())
            {
                logo.Save(ms, ImageFormat.Png);

                ms.Position = 0;

                var picture =
                    ws.AddPicture(
                        ms,
                        XLPictureFormat.Png,
                        "Logo");

                picture.MoveTo(ws.Cell("A1"));

                picture.WithSize(226, 85);
            }
        }


        private void ConstruirEncabezado(
            IXLWorksheet ws,
            int mes,
            int anio,
            string operacion,
            string usuario)
        {
            ws.Cell("E1").Value = "Página:";
            ws.Cell("E2").Value = "Fecha:";
            ws.Cell("E3").Value = "Reporte:";
            ws.Cell("E4").Value = "Usuario:";

            ws.Range("E1:E4")
              .Style.Font.SetBold(true);

            ws.Cell("F1").Value = "1";

            ws.Cell("F2").Value = DateTime.Now;

            ws.Cell("F2").Style.NumberFormat.Format =
                "dd/MM/yyyy HH:mm";

            ws.Cell("F3").Value = operacion;

            ws.Cell("F4").Value = usuario;

            ws.Range("A6:F6").Merge();

            ws.Cell("A6").Value =
                $"Reporte de Precios de Gasolina por Zona del periodo {mes:00}-{anio}";

            ws.Cell("A6").Style.Font.Bold = true;
            ws.Cell("A6").Style.Font.FontSize = 12;
            ws.Cell("A6").Style.Font.Underline =
                XLFontUnderlineValues.Single;

            ws.Cell("A6").Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
        }

        private void ConstruirDetalle(
            IXLWorksheet ws,
            DataGridView grid)
        {
            int filaInicio = 8;
            int columnaExcel = 1;

            foreach (DataGridViewColumn columna
                     in grid.Columns)
            {
                if (!columna.Visible)
                {
                    continue;
                }

                ws.Cell(
                    filaInicio,
                    columnaExcel).Value =
                    columna.HeaderText
                           .Trim()
                           .ToUpper();

                columnaExcel++;
            }

            var encabezado =
                ws.Range(
                    filaInicio,
                    1,
                    filaInicio,
                    columnaExcel - 1);

            encabezado.Style.Fill.BackgroundColor =
                XLColor.LightGray;

            encabezado.Style.Font.Bold = true;

            encabezado.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            encabezado.Style.Alignment.Vertical =
                XLAlignmentVerticalValues.Center;

            int filaExcel =
                filaInicio + 1;

            foreach (DataGridViewRow fila
                     in grid.Rows)
            {
                if (fila.IsNewRow)
                {
                    continue;
                }

                columnaExcel = 1;

                foreach (DataGridViewColumn columna
                         in grid.Columns)
                {
                    if (!columna.Visible)
                    {
                        continue;
                    }

                    object valor =
                        fila.Cells[columna.Name].Value;

                    if (columna.Name.ToUpper() == "PRECIO")
                    {
                        decimal precio = 0;

                        decimal.TryParse(
                            Convert.ToString(valor),
                            out precio);

                        ws.Cell(
                            filaExcel,
                            columnaExcel).Value =
                            precio;
                    }
                    else
                    {
                        ws.Cell(
                            filaExcel,
                            columnaExcel).Value =
                            Convert.ToString(valor);
                    }

                    columnaExcel++;
                }

                filaExcel++;
            }

            int ultimaFila = filaExcel - 1;

            var rango =
                ws.Range(
                    filaInicio,
                    1,
                    ultimaFila,
                    grid.Columns
                        .Cast<DataGridViewColumn>()
                        .Count(x => x.Visible));

            rango.Style.Border.OutsideBorder =
                XLBorderStyleValues.Thin;

            rango.Style.Border.InsideBorder =
                XLBorderStyleValues.Thin;

            ws.Columns().AdjustToContents();

            // Ajustes manuales

            ws.Column(1).Width = 30;
            ws.Column(8).Width = 15;
            ws.Column(11).Width = 15;
            ws.Column(5).Width = 15;

            if (grid.Columns
                    .Cast<DataGridViewColumn>()
                    .Any(x =>
                        x.Name.ToUpper() == "PRECIO"))
            {
                ws.Column(2).Width = 15;

                ws.Column(2).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Right;

                ws.Column(2).Style.NumberFormat.Format =
                    "#,##0.00";
            }

            ws.SheetView.FreezeRows(8);
        }
    }
}