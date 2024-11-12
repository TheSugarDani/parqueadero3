namespace parqueadero2
{
    partial class VistaPersonita
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


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
            components = new System.ComponentModel.Container();
            labelPlaca = new Label();
            labelHoraIngreso = new Label();
            labelHoraSalida = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            pictureBoxPlaca = new PictureBox();
            lblPlaca = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlaca).BeginInit();
            SuspendLayout();
            // 
            // labelPlaca
            // 
            labelPlaca.AutoSize = true;
            labelPlaca.Location = new Point(401, 40);
            labelPlaca.Name = "labelPlaca";
            labelPlaca.Size = new Size(0, 20);
            labelPlaca.TabIndex = 0;
            // 
            // labelHoraIngreso
            // 
            labelHoraIngreso.AutoSize = true;
            labelHoraIngreso.Location = new Point(223, 491);
            labelHoraIngreso.Name = "labelHoraIngreso";
            labelHoraIngreso.Size = new Size(119, 20);
            labelHoraIngreso.TabIndex = 2;
            labelHoraIngreso.Text = "Hora de Ingreso:";
            // 
            // labelHoraSalida
            // 
            labelHoraSalida.AutoSize = true;
            labelHoraSalida.Location = new Point(535, 491);
            labelHoraSalida.Name = "labelHoraSalida";
            labelHoraSalida.Size = new Size(111, 20);
            labelHoraSalida.TabIndex = 3;
            labelHoraSalida.Text = "Hora de Salida:";
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            // 
            // pictureBoxPlaca
            // 
            pictureBoxPlaca.Location = new Point(262, 87);
            pictureBoxPlaca.Margin = new Padding(3, 4, 3, 4);
            pictureBoxPlaca.Name = "pictureBoxPlaca";
            pictureBoxPlaca.Size = new Size(336, 359);
            pictureBoxPlaca.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPlaca.TabIndex = 4;
            pictureBoxPlaca.TabStop = false;
            // 
            // lblPlaca
            // 
            lblPlaca.AutoSize = true;
            lblPlaca.Location = new Point(401, 40);
            lblPlaca.Name = "lblPlaca";
            lblPlaca.Size = new Size(47, 20);
            lblPlaca.TabIndex = 5;
            lblPlaca.Text = "Placa:";
            lblPlaca.Click += lblPlaca_Click;
            // 
            // VistaPersonita
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(lblPlaca);
            Controls.Add(pictureBoxPlaca);
            Controls.Add(labelHoraSalida);
            Controls.Add(labelHoraIngreso);
            Controls.Add(labelPlaca);
            Margin = new Padding(3, 4, 3, 4);
            Name = "VistaPersonita";
            Text = "administrador";
            ((System.ComponentModel.ISupportInitialize)pictureBoxPlaca).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelPlaca;
        private Label labelHoraIngreso;
        private Label labelHoraSalida;
        private System.Windows.Forms.Timer timer1;
        private PictureBox pictureBoxPlaca;
        private Label lblPlaca;
    }
}