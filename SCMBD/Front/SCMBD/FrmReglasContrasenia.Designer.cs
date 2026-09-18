namespace SCMBD
{
    partial class FrmReglasContrasenia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReglasContrasenia));
            this.pnlBarrainicial = new System.Windows.Forms.Panel();
            this.BtnProcesar = new System.Windows.Forms.Button();
            this.BtnExcel = new System.Windows.Forms.Button();
            this.BtnRefrescar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtOperacion = new System.Windows.Forms.TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CmbRequerido = new System.Windows.Forms.ComboBox();
            this.LblEstatus = new System.Windows.Forms.Label();
            this.cmbEstatus = new System.Windows.Forms.ComboBox();
            this.TxtValorMinimo = new System.Windows.Forms.TextBox();
            this.LblValor = new System.Windows.Forms.Label();
            this.LblRequerido = new System.Windows.Forms.Label();
            this.TxtDescripcion = new System.Windows.Forms.TextBox();
            this.LblDescripcionRegla = new System.Windows.Forms.Label();
            this.TxtNombreRegla = new System.Windows.Forms.TextBox();
            this.LblNombreRegla = new System.Windows.Forms.Label();
            this.TxtRegla = new System.Windows.Forms.TextBox();
            this.LblCodRegla = new System.Windows.Forms.Label();
            this.dg = new System.Windows.Forms.DataGridView();
            this.codRegla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombreRegla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.esRequerido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requerido = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.valorMinimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idEstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.estatus = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.BtnBaja = new System.Windows.Forms.Button();
            this.pnlBarrainicial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBarrainicial
            // 
            this.pnlBarrainicial.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlBarrainicial.Controls.Add(this.BtnBaja);
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
            this.pnlBarrainicial.Size = new System.Drawing.Size(1059, 92);
            this.pnlBarrainicial.TabIndex = 2;
            this.pnlBarrainicial.Paint += new System.Windows.Forms.PaintEventHandler(this.PnlBarrainicial_Paint);
            // 
            // BtnProcesar
            // 
            this.BtnProcesar.Image = ((System.Drawing.Image)(resources.GetObject("BtnProcesar.Image")));
            this.BtnProcesar.Location = new System.Drawing.Point(765, 2);
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
            this.BtnExcel.Location = new System.Drawing.Point(880, 2);
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
            this.BtnRefrescar.Location = new System.Drawing.Point(935, 2);
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
            this.BtnSalir.Location = new System.Drawing.Point(994, 2);
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
            this.txtUsuario.Location = new System.Drawing.Point(975, 69);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.ReadOnly = true;
            this.txtUsuario.Size = new System.Drawing.Size(78, 13);
            this.txtUsuario.TabIndex = 8;
            // 
            // txtOperacion
            // 
            this.txtOperacion.BackColor = System.Drawing.Color.LightSteelBlue;
            this.txtOperacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperacion.Enabled = false;
            this.txtOperacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOperacion.Location = new System.Drawing.Point(975, 50);
            this.txtOperacion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtOperacion.Name = "txtOperacion";
            this.txtOperacion.ReadOnly = true;
            this.txtOperacion.Size = new System.Drawing.Size(78, 13);
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
            this.panel1.Controls.Add(this.CmbRequerido);
            this.panel1.Controls.Add(this.LblEstatus);
            this.panel1.Controls.Add(this.cmbEstatus);
            this.panel1.Controls.Add(this.TxtValorMinimo);
            this.panel1.Controls.Add(this.LblValor);
            this.panel1.Controls.Add(this.LblRequerido);
            this.panel1.Controls.Add(this.TxtDescripcion);
            this.panel1.Controls.Add(this.LblDescripcionRegla);
            this.panel1.Controls.Add(this.TxtNombreRegla);
            this.panel1.Controls.Add(this.LblNombreRegla);
            this.panel1.Controls.Add(this.TxtRegla);
            this.panel1.Controls.Add(this.LblCodRegla);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1059, 100);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel1_Paint);
            // 
            // CmbRequerido
            // 
            this.CmbRequerido.BackColor = System.Drawing.SystemColors.Control;
            this.CmbRequerido.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CmbRequerido.FormattingEnabled = true;
            this.CmbRequerido.Location = new System.Drawing.Point(79, 65);
            this.CmbRequerido.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CmbRequerido.Name = "CmbRequerido";
            this.CmbRequerido.Size = new System.Drawing.Size(101, 23);
            this.CmbRequerido.TabIndex = 4;
            // 
            // LblEstatus
            // 
            this.LblEstatus.AutoSize = true;
            this.LblEstatus.Location = new System.Drawing.Point(542, 75);
            this.LblEstatus.Name = "LblEstatus";
            this.LblEstatus.Size = new System.Drawing.Size(53, 13);
            this.LblEstatus.TabIndex = 14;
            this.LblEstatus.Text = "Estatus:";
            // 
            // cmbEstatus
            // 
            this.cmbEstatus.BackColor = System.Drawing.SystemColors.Control;
            this.cmbEstatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbEstatus.FormattingEnabled = true;
            this.cmbEstatus.Location = new System.Drawing.Point(624, 65);
            this.cmbEstatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEstatus.Name = "cmbEstatus";
            this.cmbEstatus.Size = new System.Drawing.Size(243, 23);
            this.cmbEstatus.TabIndex = 6;
            // 
            // TxtValorMinimo
            // 
            this.TxtValorMinimo.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TxtValorMinimo.BackColor = System.Drawing.SystemColors.Control;
            this.TxtValorMinimo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtValorMinimo.Location = new System.Drawing.Point(291, 65);
            this.TxtValorMinimo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtValorMinimo.MaxLength = 4;
            this.TxtValorMinimo.Name = "TxtValorMinimo";
            this.TxtValorMinimo.Size = new System.Drawing.Size(34, 23);
            this.TxtValorMinimo.TabIndex = 5;
            this.TxtValorMinimo.TabStop = false;
            this.TxtValorMinimo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblValor
            // 
            this.LblValor.AutoSize = true;
            this.LblValor.Location = new System.Drawing.Point(200, 69);
            this.LblValor.Name = "LblValor";
            this.LblValor.Size = new System.Drawing.Size(83, 13);
            this.LblValor.TabIndex = 11;
            this.LblValor.Text = "Valor Minimo:";
            // 
            // LblRequerido
            // 
            this.LblRequerido.AutoSize = true;
            this.LblRequerido.Location = new System.Drawing.Point(3, 69);
            this.LblRequerido.Name = "LblRequerido";
            this.LblRequerido.Size = new System.Drawing.Size(69, 13);
            this.LblRequerido.TabIndex = 10;
            this.LblRequerido.Text = "Requerido:";
            // 
            // TxtDescripcion
            // 
            this.TxtDescripcion.BackColor = System.Drawing.SystemColors.Control;
            this.TxtDescripcion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtDescripcion.Location = new System.Drawing.Point(624, 15);
            this.TxtDescripcion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtDescripcion.MaxLength = 150;
            this.TxtDescripcion.Name = "TxtDescripcion";
            this.TxtDescripcion.Size = new System.Drawing.Size(431, 23);
            this.TxtDescripcion.TabIndex = 3;
            this.TxtDescripcion.TabStop = false;
            // 
            // LblDescripcionRegla
            // 
            this.LblDescripcionRegla.AutoSize = true;
            this.LblDescripcionRegla.Location = new System.Drawing.Point(542, 19);
            this.LblDescripcionRegla.Name = "LblDescripcionRegla";
            this.LblDescripcionRegla.Size = new System.Drawing.Size(75, 13);
            this.LblDescripcionRegla.TabIndex = 9;
            this.LblDescripcionRegla.Text = "Descrpción:";
            // 
            // TxtNombreRegla
            // 
            this.TxtNombreRegla.BackColor = System.Drawing.SystemColors.Control;
            this.TxtNombreRegla.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtNombreRegla.Location = new System.Drawing.Point(291, 15);
            this.TxtNombreRegla.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtNombreRegla.MaxLength = 40;
            this.TxtNombreRegla.Name = "TxtNombreRegla";
            this.TxtNombreRegla.Size = new System.Drawing.Size(233, 23);
            this.TxtNombreRegla.TabIndex = 2;
            this.TxtNombreRegla.TabStop = false;
            // 
            // LblNombreRegla
            // 
            this.LblNombreRegla.AutoSize = true;
            this.LblNombreRegla.Location = new System.Drawing.Point(200, 19);
            this.LblNombreRegla.Name = "LblNombreRegla";
            this.LblNombreRegla.Size = new System.Drawing.Size(54, 13);
            this.LblNombreRegla.TabIndex = 7;
            this.LblNombreRegla.Text = "Nombre:";
            // 
            // TxtRegla
            // 
            this.TxtRegla.BackColor = System.Drawing.SystemColors.Control;
            this.TxtRegla.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.TxtRegla.Location = new System.Drawing.Point(79, 15);
            this.TxtRegla.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TxtRegla.MaxLength = 10;
            this.TxtRegla.Name = "TxtRegla";
            this.TxtRegla.Size = new System.Drawing.Size(101, 23);
            this.TxtRegla.TabIndex = 1;
            this.TxtRegla.TabStop = false;
            // 
            // LblCodRegla
            // 
            this.LblCodRegla.AutoSize = true;
            this.LblCodRegla.Location = new System.Drawing.Point(3, 19);
            this.LblCodRegla.Name = "LblCodRegla";
            this.LblCodRegla.Size = new System.Drawing.Size(44, 13);
            this.LblCodRegla.TabIndex = 6;
            this.LblCodRegla.Text = "Regla:";
            // 
            // dg
            // 
            this.dg.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codRegla,
            this.nombreRegla,
            this.descripcion,
            this.esRequerido,
            this.requerido,
            this.valorMinimo,
            this.idEstatus,
            this.estatus});
            this.dg.Location = new System.Drawing.Point(0, 198);
            this.dg.MultiSelect = false;
            this.dg.Name = "dg";
            this.dg.RowTemplate.Height = 28;
            this.dg.Size = new System.Drawing.Size(1059, 450);
            this.dg.TabIndex = 4;
            // 
            // codRegla
            // 
            this.codRegla.HeaderText = "Regla";
            this.codRegla.Name = "codRegla";
            this.codRegla.ReadOnly = true;
            // 
            // nombreRegla
            // 
            this.nombreRegla.FillWeight = 200F;
            this.nombreRegla.HeaderText = "Nombre";
            this.nombreRegla.Name = "nombreRegla";
            this.nombreRegla.ReadOnly = true;
            // 
            // descripcion
            // 
            this.descripcion.HeaderText = "Descripcion";
            this.descripcion.Name = "descripcion";
            // 
            // esRequerido
            // 
            this.esRequerido.HeaderText = "esRequerido";
            this.esRequerido.Name = "esRequerido";
            this.esRequerido.Visible = false;
            // 
            // requerido
            // 
            this.requerido.HeaderText = "Requerido";
            this.requerido.Name = "requerido";
            this.requerido.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.requerido.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // valorMinimo
            // 
            this.valorMinimo.HeaderText = "Valor Minimo";
            this.valorMinimo.Name = "valorMinimo";
            // 
            // idEstatus
            // 
            this.idEstatus.HeaderText = "idEstatus";
            this.idEstatus.Name = "idEstatus";
            this.idEstatus.Visible = false;
            // 
            // estatus
            // 
            this.estatus.HeaderText = "Estatus";
            this.estatus.Name = "estatus";
            this.estatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.estatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // BtnBaja
            // 
            this.BtnBaja.Image = global::SCMBD.Properties.Resources.Elimina;
            this.BtnBaja.Location = new System.Drawing.Point(822, 2);
            this.BtnBaja.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtnBaja.Name = "BtnBaja";
            this.BtnBaja.Size = new System.Drawing.Size(59, 42);
            this.BtnBaja.TabIndex = 14;
            this.BtnBaja.UseVisualStyleBackColor = true;
            this.BtnBaja.Click += new System.EventHandler(this.BtnBaja_Click);
            // 
            // FrmReglasContrasenia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1059, 639);
            this.Controls.Add(this.dg);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBarrainicial);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmReglasContrasenia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FrmReglasContrasenia_Load);
            this.pnlBarrainicial.ResumeLayout(false);
            this.pnlBarrainicial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
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
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.Label LblCodRegla;
        private System.Windows.Forms.TextBox TxtRegla;
        private System.Windows.Forms.TextBox TxtNombreRegla;
        private System.Windows.Forms.Label LblNombreRegla;
        private System.Windows.Forms.TextBox TxtDescripcion;
        private System.Windows.Forms.Label LblDescripcionRegla;
        private System.Windows.Forms.Label LblRequerido;
        private System.Windows.Forms.Label LblValor;
        private System.Windows.Forms.TextBox TxtValorMinimo;
        private System.Windows.Forms.ComboBox cmbEstatus;
        private System.Windows.Forms.Label LblEstatus;
        private System.Windows.Forms.ComboBox CmbRequerido;
        private System.Windows.Forms.DataGridViewTextBoxColumn codRegla;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreRegla;
        private System.Windows.Forms.DataGridViewTextBoxColumn descripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn esRequerido;
        private System.Windows.Forms.DataGridViewComboBoxColumn requerido;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorMinimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn idEstatus;
        private System.Windows.Forms.DataGridViewComboBoxColumn estatus;
        private System.Windows.Forms.Button BtnBaja;
    }
}
