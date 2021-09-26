using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs332ComputerGraphic
{
    public partial class Form3 : Form
    { 
        public void RGBtoMonochrome(bool pb)
        {
            Bitmap bmp = pictureBox1.Image as Bitmap;

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
 
            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                int gray;
                if (pb)
                    gray = Convert.ToInt32(
                        0.299 * rgbValues[i + 0] +
                        0.587 * rgbValues[i + 1] +
                        0.114 * rgbValues[i + 2]);
                else
                    gray = Convert.ToInt32(
                        0.0722 * rgbValues[i + 0] +
                        0.7152 * rgbValues[i + 1] +
                        0.2126 * rgbValues[i + 2]);
                rgbValues[i + 0] = (byte)gray;
                rgbValues[i + 1] = (byte)gray;
                rgbValues[i + 2] = (byte)gray;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
        }

        public Form3(Bitmap img)
        {
            InitializeComponent();
            pictureBox1.Image = img;
            pictureBox2.Image = img;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            RGBtoMonochrome(true);
            RGBtoMonochrome(false);


        }
    }
}
