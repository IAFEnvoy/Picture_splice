using QR;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Picture_splice
{
    public partial class QRCodegenerate : Form
    {
        public QRCodegenerate()
        {
            InitializeComponent();
        }
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        private void newQR()
        {
            if (IsNumeric(textBox4.Text) && IsNumeric(textBox5.Text) && textBox3.Text != string.Empty && textBox4.Text != string.Empty && textBox5.Text != string.Empty)
                pictureBox2.Image = QRcode.BulidQRcode(textBox3.Text, int.Parse(textBox4.Text), int.Parse(textBox5.Text));
        }

        private void textBox3_TextChanged(object sender, System.EventArgs e)
        {
            newQR();
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            pictureBox2.Image.Save(Application.StartupPath + @"\temp\QRcode\" + textBox3.Text);
            MainForm.QRc = Application.StartupPath + @"\temp\QRcode\" + textBox3.Text;
            MainForm.yesQR = true;
            Hide();
        }

        private void QRCodegenerate_Load(object sender, System.EventArgs e)
        {
            newQR();
        }
    }
}
