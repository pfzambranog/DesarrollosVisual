using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace ReporteGasolina
{
    partial class FrmConexion
    {
        private IContainer components = null;

        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblContrasena;
        private TextBox txtPassword;
        private Label lblCompanias;
        private ComboBox cmbCompanias;
        private Button BtnConectar;
        private Button btnCancelar;
        private PictureBox picLogo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {

            this.picLogo = new System.Windows.Forms.PictureBox();

            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConexion));
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblContrasena = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblCompanias = new System.Windows.Forms.Label();
            this.cmbCompanias = new System.Windows.Forms.ComboBox();
            this.BtnConectar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // picLogo

            this.picLogo.Location = new Point(5, 5);
            this.picLogo.BackColor = Color.Transparent;
            this.picLogo.Image = Properties.Resources.Logo_adam_3;
            this.picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.picLogo.Size = new Size(180, 50);
            this.picLogo.BorderStyle = BorderStyle.None;
          //  this.picLogo.SizeMode = PictureBoxSizeMode.AutoSize;
            

            // 
            // lblUsuario
            //

            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblUsuario.Location = new Point(40, 105);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(54, 13);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario:";


            // 
            // txtUsuario
            //

            this.txtUsuario.AutoSize = false;
            this.txtUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsuario.Location = new Point(140, 100);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.TabIndex = 1;
            this.txtUsuario.Font = new Font("Segoe UI", 9F);
            this.txtUsuario.Height = 24;
        

            // 
            // lblContrasena
            // 
            this.lblContrasena.AutoSize = true;
            this.lblContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblContrasena.Location = new Point(40, 140);
            this.lblContrasena.Name = "lblContrasena";
            this.lblContrasena.Size = new System.Drawing.Size(75, 13);
            this.lblContrasena.TabIndex = 2;
            this.lblContrasena.Text = "Contraseña:";
            // 
            // txtPassword
            //

            this.txtPassword.AutoSize = false;
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new Point(140, 135);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Font = new Font("Segoe UI", 9F);
            this.txtPassword.Height = 24;
            // 
            // lblCompanias
            // 
            this.lblCompanias.AutoSize = false;
            this.lblCompanias.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCompanias.Location = new Point(40, 175);
            this.lblCompanias.Name = "lblCompanias";
            this.lblCompanias.Size = new System.Drawing.Size(68, 13);
            this.lblCompanias.TabIndex = 4;
            this.lblCompanias.Text = "Compañía:";
            this.lblCompanias.Visible = false;
            // 
            // cmbCompanias
            //

            this.cmbCompanias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCompanias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompanias.Location = new Point(140, 170);
            this.cmbCompanias.Name = "cmbCompanias";
            this.cmbCompanias.Size = new Size(280, 24);
            this.cmbCompanias.TabIndex = 5;
            this.cmbCompanias.Visible = false;

            // Conectar

            this.BtnConectar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.BtnConectar.FlatStyle = FlatStyle.Flat;
            this.BtnConectar.FlatAppearance.BorderSize = 0;
            this.BtnConectar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.BtnConectar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.BtnConectar.Location = new Point(450, 230);
            this.BtnConectar.Size = new Size(40, 36);
            this.BtnConectar.Image = Properties.Resources.ACEPTA1;
            this.BtnConectar.ImageAlign = ContentAlignment.MiddleCenter;
            this.BtnConectar.TabIndex = 7;
            this.AcceptButton = this.BtnConectar;
            this.BtnConectar.TabStop = false;

            // Cancelar

            this.btnCancelar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnCancelar.DialogResult = DialogResult.Cancel;
            this.btnCancelar.FlatStyle = FlatStyle.Flat;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatAppearance.MouseDownBackColor = Color.Transparent;
            this.btnCancelar.FlatAppearance.MouseOverBackColor = Color.Transparent;
            this.btnCancelar.Location = new Point(400, 230);
            this.btnCancelar.Size = new Size(40, 36);
            this.btnCancelar.Image = Properties.Resources.CANCELA1;
            this.btnCancelar.ImageAlign = ContentAlignment.MiddleCenter;
            this.CancelButton = this.btnCancelar;
            this.btnCancelar.TabIndex = 6;
             this.btnCancelar.TabStop = false;

            // 
            // FrmConexion
            //

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(196)))), ((int)(((byte)(222)))));
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(540, 280);

            this.Controls.Add(this.picLogo);

            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblContrasena);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblCompanias);
            this.Controls.Add(this.cmbCompanias);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.BtnConectar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmConexion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Credenciales de Conexión";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
