namespace ÇARK
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde çalışmasını istediğin kodları buraya ekleyebilirsin
            Form1 form1 = new Form1();
            form1.BackColor = Color.OrangeRed;

        }

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
            components = new System.ComponentModel.Container();
            btnSpin = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // btnSpin
            // 
            btnSpin.AutoSize = true;
            btnSpin.BackColor = Color.DarkSalmon;
            btnSpin.Dock = DockStyle.Bottom;
            btnSpin.FlatStyle = FlatStyle.Flat;
            btnSpin.Location = new Point(0, 461);
            btnSpin.Name = "btnSpin";
            btnSpin.Size = new Size(733, 32);
            btnSpin.TabIndex = 0;
            btnSpin.Text = "ÇARKI ÇEVİR";
            btnSpin.UseVisualStyleBackColor = false;
            btnSpin.Click += btnSpin_Click_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(50, 20);
            label1.TabIndex = 1;
            label1.Text = "label1";
            label1.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 199.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            label2.Location = new Point(157, 28);
            label2.Name = "label2";
            label2.Size = new Size(380, 402);
            label2.TabIndex = 2;
            label2.Text = "3";
            label2.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Crimson;
            ClientSize = new Size(733, 493);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSpin);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSpin;
        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private Label label2;
    }
}