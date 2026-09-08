using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SCMBD
{
    public static class RecursosCompartidos
    {
        /// <summary>
        /// Carga el logo estándar en un PictureBox
        /// </summary>
        public static void CargarLogo(PictureBox picLogo)
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
                    picLogo.BackColor = Color.LightSteelBlue;

                }
            }
            catch { }
        }
    }
}
