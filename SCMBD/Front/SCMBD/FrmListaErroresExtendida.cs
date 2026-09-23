using SCMBD.Properties;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCMBD
{
    public partial class FrmListaErroresExtendida : Form
    {
        private readonly DataTable _dtErrores;
        private DataGridView dg;

        public FrmListaErroresExtendida(DataTable dtErrores)
        {
            _dtErrores = dtErrores;
            ConfigurarPantalla();
        }

        private void ConfigurarPantalla()
        {
            Text = "Lista de Errores de Validación";
            Size = new Size(1200, 500);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.LightBlue;
            MinimumSize = new Size(900, 400);

            // Panel Superior
            var pnlTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.LightBlue
            };
            Controls.Add(pnlTop);

            // Logo
            var picLogo = new PictureBox
            {
                Size = new Size(180, 60),
                Location = new Point(10, 5),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            RecursosCompartidos.CargarLogo(picLogo);
            pnlTop.Controls.Add(picLogo);

            // Botón Excel
            var btnExcel = new Button
            {
                Name = "BtnExcel",
                Size = new Size(59, 42),
                Location = new Point(pnlTop.Width - 170, 12),
                BackColor = Color.LightSteelBlue,
                Image = Resources.Excel,
                ImageAlign = ContentAlignment.MiddleCenter
            };
            btnExcel.Click += BtnExcel_Click;
            pnlTop.Controls.Add(btnExcel);

            // Botón Salir
            var btnSalir = new Button
            {
                Name = "BtnSalir",
                Size = new Size(59, 42),
                Location = new Point(pnlTop.Width - 100, 12),
                BackColor = Color.LightSteelBlue,
                Image = Resources.salir,
                ImageAlign = ContentAlignment.MiddleCenter
            };
            btnSalir.Click += (s, e) => Close();
            pnlTop.Controls.Add(btnSalir);

            // ✅ Grid — Configuración corregida
            dg = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                AlternatingRowsDefaultCellStyle = { BackColor = Color.AliceBlue }
            };

            // ✅ Asignar datos ANTES de aplicar encabezados
            dg.DataSource = _dtErrores;

            Controls.Add(dg);
            dg.BringToFront();

            AplicarEncabezadosYAnchos();
        }

        private void AplicarEncabezadosYAnchos()
        {
            if (dg.Columns.Count == 0) return; // Evitar error si no hay columnas

            foreach (DataGridViewColumn col in dg.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                col.HeaderCell.Style.BackColor = Color.LightSteelBlue;

                string nombre = col.Name.ToLower();
                switch (nombre)
                {
                    case "secuencia":
                        col.HeaderText = "Sec";
                        col.FillWeight = 8;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "descripcion":
                        col.HeaderText = "Descripción";
                        col.FillWeight = 25;
                        break;
                    case "codigo":
                    case "error":
                        col.HeaderText = "Código";
                        col.FillWeight = 10;
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case "mensaje":
                        col.HeaderText = "Mensaje / Detalle";
                        col.FillWeight = 57;
                        break;
                    default:
                        col.Visible = false;
                        break;
                }
            }
            dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dtErrores == null || _dtErrores.Rows.Count == 0) return;

                string carpeta = ConfigurationManager.AppSettings["ReportsDirectory"]
                              ?? @"C:\TempAdam\";
                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string archivo = $"ErroresValidacion_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string ruta = Path.Combine(carpeta, archivo);

                Services.ExcelExportService.ExportarUsuarios(
                    ruta, dg, "ERRORES", "Lista de Errores", "Sistema");

                MessageBox.Show($"Exportado correctamente:\n{ruta}", "Exportación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
