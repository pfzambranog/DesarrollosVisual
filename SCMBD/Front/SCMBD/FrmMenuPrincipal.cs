using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

namespace SCMBD
{
    public partial class FrmMenuPrincipal : Form
    {
        private int _idUsuario;
        private string _claveUsuario;
        private DataTable _dtPermisos;
        private string _cadenaConexion;

        // Controles dinámicos
        private Panel pnlHeader;
        private Label lblTitulo;
        private Label lblFecha, lblOperacion, lblUsuario;
        private TextBox txtFecha, txtOperacion, txtUsuario;
        private ListView lstOperaciones;

        public FrmMenuPrincipal(int idUsuario, string claveUsuario, DataTable dtPermisos, string cadenaConexion)
        {
            _idUsuario = idUsuario;
            _claveUsuario = claveUsuario;
            _dtPermisos = dtPermisos;
            _cadenaConexion = cadenaConexion;
            InitializeComponent();
            CargarDatosPantalla();
            CargarOperaciones();
        }

        private void InitializeComponent()
        {
            this.Text = "SCMBD — Sistema de Control y Mantenimiento de Bases de Datos";
            this.Size = new Size(820, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(800, 500);

            // =============================================
            // Panel Superior (Encabezado)
            // =============================================
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 144,
                BackColor = SystemColors.ActiveCaption
            };

            // --- Logo (izquierda superior) ---
            PictureBox picLogo = new PictureBox
            {
                ImageLocation = @"Imagenes\LogoSCMBD.png",
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(10, 10),
                Size = new Size(180, 120)
            };
            pnlHeader.Controls.Add(picLogo);

            // --- Etiquetas y Campos a la Derecha ---
            lblFecha = CrearLabel("Fecha:", 580, 6);
            txtFecha = CrearTexto(654, 6);
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblOperacion = CrearLabel("Operación:", 580, 22);
            txtOperacion = CrearTexto(654, 20);
            txtOperacion.Text = ConfigurationManager.AppSettings["Operacion"] ?? "FPLS001";

            lblUsuario = CrearLabel("Usuario:", 580, 38);
            txtUsuario = CrearTexto(654, 38);
            txtUsuario.Text = _claveUsuario;

            lblTitulo = new Label
            {
                Text = "MENÚ PRINCIPAL",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 40
            };

            pnlHeader.Controls.AddRange(new Control[] { lblFecha, txtFecha, lblOperacion, txtOperacion, lblUsuario, txtUsuario, lblTitulo });
            this.Controls.Add(pnlHeader);

            // =============================================
            // Lista de Operaciones (Centro de pantalla)
            // =============================================
            lstOperaciones = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Segoe UI", 10),
                Margin = new Padding(20)
            };
            lstOperaciones.Columns.Add("Operación", 600);
            lstOperaciones.DoubleClick += LstOperaciones_DoubleClick;
            lstOperaciones.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) AbrirPantallaSeleccionada(); };

            this.Controls.Add(lstOperaciones);
        }

        private Label CrearLabel(string texto, int x, int y)
        {
            return new Label
            {
                Text = texto,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold)
            };
        }

        private TextBox CrearTexto(int x, int y)
        {
            return new TextBox
            {
                Location = new Point(x, y),
                Width = 150,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = SystemColors.ActiveCaption,
                Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold)
            };
        }

        private void CargarDatosPantalla()
        {
            // Campos ya asignados en InitializeComponent
        }

        private void CargarOperaciones()
        {
            lstOperaciones.Items.Clear();

            // Agrupar por nombre de operación y guardar idOperacion
            var ops = _dtPermisos.AsEnumerable()
                .GroupBy(r => r["Operacion"].ToString().Trim())
                .Select(g => new { Nombre = g.Key, IdOperacion = g.First()["idOperacion"].ToString() })
                .OrderBy(x => x.Nombre);

            foreach (var op in ops)
            {
                ListViewItem item = new ListViewItem(op.Nombre);
                item.Tag = op.IdOperacion; // Guardar idOperacion para buscar pantalla
                lstOperaciones.Items.Add(item);
            }
        }

        private void LstOperaciones_DoubleClick(object sender, EventArgs e)
        {
            AbrirPantallaSeleccionada();
        }

        private void AbrirPantallaSeleccionada()
        {
            if (lstOperaciones.SelectedItems.Count == 0) return;

            string idOperacion = lstOperaciones.SelectedItems[0].Tag.ToString();
            string nombreOp = lstOperaciones.SelectedItems[0].Text;

            try
            {
                // =============================================
                // Consultar nombre de pantalla en catOperacionesTbl
                // =============================================
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SELECT llamada FROM dbo.catOperacionesTbl WHERE idOperacion = @IdOp", cn))
                {
                    cmd.Parameters.AddWithValue("@IdOp", idOperacion);
                    cn.Open();
                    string nombrePantalla = cmd.ExecuteScalar()?.ToString();

                    if (string.IsNullOrWhiteSpace(nombrePantalla))
                    {
                        MessageBox.Show($"No está definida la pantalla para: {nombreOp}", "Sin Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // =============================================
                    // 🚀 Abrir pantalla por reflexión
                    // =============================================
                    Type tipoPantalla = Type.GetType($"SCMBD.{nombrePantalla}");
                    if (tipoPantalla == null)
                    {
                        MessageBox.Show($"No se encontró la pantalla: {nombrePantalla}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Form pantalla = Activator.CreateInstance(tipoPantalla, _idUsuario, _cadenaConexion) as Form;
                    if (pantalla != null)
                    {
                        pantalla.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir operación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
