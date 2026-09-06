using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SCMBD
{
    public partial class FrmConexion : Form
    {
        // ? SOLO las propiedades de datos — NO declarar controles aquí
        public int IdUsuario { get; private set; }
        public string ClaveUsuario { get; private set; }
        public DataTable Permisos { get; private set; }
        public string CadenaConexion { get; private set; }

        public FrmConexion()
        {
            InitializeComponent();
            this.Load += FrmConexion_Load;
        }

        private void FrmConexion_Load(object sender, EventArgs e)
        {
            CargarLogo();
            CargarImagenesBotones();
            CargarIconoVentana();
        }

        private void CargarLogo()
        {
            try
            {
                string rutaLogo = Path.Combine(Application.StartupPath, @"Imagenes\LogoSCMBD.png");

                if (File.Exists(rutaLogo))
                {
                    using (Image imgOriginal = Image.FromFile(rutaLogo))
                    {
                        Bitmap imgNueva = new Bitmap(imgOriginal.Width, imgOriginal.Height);

                        using (Graphics g = Graphics.FromImage(imgNueva))
                        {
                            Color colorFondo = Color.LightSteelBlue;
                            g.Clear(colorFondo);
                            g.DrawImage(imgOriginal, 0, 0, imgOriginal.Width, imgOriginal.Height);
                        }

                        picLogo.Image = imgNueva;
                    }

                    picLogo.SizeMode = PictureBoxSizeMode.Zoom;
                    picLogo.BorderStyle = BorderStyle.None;
                    picLogo.Padding = new Padding(0);
                    picLogo.Margin = new Padding(0);
                    picLogo.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    picLogo.BackColor = Color.LightSteelBlue; // ✅ Coincide con formulario
                    picLogo.Location = new Point(0, 0);
                }
            }
            catch
            {
                // ✅ Solo si necesitas depurar; en producción déjalo vacío
                // MessageBox.Show("No se pudo cargar el logo");
            }
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
                        // ✅ Convertir PNG a Icono y asignarlo a la ventana
                        this.Icon = Icon.FromHandle(bmp.GetHicon());
                    }
                }
            }
            catch { }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CargarImagenesBotones()
        {
            try
            {
                if (Properties.Resources.ACEPTA1 != null)
                {
                    BtnConectar.Image = Properties.Resources.ACEPTA1.ToBitmap();
                    BtnConectar.ImageAlign = ContentAlignment.MiddleLeft;
                    BtnConectar.TextImageRelation = TextImageRelation.ImageBeforeText;
                }

                if (Properties.Resources.CANCELA1 != null)
                {
                    btnCancelar.Image = Properties.Resources.CANCELA1.ToBitmap();
                    btnCancelar.ImageAlign = ContentAlignment.MiddleLeft;
                    btnCancelar.TextImageRelation = TextImageRelation.ImageBeforeText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar las imágenes: {ex.Message}", "Nota", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnConectar_Click(object sender, EventArgs e)
        {
            // ✅ Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ingrese Usuario y Contraseña", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // ✅ Construir cadena con credenciales

                string cadenaBase = AppSettings.GetConnectionString("SCMBD");
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cadenaBase)
                {
                    UserID = txtUsuario.Text.Trim(),
                    Password = txtPassword.Text.Trim()
                };
                string cadena = builder.ConnectionString;

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();

                    // Consulta de validación del usuario

                    string sqlUsuario = "Select  idUsuario, idEstatus As Bloqueado " +
                                        "From    dbo.segUsuariosTbl " +
                                        "Where   ClaveUsuario = @ClaveUsuario ";

                    using (SqlCommand cmd = new SqlCommand(sqlUsuario, cn))
                    {
                        cmd.Parameters.AddWithValue("@ClaveUsuario", txtUsuario.Text.Trim());

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                MessageBox.Show("El usuario no está relacionado en la aplicación.",
                                                "Acceso Denegado",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Hand);
                                return;
                            }

                            // Validar estatus (ajusta el número si es distinto en tu tabla)
                            int estatus = Convert.ToInt32(dr["Bloqueado"]);
                            bool bloqueado = (estatus == 0);

                            if (bloqueado)
                            {
                                MessageBox.Show("Usuario se encuentra BLOQUEADO",
                                                "Acceso Denegado",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Stop);
                                return;
                            }

                            IdUsuario = Convert.ToInt32(dr["idUsuario"]);
                            ClaveUsuario = txtUsuario.Text.Trim();
                            CadenaConexion = cadena;
                            AppSettings.SetRuntimeConnectionString(cadena);
                        }
                    }

                    // permisos a operaciones del usuario en el menu
                    Permisos = new DataTable();
                    string sqlPermisos = "Select idMenu, Menu, idOperacion, Operacion, idAutorizacion, llamada " +
                                         "FROM   dbo.MenuUsuariosVw " +
                                         "WHERE idUsuario = @IdUsuario " +
                                         "Order  by idMenu, idOperacion";

                    using (SqlCommand cmd = new SqlCommand(sqlPermisos, cn))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(Permisos);
                        }
                    }

                    if (Permisos.Rows.Count == 0)
                    {
                        MessageBox.Show("El usuario no tiene permiso a ninguna operación.", "Acceso Denegado", MessageBoxButtons.OK,
                                        MessageBoxIcon.Stop);
                        return; // ⛔ NO continúa, NO abre el menú
                    }
                }

                this.ClaveUsuario = txtUsuario.Text.Trim().ToUpper();

                FrmMenuPrincipal frm = new FrmMenuPrincipal(IdUsuario, ClaveUsuario, Permisos, CadenaConexion);
                this.Hide();
                frm.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
