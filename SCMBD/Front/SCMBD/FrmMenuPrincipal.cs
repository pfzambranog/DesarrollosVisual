using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace SCMBD
{
    public partial class FrmMenuPrincipal : Form
    {
        private readonly int _idUsuario;
        private readonly string _claveUsuario;
        private readonly DataTable _dtPermisos;
        private readonly string _operacion;
        private readonly string _cadenaConexion;

        private Panel pnlHeader;
        private Panel pnlBarraInferior;
        private Label lblTitulo;
        private Label lblFecha, lblOperacion, lblUsuario;
        private TextBox txtFecha, txtOperacion, txtUsuario;
        private ListView lstOperaciones;
        private PictureBox picLogo;
        private Button btnCambioContrasenia;
        private Button btnSalir;
        private ToolTip toolTipBotones;

        // ✅ Constructor ajustado: recibe operacion como parámetro
        public FrmMenuPrincipal(int idUsuario, string claveUsuario, DataTable dtPermisos, string operacion, string cadenaConexion)
        {
            _idUsuario = idUsuario;
            _claveUsuario = claveUsuario;
            _dtPermisos = dtPermisos;
            _operacion = operacion;
            _cadenaConexion = cadenaConexion;

            InitializeComponent();
            this.Load += FrmMenuPrincipal_Load;
        }

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            txtOperacion.Text = ConfigurationManager.AppSettings["Operacion"] ?? "SCMBD01";
            txtUsuario.Text = _claveUsuario;

            CargarLogo();
            CargarOperaciones();
            CargarIconoVentana();
            CargarImagenesBotones();
            ConfigurarTooltips();

            string nombrePantalla = ObtenerLlamadaCambioContrasenia();
            btnCambioContrasenia.Enabled = !string.IsNullOrEmpty(nombrePantalla);
            btnCambioContrasenia.Tag = nombrePantalla;
        }

        private void ConfigurarTooltips()
        {
            toolTipBotones = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 400,
                ReshowDelay = 200,
                ShowAlways = true
            };
            toolTipBotones.SetToolTip(btnCambioContrasenia, "Cambiar contraseña del usuario actual");
            toolTipBotones.SetToolTip(btnSalir, "Salir de la aplicación");
        }

        private string ObtenerLlamadaCambioContrasenia()
        {
            string nombrePantalla = null;
            try
            {
                const string operacionClave = "SU1999";
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    string sql = @"SELECT TRIM(a.ruta) + '.' + TRIM(a.llamada) AS llamadaOperacion
                                   FROM   dbo.catOperacionesTbl a
                                   INNER JOIN dbo.segAutOperacionesTbl b 
                                           ON b.idOperacion = a.idOperacion
                                   WHERE  a.operacion = @Operacion 
                                     AND b.idAutorizacion = 4";
                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@Operacion", operacionClave);
                        cn.Open();
                        var resultado = cmd.ExecuteScalar();
                        if (resultado != null)
                        {
                            string llamadaOperacion = resultado.ToString().Trim();
                            if (llamadaOperacion.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                                llamadaOperacion = llamadaOperacion.Substring(0, llamadaOperacion.Length - 3);
                            if (llamadaOperacion.StartsWith("SCMBD.", StringComparison.OrdinalIgnoreCase))
                                llamadaOperacion = llamadaOperacion.Substring(6);
                            nombrePantalla = llamadaOperacion;
                        }
                    }
                }
            }
            catch { }
            return nombrePantalla;
        }

        private void CargarImagenesBotones()
        {
            try
            {
                if (Properties.Resources.CANCELA1 != null)
                {
                    btnSalir.Image = Properties.Resources.CANCELA1.ToBitmap();
                    btnSalir.ImageAlign = ContentAlignment.MiddleLeft;
                }
                if (Properties.Resources.cambio != null)
                {
                    btnCambioContrasenia.Image = Properties.Resources.cambio.ToBitmap();
                    btnCambioContrasenia.ImageAlign = ContentAlignment.MiddleLeft;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar las imágenes: {ex.Message}", "Nota", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Seguro que desea salir de la aplicación?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2); // ✅ NO por defecto

            // ✅ Compara AMBOS valores por seguridad
            if (respuesta == DialogResult.Yes || respuesta == DialogResult.OK)
            {
                Application.Exit();
            }

            // ✅ Si llega aquí → NO hace nada
        }



        private void BtnCambioContrasenia_Click(object sender, EventArgs e)
        {
            string nombrePantalla = btnCambioContrasenia.Tag?.ToString() ?? "FrmCambioContrasenia";
            Type tipoPantalla = Type.GetType($"SCMBD.{nombrePantalla}");
            if (tipoPantalla != null)
            {
                // ✅ Pasar los 5 parámetros al constructor

                Form pantalla = Activator.CreateInstance(tipoPantalla, _idUsuario, _claveUsuario, _dtPermisos, _operacion, _cadenaConexion) as Form;
                if (pantalla != null)
                {
                    pantalla.Owner = this;
                    pantalla.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("No se encontró la pantalla de Cambio de Contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void CargarOperaciones()
        {
            lstOperaciones.Items.Clear();
            lstOperaciones.Columns.Clear();
            lstOperaciones.Columns.Add("", -1);

            if (_dtPermisos == null || _dtPermisos.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron permisos para este usuario.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var filasOrdenadas = _dtPermisos.AsEnumerable()
                .OrderBy(r => r["idMenu"])
                .ThenBy(r => r["idOperacion"]);

            foreach (var fila in filasOrdenadas)
            {
                int idOperacion = Convert.ToInt32(fila["idOperacion"]);
                int idAutorizacion = Convert.ToInt32(fila["idAutorizacion"]);
                string nombreMenu = fila["Menu"].ToString();
                string nombreOp = fila["Operacion"].ToString();
                string llamada = fila["llamada"]?.ToString() ?? "";

                if (idOperacion == 0 || idAutorizacion == 0)
                {
                    var itemMenu = new ListViewItem(nombreMenu)
                    {
                        Tag = null,
                        Font = new Font(lstOperaciones.Font, FontStyle.Bold)
                    };
                    lstOperaciones.Items.Add(itemMenu);
                }
                else
                {
                    var itemOp = new ListViewItem("    " + nombreOp)
                    {
                        Tag = new Tuple<string, string>(idOperacion.ToString(), llamada),
                        Font = new Font(lstOperaciones.Font, FontStyle.Regular)
                    };
                    lstOperaciones.Items.Add(itemOp);
                }
            }

            lstOperaciones.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lstOperaciones.DoubleClick += LstOperaciones_DoubleClick;
            lstOperaciones.KeyDown += LstOperaciones_KeyDown;
        }

        private void LstOperaciones_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                AbrirPantallaSeleccionada();
            }
        }

        private void LstOperaciones_DoubleClick(object sender, EventArgs e)
        {
            AbrirPantallaSeleccionada();
        }

        private void FrmMenuPrincipal_Load_1(object sender, EventArgs e)
        {

        }

        private void AbrirPantallaSeleccionada()
        {
            if (lstOperaciones.SelectedItems.Count == 0) return;
            var item = lstOperaciones.SelectedItems[0];

            if (item.Tag is Tuple<string, string> datos)
            {
                string llamada = datos.Item2;
                if (string.IsNullOrWhiteSpace(llamada))
                {
                    MessageBox.Show("No está definida la pantalla para esta operación.",
                                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string nombrePantalla = llamada.Replace(".cs", "").Trim();
                Type tipoPantalla = Type.GetType($"SCMBD.{nombrePantalla}");

                if (tipoPantalla == null)
                {
                    MessageBox.Show($"No se encontró la pantalla: {nombrePantalla}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }

                // ✅ Abrir con los 5 parámetros: id, clave, permisos, operacion, cadena
                Form pantalla = Activator.CreateInstance(tipoPantalla,
                                    _idUsuario, _claveUsuario, _dtPermisos, _operacion, _cadenaConexion) as Form;
                if (pantalla != null)
                    pantalla.ShowDialog();
                else
                    MessageBox.Show("No se pudo abrir la pantalla.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarLogo()
        {
            RecursosCompartidos.CargarLogo(picLogo);
        }

        private void CargarIconoVentana()
        {
            try
            {
                string rutaLogo = Path.Combine(Application.StartupPath, @"Imagenes\LogoSCMBD.png");
                if (File.Exists(rutaLogo))
                {
                    using (Bitmap bmp = new Bitmap(rutaLogo))
                    {
                        this.Icon = Icon.FromHandle(bmp.GetHicon());
                    }
                }
            }
            catch { }
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.txtFecha = new System.Windows.Forms.TextBox();
            this.lblOperacion = new System.Windows.Forms.Label();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lstOperaciones = new System.Windows.Forms.ListView();
            this.pnlBarraInferior = new System.Windows.Forms.Panel();
            this.btnCambioContrasenia = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlBarraInferior.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlHeader.Controls.Add(this.picLogo);
            this.pnlHeader.Controls.Add(this.lblFecha);
            this.pnlHeader.Controls.Add(this.txtFecha);
            this.pnlHeader.Controls.Add(this.lblOperacion);
            this.pnlHeader.Controls.Add(this.txtOperacion);
            this.pnlHeader.Controls.Add(this.lblUsuario);
            this.pnlHeader.Controls.Add(this.txtUsuario);
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(804, 144);
            this.pnlHeader.TabIndex = 0;
            // 
            // picLogo
            // 
            this.picLogo.Location = new System.Drawing.Point(1, 1);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(106, 41);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFecha.Location = new System.Drawing.Point(580, 6);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(46, 13);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.Text = "Fecha:";
            // 
            // txtFecha
            // 
            this.txtFecha.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtFecha.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFecha.Enabled = false;
            this.txtFecha.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtFecha.Location = new System.Drawing.Point(654, 6);
            this.txtFecha.Name = "txtFecha";
            this.txtFecha.ReadOnly = true;
            this.txtFecha.Size = new System.Drawing.Size(150, 13);
            this.txtFecha.TabIndex = 2;
            // 
            // lblOperacion
            // 
            this.lblOperacion.AutoSize = true;
            this.lblOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblOperacion.Location = new System.Drawing.Point(580, 22);
            this.lblOperacion.Name = "lblOperacion";
            this.lblOperacion.Size = new System.Drawing.Size(69, 13);
            this.lblOperacion.TabIndex = 3;
            this.lblOperacion.Text = "Operación:";
            // 
            // txtOperacion
            // 
            this.txtOperacion.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtOperacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperacion.Enabled = false;
            this.txtOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtOperacion.Location = new System.Drawing.Point(654, 20);
            this.txtOperacion.Name = "txtOperacion";
            this.txtOperacion.ReadOnly = true;
            this.txtOperacion.Size = new System.Drawing.Size(150, 13);
            this.txtOperacion.TabIndex = 4;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblUsuario.Location = new System.Drawing.Point(580, 38);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(54, 13);
            this.lblUsuario.TabIndex = 5;
            this.lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsuario.Enabled = false;
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtUsuario.Location = new System.Drawing.Point(654, 38);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(150, 13);
            this.txtUsuario.TabIndex = 6;
            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.LightSteelBlue;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(0, 104);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(804, 40);
            this.lblTitulo.TabIndex = 7;
            this.lblTitulo.Text = "MENÚ PRINCIPAL";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstOperaciones
            // 
            this.lstOperaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstOperaciones.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstOperaciones.FullRowSelect = true;
            this.lstOperaciones.GridLines = true;
            this.lstOperaciones.HideSelection = false;
            this.lstOperaciones.Location = new System.Drawing.Point(0, 144);
            this.lstOperaciones.Margin = new System.Windows.Forms.Padding(5);
            this.lstOperaciones.Name = "lstOperaciones";
            this.lstOperaciones.Size = new System.Drawing.Size(804, 336);
            this.lstOperaciones.TabIndex = 2;
            this.lstOperaciones.UseCompatibleStateImageBehavior = false;
            this.lstOperaciones.View = System.Windows.Forms.View.Details;
            // 
            // pnlBarraInferior
            // 
            this.pnlBarraInferior.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlBarraInferior.Controls.Add(this.btnCambioContrasenia);
            this.pnlBarraInferior.Controls.Add(this.btnSalir);
            this.pnlBarraInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBarraInferior.Location = new System.Drawing.Point(0, 480);
            this.pnlBarraInferior.Name = "pnlBarraInferior";
            this.pnlBarraInferior.Size = new System.Drawing.Size(804, 70);
            this.pnlBarraInferior.TabIndex = 1;
            // 
            // btnCambioContrasenia
            // 
            this.btnCambioContrasenia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCambioContrasenia.AutoSize = true;
            this.btnCambioContrasenia.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnCambioContrasenia.FlatAppearance.BorderSize = 0;
            this.btnCambioContrasenia.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCambioContrasenia.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCambioContrasenia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambioContrasenia.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCambioContrasenia.Location = new System.Drawing.Point(660, 10);
            this.btnCambioContrasenia.Name = "btnCambioContrasenia";
            this.btnCambioContrasenia.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btnCambioContrasenia.Size = new System.Drawing.Size(70, 55);
            this.btnCambioContrasenia.TabIndex = 0;
            this.btnCambioContrasenia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCambioContrasenia.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnCambioContrasenia.UseVisualStyleBackColor = false;
            this.btnCambioContrasenia.Click += new System.EventHandler(this.BtnCambioContrasenia_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSalir.FlatAppearance.BorderSize = 0;
            this.btnSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.Location = new System.Drawing.Point(724, 8);
            this.btnSalir.Margin = new System.Windows.Forms.Padding(5, 0, 8, 0);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(70, 55);
            this.btnSalir.TabIndex = 1;
            this.btnSalir.TabStop = false;
            this.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // FrmMenuPrincipal
            // 
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(804, 550);
            this.Controls.Add(this.lstOperaciones);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlBarraInferior);
            this.MinimumSize = new System.Drawing.Size(800, 580);
            this.Name = "FrmMenuPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCMBD — Sistema de Control y Mantenimiento de Bases de Datos";
            this.Load += new System.EventHandler(this.FrmMenuPrincipal_Load_1);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlBarraInferior.ResumeLayout(false);
            this.pnlBarraInferior.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
