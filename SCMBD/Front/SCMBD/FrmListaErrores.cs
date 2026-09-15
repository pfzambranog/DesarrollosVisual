using SCMBD.Properties;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCMBD
{
    public partial class FrmListaErrores : Form
    {
        private readonly DataTable _dtErrores;
        private DataGridView dgv;

        public FrmListaErrores(DataTable dtErrores, string rutaLogo = "")
        {
            _dtErrores = dtErrores;
            ConfigurarPantalla();
        }

        private void ConfigurarPantalla()
        {
            Text = "Lista de Errores de Validación";
            Size = new Size(1200, 650);
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(1000, 500);
            BackColor = Color.LightSteelBlue;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            const int alturaCabecera = 80;

            // ✅ Panel superior
            Panel pnlCabecera = new Panel
            {
                Bounds = new Rectangle(0, 0, this.ClientSize.Width, alturaCabecera),
                BackColor = Color.Transparent
            };

            // ✅ LOGO arriba a la izquierda
            PictureBox picLogo = new PictureBox
            {
                Size = new Size(70, 70),
                Location = new Point(10, 5),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            RecursosCompartidos.CargarLogo(picLogo);
            pnlCabecera.Controls.Add(picLogo);

            // ✅ BOTÓN EXCEL — solo ícono
            Button btnExcel = new Button
            {
                Name = "BtnExcel",
                Location = new Point(1060, 19),
                Size = new Size(59, 42),
                BackColor = Color.LightSteelBlue,
                Image = Resources.Excel,
                ImageAlign = ContentAlignment.MiddleCenter,
                Text = "",
                FlatStyle = FlatStyle.Flat
            };
            btnExcel.FlatAppearance.BorderSize = 0;
            btnExcel.Click += BtnExcel_Click;
            pnlCabecera.Controls.Add(btnExcel);

            // ✅ BOTÓN SALIR — solo ícono
            Button btnSalir = new Button
            {
                Name = "BtnSalir",
                Location = new Point(1125, 19),
                Size = new Size(59, 42),
                BackColor = Color.LightSteelBlue,
                Image = Resources.salir,
                ImageAlign = ContentAlignment.MiddleCenter,
                Text = "",
                FlatStyle = FlatStyle.Flat,
                DialogResult = DialogResult.OK
            };
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.Click += (s, e) => Close();
            pnlCabecera.Controls.Add(btnSalir);

            // ✅ GRID
            dgv = new DataGridView
            {
                Bounds = new Rectangle(0, alturaCabecera, this.ClientSize.Width, this.ClientSize.Height - alturaCabecera),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                DefaultCellStyle = { WrapMode = DataGridViewTriState.True }
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgv.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.RowTemplate.Height = 45;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // ✅ EVENTO: AJUSTAR COLUMNAS CUANDO YA ESTÉN CREADAS
            dgv.DataBindingComplete += Dgv_DataBindingComplete;

            dgv.DataSource = _dtErrores;

            Controls.Add(pnlCabecera);
            Controls.Add(dgv);
        }

        // ✅ SE DISPARA SOLO CUANDO LAS COLUMNAS YA EXISTEN 100%
        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgv.Columns.Count >= 7)
            {
                // ✅ Anchos fijos iguales al diseño original
                dgv.Columns[0].Width = 70;
                dgv.Columns[0].HeaderText = "Secuencia";
                dgv.Columns[0].ReadOnly = true;

                dgv.Columns[1].Width = 200;
                dgv.Columns[1].HeaderText = "Usuario";
                dgv.Columns[1].ReadOnly = true;

                dgv.Columns[2].Width = 300;
                dgv.Columns[2].HeaderText = "Operación";
                dgv.Columns[2].ReadOnly = true;

                dgv.Columns[3].Width = 110;
                dgv.Columns[3].HeaderText = "Autorización";
                dgv.Columns[3].ReadOnly = true;

                dgv.Columns[4].Width = 80;
                dgv.Columns[4].HeaderText = "Estatus";
                dgv.Columns[4].ReadOnly = true;

                dgv.Columns[5].Width = 90;
                dgv.Columns[5].HeaderText = "N° Error";
                dgv.Columns[5].ReadOnly = true;

                // ✅ Mensaje → LLENA TODO el ancho restante de la pantalla
                dgv.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns[6].MinimumWidth = 280;
                dgv.Columns[6].HeaderText = "Mensaje";
                dgv.Columns[6].ReadOnly = true;
                dgv.Columns[6].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
        }

        // ✅ EXPORTAR con tu estándar
        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv == null) return;

                string carpetaReportes = ConfigurationManager.AppSettings["ReportsDirectory"]
                                      ?? @"C:\TempAdam\";

                if (!Directory.Exists(carpetaReportes))
                    Directory.CreateDirectory(carpetaReportes);

                string nombreArchivo = $"ErroresValidacion_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string rutaCompleta = Path.Combine(carpetaReportes, nombreArchivo);

                string operReporte = "ERRORES";
                string tituloReporte = "Lista de Errores de Validación";
                Services.ExcelExportService.ExportarUsuarios(rutaCompleta, dgv, operReporte, tituloReporte, "");

                MessageBox.Show($"✅ Exportado correctamente:\n{rutaCompleta}",
                                "Exportación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = rutaCompleta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al exportar:\n{ex.Message}",
                                "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmListaErrores_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.Error;
            this.Text = "Lista de Errores de Validación";
        }
    }
}
