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
            Size = new Size(1400, 700);
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(1100, 550);
            BackColor = Color.LightSteelBlue;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            const int alturaCabecera = 80;
            Panel pnlCabecera = new Panel
            {
                Bounds = new Rectangle(0, 0, this.ClientSize.Width, alturaCabecera),
                BackColor = Color.Transparent
            };

            // Logo
            PictureBox picLogo = new PictureBox
            {
                Size = new Size(70, 70),
                Location = new Point(10, 5),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            RecursosCompartidos.CargarLogo(picLogo);
            pnlCabecera.Controls.Add(picLogo);

            // ✅ BOTÓN EXCEL — movido hacia la izquierda
            Button btnExcel = new Button
            {
                Name = "BtnExcel",
                Location = new Point(1200, 19),  // ← Antes: 1280
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

            // ✅ BOTÓN SALIR — movido hacia la izquierda
            Button btnSalir = new Button
            {
                Name = "BtnSalir",
                Location = new Point(1270, 19),  // ← Antes: 1345
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

            // Grid
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

            dgv.DataBindingComplete += Dgv_DataBindingComplete;
            dgv.DataSource = _dtErrores;

            Controls.Add(pnlCabecera);
            Controls.Add(dgv);
        }

        private void Dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgv.Columns.Count == 0) return;

            bool esReglas = _dtErrores.Columns.Contains("Regla")
                         || _dtErrores.Columns.Contains("codRegla");

            if (esReglas)
            {
                AplicarEstiloComun();
                ConfigurarColumnasReglas();
            }
            else
            {
                AplicarEstiloComun();
                ConfigurarColumnasUsuarioOperacion();
            }
        }

        private void AplicarEstiloComun()
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.ReadOnly = true;
            }
        }

        private void ConfigurarColumnasReglas()
        {
            if (dgv.Columns.Count >= 9)
            {
                dgv.Columns[0].Width = 60;
                dgv.Columns[0].HeaderText = "Sec";

                dgv.Columns[1].Width = 110;
                dgv.Columns[1].HeaderText = "Código Regla";

                dgv.Columns[2].Width = 220;
                dgv.Columns[2].HeaderText = "Nombre de Regla";
                dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                dgv.Columns[3].Width = 300;
                dgv.Columns[3].HeaderText = "Descripción";
                dgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                dgv.Columns[4].Width = 100;
                dgv.Columns[4].HeaderText = "Requerido";

                dgv.Columns[5].Width = 130;
                dgv.Columns[5].HeaderText = "Valor Mínimo";

                dgv.Columns[6].Width = 100;
                dgv.Columns[6].HeaderText = "Estatus";

                dgv.Columns[7].Width = 100;
                dgv.Columns[7].HeaderText = "N° Error";

                dgv.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns[8].MinimumWidth = 350;
                dgv.Columns[8].HeaderText = "Mensaje";
                dgv.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        private void ConfigurarColumnasUsuarioOperacion()
        {
            if (dgv.Columns.Count >= 7)
            {
                dgv.Columns[0].Width = 70;
                dgv.Columns[0].HeaderText = "Secuencia";

                dgv.Columns[1].Width = 220;
                dgv.Columns[1].HeaderText = "Usuario";

                dgv.Columns[2].Width = 320;
                dgv.Columns[2].HeaderText = "Operación";
                dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                dgv.Columns[3].Width = 150;
                dgv.Columns[3].HeaderText = "Autorización";

                dgv.Columns[4].Width = 90;
                dgv.Columns[4].HeaderText = "Estatus";

                dgv.Columns[5].Width = 100;
                dgv.Columns[5].HeaderText = "N° Error";

                dgv.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns[6].MinimumWidth = 350;
                dgv.Columns[6].HeaderText = "Mensaje";
                dgv.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv == null) return;
                string carpeta = ConfigurationManager.AppSettings["ReportsDirectory"] ?? @"C:\TempAdam\";
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                string archivo = $"ErroresValidacion_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                string ruta = Path.Combine(carpeta, archivo);
                string operReporte = "ERRORES";
                string titulo = "Lista de Errores de Validación";
                Services.ExcelExportService.ExportarUsuarios(ruta, dgv, operReporte, titulo, "");
                MessageBox.Show($"✅ Exportado correctamente:\n{ruta}",
                                "Exportación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al exportar:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmListaErrores_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.Error;
            this.Text = "Lista de Errores de Validación";
        }
    }
}
