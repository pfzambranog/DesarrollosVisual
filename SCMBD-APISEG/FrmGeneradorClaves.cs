using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace SCMBD_APISEG
{
    public partial class FrmGeneradorClaves : Form
    {
        // 🔐 MISMA CLAVE MAESTRA — NO MODIFICAR
        private static readonly byte[] ClaveMaestra = new byte[]
        {
            0x53, 0x43, 0x4D, 0x42, 0x44, 0x2D, 0x41, 0x50,
            0x49, 0x53, 0x45, 0x47, 0x2D, 0x32, 0x30, 0x32,
            0x34, 0x2D, 0x43, 0x4C, 0x41, 0x56, 0x45, 0x2D,
            0x53, 0x45, 0x47, 0x55, 0x52, 0x41, 0x31, 0x32
        };

        public FrmGeneradorClaves()
        {
            InitializeComponent();
        }

        private string Cifrar(string textoPlano)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = ClaveMaestra;
                aes.GenerateIV();

                using (var ms = new System.IO.MemoryStream())
                {
                    ms.Write(aes.IV, 0, 16);

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (var sw = new System.IO.StreamWriter(cs))
                    {
                        sw.Write(textoPlano);
                        sw.Flush();
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private void btnCifrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEntrada.Text))
            {
                MessageBox.Show("Escribe el texto a cifrar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtResultado.Text = Cifrar(txtEntrada.Text);
            lblEstado.Text = "✅ Cifrado correctamente — copia el resultado";
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtResultado.Text))
            {
                Clipboard.SetText(txtResultado.Text);
                lblEstado.Text = "✅ Copiado al portapapeles";
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtEntrada.Clear();
            txtResultado.Clear();
            lblEstado.Text = "";
        }
    }
}
