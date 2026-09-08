using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SCMBD
{
    public partial class FrmConexion : Form
    {
        // ✅ Propiedades públicas para pasar al Menú
        public int IdUsuario { get; private set; }
        public string ClaveUsuario { get; private set; }
        public DataTable Permisos { get; private set; }
        public string CadenaConexion { get; private set; }
        public string Operacion { get; private set; }  // ✅ Declarada

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

        private void BtnCancelar_Click(object sender, EventArgs e)
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
                    this.btnCancelar.Image = Properties.Resources.CANCELA1.ToBitmap();
                    this.btnCancelar.ImageAlign = ContentAlignment.MiddleLeft;
                    this.btnCancelar.TextImageRelation = TextImageRelation.ImageBeforeText;
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
                // ✅ Construir cadena de conexión con credenciales
                string cadenaBase = ConfigurationManager.ConnectionStrings["SCMBD"].ConnectionString;
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cadenaBase)
                {
                    UserID = txtUsuario.Text.Trim(),
                    Password = txtPassword.Text.Trim()
                };
                string cadena = builder.ConnectionString;

                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    cn.Open();

                    // ✅ Validar usuario y obtener datos básicos
                    string sqlUsuario = @"SELECT idUsuario, 
                                                  idEstatus AS Bloqueado, 
                                                  idTipoUsuario
                                           FROM   dbo.segUsuariosTbl 
                                           WHERE  ClaveUsuario = @ClaveUsuario";

                    using (SqlCommand cmd = new SqlCommand(sqlUsuario, cn))
                    {
                        cmd.Parameters.AddWithValue("@ClaveUsuario", txtUsuario.Text.Trim());
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // 
                            if (!dr.Read())
                            {
                                MessageBox.Show("El usuario no está relacionado en la aplicación.",
                                                "Acceso Denegado",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Hand);
                                return;
                            }

                            // ✅ Usuario existe → validar estatus
                            int estatus = Convert.ToInt32(dr["Bloqueado"]);
                            bool estaBloqueado = (estatus == 0); // 0 = Inactivo/Bloqueado
                            if (estaBloqueado)
                            {
                                MessageBox.Show("Usuario se encuentra BLOQUEADO",
                                                "Acceso Denegado",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Stop);
                                return;
                            }

                            // Guardar datos del usuario
                            IdUsuario = Convert.ToInt32(dr["idUsuario"]);
                            ClaveUsuario = txtUsuario.Text.Trim().ToUpper();
                            CadenaConexion = cadena;
                        }
                    }

                    //  Cargar permisos y datos del menu del usuario

                    Permisos = new DataTable();
                    string sqlPermisos = @"SELECT idMenu, Menu, idOperacion, 
                              Operacion, idAutorizacion, llamada, claveOperacion
                       FROM   dbo.MenuUsuariosVw 
                       WHERE  idUsuario = @IdUsuario 
                       ORDER BY idMenu, idOperacion";

                    using (SqlCommand cmdPerm = new SqlCommand(sqlPermisos, cn))
                    {
                        cmdPerm.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmdPerm))
                        {
                            da.Fill(Permisos);
                        }
                    }

                    if (Permisos.Rows.Count == 0)
                    {
                        MessageBox.Show("El usuario no tiene permiso a ninguna operación.",
                                        "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    // =====================================================
                    //  OPCIÓN 2 — LEER claveOperacion DESDE LA BD
                    // =====================================================

                    DataRow filaOperacion = Permisos.AsEnumerable()
                        .FirstOrDefault(f => Convert.ToInt32(f["idOperacion"]) > 0);

                    if (filaOperacion != null)
                    {
                        Operacion = filaOperacion["claveOperacion"]?.ToString().Trim() ?? "";
                    }

                    // Si no se encontró o viene vacío → usar valor por defecto
                    if (string.IsNullOrWhiteSpace(Operacion))
                    {
                        Operacion = ConfigurationManager.AppSettings["Operacion"] ?? "SCMBD01";
                    }


                    // ? ABRIR MENÚ CON LOS 5 PARÁMETROS EN ORDEN
                    FrmMenuPrincipal frm = new FrmMenuPrincipal(
                        IdUsuario,        // 1
                        ClaveUsuario,     // 2
                        Permisos,         // 3
                        Operacion,        // 4 ? ? VIENE DE: MenuUsuariosVw ? columna claveOperacion
                        CadenaConexion);  // 5

                    this.Hide();
                    frm.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TxtUsuario_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
