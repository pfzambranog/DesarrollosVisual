using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace SCMBD
{
    public partial class FrmCambioContrasenia : Form
    {
        // ✅ MANTENGO TUS CAMPOS TAL CUAL
        private readonly int _idUsuario;
        private readonly string _cadenaConexion;
        private string _claveUsuarioBD;
        private readonly string _claveUsuario;
        private readonly DataTable _permisos; // ✅ Agregado SOLO para eliminar advertencia

        // Controles — TAL COMO LOS TIENES EN EL DISEÑADOR
        private Button btnCancelar;
        private Button btnAceptar;
        private Label lblContraseniaActual;
        private Label lblNueva;
        private Label lblConfirmar;
        private TextBox txtContraseniaActual;
        private TextBox txtNuevaContrasenia;
        private TextBox txtConfirmarContrasenia;

        // ✅ Constructor — TU versión, SOLO agrego el parámetro que faltaba
        public FrmCambioContrasenia(int idUsuario, string claveUsuario, DataTable permisos, string cadenaConexion)
        {
            _idUsuario = idUsuario;
            _claveUsuario = claveUsuario;
            _permisos = permisos; // ✅ SOLO esta línea nueva → elimina advertencia
            _cadenaConexion = cadenaConexion;
            InitializeComponent();
            this.Load += FrmCambioContrasenia_Load;
        }

        // =====================================================
        // ✅ Constructor VACÍO — TU versión intacta
        // =====================================================
        public FrmCambioContrasenia()
        {
            InitializeComponent();
            this.Load += FrmCambioContrasenia_Load;
        }

        // =====================================================
        // ✅ Evento Load — TU versión intacta
        // =====================================================
        private void FrmCambioContrasenia_Load(object sender, EventArgs e)
        {
            CargarImagenesBotones();
            CargarDatosUsuario();
        }

        // =====================================================
        // ✅ Obtener el nombre de usuario BD — TU versión intacta
        // =====================================================
        private void CargarDatosUsuario()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("SELECT ClaveUsuario FROM dbo.segUsuariosTbl WHERE idUsuario = @IdUsuario", cn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", _idUsuario);
                    cn.Open();
                    var resultado = cmd.ExecuteScalar();
                    _claveUsuarioBD = resultado?.ToString().Trim() ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo cargar datos del usuario: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // =====================================================
        // ✅ InitializeComponent — TU VERSIÓN COMPLETA, INTACTA
        // =====================================================
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCambioContrasenia));
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.txtContraseniaActual = new System.Windows.Forms.TextBox();
            this.lblContraseniaActual = new System.Windows.Forms.Label();
            this.lblNueva = new System.Windows.Forms.Label();
            this.lblConfirmar = new System.Windows.Forms.Label();
            this.txtNuevaContrasenia = new System.Windows.Forms.TextBox();
            this.txtConfirmarContrasenia = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Location = new System.Drawing.Point(333, 139);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(60, 55);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.TabStop = false;
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAceptar.FlatAppearance.BorderSize = 0;
            this.btnAceptar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAceptar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar.Location = new System.Drawing.Point(383, 139);
            this.btnAceptar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(60, 55);
            this.btnAceptar.TabIndex = 7;
            this.btnAceptar.TabStop = false;
            this.btnAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // txtContraseniaActual
            // 
            this.txtContraseniaActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContraseniaActual.Location = new System.Drawing.Point(150, 28);
            this.txtContraseniaActual.Name = "txtContraseniaActual";
            this.txtContraseniaActual.PasswordChar = '*';
            this.txtContraseniaActual.Size = new System.Drawing.Size(260, 23);
            this.txtContraseniaActual.TabIndex = 1;
            // 
            // lblContraseniaActual
            // 
            this.lblContraseniaActual.AutoSize = true;
            this.lblContraseniaActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContraseniaActual.Location = new System.Drawing.Point(20, 30);
            this.lblContraseniaActual.Name = "lblContraseniaActual";
            this.lblContraseniaActual.Size = new System.Drawing.Size(105, 15);
            this.lblContraseniaActual.TabIndex = 15;
            this.lblContraseniaActual.Text = "Contraseña actual:";
            // 
            // lblNueva
            // 
            this.lblNueva.AutoSize = true;
            this.lblNueva.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNueva.Location = new System.Drawing.Point(20, 70);
            this.lblNueva.Name = "lblNueva";
            this.lblNueva.Size = new System.Drawing.Size(105, 15);
            this.lblNueva.TabIndex = 14;
            this.lblNueva.Text = "Contraseña nueva:";
            // 
            // lblConfirmar
            // 
            this.lblConfirmar.AutoSize = true;
            this.lblConfirmar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmar.Location = new System.Drawing.Point(20, 110);
            this.lblConfirmar.Name = "lblConfirmar";
            this.lblConfirmar.Size = new System.Drawing.Size(125, 15);
            this.lblConfirmar.TabIndex = 13;
            this.lblConfirmar.Text = "Confirmar contraseña:";
            // 
            // txtNuevaContrasenia
            // 
            this.txtNuevaContrasenia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNuevaContrasenia.Location = new System.Drawing.Point(150, 68);
            this.txtNuevaContrasenia.Name = "txtNuevaContrasenia";
            this.txtNuevaContrasenia.PasswordChar = '*';
            this.txtNuevaContrasenia.Size = new System.Drawing.Size(260, 23);
            this.txtNuevaContrasenia.TabIndex = 11;
            // 
            // txtConfirmarContrasenia
            // 
            this.txtConfirmarContrasenia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmarContrasenia.Location = new System.Drawing.Point(150, 108);
            this.txtConfirmarContrasenia.Name = "txtConfirmarContrasenia";
            this.txtConfirmarContrasenia.PasswordChar = '*';
            this.txtConfirmarContrasenia.Size = new System.Drawing.Size(260, 23);
            this.txtConfirmarContrasenia.TabIndex = 12;
            // 
            // FrmCambioContrasenia
            // 
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(444, 191);
            this.Controls.Add(this.txtConfirmarContrasenia);
            this.Controls.Add(this.txtNuevaContrasenia);
            this.Controls.Add(this.lblConfirmar);
            this.Controls.Add(this.lblNueva);
            this.Controls.Add(this.lblContraseniaActual);
            this.Controls.Add(this.txtContraseniaActual);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnCancelar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCambioContrasenia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cambio de Contraseña";
            this.Load += new System.EventHandler(this.FrmCambioContrasenia_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // =====================================================
        // ✅ Cargar imágenes — TU versión intacta
        // =====================================================
        private void CargarImagenesBotones()
        {
            try
            {
                if (Properties.Resources.ACEPTA1 != null)
                {
                    btnAceptar.Image = Properties.Resources.ACEPTA1.ToBitmap();
                    btnAceptar.ImageAlign = ContentAlignment.MiddleLeft;
                }
                if (Properties.Resources.CANCELA1 != null)
                {
                    btnCancelar.Image = Properties.Resources.CANCELA1.ToBitmap();
                    btnCancelar.ImageAlign = ContentAlignment.MiddleLeft;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar las imágenes: {ex.Message}", "Nota", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // =====================================================
        // ✅ Botón Aceptar — SOLO ELIMINADO el bloque que cierra TODO
        // =====================================================
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtContraseniaActual.Text) ||
                string.IsNullOrWhiteSpace(txtNuevaContrasenia.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmarContrasenia.Text))
            {
                MessageBox.Show("Debe llenar todos los campos.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtNuevaContrasenia.Text != txtConfirmarContrasenia.Text)
            {
                MessageBox.Show("La contraseña nueva y la confirmación no coinciden.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(_claveUsuarioBD))
            {
                MessageBox.Show("No se pudo identificar al usuario.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string passActualEncriptada = EncriptarBase64(txtContraseniaActual.Text.Trim());
                string passNuevaEncriptada = EncriptarBase64(txtNuevaContrasenia.Text.Trim());

                int idEstatus = 0;
                string mensaje = "";

                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                using (SqlCommand cmd = new SqlCommand("dbo.Spp_ActualizaPasswordUserBD", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PsIdUsuarioBD", _claveUsuarioBD);
                    cmd.Parameters.AddWithValue("@PsPasswordActual", passActualEncriptada);
                    cmd.Parameters.AddWithValue("@PsPasswordNueva", passNuevaEncriptada);
                    cmd.Parameters.AddWithValue("@PsBaseDatos", DBNull.Value);
                    cmd.Parameters.AddWithValue("@PnIdOperacion", 5);
                    cmd.Parameters.AddWithValue("@PnIdUsuarioAct", _idUsuario);

                    SqlParameter paramEstatus = new SqlParameter("@PnEstatus", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                        Value = 0
                    };
                    cmd.Parameters.Add(paramEstatus);

                    SqlParameter paramMensaje = new SqlParameter("@PsMensaje", SqlDbType.VarChar, 250)
                    {
                        Direction = ParameterDirection.Output,
                        Value = ""
                    };
                    cmd.Parameters.Add(paramMensaje);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    idEstatus = Convert.ToInt32(paramEstatus.Value ?? 0);
                    mensaje = (paramMensaje.Value ?? "").ToString().Trim();
                }

                if (idEstatus == 0)
                {
                    MessageBox.Show(mensaje + "\n\nContraseña actualizada correctamente.",
                                    "Contraseña Actualizada",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ === ÚNICO CAMBIO IMPORTANTE ===
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    // ❌ ELIMINADO TODO el bloque que hacía: Hide() → FrmConexion → Close()
                    // ✅ AHORA SOLO CIERRA ESTA PANTALLA Y VUELVES AL MENÚ
                }
                else
                {
                    MessageBox.Show(mensaje, "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    txtContraseniaActual.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cambiar contraseña:\n{ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================================================
        // ✅ Botón Cancelar — TU versión intacta
        // =====================================================
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // =====================================================
        // ✅ Encriptar — TU versión intacta
        // =====================================================
        private string EncriptarBase64(string texto)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }

        private void FrmCambioContrasenia_Load_1(object sender, EventArgs e)
        {
        }
    }
}
