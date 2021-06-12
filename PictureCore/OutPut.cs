using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TPBGPictureBox;

namespace PictureCore
{
    public class OutPut
    {
        public static Bitmap SavePicture(List<PictureBox> pics,double suofang, Color bg)
        {
            try
            {
                if (pics.Count == 0) return null;
                int h = 0, w = 0, hm = int.MaxValue, wm = int.MaxValue;
                foreach (PictureBox pi in pics)//定位最终图片位置
                {
                    h = (int)(Math.Max(h, pi.Top + pi.Height) / suofang);
                    w = (int)(Math.Max(w, pi.Left + pi.Width) / suofang);
                    hm = (int)(Math.Min(hm, pi.Top) / suofang);
                    wm = (int)(Math.Min(wm, pi.Left) / suofang);
                }
                Bitmap bm = new Bitmap(w - wm, h - hm);
                Graphics g = Graphics.FromImage(bm);
                g.FillRectangle(new SolidBrush(bg), 0, 0, bm.Width, bm.Height);
                foreach (PictureBox p in pics)
                {
                    g.DrawImage(p.Image, (int)(p.Location.X / suofang - wm), (int)(p.Location.Y / suofang - hm), (int)(p.Width / suofang), (int)(p.Height / suofang));
                }
                GC.Collect();
                return bm;
            }
            catch
            {
                MessageBox.Show("图片过大","",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return null;
            }
        }
        public static Bitmap SavePicture(List<TPBGPictureBox.TPBGPictureBox> pics, double suofang,Color bg)
        {
            try
            {
                if (pics.Count == 0) return null;
                int h = 0, w = 0, hm = int.MaxValue, wm = int.MaxValue;
                foreach (PictureBox pi in pics)//定位最终图片位置
                {
                    h = (int)(Math.Max(h, pi.Top + pi.Height) / suofang);
                    w = (int)(Math.Max(w, pi.Left + pi.Width) / suofang);
                    hm = (int)(Math.Min(hm, pi.Top) / suofang);
                    wm = (int)(Math.Min(wm, pi.Left) / suofang);
                }
                Bitmap bm = new Bitmap(w - wm, h - hm);
                Graphics g = Graphics.FromImage(bm);
                g.FillRectangle(new SolidBrush(bg), 0, 0, bm.Width, bm.Height);
                foreach (PictureBox p in pics)
                {
                    g.DrawImage(p.Image, (int)(p.Location.X / suofang - wm), (int)(p.Location.Y / suofang - hm), (int)(p.Width / suofang), (int)(p.Height / suofang));
                }
                GC.Collect();
                return bm;
            }
            catch
            {
                MessageBox.Show("图片过大", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
