using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using PictureCore;

namespace Picture_splice
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }
        int suonow = 100;
        int refleshcntmax = 10;
        Color bg = Color.FromArgb(0, 0, 0, 0);

        #region list操作
        List<TPBGPictureBox.TPBGPictureBox> pics = new List<TPBGPictureBox.TPBGPictureBox>();//index越大，图层越上面
        struct Datas
        {
            public string filename;
            public int rotate;
            public bool mirror;
        }
        private Datas createdatas(string name, int rotate, bool mirror)
        {
            return new Datas()
            {
                filename = name,
                rotate = rotate,
                mirror = mirror
            };
        }

        private void add(string filename)
        {
            TPBGPictureBox.TPBGPictureBox pic = new TPBGPictureBox.TPBGPictureBox();
            Image image = Image.FromFile(filename);
            pic.Size = new Size(image.Width / 4, image.Height / 4);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Image = image;
            pic.Location = new Point(0, 0);
            pic.MouseMove += picBox_MouseMove;
            pic.MouseUp += picBox_MouseUp;//过会写refresh
            pic.MouseDown += picBox_MouseDown;
            pic.MouseWheel += Pic_MouseWheel;
            pic.Tag = createdatas(filename, 0, false);
            if (pic.Left < 0) pic.Left = 0;
            if (pic.Top < 0) pic.Top = 0;
            pic.ContextMenuStrip = leftcontroller;

            pics.Add(pic);
        }

        private void add(string filename, Point locate, Size size, int rotates, bool mirror)
        {
            TPBGPictureBox.TPBGPictureBox pic = new TPBGPictureBox.TPBGPictureBox();
            Image image = Image.FromFile(filename);
            pic.Size = size;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Image = image;
            pic.Location = locate;
            pic.MouseMove += picBox_MouseMove;
            pic.MouseUp += picBox_MouseUp;//过会写refresh
            pic.MouseDown += picBox_MouseDown;
            pic.MouseWheel += Pic_MouseWheel;
            pic.Tag = createdatas(filename, rotates, mirror);
            if (pic.Left < 0) pic.Left = 0;
            if (pic.Top < 0) pic.Top = 0;
            pic.ContextMenuStrip = leftcontroller;

            pic.Image = PictureCore.PictureHandle.Rotate(pic.Image, rotates);

            pics.Add(pic);
        }
        private void delete(int num)
        {
            pics.RemoveAt(num);
        }
        private void getlist(ListBox lb)
        {
            lb.Items.Clear();
            for (int i = pics.Count - 1; i >= 0; i--)
                lb.Items.Add(((Datas)pics[i].Tag).filename);
        }
        private void show(PictureBox p)
        {
            p.Controls.Clear();
            for (int i = pics.Count - 1; i >= 0; i--)
                p.Controls.Add(pics[i]);
            shower.Refresh();
        }
        private void swap(int a, int b)
        {
            TPBGPictureBox.TPBGPictureBox pi = pics[a];
            pics[a] = pics[b];
            pics[b] = pi;
        }
        #endregion

        bool MoveFlag; Control con; int xPos, yPos;
        ListView listboxclick;
        private void 导入文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.Cancel)
            {
                ListView lv = new ListView
                {
                    SmallImageList = il,
                    LargeImageList = il,
                    Dock = DockStyle.Fill,
                    ContextMenuStrip = rightcontroller
                };
                DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
                FileInfo[] files = di.GetFiles();
                tabControl1.TabPages.Add(di.Name);
                tabControl1.SelectedIndex = tabControl1.TabCount - 1;
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(lv);
                ProgressBar1.Maximum = files.Length;
                ProgressBar1.Value = 0;
                foreach (FileInfo fi in files)//加载图像文件
                {
                    if (fi.Extension.ToLower() == ".jpg" || fi.Extension.ToLower() == ".png" || fi.Extension.ToLower() == ".bmp" || fi.Extension.ToLower() == ".ico")
                    {
                        Image im = Image.FromFile(fi.FullName);
                        il.Images.Add(im);
                        ListViewItem lvi = new ListViewItem(fi.Name, il.Images.Count - 1)
                        {
                            Tag = fi.FullName
                        };
                        lv.Items.Add(lvi);
                        im.Dispose();
                        GC.Collect();
                    }
                    ProgressBar1.Value += 1;
                    Application.DoEvents();
                }
                lv.DoubleClick += Lv_DoubleClick;
                lv.Click += Lv_Click;
            }
            fbd.Dispose();
            GC.Collect();
        }

        private void Lv_Click(object sender, EventArgs e)
        {
            listboxclick = (ListView)sender;
        }

        private void Lv_DoubleClick(object sender, EventArgs e)
        {
            add((string)((ListView)sender).SelectedItems[0].Tag);

            show(shower);
            getlist(listBox1);
        }
        private void suofang(double r)//r为比值
        {
            foreach (Control co in shower.Controls)
            {
                Size size1 = co.Size;
                size1.Width = (int)(size1.Width * r); size1.Height = (int)(size1.Height * r);
                co.Size = size1;
                Point point = co.Location;
                point.X = (int)(point.X * r); point.Y = (int)(point.Y * r);
                co.Location = point;
            }
            suofangshow.Text = Math.Round(r * suonow).ToString() + "%";
        }
        private void Pic_MouseWheel(object sender, MouseEventArgs e)
        {
            //缩放
            int kk = 5;
            if (Control.ModifierKeys == Keys.Shift)//按下Shift，快速缩放
                kk = 1;
            TPBGPictureBox.TPBGPictureBox pic = (TPBGPictureBox.TPBGPictureBox)sender;
            Size size = pic.Size;
            double angle = Math.Atan((double)size.Height / size.Width);
            size.Height += (int)(e.Delta * Math.Sin(angle) / kk);
            size.Width += (int)(e.Delta * Math.Cos(angle) / kk);
            if (size.Width <= 0 || size.Height <= 0) return;
            pic.Size = size;
            //writeback((PictureBox)sender);
            GC.Collect();
        }
        int refreshcnt = 0;
        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                TPBGPictureBox.TPBGPictureBox picBox = (TPBGPictureBox.TPBGPictureBox)sender;
                picBox.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                picBox.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
                //if (picBox.Left < 0) picBox.Left = 0;
                //if (picBox.Top < 0) picBox.Top = 0;
                //picBox.Refresh();
                refreshcnt += 1;
                if (refreshcnt == refleshcntmax)
                {
                    //show(shower);
                    shower.Refresh();
                    refreshcnt = 0;
                }
            }

        }

        //在picturebox的鼠标按下事件里.
        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            MoveFlag = false;
            shower.Refresh();
            //writeback((PictureBox)sender);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            suofang(25.0 / suonow);
            suonow = 25;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            suofang(50.0 / suonow);
            suonow = 50;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            suofang(100.0 / suonow);
            suonow = 100;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            suofang(250.0 / suonow);
            suonow = 250;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            suofang(500.0 / suonow);
            suonow = 500;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int i = 0; i < pics.Count; i++)
                    if (pics[i] == con)
                        delete(i);
                con.Dispose();
                show(shower);
                getlist(listBox1);
            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Text = "Picture splice";
            pics = new List<TPBGPictureBox.TPBGPictureBox>();
            show(shower);
            getlist(listBox1);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Text != "Picture splice")
            {
                StreamWriter sw = new StreamWriter(Text.Replace(" - Picture splice", ""));
                for (int i = 0; i < pics.Count; i++)
                {
                    sw.WriteLine(((Datas)pics[i].Tag).filename);
                    sw.WriteLine(pics[i].Top.ToString() + " " + pics[i].Left.ToString());
                    sw.WriteLine(pics[i].Width.ToString() + " " + pics[i].Height);
                    sw.WriteLine(((Datas)pics[i].Tag).rotate.ToString() + " " + ((Datas)pics[i].Tag).mirror.ToString());
                }
                sw.Close();
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "*.psplice|*.psplice";
                sfd.Title = "保存";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Text = sfd.FileName + " - Picture_splice";
                    StreamWriter sw = new StreamWriter(sfd.FileName);
                    for (int i = 0; i < pics.Count; i++)
                    {
                        sw.WriteLine((Datas)pics[i].Tag);
                        sw.WriteLine(pics[i].Top.ToString() + " " + pics[i].Left.ToString());
                        sw.WriteLine(pics[i].Width.ToString() + " " + pics[i].Height);
                        sw.WriteLine(((Datas)pics[i].Tag).rotate.ToString() + " " + ((Datas)pics[i].Tag).mirror.ToString());
                    }
                }
                sfd.Dispose();
            }
            GC.Collect();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.psplice|*.psplice";
            ofd.Title = "打开文件";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pics = new List<TPBGPictureBox.TPBGPictureBox>();
                StreamReader sr = new StreamReader(ofd.FileName);
                string s = sr.ReadLine();
                while (sr.EndOfStream == false)
                {
                    if (File.Exists(s) == false)
                    {
                        MessageBox.Show("文件" + s + "未找到!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sr.ReadLine();
                        sr.ReadLine();
                    }
                    else
                    {
                        string name = s;
                        s = sr.ReadLine();
                        string[] str1 = s.Split(' ');
                        s = sr.ReadLine();
                        string[] str2 = s.Split(' ');
                        s = sr.ReadLine();
                        string[] str3 = s.Split(' ');

                        add(name, new Point(int.Parse(str1[1]), int.Parse(str1[0])), new Size(int.Parse(str2[0]), int.Parse(str2[1])), int.Parse(str3[0]), bool.Parse(str3[1]));
                    }
                    s = sr.ReadLine();
                    getlist(listBox1);
                    show(shower);
                }
                sr.Close();
                sr.Dispose();
                Text = ofd.FileName + " - Picture_splice";
                getlist(listBox1);
                show(shower);
                GC.Collect();
            }
            ofd.Dispose();
            GC.Collect();
        }

        private void 单张图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "支持的图片格式|*.jpg;*.ico;*.bmp;*.png";
            ofd.Title = "选择图片";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                add(ofd.FileName);
                getlist(listBox1);
                show(shower);
            }
        }

        public static Image imageeeeeeeeee;
        public static bool isok = false;
        public static string text = string.Empty;
        private void 文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new Newtext();
            form.ShowDialog();
            if (isok == true)
            {
                add(text);

                getlist(listBox1);
                show(shower);
            }
            form.Dispose();
            GC.Collect();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            int num = pics.IndexOf((TPBGPictureBox.TPBGPictureBox)con);
            if (num == pics.Count - 1) return;
            swap(num, num + 1);
            show(shower);
            getlist(listBox1);
            shower.Refresh();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            int num = pics.IndexOf((TPBGPictureBox.TPBGPictureBox)con);
            if (num == 0) return;
            swap(num, num - 1);
            show(shower);
            getlist(listBox1);
            shower.Refresh();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            int num = pics.IndexOf((TPBGPictureBox.TPBGPictureBox)con);
            TPBGPictureBox.TPBGPictureBox pi = pics[num];
            for (int i = num + 1; i < pics.Count; i++)
                pics[i - 1] = pics[i];
            pics[pics.Count - 1] = pi;
            show(shower);
            getlist(listBox1);
            shower.Refresh();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            int num = pics.IndexOf((TPBGPictureBox.TPBGPictureBox)con);
            TPBGPictureBox.TPBGPictureBox pi = pics[num];
            for (int i = num - 1; i >= 0; i--)
                pics[i + 1] = pics[i];
            pics[0] = pi;
            show(shower);
            getlist(listBox1);
            shower.Refresh();
        }

        private void 导出图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image image = PictureCore.OutPut.SavePicture(pics, 1.0 * suonow / 100.0, bg);
            if (image != null)
            {
                SaveFileDialog sfg = new SaveFileDialog();
                sfg.Filter = "*.png|*.png|*.jpg|*.jpg";
                sfg.Title = "保存图片到";
                if (sfg.ShowDialog() == DialogResult.OK)
                {
                    image.Save(sfg.FileName);
                }
            }
            GC.Collect();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < pics.Count; i++)
            {
                if (con == pics[i])
                {
                    TPBGPictureBox.TPBGPictureBox pic = (TPBGPictureBox.TPBGPictureBox)con;
                    pic.Image = PictureCore.PictureHandle.Rotate(pic.Image, 270);
                    int aaaa = pic.Width;
                    pic.Width = pic.Height;
                    pic.Height = aaaa;
                    pics[i] = pic;
                    pics[i].Tag = new Datas() { filename = ((Datas)pics[i].Tag).filename, mirror = ((Datas)pics[i].Tag).mirror, rotate = (((Datas)pics[i].Tag).rotate + 270) % 360 };
                }
            }
            show(shower);
            getlist(listBox1);
            GC.Collect();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < pics.Count; i++)
            {
                if (con == pics[i])
                {
                    TPBGPictureBox.TPBGPictureBox pic = (TPBGPictureBox.TPBGPictureBox)con;
                    pic.Image = PictureCore.PictureHandle.Rotate(pic.Image, 90);
                    int aaaa = pic.Width;
                    pic.Width = pic.Height;
                    pic.Height = aaaa;
                    pics[i] = pic;
                    pics[i].Tag = new Datas() { filename = ((Datas)pics[i].Tag).filename, mirror = ((Datas)pics[i].Tag).mirror, rotate = (((Datas)pics[i].Tag).rotate + 90) % 360 };
                }
            }
            show(shower);
            getlist(listBox1);
            GC.Collect();
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.psplice|*.psplice";
            sfd.Title = "另存为";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Text = sfd.FileName + " - Picture splice";
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < pics.Count; i++)
                {
                    sw.WriteLine((Datas)pics[i].Tag);
                    sw.WriteLine(pics[i].Top.ToString() + " " + pics[i].Left.ToString());
                    sw.WriteLine(pics[i].Width.ToString() + " " + pics[i].Height);
                    sw.WriteLine(((Datas)pics[i].Tag).rotate.ToString() + " " + ((Datas)pics[i].Tag).mirror.ToString());
                }
                sw.Close();
                sw.Dispose();
            }
            sfd.Dispose();
            GC.Collect();
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new Setting();
            form.ShowDialog();
            form.Dispose();
            GC.Collect();
        }
        public static string semoji = string.Empty;
        private void 表情ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new emoji();
            form.ShowDialog();
            if (string.IsNullOrEmpty(semoji) == false) add(semoji);
            show(shower);
            getlist(listBox1);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new About();
            form.ShowDialog();
            form.Dispose();
            GC.Collect();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pics.Count == 0)
            {
                MessageBox.Show("你啥也没有放啊，我怎么打印。。。");
                return;
            }

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            Margins margin = new Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Margins = margin;

            PrintDialog pdi = new PrintDialog();
            if (pdi.ShowDialog() == DialogResult.Cancel) return;
            pd.PrinterSettings = pdi.PrinterSettings;

            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pd.PrintController.OnEndPrint(pd, new PrintEventArgs());
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(PictureCore.OutPut.SavePicture(pics, suonow / 100.0, bg), new PointF(0, 0));
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {

        }

        private void 打印浏览ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pics.Count == 0)
            {
                MessageBox.Show("你啥也没有放啊，我怎么生成浏览。。。");
                return;
            }

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            Margins margin = new Margins(20, 20, 20, 20);
            pd.DefaultPageSettings.Margins = margin;

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.ShowDialog();
        }

        private void 插入此图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add((string)listboxclick.SelectedItems[0].Tag);
            show(shower);
            getlist(listBox1);
        }

        public static string QRc;
        public static bool yesQR = false;
        private void 二维码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form form = new QRCodegenerate();
            form.ShowDialog();
            if (yesQR == false) return;
            add(QRc);
            show(shower);
            getlist(listBox1);
        }

        private void 透明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bg = Color.FromArgb(0, 0, 0, 0);
            shower.BackColor = bg;
        }

        private void 纯色ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = bg;
            if (cd.ShowDialog() == DialogResult.Cancel) return;
            bg = cd.Color;
            shower.BackColor = bg;
        }


        //在picturebox的鼠标按下事件里,记录三个变量.
        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                con = (Control)sender;
            }
            MoveFlag = true;//已经按下.
            xPos = e.X;//当前x坐标.
            yPos = e.Y;//当前y坐标.
        }
    }
}
