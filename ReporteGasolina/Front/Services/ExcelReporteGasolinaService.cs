using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using DocumentFormat.OpenXml.Spreadsheet;
using ReporteGasolina.Infrastructure;
using ReporteGasolina.Services;

using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;


namespace ReporteGasolina.Services
{
    public class ExcelReporteGasolinaService
    {

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

        public void ExportarReporteCompleto(
            string archivo,
            DataTable dtReporte,
            DataTable dtAltas,
            DataTable dtFaltas,
            DataTable dtIncapacidades,
            int mes,
            int anio,
            string operacion,
            string usuario)
        {
            using (XLWorkbook workbook =
                new XLWorkbook())
            {
                IXLWorksheet ws =
                    workbook.Worksheets.Add("Gasolina");

                AgregarLogo(ws, AppSettings.Compania);

                ConstruirEncabezado(
                    ws,
                    mes,
                    anio,
                    operacion,
                    usuario);

                int filaActual = 8;

                filaActual =
                    ConstruirSeccion(
                        ws,
                        "Reporte de Asignación de Gasolina",
                        dtReporte,
                        filaActual,
                        false);

                filaActual += 3;


                if (dtAltas != null &&
                    dtAltas.Rows.Count > 0)
                {
                    ws.Range($"A{filaActual}:Q{filaActual}").Merge();

                    ws.Range($"A{filaActual}:Q{filaActual}").Merge()
                       .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                    filaActual =
                        ConstruirSeccionAltas(
                            ws,
                            $"Altas del Período: {mes:00}-{anio}",
                            dtAltas,
                            filaActual,
                            true);

                    filaActual += 3;
                }


                //  filaActual += 3;

                ws.Range($"A{filaActual}:Q{filaActual}").Merge();

                ws.Range($"A{filaActual}:Q{filaActual}").Merge()
                   .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                filaActual =
                    ConstruirSeccionFaltas(
                        ws,
                        $"Detalle de Faltas del Período: {mes:00}-{anio}",
                        dtFaltas,
                        filaActual,
                        true);

                filaActual += 3;

                if (dtIncapacidades != null &&
                    dtIncapacidades.Rows.Count > 0)
                {
                    filaActual =
                        ConstruirSeccionIncapacidades(
                            ws,
                            $"Detalle de Incapacidades del Período: {mes:00}-{anio}",
                            dtIncapacidades,
                            filaActual,
                            true);

                    filaActual += 3;
                }

               // Aplicar formato de columnas

                FormatearCeldas(ws);

                workbook.SaveAs(archivo);
            }
        }

        private void FormatearCeldas(IXLWorksheet ws)
        {

            // Ajustes manuales

            ws.Column(1).Width = 15;
            ws.Column(2).Width = 12;
            ws.Column(4).Width = 60;
            ws.Column(5).Width = 15;
            ws.Column(6).Width = 20;
            ws.Column(7).Width = 15;
            ws.Column(8).Width = 15;
            ws.Column(9).Width = 15;
            ws.Column(10).Width = 10;
            ws.Column(11).Width = 15;
            ws.Column(12).Width = 15;
            ws.Column(13).Width = 15;
            ws.Column(14).Width = 15;
            ws.Column(15).Width = 15;
            ws.Column(16).Width = 15;
            ws.Column(17).Width = 25;

            // ALINEACIONES

            ws.Column("B").Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            ws.Column("C").Style.Alignment.Horizontal =
              XLAlignmentHorizontalValues.Center;

            ws.Column("E").Style.Alignment.Horizontal =
               XLAlignmentHorizontalValues.Center;

            ws.Column("K").Style.Alignment.Horizontal =
               XLAlignmentHorizontalValues.Center;

            ws.Column("L").Style.Alignment.Horizontal =
               XLAlignmentHorizontalValues.Center;

            ws.Column("M").Style.Alignment.Horizontal =
               XLAlignmentHorizontalValues.Center;

            ws.Column("N").Style.Alignment.Horizontal =
               XLAlignmentHorizontalValues.Center;

            //

            ws.Cell("A6").Style.Font.Bold = true;
            ws.Cell("A6").Style.Font.FontSize = 12;

            ws.Cell("A6")
              .Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            ws.Cell("L2")
             .Style.NumberFormat.Format = "dd/MM/yyyy HH:mm";

            ws.Range("K1:L4")
                   .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            ws.Row(8)
               .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.Range("K1:K4")
                .Style.Font.SetBold(true);

            var encabezado = ws.Range("A8:Q8");

            encabezado.Style.Font.Bold = true;
            encabezado.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            encabezado.Style.Font.FontColor = XLColor.White;
            encabezado.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

        }

