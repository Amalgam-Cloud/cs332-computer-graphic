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
    public partial class Form2 : Form

    {
        public Bitmap cur_image;

        public static Bitmap[] ConvertToRGB(Bitmap res)
        {
            Bitmap[] result = new Bitmap[3] { new Bitmap(res.Width, res.Height), new Bitmap(res.Width, res.Height), new Bitmap(res.Width, res.Height) };
            for (int i = 0; i < res.Width; i++)
            {
                for (int j = 0; j < res.Height; j++)
                {
                    Color color = res.GetPixel(i, j);
                    result[0].SetPixel(i, j, Color.FromArgb(color.A, color.R, 0, 0));
                    result[1].SetPixel(i, j, Color.FromArgb(color.A, 0, color.G, 0));
                    result[2].SetPixel(i, j, Color.FromArgb(color.A, 0, 0, color.B));
                }
            }
            return result;
        }

        public static Bitmap RGBHist(Bitmap res)
        {
            int Width = 256, Height = res.Height;

            Bitmap Bars = new Bitmap(Width,Height);

            int[] R = new int[256];
            int[] G = new int[256];
            int[] B = new int[256];

            int i, j;
            Color color;
            for (i = 0; i < res.Width; i++)
                for (j = 0; j < res.Height-1; ++j)
                {
                    color = res.GetPixel(i, j);
                    R[color.R]++;
                    G[color.G]++;
                    B[color.B]++;
                }

            int max = 0;

            for (i = 0; i < 256; ++i)
            {
                if (R[i] > max)
                    max = R[i];
                if (G[i] > max)
                    max = G[i];
                if (B[i] > max)
                    max = B[i];
            }

            double point = (double)max / Height;

            for (i = 0; i < Width - 3; ++i)
            {
                for (j = Height - 1; j > Height - R[i / 3] / point; --j)
                {
                    Bars.SetPixel(i, j, Color.Red);
                }
                ++i;
                for (j = Height - 1; j > Height - G[i / 3] / point; --j)
                {
                    Bars.SetPixel(i, j, Color.Green);
                }
                ++i;
                for (j = Height - 1; j > Height - B[i / 3] / point; --j)
                {
                    Bars.SetPixel(i, j, Color.Blue);
                }
            }
            return Bars;
        } 


        public Form2(Bitmap res)
        {
            InitializeComponent(); 
            Bitmap[] RGB = ConvertToRGB(res);
            REDpictureBox.Image = RGB[0];
            REDpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            GREENpictureBox.Image = RGB[2];
            GREENpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            BLUEpictureBox.Image = RGB[1];
            BLUEpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Bitmap Hist = RGBHist(RGB[0]);
            pictureBox1.Image = Hist;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Hist = RGBHist(RGB[1]);
            pictureBox2.Image = Hist;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            Hist = RGBHist(RGB[2]);
            pictureBox3.Image = Hist;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

       
    }
}
