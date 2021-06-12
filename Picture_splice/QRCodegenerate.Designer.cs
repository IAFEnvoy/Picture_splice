namespace Picture_splice
{
    partial class QRCodegenerate
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
            this.button4 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(73, 104);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 25);
            this.button4.TabIndex = 13;
            this.button4.Text = "插入";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(62, 73);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(135, 25);
            this.textBox5.TabIndex = 12;
            this.textBox5.Text = "0";
            this.textBox5.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(62, 42);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(135, 25);
            this.textBox4.TabIndex = 11;
            this.textBox4.Text = "250";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(62, 11);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(608, 25);
            this.textBox3.TabIndex = 9;
            this.textBox3.Text = "Hello World";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(203, 42);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(250, 250);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 75);
            this.label2.TabIndex = 10;
            this.label2.Text = "内容：\r\n\r\n长宽：\r\n\r\n边框：";
            // 
            // QRCodegenerate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 308);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QRCodegenerate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "QRCodegenerate";
            this.Load += new System.EventHandler(this.QRCodegenerate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label2;
    }
}