using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace RepGas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConexion));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblContrasena = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblCompanias = new System.Windows.Forms.Label();
            this.cmbCompanias = new System.Windows.Forms.ComboBox();
            this.BtnConectar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.Transparent;
            this.picLogo.Image = global::RepGas.Properties.Resources.Logo_adam_3;
            this.picLogo.Location = new System.Drawing.Point(5, 5);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(180, 50);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblUsuario.Location = new System.Drawing.Point(40, 105);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(54, 13);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtUsuario.Location = new System.Drawing.Point(140, 100);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(100, 23);
            this.txtUsuario.TabIndex = 1;
            // 
            // lblContrasena
            // 
            this.lblContrasena.AutoSize = true;
            this.lblContrasena.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblContrasena.Location = new System.Drawing.Point(40, 140);
            this.lblContrasena.Name = "lblContrasena";
            this.lblContrasena.Size = new System.Drawing.Size(75, 13);
            this.lblContrasena.TabIndex = 2;
            this.lblContrasena.Text = "Contraseña:";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPassword.Location = new System.Drawing.Point(140, 135);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 23);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblCompanias
            // 
            this.lblCompanias.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblCompanias.Location = new System.Drawing.Point(40, 175);
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
            this.cmbCompanias.Location = new System.Drawing.Point(140, 170);
            this.cmbCompanias.Name = "cmbCompanias";
            this.cmbCompanias.Size = new System.Drawing.Size(280, 21);
            this.cmbCompanias.TabIndex = 5;
            this.cmbCompanias.Visible = false;
            // 
            // BtnConectar
            // 
            this.BtnConectar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnConectar.FlatAppearance.BorderSize = 0;
            this.BtnConectar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnConectar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnConectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConectar.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.BtnConectar.Image = global::RepGas.Properties.Resources.ACEPTA1;
            this.BtnConectar.Location = new System.Drawing.Point(450, 230);
            this.BtnConectar.Name = "BtnConectar";
            this.BtnConectar.Size = new System.Drawing.Size(40, 36);
            this.BtnConectar.TabIndex = 7;
            this.BtnConectar.TabStop = false;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Image = global::RepGas.Properties.Resources.CANCELA1;
            this.btnCancelar.Location = new System.Drawing.Point(400, 230);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(40, 36);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.TabStop = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmConexion
            // 
            this.AcceptButton = this.BtnConectar;
            this.AllowDrop = true;
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
            this.Load += new System.EventHandler(this.FrmConexion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
