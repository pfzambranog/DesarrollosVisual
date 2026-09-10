using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SCMBD.Services
{
    public static class ExcelExportService
    {
        // ANCHOS DE COLUMNA
        private static readonly double[] anchosColumnas =
        {
            15,   
            30,  
            30,
            30,
            30,
            30, 
            20,   
            20    
        };

        public static void ExportarUsuarios(
            string rutaArchivo,
            DataGridView dgv,
            string operacion,
            string tituloreporte,
            string usuario)
        {
            using (var libro = new XLWorkbook())
            {
                var hoja = libro.Worksheets.Add("Usuarios");

                // ✅ 1. LOGO en celda A1
                AgregarLogo(hoja);

                // ✅ 2. Encabezados (Fecha, Reporte, Usuario) — columna G/H
                ConstruirEncabezado(hoja, operacion, tituloreporte, usuario);

                // ✅ 3. Datos del Grid con encabezados renombrados
                ConstruirDetalle(hoja, dgv);

                // ✅ Guardar archivo
                libro.SaveAs(rutaArchivo);
            }
        }

        // =====================================================
        // Agregar Logo en celda A1
        // =====================================================

        private static void AgregarLogo(IXLWorksheet hoja)
        {
            string rutaLogo = Path.Combine(Application.StartupPath, @"Imagenes\LogoSCMBD.png");

            if (!File.Exists(rutaLogo))
            {
                MessageBox.Show($"No se encontró el logo en:\n{rutaLogo}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Image logo = Image.FromFile(rutaLogo))
            using (var ms = new MemoryStream())
            {
                logo.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                var imagen = hoja.AddPicture(ms, XLPictureFormat.Png, "Logo");
                imagen.MoveTo(hoja.Cell("A1"));   // Posición A1
                imagen.WithSize(120, 80);         // Tamaño ajustado
            }
        }

        // =====================================================
        // ✅ Construir Encabezado — columna G/H
        // =====================================================
        private static void ConstruirEncabezado(
            IXLWorksheet hoja,
            string operacion,
            string tituloreporte,
            string usuario)
        {
            // Etiquetas en columna G
            hoja.Cell("G1").Value = "Página:";
            hoja.Cell("G2").Value = "Fecha:";
            hoja.Cell("G3").Value = "Reporte:";
            hoja.Cell("G4").Value = "Usuario:";
            hoja.Range("G1:G4").Style.Font.SetBold();


            hoja.Cell("H1").Value = 1;
            hoja.Cell("H2").Value = DateTime.Now;
            hoja.Range("H2:I2").Merge();
            hoja.Cell("H2").Style.NumberFormat.Format = "dd/MM/yyyy HH:mm";
            hoja.Cell("H2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            hoja.Cell("H3").Value = operacion;
            hoja.Cell("H4").Value = usuario;


            hoja.Range("A6:H6").Merge();
            hoja.Cell("A6").Value = tituloreporte;
            hoja.Cell("A6").Style.Font.SetBold();           
                                                            
            hoja.Cell("A6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        // =====================================================
        // Construir Detalle — ENCABEZADOS RENOMBRADOS
        // =====================================================
        private static void ConstruirDetalle(IXLWorksheet hoja, DataGridView dgv)
        {
            int filaEncabezados = 8; // Debajo del título

            // ENCABEZADOS CON NOMBRES PERSONALIZADOS
            int col = 1;
            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                if (!dgv.Columns[c].Visible) continue;

                string nombreMostrar = dgv.Columns[c].HeaderText.Trim().ToUpper();

                //  RENOMBRES SEGÚN SOLICITUD
                switch (nombreMostrar)
                {
                    case "CLAVEUSUARIO": nombreMostrar = "USUARIO"; break;
                    case "PRIMERAPELLIDO": nombreMostrar = "PRIMER APELLIDO"; break;
                    case "SEGUNDOAPELLIDO": nombreMostrar = "SEGUNDO APELLIDO"; break;
                    case "TIPOUSUARIO": nombreMostrar = "TIPO DE USUARIO"; break;
                }

                hoja.Cell(filaEncabezados, col).Value = nombreMostrar;
                col++;
            }

            //  Estilo encabezados
            var rangoEnc = hoja.Range(filaEncabezados, 1, filaEncabezados, col - 1);
            rangoEnc.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
            rangoEnc.Style.Font.SetBold();
            rangoEnc.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // ✅ CARGA DE DATOS
            int fila = filaEncabezados + 1;
            for (int f = 0; f < dgv.Rows.Count; f++)
            {
                if (dgv.Rows[f].IsNewRow) continue;

                col = 1;
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    if (!dgv.Columns[c].Visible) continue;

                    var valor = dgv.Rows[f].Cells[c].Value;
                    hoja.Cell(fila, col).Value = valor?.ToString() ?? "";
                    col++;
                }
                fila++;
            }

            //  APLICAR ANCHOS DE COLUMNA
            for (int c = 0; c < anchosColumnas.Length && c < dgv.Columns.Count; c++)
            {
                hoja.Column(c + 1).Width = anchosColumnas[c];
            }

            //  BORDES A TODA LA TABLA
            int ultimaFila = fila - 1;
            var rangoDatos = hoja.Range(filaEncabezados, 1, ultimaFila, dgv.Columns.Count);
            rangoDatos.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rangoDatos.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            //  BLOQUEAR FILAS DE ENCABEZADO
            hoja.SheetView.FreezeRows(filaEncabezados);
        }
    }
}
