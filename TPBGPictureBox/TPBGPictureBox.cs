using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TPBGPictureBox
{
    public class TPBGPictureBox : PictureBox
    {
        public TPBGPictureBox()
        {
            SetTransparenz();
        }

        PaintEventArgs ep;
        private void SetTransparenz()
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.Opaque, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, false);
            this.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            // if you want to execute the original PaintBox logic before you execute your own code, use the next line of code.
            //if (this.Image == null) e.Graphics.DrawImage(new Bitmap(e.ClipRectangle.Width,e.ClipRectangle.Height), e.ClipRectangle);
            //else e.Graphics.DrawImage(this.Image, e.ClipRectangle);
            base.OnPaint(e);
            ep = e;
        }
        // now do whatever you want 
        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20;
                // Turn on WS_EX_TRANSPARENT
                return cp;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
    }
}
