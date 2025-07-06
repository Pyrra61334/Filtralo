
namespace Filtralo
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            dgv_Usuarios = new DataGridView();
            btn_Reset1 = new Button();
            btn_Reset2 = new Button();
            btn_Reset3 = new Button();
            btn_Reset4 = new Button();
            btn_Reset5 = new Button();
            btn_SaveAndQui = new Button();
            lab_Estado1 = new Label();
            lab_Estado2 = new Label();
            btn_SalirSinGuardar = new Button();
            lab_HFE = new Label();
            lab_TE = new Label();
            testButton = new Button();
            Column1 = new DataGridViewTextBoxColumn();
            Funcionando = new DataGridViewTextBoxColumn();
            UltimoGuardado = new DataGridViewTextBoxColumn();
            Region = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgv_Usuarios).BeginInit();
            SuspendLayout();
            // 
            // dgv_Usuarios
            // 
            dgv_Usuarios.AllowUserToAddRows = false;
            dgv_Usuarios.AllowUserToDeleteRows = false;
            dgv_Usuarios.BackgroundColor = SystemColors.Highlight;
            dgv_Usuarios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_Usuarios.Columns.AddRange(new DataGridViewColumn[] { Column1, Funcionando, UltimoGuardado, Region });
            dgv_Usuarios.GridColor = SystemColors.HotTrack;
            dgv_Usuarios.Location = new Point(29, 14);
            dgv_Usuarios.Margin = new Padding(4, 3, 4, 3);
            dgv_Usuarios.Name = "dgv_Usuarios";
            dgv_Usuarios.ReadOnly = true;
            dgv_Usuarios.RowHeadersVisible = false;
            dgv_Usuarios.Size = new Size(573, 380);
            dgv_Usuarios.TabIndex = 0;
            // 
            // btn_Reset1
            // 
            btn_Reset1.BackColor = SystemColors.Control;
            btn_Reset1.Location = new Point(634, 14);
            btn_Reset1.Margin = new Padding(4, 3, 4, 3);
            btn_Reset1.Name = "btn_Reset1";
            btn_Reset1.Size = new Size(88, 27);
            btn_Reset1.TabIndex = 1;
            btn_Reset1.Text = "Filtro 1";
            btn_Reset1.UseVisualStyleBackColor = false;
            btn_Reset1.Click += btn_Reset1_Click;
            // 
            // btn_Reset2
            // 
            btn_Reset2.Location = new Point(632, 47);
            btn_Reset2.Margin = new Padding(4, 3, 4, 3);
            btn_Reset2.Name = "btn_Reset2";
            btn_Reset2.Size = new Size(88, 27);
            btn_Reset2.TabIndex = 2;
            btn_Reset2.Text = "Filtro 2";
            btn_Reset2.UseVisualStyleBackColor = true;
            btn_Reset2.Click += btn_Reset2_Click;
            // 
            // btn_Reset3
            // 
            btn_Reset3.Location = new Point(632, 81);
            btn_Reset3.Margin = new Padding(4, 3, 4, 3);
            btn_Reset3.Name = "btn_Reset3";
            btn_Reset3.Size = new Size(88, 27);
            btn_Reset3.TabIndex = 3;
            btn_Reset3.Text = "Filtro 3";
            btn_Reset3.UseVisualStyleBackColor = true;
            btn_Reset3.Click += btn_Reset3_Click;
            // 
            // btn_Reset4
            // 
            btn_Reset4.Location = new Point(632, 114);
            btn_Reset4.Margin = new Padding(4, 3, 4, 3);
            btn_Reset4.Name = "btn_Reset4";
            btn_Reset4.Size = new Size(88, 27);
            btn_Reset4.TabIndex = 4;
            btn_Reset4.Text = "Filtro 4";
            btn_Reset4.UseVisualStyleBackColor = true;
            btn_Reset4.Click += btn_Reset4_Click;
            // 
            // btn_Reset5
            // 
            btn_Reset5.Location = new Point(632, 148);
            btn_Reset5.Margin = new Padding(4, 3, 4, 3);
            btn_Reset5.Name = "btn_Reset5";
            btn_Reset5.Size = new Size(88, 27);
            btn_Reset5.TabIndex = 5;
            btn_Reset5.Text = "Filtro 5";
            btn_Reset5.UseVisualStyleBackColor = true;
            btn_Reset5.Click += btn_Reset5_Click;
            // 
            // btn_SaveAndQui
            // 
            btn_SaveAndQui.DialogResult = DialogResult.Cancel;
            btn_SaveAndQui.Location = new Point(622, 397);
            btn_SaveAndQui.Margin = new Padding(4, 3, 4, 3);
            btn_SaveAndQui.Name = "btn_SaveAndQui";
            btn_SaveAndQui.Size = new Size(112, 27);
            btn_SaveAndQui.TabIndex = 6;
            btn_SaveAndQui.Text = "Guardar y salir";
            btn_SaveAndQui.UseVisualStyleBackColor = true;
            btn_SaveAndQui.Click += btn_SaveAndQui_Click;
            // 
            // lab_Estado1
            // 
            lab_Estado1.AutoSize = true;
            lab_Estado1.Location = new Point(26, 403);
            lab_Estado1.Margin = new Padding(4, 0, 4, 0);
            lab_Estado1.Name = "lab_Estado1";
            lab_Estado1.Size = new Size(48, 15);
            lab_Estado1.TabIndex = 7;
            lab_Estado1.Text = "Estado: ";
            // 
            // lab_Estado2
            // 
            lab_Estado2.AutoSize = true;
            lab_Estado2.Location = new Point(74, 403);
            lab_Estado2.Margin = new Padding(4, 0, 4, 0);
            lab_Estado2.Name = "lab_Estado2";
            lab_Estado2.Size = new Size(57, 15);
            lab_Estado2.TabIndex = 8;
            lab_Estado2.Text = "En espera";
            // 
            // btn_SalirSinGuardar
            // 
            btn_SalirSinGuardar.Location = new Point(491, 397);
            btn_SalirSinGuardar.Margin = new Padding(4, 3, 4, 3);
            btn_SalirSinGuardar.Name = "btn_SalirSinGuardar";
            btn_SalirSinGuardar.Size = new Size(111, 27);
            btn_SalirSinGuardar.TabIndex = 9;
            btn_SalirSinGuardar.Text = "Salir sin guardar";
            btn_SalirSinGuardar.UseVisualStyleBackColor = true;
            btn_SalirSinGuardar.Click += btn_SalirSinGuardar_Click;
            // 
            // lab_HFE
            // 
            lab_HFE.AutoSize = true;
            lab_HFE.Location = new Point(606, 363);
            lab_HFE.Margin = new Padding(4, 0, 4, 0);
            lab_HFE.Name = "lab_HFE";
            lab_HFE.Size = new Size(130, 15);
            lab_HFE.TabIndex = 11;
            lab_HFE.Text = "(Falta lista de numeros)";
            // 
            // lab_TE
            // 
            lab_TE.AutoSize = true;
            lab_TE.Location = new Point(606, 338);
            lab_TE.Margin = new Padding(4, 0, 4, 0);
            lab_TE.Name = "lab_TE";
            lab_TE.Size = new Size(103, 15);
            lab_TE.TabIndex = 10;
            lab_TE.Text = "Tiempo estimado:";
            // 
            // testButton
            // 
            testButton.Location = new Point(444, 397);
            testButton.Name = "testButton";
            testButton.Size = new Size(40, 27);
            testButton.TabIndex = 12;
            testButton.Text = "Test";
            testButton.UseVisualStyleBackColor = true;
            testButton.Visible = false;
            testButton.Click += testButton_Click;
            // 
            // Column1
            // 
            Column1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            Column1.HeaderText = "°";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 70;
            // 
            // Funcionando
            // 
            Funcionando.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Funcionando.HeaderText = "Celular";
            Funcionando.Name = "Funcionando";
            Funcionando.ReadOnly = true;
            Funcionando.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // UltimoGuardado
            // 
            UltimoGuardado.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            UltimoGuardado.HeaderText = "Compañia";
            UltimoGuardado.Name = "UltimoGuardado";
            UltimoGuardado.ReadOnly = true;
            UltimoGuardado.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Region
            // 
            Region.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Region.HeaderText = "Region";
            Region.Name = "Region";
            Region.ReadOnly = true;
            Region.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // Form2
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.MenuHighlight;
            ClientSize = new Size(749, 427);
            Controls.Add(testButton);
            Controls.Add(lab_HFE);
            Controls.Add(lab_TE);
            Controls.Add(btn_SalirSinGuardar);
            Controls.Add(lab_Estado2);
            Controls.Add(lab_Estado1);
            Controls.Add(btn_SaveAndQui);
            Controls.Add(btn_Reset5);
            Controls.Add(btn_Reset4);
            Controls.Add(btn_Reset3);
            Controls.Add(btn_Reset2);
            Controls.Add(btn_Reset1);
            Controls.Add(dgv_Usuarios);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Filtralo!!!";
            Load += Form2_Load;
            DragDrop += Form2_DragDrop;
            DragEnter += Form2_DragEnter;
            ((System.ComponentModel.ISupportInitialize)dgv_Usuarios).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_Usuarios;
        private System.Windows.Forms.Button btn_Reset1;
        private System.Windows.Forms.Button btn_Reset2;
        private System.Windows.Forms.Button btn_Reset3;
        private System.Windows.Forms.Button btn_Reset4;
        private System.Windows.Forms.Button btn_Reset5;
        private System.Windows.Forms.Button btn_SaveAndQui;
        private System.Windows.Forms.Label lab_Estado1;
        private System.Windows.Forms.Label lab_Estado2;
        private System.Windows.Forms.Button btn_SalirSinGuardar;
        private System.Windows.Forms.Label lab_HFE;
        private System.Windows.Forms.Label lab_TE;
        private Button testButton;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Funcionando;
        private DataGridViewTextBoxColumn UltimoGuardado;
        private DataGridViewTextBoxColumn Region;
    }
}