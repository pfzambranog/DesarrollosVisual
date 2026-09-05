using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace SCMBD
{
    public partial class FrmConexion : Form
    {
        // Datos públicos para pasar al Menú
        public int IdUsuario { get; private set; }
        public string ClaveUsuario { get; private set; }
        public int IdTipoUsuario { get; private set; }
        public string CadenaConexion { get; private set; }

        public FrmConexion()
        {
            InitializeComponent();
        }

        private void FrmConexion_Load(object sender, EventArgs e)
        {
            txtUsuario.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUsuario.Focus();

            try
            {
                // 📋 Leer rutas desde App.config
                string rutaImagenes = ConfigurationManager.AppSettings["Imagenes"];
                string rutaLogo = Path.Combine(rutaImagenes, "LogoSCMBD.png");

                // ✅ VERIFICAR SI EXISTE EL ARCHIVO
                if (File.Exists(rutaLogo))
                {
                    picLogo.Image = Image.FromFile(rutaLogo);
                    picLogo.Visible = true;

                    // ✅ ASIGNAR ÍCONO DEL FORMULARIO DESDE EL MISMO ARCHIVO
                    using (Bitmap bmp = new Bitmap(rutaLogo))
                    {
                        this.Icon = Icon.FromHandle(bmp.GetHicon());
                    }
                }
                else
                {
                    MessageBox.Show($"No se encontró el archivo:\n{rutaLogo}\n\nVerifica que la carpeta Imagenes existe junto al ejecutable.",
                                    "Archivo No Encontrado",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando imágenes: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ✅ Validar recursos de botones
            if (global::SCMBD.Properties.Resources.ACEPTA1 == null)
            {
                MessageBox.Show("Recurso ACEPTA1 no está registrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (global::SCMBD.Properties.Resources.CANCELA1 == null)
            {
                MessageBox.Show("Recurso CANCELA1 no está registrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ✅ Corregido nombre: B mayúscula
        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    MessageBox.Show("Debe ingresar el Usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsuario.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Debe ingresar la Contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // 📌 Armar cadena con credenciales
                string strBase = ConfigurationManager.ConnectionStrings["SCMBD"].ConnectionString;
                CadenaConexion = $"{strBase};User ID={txtUsuario.Text.Trim()};Password={txtPassword.Text};";

                using (SqlConnection cn = new SqlConnection(CadenaConexion))
                {
                    cn.Open(); // Prueba conexión

                    // =============================================
                    // 1. Validar que el usuario exista en la app
                    // =============================================
                    string sqlUsuario = @"
                        SELECT idUsuario, idTipoUsuario, idEstatus
                        FROM dbo.segUsuariosTbl
                        WHERE claveUsuario = @ClaveUsuario";

                    int idEstatus;
                    using (SqlCommand cmd = new SqlCommand(sqlUsuario, cn))
                    {
                        cmd.Parameters.AddWithValue("@ClaveUsuario", txtUsuario.Text.Trim());
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                MessageBox.Show("Usuario no registrado en la aplicación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            IdUsuario = Convert.ToInt32(dr["idUsuario"]);
                            IdTipoUsuario = Convert.ToInt32(dr["idTipoUsuario"]);
                            idEstatus = Convert.ToInt32(dr["idEstatus"]);
                            ClaveUsuario = txtUsuario.Text.Trim();
                        }
                    }

                    // =============================================
                    // 2. Validar Estatus = 0 → Bloqueado
                    // =============================================
                    if (idEstatus == 0)
                    {
                        MessageBox.Show("Usuario Bloqueado.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    // =============================================
                    // 3. Obtener permisos del menú
                    // =============================================
                    string sqlMenu = @"
                        SELECT claveUsuario, idMenu, Menu, idOperacion, Operacion, idAutorizacion
                        FROM dbo.MenuUsuariosVw
                        WHERE IdEstatus = 1 AND idUsuario = @IdUsuario";

                    DataTable dtPermisos = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(sqlMenu, cn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtPermisos);
                        }
                    }

                    if (dtPermisos.Rows.Count == 0)
                    {
                        MessageBox.Show("Usuario no Tiene Permisos a Operaciones de la aplicación.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    // =============================================
                    // ✅ TODO OK → Abrir Menú Principal
                    // =============================================
                    this.Hide();
                    FrmMenuPrincipal frmMenu = new FrmMenuPrincipal(IdUsuario, ClaveUsuario, dtPermisos, CadenaConexion);
                    frmMenu.ShowDialog();
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error de Conexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
