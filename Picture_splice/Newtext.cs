using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Picture_splice
{
    public partial class Newtext : Form
    {
        public Newtext()
        {
            InitializeComponent();
        }
        Color fontcolor, backcolor;
        Font font;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = fontcolor;
            colorDialog1.ShowDialog();
            fontcolor = colorDialog1.Color;
            Setcolor();
        }
        private void Setcolor()
        {
            pictureBox1.BackColor = fontcolor;
            pictureBox2.BackColor = backcolor;
            textBox3.Text = font.Name + "," + font.Size.ToString()+"pt";
        }
        private void Newtext_Load(object sender, EventArgs e)
        {
            fontcolor = Color.Black;
            backcolor = Color.White;
            font = textBox1.Font;
            Setcolor();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            font = fontDialog1.Font;
            Setcolor();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int alpha;
            bool o = int.TryParse(textBox2.Text, out alpha);
            if (o == false)
            {
                MessageBox.Show("无法生成图片，看看哪里写错了？", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Bitmap bt = Texts.Text2Pic.TextToBitmap(textBox1.Text, font, Rectangle.Empty, fontcolor, backcolor, alpha);
            MainForm.text = Application.StartupPath + @"\temp\texts\" + font.Name + font.Size.ToString() + fontcolor.ToArgb().ToString() + backcolor.ToArgb().ToString() + ".png";
            bt.Save(MainForm.text, ImageFormat.Png);
            MainForm.imageeeeeeeeee = bt;
            MainForm.isok = true;
            Hide();
        }

        private void Newtext_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.isok = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = backcolor;
            colorDialog1.ShowDialog();
            backcolor = colorDialog1.Color;
            Setcolor();
        }
    }
}
