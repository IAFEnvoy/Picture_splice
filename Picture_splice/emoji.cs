using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Picture_splice
{
    public partial class emoji : Form
    {
        public emoji()
        {
            InitializeComponent();
        }

        private void emoji_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath+@"\emoji");
            foreach(DirectoryInfo d in dir.GetDirectories())
            {
                tabControl1.TabPages.Add(d.Name);
                ListView lv = new ListView();
                lv.SmallImageList = il;
                lv.LargeImageList = il;
                lv.View = View.LargeIcon;
                lv.Dock = DockStyle.Fill;
                lv.DoubleClick += Lv_DoubleClick;
                foreach(FileInfo fi in d.GetFiles())
                {
                    if (fi.Extension == ".png")
                    {
                        Image im = Image.FromFile(fi.FullName);
                        il.Images.Add(im);
                        ListViewItem lvi = new ListViewItem(fi.Name, il.Images.Count - 1)
                        {
                            Tag = fi.FullName
                        };
                        lv.Items.Add(lvi);
                    }
                }
                tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(lv);
                GC.Collect();
            }
            GC.Collect();
        }

        private void Lv_DoubleClick(object sender, EventArgs e)
        {
            MainForm.semoji = (string)((ListView)sender).SelectedItems[0].Tag;
            Hide();
        }
    }
}
