namespace SCMBD
{
    partial class FrmManUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManUser));
            this.pnlBarrainicial = new System.Windows.Forms.Panel();
            this.BtnProcesar = new System.Windows.Forms.Button();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.BtnRefrescar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TxtCorreo = new System.Windows.Forms.TextBox();
            this.LblCorreo = new System.Windows.Forms.Label();
            this.cmbIdTipoUsuario = new System.Windows.Forms.ComboBox();
            this.LblidTipoUsuario = new System.Windows.Forms.Label();
            this.cmbEstatus = new System.Windows.Forms.ComboBox();
            this.LblEstatus = new System.Windows.Forms.Label();
            this.TxtSegundoApellido = new System.Windows.Forms.TextBox();
            this.lblSegundoApellido = new System.Windows.Forms.Label();
            this.TxtPrimerApellido = new System.Windows.Forms.TextBox();
            this.LblPrimerApellido = new System.Windows.Forms.Label();
            this.TxtNombres = new System.Windows.Forms.TextBox();
            this.LblNombres = new System.Windows.Forms.Label();
            this.TextPassw = new System.Windows.Forms.TextBox();
            this.TxtClaveUsuario = new System.Windows.Forms.TextBox();
            this.LblContrasenia = new System.Windows.Forms.Label();
            this.LblClaveUsuario = new System.Windows.Forms.Label();
            this.TxtIdUsuario = new System.Windows.Forms.TextBox();
            this.LblIdUsuario = new System.Windows.Forms.Label();
            this.pnlBarrainicial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBarrainicial
            // 
            this.pnlBarrainicial.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlBarrainicial.Controls.Add(this.BtnProcesar);
            this.pnlBarrainicial.Controls.Add(this.BtnExcel);
            this.pnlBarrainicial.Controls.Add(this.BtnRefrescar);
            this.pnlBarrainicial.Controls.Add(this.BtnSalir);
            this.pnlBarrainicial.Controls.Add(this.txtUsuario);
            this.pnlBarrainicial.Controls.Add(this.txtOperacion);
            this.pnlBarrainicial.Controls.Add(this.picLogo);
            this.pnlBarrainicial.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarrainicial.Location = new System.Drawing.Point(0, 0);
            this.pnlBarrainicial.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pnlBarrainicial.Name = "pnlBarrainicial";
            this.pnlBarrainicial.Size = new System.Drawing.Size(1256, 92);
            this.pnlBarrainicial.TabIndex = 2;
            this.pnlBarrainicial.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBarrainicial_Paint);
            // 
            // BtnProcesar
            // 
            this.BtnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("BtnProcesar.Image")));
            this.BtnProcesar.Location = new System.Drawing.Point(1001, 2);
            this.BtnProcesar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnProcesar.Name = "BtnProcesar";
            this.BtnProcesar.Size = new System.Drawing.Size(59, 42);
            this.BtnProcesar.TabIndex = 12;
            this.BtnProcesar.UseVisualStyleBackColor = true;
            this.BtnProcesar.Click += new System.EventHandler(this.BtnProcesar_Click);
            // 
            // BtnExcel
            // 
            this.BtnExcel.Image = global::SCMBD.Properties.Resources.Excel;
            this.BtnExcel.Location = new System.Drawing.Point(1060, 2);
            this.BtnExcel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnExcel.Name = "BtnExcel";
            this.BtnExcel.Size = new System.Drawing.Size(59, 42);
            this.BtnExcel.TabIndex = 11;
            this.BtnExcel.UseVisualStyleBackColor = true;
            this.BtnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // BtnRefrescar
            // 
            this.BtnRefrescar.Image = global::SCMBD.Properties.Resources.renovar;
            this.BtnRefrescar.Location = new System.Drawing.Point(1115, 2);
            this.BtnRefrescar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnRefrescar.Name = "BtnRefrescar";
            this.BtnRefrescar.Size = new System.Drawing.Size(59, 42);
            this.BtnRefrescar.TabIndex = 10;
            this.BtnRefrescar.UseVisualStyleBackColor = true;
            this.BtnRefrescar.Click += new System.EventHandler(this.BtnRefrescar_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.Image = global::SCMBD.Properties.Resources.salir;
            this.BtnSalir.Location = new System.Drawing.Point(1174, 2);
            this.BtnSalir.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(59, 42);
            this.BtnSalir.TabIndex = 9;
            this.BtnSalir.UseVisualStyleBackColor = true;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsuario.Enabled = false;
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(1165, 68);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(68, 13);
            this.txtUsuario.TabIndex = 8;
            // 
            // txtOperacion
            // 
            this.txtOperacion.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtOperacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperacion.Enabled = false;
            this.txtOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOperacion.Location = new System.Drawing.Point(1165, 50);
            this.txtOperacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtOperacion.Name = "txtOperacion";
            this.txtOperacion.ReadOnly = true;
            this.txtOperacion.Size = new System.Drawing.Size(68, 13);
            this.txtOperacion.TabIndex = 7;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.LightSteelBlue;
            this.picLogo.Location = new System.Drawing.Point(6, 5);
            this.picLogo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(114, 58);
            this.picLogo.TabIndex = 1;
            this.picLogo.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.TxtCorreo);
            this.panel1.Controls.Add(this.LblCorreo);
            this.panel1.Controls.Add(this.cmbIdTipoUsuario);
            this.panel1.Controls.Add(this.LblidTipoUsuario);
            this.panel1.Controls.Add(this.cmbEstatus);
            this.panel1.Controls.Add(this.LblEstatus);
            this.panel1.Controls.Add(this.TxtSegundoApellido);
            this.panel1.Controls.Add(this.lblSegundoApellido);
            this.panel1.Controls.Add(this.TxtPrimerApellido);
            this.panel1.Controls.Add(this.LblPrimerApellido);
            this.panel1.Controls.Add(this.TxtNombres);
            this.panel1.Controls.Add(this.LblNombres);
            this.panel1.Controls.Add(this.TextPassw);
            this.panel1.Controls.Add(this.TxtClaveUsuario);
            this.panel1.Controls.Add(this.LblContrasenia);
            this.panel1.Controls.Add(this.LblClaveUsuario);
            this.panel1.Controls.Add(this.TxtIdUsuario);
            this.panel1.Controls.Add(this.LblIdUsuario);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1256, 164);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // TxtCorreo
            // 
            this.TxtCorreo.BackColor = System.Drawing.SystemColors.Control;
            this.TxtCorreo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtCorreo.Location = new System.Drawing.Point(134, 130);
            this.TxtCorreo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtCorreo.Name = "TxtCorreo";
            this.TxtCorreo.Size = new System.Drawing.Size(626, 23);
            this.TxtCorreo.TabIndex = 14;
            // 
            // LblCorreo
            // 
            this.LblCorreo.AutoSize = true;
            this.LblCorreo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblCorreo.Location = new System.Drawing.Point(4, 133);
            this.LblCorreo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblCorreo.Name = "LblCorreo";
            this.LblCorreo.Size = new System.Drawing.Size(46, 15);
            this.LblCorreo.TabIndex = 13;
            this.LblCorreo.Text = "Correo:";
            // 
            // cmbIdTipoUsuario
            // 
            this.cmbIdTipoUsuario.BackColor = System.Drawing.SystemColors.Control;
            this.cmbIdTipoUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbIdTipoUsuario.FormattingEnabled = true;
            this.cmbIdTipoUsuario.Location = new System.Drawing.Point(542, 91);
            this.cmbIdTipoUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbIdTipoUsuario.Name = "cmbIdTipoUsuario";
            this.cmbIdTipoUsuario.Size = new System.Drawing.Size(218, 23);
            this.cmbIdTipoUsuario.TabIndex = 13;
            this.cmbIdTipoUsuario.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // LblidTipoUsuario
            // 
            this.LblidTipoUsuario.AutoSize = true;
            this.LblidTipoUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblidTipoUsuario.Location = new System.Drawing.Point(432, 91);
            this.LblidTipoUsuario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblidTipoUsuario.Name = "LblidTipoUsuario";
            this.LblidTipoUsuario.Size = new System.Drawing.Size(77, 15);
            this.LblidTipoUsuario.TabIndex = 12;
            this.LblidTipoUsuario.Text = "Tipo Usuario:";
            // 
            // cmbEstatus
            // 
            this.cmbEstatus.BackColor = System.Drawing.SystemColors.Control;
            this.cmbEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbEstatus.FormattingEnabled = true;
            this.cmbEstatus.Location = new System.Drawing.Point(134, 91);
            this.cmbEstatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEstatus.Name = "cmbEstatus";
            this.cmbEstatus.Size = new System.Drawing.Size(218, 23);
            this.cmbEstatus.TabIndex = 11;
            // 
            // LblEstatus
            // 
            this.LblEstatus.AutoSize = true;
            this.LblEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblEstatus.Location = new System.Drawing.Point(4, 91);
            this.LblEstatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblEstatus.Name = "LblEstatus";
            this.LblEstatus.Size = new System.Drawing.Size(47, 15);
            this.LblEstatus.TabIndex = 10;
            this.LblEstatus.Text = "Estatus:";
            // 
            // TxtSegundoApellido
            // 
            this.TxtSegundoApellido.BackColor = System.Drawing.SystemColors.Control;
            this.TxtSegundoApellido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtSegundoApellido.Location = new System.Drawing.Point(964, 57);
            this.TxtSegundoApellido.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtSegundoApellido.Name = "TxtSegundoApellido";
            this.TxtSegundoApellido.Size = new System.Drawing.Size(289, 23);
            this.TxtSegundoApellido.TabIndex = 9;
            // 
            // lblSegundoApellido
            // 
            this.lblSegundoApellido.AutoSize = true;
            this.lblSegundoApellido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSegundoApellido.Location = new System.Drawing.Point(835, 60);
            this.lblSegundoApellido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSegundoApellido.Name = "lblSegundoApellido";
            this.lblSegundoApellido.Size = new System.Drawing.Size(104, 15);
            this.lblSegundoApellido.TabIndex = 8;
            this.lblSegundoApellido.Text = "Segundo Apellido:";
            // 
            // TxtPrimerApellido
            // 
            this.TxtPrimerApellido.BackColor = System.Drawing.SystemColors.Control;
            this.TxtPrimerApellido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtPrimerApellido.Location = new System.Drawing.Point(542, 57);
            this.TxtPrimerApellido.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtPrimerApellido.Name = "TxtPrimerApellido";
            this.TxtPrimerApellido.Size = new System.Drawing.Size(279, 23);
            this.TxtPrimerApellido.TabIndex = 7;
            // 
            // LblPrimerApellido
            // 
            this.LblPrimerApellido.AutoSize = true;
            this.LblPrimerApellido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblPrimerApellido.Location = new System.Drawing.Point(428, 60);
            this.LblPrimerApellido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblPrimerApellido.Name = "LblPrimerApellido";
            this.LblPrimerApellido.Size = new System.Drawing.Size(92, 15);
            this.LblPrimerApellido.TabIndex = 6;
            this.LblPrimerApellido.Text = "Primer Apellido:";
            // 
            // TxtNombres
            // 
            this.TxtNombres.BackColor = System.Drawing.SystemColors.Control;
            this.TxtNombres.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtNombres.Location = new System.Drawing.Point(134, 54);
            this.TxtNombres.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtNombres.Name = "TxtNombres";
            this.TxtNombres.Size = new System.Drawing.Size(279, 23);
            this.TxtNombres.TabIndex = 5;
            this.TxtNombres.TextChanged += new System.EventHandler(this.TxtPrimerNombre_TextChanged);
            // 
            // LblNombres
            // 
            this.LblNombres.AutoSize = true;
            this.LblNombres.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblNombres.Location = new System.Drawing.Point(4, 57);
            this.LblNombres.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblNombres.Name = "LblNombres";
            this.LblNombres.Size = new System.Drawing.Size(54, 15);
            this.LblNombres.TabIndex = 4;
            this.LblNombres.Text = "Nombre:";
            this.LblNombres.Click += new System.EventHandler(this.LblPrimerNombre_Click);
            // 
            // TextPassw
            // 
            this.TextPassw.BackColor = System.Drawing.SystemColors.Control;
            this.TextPassw.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TextPassw.Location = new System.Drawing.Point(964, 16);
            this.TextPassw.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TextPassw.Name = "TextPassw";
            this.TextPassw.PasswordChar = '*';
            this.TextPassw.Size = new System.Drawing.Size(178, 23);
            this.TextPassw.TabIndex = 3;
            // 
            // TxtClaveUsuario
            // 
            this.TxtClaveUsuario.BackColor = System.Drawing.SystemColors.Control;
            this.TxtClaveUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtClaveUsuario.Location = new System.Drawing.Point(542, 16);
            this.TxtClaveUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtClaveUsuario.Name = "TxtClaveUsuario";
            this.TxtClaveUsuario.Size = new System.Drawing.Size(160, 23);
            this.TxtClaveUsuario.TabIndex = 2;
            // 
            // LblContrasenia
            // 
            this.LblContrasenia.AutoSize = true;
            this.LblContrasenia.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblContrasenia.Location = new System.Drawing.Point(835, 24);
            this.LblContrasenia.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblContrasenia.Name = "LblContrasenia";
            this.LblContrasenia.Size = new System.Drawing.Size(70, 15);
            this.LblContrasenia.TabIndex = 3;
            this.LblContrasenia.Text = "Contraseña:";
            // 
            // LblClaveUsuario
            // 
            this.LblClaveUsuario.AutoSize = true;
            this.LblClaveUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblClaveUsuario.Location = new System.Drawing.Point(428, 24);
            this.LblClaveUsuario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblClaveUsuario.Name = "LblClaveUsuario";
            this.LblClaveUsuario.Size = new System.Drawing.Size(50, 15);
            this.LblClaveUsuario.TabIndex = 2;
            this.LblClaveUsuario.Text = "Usuario:";
            // 
            // TxtIdUsuario
            // 
            this.TxtIdUsuario.BackColor = System.Drawing.SystemColors.Control;
            this.TxtIdUsuario.Enabled = false;
            this.TxtIdUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtIdUsuario.Location = new System.Drawing.Point(134, 13);
            this.TxtIdUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtIdUsuario.Name = "TxtIdUsuario";
            this.TxtIdUsuario.Size = new System.Drawing.Size(101, 23);
            this.TxtIdUsuario.TabIndex = 1;
            // 
            // LblIdUsuario
            // 
            this.LblIdUsuario.AutoSize = true;
            this.LblIdUsuario.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.LblIdUsuario.Location = new System.Drawing.Point(4, 16);
            this.LblIdUsuario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LblIdUsuario.Name = "LblIdUsuario";
            this.LblIdUsuario.Size = new System.Drawing.Size(63, 15);
            this.LblIdUsuario.TabIndex = 0;
            this.LblIdUsuario.Text = "Id Usuario:";
            // 
            // FrmManUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1256, 639);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBarrainicial);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administración de Usuarios";
            this.Load += new System.EventHandler(this.FrmManUser_Load);
            this.pnlBarrainicial.ResumeLayout(false);
            this.pnlBarrainicial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBarrainicial;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtOperacion;
        private System.Windows.Forms.Button BtnRefrescar;
        private System.Windows.Forms.Button BtnSalir;
        private System.Windows.Forms.Button BtnExcel;
        private System.Windows.Forms.Button BtnProcesar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TxtIdUsuario;
        private System.Windows.Forms.Label LblIdUsuario;
        private System.Windows.Forms.Label LblContrasenia;
        private System.Windows.Forms.Label LblClaveUsuario;
        private System.Windows.Forms.TextBox TextPassw;
        private System.Windows.Forms.TextBox TxtClaveUsuario;
        private System.Windows.Forms.TextBox TxtNombres;
        private System.Windows.Forms.Label LblNombres;
        private System.Windows.Forms.TextBox TxtPrimerApellido;
        private System.Windows.Forms.Label LblPrimerApellido;
        private System.Windows.Forms.TextBox TxtSegundoApellido;
        private System.Windows.Forms.Label lblSegundoApellido;
        private System.Windows.Forms.Label LblEstatus;
        private System.Windows.Forms.ComboBox cmbEstatus;
        private System.Windows.Forms.ComboBox cmbIdTipoUsuario;
        private System.Windows.Forms.Label LblidTipoUsuario;
        private System.Windows.Forms.TextBox TxtCorreo;
        private System.Windows.Forms.Label LblCorreo;
    }
}