        private void ConstruirEncabezado(
            IXLWorksheet ws,
            int mes,
            int anio,
            string operacion,
            string usuario)
        {
            ws.Cell("K1").Value = "Página:";
            ws.Cell("K2").Value = "Fecha:";
            ws.Cell("K3").Value = "Reporte:";
            ws.Cell("K4").Value = "Usuario:";


            ws.Cell("L1").Value = "1";

            ws.Cell("L2").Value =
                DateTime.Now;


            ws.Cell("L3").Value = operacion;

            ws.Cell("L4").Value = usuario;

            ws.Range("A6:R6").Merge();

            ws.Cell("A6").Value =
                $"Reporte de Asignación de Gasolina Correspondiente al Período: {mes:00}-{anio}";


        }

        private int ConstruirSeccionFaltas(
           IXLWorksheet ws,
           string titulo,
           DataTable dt,
           int filaInicio,
           bool imprimirTitulo)
        {
         
            
            if (dt == null)
            {
                return filaInicio;
            }

            if (imprimirTitulo)
            {
                ws.Range(
                    filaInicio,
                    1,
                    filaInicio,
                    17)
                  .Merge();

                ws.Cell(filaInicio, 1).Value = titulo;
                ws.Cell(filaInicio, 1).Style.Font.Bold = true;
                ws.Cell(filaInicio, 1).Style.Font.FontSize = 12;
                ws.Cell(filaInicio, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                filaInicio += 2;
            }

            if (dt.Rows.Count == 0)
            {
                return filaInicio + 1;
            }

            ws.Cell($"A{filaInicio}").Value = "Región";
            ws.Cell($"B{filaInicio}").Value = "Dep-Zona";
            ws.Cell($"C{filaInicio}").Value = "Trabajador";
            ws.Cell($"D{filaInicio}").Value = "Nombre";
            ws.Cell($"E{filaInicio}").Value = "NSS";
            ws.Cell($"F{filaInicio}").Value = "Ciudad";
            ws.Cell($"G{filaInicio}").Value = "Concepto";

            ws.Range($"H{filaInicio}:J{filaInicio}").Merge();
            ws.Cell($"H{filaInicio}").Value = "Descripción";

            ws.Cell($"K{filaInicio}").Value = "Fecha";
            ws.Cell($"L{filaInicio}").Value = "Litros";
            ws.Cell($"M{filaInicio}").Value = "Días";
            ws.Cell($"N{filaInicio}").Value = "Pagar";

            ws.Range($"O{filaInicio}:P{filaInicio}").Merge();
            ws.Cell($"O{filaInicio}").Value = "Observaciones";

            ws.Cell($"Q{filaInicio}").Value = "Tarjeta";

            // FORMATO ENCABEZADO

            var encabezado =
                ws.Range($"A{filaInicio}:Q{filaInicio}");

            encabezado.Style.Font.Bold = true;
            encabezado.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            encabezado.Style.Font.FontColor = XLColor.White;
            encabezado.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            int fila = filaInicio + 1;


            // DETALLE

            foreach (DataRow dr in dt.Rows)
            {
                ws.Cell(fila, 1).Value =
                    Convert.ToString(dr["Region"]);

                ws.Cell(fila, 2).Value =
                    Convert.ToString(dr["DepZona"]);

                ws.Cell(fila, 3).Value =
                    Convert.ToString(dr["trabajador"]);

                ws.Cell(fila, 4).Value =
                    Convert.ToString(dr["Nombre"]);

                ws.Cell(fila, 5).Value =
                    Convert.ToString(dr["NSS"]);

                ws.Cell(fila, 6).Value =
                    Convert.ToString(dr["Ciudad"]);

                // G = Concepto
                ws.Cell(fila, 7).Value =
                    Convert.ToString(dr["Concepto"]);

                // H:J = Descripción
                ws.Range(fila, 8, fila, 10).Merge();

                ws.Cell(fila, 8).Value =
                    Convert.ToString(dr["Descripcion"]);

                // K = Fecha
                if (dr["Fecha"] != DBNull.Value)
                {
                    ws.Cell(fila, 11).Value =
                        Convert.ToDateTime(dr["Fecha"]);

                    ws.Cell(fila, 11)
                      .Style.DateFormat.Format =
                        "dd/MM/yyyy";
                }

                ws.Cell(fila, 11)
                  .Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                // L = Litros
                if (dr["Litros"] != DBNull.Value)
                {
                   // ws.Cell(fila, 12)

                   // L = Litros
                    if (dr["Litros"] != DBNull.Value)
                    {
                        ws.Cell(fila, 12).Value =
                            Convert.ToDecimal(dr["Litros"]);
                    }

                    ws.Cell(fila, 12)
                        .Style.NumberFormat.Format =
                        "###,##0";

                    ws.Cell(fila, 12)
                        .Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    // M = Días

                    if (dr["Dias"] != DBNull.Value)
                    {
                        ws.Cell(fila, 13).Value =
                            Convert.ToInt32(dr["Dias"]);
                    }

                    ws.Cell(fila, 13)
                        .Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    // N = Pagar

                    if (dr["Pagar"] != DBNull.Value)
                    {
                        ws.Cell(fila, 14).Value =
                            Convert.ToDecimal(dr["Pagar"]);
                    }

                    ws.Cell(fila, 14)
                        .Style.NumberFormat.Format =
                        "###,##0.00";

                    ws.Cell(fila, 14)
                        .Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Right;

                    // O:P = Observaciones
                    var rangoObservaciones =
                        ws.Range(fila, 15, fila, 16);

                    rangoObservaciones.Merge();

                    ws.Cell(fila, 15).Value =
                        Convert.ToString(dr["Observaciones"]);

                    rangoObservaciones.Style.Alignment.Horizontal =
                        XLAlignmentHorizontalValues.Center;

                    rangoObservaciones.Style.Alignment.Vertical =
                        XLAlignmentVerticalValues.Center;

                    rangoObservaciones.Style.Alignment.WrapText =
                        true;

                    rangoObservaciones.Style.Fill.BackgroundColor =
                        XLColor.Red;

                    rangoObservaciones.Style.Font.FontColor =
                        XLColor.White;

                    rangoObservaciones.Style.Font.Bold = true;

                    // Q = Tarjeta
                    ws.Cell(fila, 17).Value =
                        Convert.ToString(dr["tarjeta"]);

                    fila++;
                }
            }

            int filaUltima = fila - 1;

                // BORDES
                var rango = ws.Range(
                    filaInicio,
                    1,
                    filaUltima,
                    17);

                rango.Style.Border.OutsideBorder =
                    XLBorderStyleValues.Thin;

                rango.Style.Border.InsideBorder =
                    XLBorderStyleValues.Thin;




            return fila;
            }

        private int ConstruirSeccionIncapacidades(
           IXLWorksheet ws,
           string titulo,
           DataTable dt,
           int filaInicio,
           bool imprimirTitulo)
        {

            if (dt == null)
            {
                return filaInicio;
            }

            if (dt.Rows.Count == 0)
            {
                return filaInicio + 1;
            }

            if (imprimirTitulo)
            {
                ws.Range(
                    filaInicio,
                    1,
                    filaInicio,
                    17)
                  .Merge();

                ws.Cell(filaInicio, 1).Value = titulo;
                ws.Cell(filaInicio, 1).Style.Font.Bold = true;
                ws.Cell(filaInicio, 1).Style.Font.FontSize = 12;
                ws.Cell(filaInicio, 1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                filaInicio += 2;
               
            }

            ws.Cell($"A{filaInicio}").Value = "Región";
            ws.Cell($"B{filaInicio}").Value = "Dep-Zona";
            ws.Cell($"C{filaInicio}").Value = "Trabajador";
            ws.Cell($"D{filaInicio}").Value = "Nombre";
            ws.Cell($"E{filaInicio}").Value = "NSS";
            ws.Cell($"F{filaInicio}").Value = "Ciudad";
            ws.Cell($"G{filaInicio}").Value = "Baja";
            ws.Cell($"H{filaInicio}").Value = "Alta";
            ws.Cell($"I{filaInicio}").Value = "Dias Incap";
            ws.Cell($"J{filaInicio}").Value = "Litros";

            ws.Range($"K{filaInicio}:M{filaInicio}").Merge();
            ws.Cell($"K{filaInicio}").Value = "Dias";

            ws.Cell(filaInicio, 13)
              .Style.NumberFormat.Format = "###,###";

            ws.Cell($"N{filaInicio}").Value = "Pagar";

            ws.Range($"O{filaInicio}:P{filaInicio}").Merge();
            ws.Cell($"O{filaInicio}").Value = "Observaciones";

            ws.Cell($"Q{filaInicio}").Value = "Tarjeta";

            // FORMATO ENCABEZADO

            var encabezado =
                ws.Range($"A{filaInicio}:Q{filaInicio}");

            encabezado.Style.Font.Bold = true;
            encabezado.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            encabezado.Style.Font.FontColor = XLColor.White;
            encabezado.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            int fila = filaInicio + 1;

            // DETALLE

            foreach (DataRow dr in dt.Rows)
            {
                ws.Cell(fila, 1).Value =
                    Convert.ToString(dr["Region"]);

                ws.Cell(fila, 2).Value =
                    Convert.ToString(dr["DepZona"]);
                ws.Cell(fila, 2)
                  .Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                ws.Cell(fila, 3).Value =
                    Convert.ToString(dr["trabajador"]);

                ws.Cell(fila, 3)
                  .Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                ws.Cell(fila, 4).Value =
                    Convert.ToString(dr["Nombre"]);
                
                ws.Cell(fila, 4)
                  .Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                ws.Cell(fila, 5).Value =
                    Convert.ToString(dr["NSS"]);

                ws.Cell(fila, 5)
                  .Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                ws.Cell(fila, 6).Value =
                    Convert.ToString(dr["Ciudad"]);

                // G = Baja. Inicio de Incapacidad
                // 
                if (dr["Baja"] != DBNull.Value)
                {
                    ws.Cell(fila, 7).Value =
                        Convert.ToDateTime(dr["Baja"]);

                    ws.Cell(fila, 7)
                        .Style.DateFormat.Format =
                        "dd/MM/yyyy";

                   ws.Cell(fila, 7)
                        .Style.Alignment.Horizontal =
                         XLAlignmentHorizontalValues.Center;
                }

                // H = Alta. Termino de Incapacidad

                if (dr["Alta"] != DBNull.Value)
                {
                    ws.Cell(fila, 8).Value =
                        Convert.ToDateTime(dr["Alta"]);

                    ws.Cell(fila, 8)
                        .Style.DateFormat.Format =
                        "dd/MM/yyyy";
                }
                
                ws.Cell(fila, 8)
                         .Style.Alignment.Horizontal =
                          XLAlignmentHorizontalValues.Center;


                // H = Baja. Inicio de Incapacidad

                ws.Cell(fila, 9).Value =
                    Convert.ToString(dr["DiasIncap"]);

                ws.Cell(fila, 9)
                    .Style.NumberFormat.Format =
                    "###,###";

                ws.Cell(fila, 9)
                  .Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                // J = Litros

                ws.Cell(fila, 10).Value =
                    Convert.ToString(dr["Litros"]);
                ws.Cell(fila, 10)
                    .Style.NumberFormat.Format =
                    "###,###";

                ws.Cell(fila, 10)
                    .Style.Alignment.Horizontal =
                         XLAlignmentHorizontalValues.Center;

                // K-M = Detalle de DiasDetalle

                var rangoDias =
                    ws.Range(fila, 11, fila, 13);
                rangoDias.Merge();

                ws.Cell(fila, 11).Value =
                    Convert.ToString(dr["DiasDetalle"]);

                rangoDias.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                rangoDias.Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Center;

                // N = Pagar

                ws.Cell(fila, 14).Value =
                    Convert.ToString(dr["Pagar"]);

                ws.Cell(fila, 14)
                    .Style.NumberFormat.Format =
                    "###,###";

                ws.Cell(fila, 14)
                    .Style.Alignment.Horizontal =
                         XLAlignmentHorizontalValues.Center;

                // O:P = Observaciones

                var rangoObservaciones =
                    ws.Range(fila, 15, fila, 16);

                rangoObservaciones.Merge();

                string observaciones =
                    Convert.ToString(dr["Observaciones"]).Trim();

                ws.Cell(fila, 15).Value = observaciones;

                rangoObservaciones.Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                rangoObservaciones.Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Center;

                rangoObservaciones.Style.Alignment.WrapText =
                    true;

                rangoObservaciones.Style.Font.Bold = true;

                if (!string.IsNullOrWhiteSpace(observaciones))
                {
                    // Tiene observaciones -> ROJO
                    rangoObservaciones.Style.Fill.BackgroundColor =
                        XLColor.Red;

                    rangoObservaciones.Style.Font.FontColor =
                        XLColor.White;
                }
                else
                {
                    // Vacío -> ROSA PÁLIDO
                    var rangoInc =
                        ws.Range(fila, 1, fila, 17);

                    rangoInc.Style.Fill.BackgroundColor =
                        XLColor.LightPink;

                    rangoInc.Style.Font.FontColor =
                        XLColor.Black;
                }

                // Q = Tarjeta

                ws.Cell(fila, 17).Value =
                        Convert.ToString(dr["tarjeta"]);

                ws.Cell(fila, 17)
                      .Style.NumberFormat.Format =  "######";

                ws.Cell(fila, 17)
                    .Style.Alignment.Horizontal =
                         XLAlignmentHorizontalValues.Center;

                fila++;

            }

            int filaUltima = fila - 1;

            // BORDES

            var rango = ws.Range(filaInicio, 1, filaUltima, 17);

            rango.Style.Border.OutsideBorder =
                XLBorderStyleValues.Thin;

            rango.Style.Border.InsideBorder =
                XLBorderStyleValues.Thin;

            return fila;
        }


        private int ConstruirSeccionAltas(
         IXLWorksheet ws,
         string titulo,
         DataTable dt,
         int filaInicio,
         bool imprimirTitulo)
        {
            if (dt == null)
            {
                return filaInicio;
            }

            // Título
            if (imprimirTitulo)
            {
                ws.Range(
                    filaInicio,
                    1,
                    filaInicio,
                    17)
                  .Merge();

                ws.Cell(filaInicio, 1).Value = titulo;
                ws.Cell(filaInicio, 1).Style.Font.Bold = true;
                ws.Cell(filaInicio, 1).Style.Font.FontSize = 12;
                ws.Cell(filaInicio, 1).Style.Alignment.Horizontal =
                    XLAlignmentHorizontalValues.Center;

                filaInicio += 2;
            }

            if (dt.Rows.Count == 0)
            {
                return filaInicio + 1;
            }

            // Encabezados

            ws.Cell($"A{filaInicio}").Value = "Región";
            ws.Cell($"B{filaInicio}").Value = "DepZona";
            ws.Cell($"C{filaInicio}").Value = "Trabajador";
            ws.Cell($"D{filaInicio}").Value = "Nombre";
            ws.Cell($"E{filaInicio}").Value = "NSS";
            ws.Cell($"F{filaInicio}").Value = "Ciudad";
            ws.Cell($"G{filaInicio}").Value = "PVP Ciudad";
            ws.Cell($"H{filaInicio}").Value = "Litros";
            ws.Cell($"I{filaInicio}").Value = "Alta";
            ws.Cell($"J{filaInicio}").Value = "Días";
            ws.Cell($"K{filaInicio}").Value = "Pagar";

            ws.Range($"L{filaInicio}:P{filaInicio}").Merge();
            ws.Cell($"L{filaInicio}").Value = "Observaciones";

            ws.Cell($"Q{filaInicio}").Value = "Tarjeta";

            // Formato encabezado
            var encabezado = ws.Range($"A{filaInicio}:Q{filaInicio}");


            encabezado.Style.Font.Bold = true;
            encabezado.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            encabezado.Style.Font.FontColor = XLColor.White;
            encabezado.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            int fila = filaInicio + 1;

            // Datos
            foreach (DataRow dr in dt.Rows)
            {
                // A:K
                for (int col = 0; col <= 10; col++)
                {
                    object valor = dr[col];

                    var celda = ws.Cell(fila, col + 1);

                    if (valor == DBNull.Value)
                    {
                        celda.Value = string.Empty;
                        continue;
                    }

                    switch (col)
                    {
                        // G = PVP Ciudad
                        case 6:
                            celda.Value = Convert.ToDecimal(valor);
                            celda.Style.NumberFormat.Format = "$ #,##0.00";
                            break;

                        // H = Litros
                        case 7:
                            celda.Value = Convert.ToDecimal(valor);
                            celda.Style.NumberFormat.Format = "#,##0.00";
                            break;

                        // I = Alta
                        case 8:
                            celda.Value = Convert.ToDateTime(valor);
                            celda.Style.DateFormat.Format = "dd/MM/yyyy";
                            celda.Style.Alignment.Horizontal =
                                XLAlignmentHorizontalValues.Center;
                            break;

                        // J = Días
                        case 9:
                            celda.Value = Convert.ToInt32(valor);
                            break;

                        // K = Pagar
                        case 10:
                            celda.Value = Convert.ToDecimal(valor);
                            celda.Style.NumberFormat.Format = "$ #,##0.00";
                            break;

                        default:
                            celda.Value = Convert.ToString(valor)?.Trim();
                            break;
                    }
                }

                // L:P = Observaciones
                ws.Cell(fila, 12).Value =
                    Convert.ToString(dr["Observaciones"]);

                var rangoObservaciones =
                    ws.Range(fila, 12, fila, 16);

                rangoObservaciones.Merge();
                rangoObservaciones.Style.Alignment.WrapText = true;
                rangoObservaciones.Style.Alignment.Vertical =
                    XLAlignmentVerticalValues.Center;
                rangoObservaciones.Style.Fill.BackgroundColor = XLColor.Red;

                rangoObservaciones.Style.Font.FontColor = XLColor.White;

                rangoObservaciones.Style.Font.Bold = true;

                // Q = Tarjeta
                ws.Cell(fila, 17).Value =
                    Convert.ToString(dr["tarjeta"]);

                fila++;
            }

            int filaUltima = fila - 1;

            // Bordes
            var rango = ws.Range(
                filaInicio,
                1,
                filaUltima,
                17);

            rango.Style.Border.OutsideBorder =
                XLBorderStyleValues.Thin;

            rango.Style.Border.InsideBorder =
                XLBorderStyleValues.Thin;

            // Ajuste de columnas
            ws.Columns("A:Q").AdjustToContents();

            return fila;
        }

        private int ConstruirSeccion(
            IXLWorksheet ws,
            string titulo,
            DataTable dt,
            int filaInicio,
            bool imprimirTitulo)
        {

            if (dt == null)
            {
                return filaInicio;
            }

            if (imprimirTitulo)
            {
                ws.Range(
                    filaInicio,
                    1,
                    filaInicio,
                    Math.Max(1,
                    dt.Columns.Count))
                    .Merge();

                ws.Cell(
                    filaInicio,
                    1).Value = titulo;

                ws.Cell(
                    filaInicio,
                    1).Style.Font.Bold = true;

                ws.Cell(
                    filaInicio,
                    1).Style.Font.FontSize = 12;

                filaInicio += 2;
            }

            if (dt == null ||
                dt.Rows.Count == 0)
            {
                return filaInicio + 1;
            }

            int columna = 1;

            foreach (DataColumn dc in dt.Columns)
            {
                ws.Cell(
                    filaInicio,
                    columna).Value =
                    dc.ColumnName;

                columna++;
            }

            var encabezado =
                ws.Range(
                    filaInicio,
                    1,
                    filaInicio,
                    dt.Columns.Count);

            encabezado.Style.Fill.BackgroundColor =
                XLColor.LightGray;

            encabezado.Style.Font.Bold = true;
                  encabezado.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            int fila = filaInicio + 1;

            foreach (DataRow dr in dt.Rows)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    object valor = dr[col];

                    string nombreColumna =
                        dt.Columns[col].ColumnName
                          .Trim()
                          .ToUpper();

                    if (valor == DBNull.Value)
                    {
                        ws.Cell(fila, col + 1).Value = "";
                    }
                    else
                    {
                        string texto =
                            Convert.ToString(valor)
                                .Trim();

                        //
                        // Columnas enteras
                        //

                        if (nombreColumna == "CANTLITROS" ||
                            nombreColumna == "DIASFALTA" ||
                            nombreColumna == "DIASINCAP" ||
                            nombreColumna == "TOTALDIAS")
                        {
                            if (decimal.TryParse(
                                    texto.Replace(",", ""),
                                    out decimal numero))
                            {
                                ws.Cell(
                                    fila,
                                    col + 1).Value =
                                    (int)numero;
                            }
                            else
                            {
                                ws.Cell(
                                    fila,
                                    col + 1).Value =
                                    texto;
                            }
                        }
                        else
                        {
                            ws.Cell(
                                fila,
                                col + 1).Value =
                                texto;
                        }
                    }
                }


                if (dt.Columns.Contains("netoMes"))
                {
                    string neto =
                        Convert.ToString(
                            dr["netoMes"])
                        .Trim()
                        .ToUpper();

                    if (neto == "EXENTO" ||
                        neto == "INCAPACITADO")
                    {
                        IXLCell celdaNeto =
                            ws.Cell(
                                fila,
                                16);

                        celdaNeto.Style.Fill.BackgroundColor =
                            XLColor.Red;

                        celdaNeto.Style.Font.FontColor =
                            XLColor.White;

                        celdaNeto.Style.Font.Bold = true;

                        celdaNeto.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        celdaNeto.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                }

                fila++;

            }

            // rango que incluye encabezado y todas las filas de datos

            int filaUltima = fila - 1; // última fila escrita
            var rango = ws.Range(filaInicio, 1, filaUltima, dt.Columns.Count);
            rango.Style.Border.OutsideBorder =
                XLBorderStyleValues.Thin;

            rango.Style.Border.InsideBorder =
                XLBorderStyleValues.Thin;

            //
            // COLUMNAS MONETARIAS
            // G, I, K, M, O, P -> índices 7,9,11,13,15,16
            //

            int[] monetarias = { 7, 9, 11, 13, 15, 16 };

            foreach (int colIndex in monetarias)
            {
                if (colIndex <= dt.Columns.Count) // proteger contra tablas con menos columnas
                {
                    ws.Column(colIndex)
                        .Style.Alignment.Horizontal =
                            XLAlignmentHorizontalValues.Right;

                    ws.Column(colIndex)
                        .Style.NumberFormat.Format =
                            "#,##0.00";
                }
            }

            //
            // COLUMNAS ENTERAS
            // H, J, L, N, Q -> índices 8,10,12,14,17
            //

            int[] enteras = { 8, 10, 12, 14, 17 };

            foreach (int colIndex in enteras)
            {
                if (colIndex <= dt.Columns.Count)
                {
                    ws.Column(colIndex)
                        .Style.Alignment.Horizontal =
                            XLAlignmentHorizontalValues.Center;

                    ws.Column(colIndex)
                        .Style.NumberFormat.Format =
                            "#,##0";
                }
            }



            return fila;


        }
    }
}
