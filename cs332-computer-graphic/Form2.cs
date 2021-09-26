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

        public static Bitmap[] RGBHist(Bitmap res)
        {
            int Width = 512, Height = 600;

            Bitmap[] Bars = new Bitmap[] { new Bitmap(Width, Height), new Bitmap(Width, Height), new Bitmap(Width, Height) };

            int[] R = new int[256];
            int[] G = new int[256];
            int[] B = new int[256];

            Color color;
            for (int i = 0; i < res.Width; i++)
                for (int j = 0; j < res.Height; ++j)
                {
                    color = res.GetPixel(i, j);
                    R[color.R]++;
                    G[color.G]++;
                    B[color.B]++;
                }
            int max = 0;
            for (int i = 0; i < 256; ++i)
            {
                if (R[i] > max)
                    max = R[i];
                if (G[i] > max)
                    max = G[i];
                if (B[i] > max)
                    max = B[i];
            }
           
            double point = (double)max / Height;

            for (int i = 0; i < Width - 3; ++i)
            {
                for (int j = Height - 1; j > Height - R[i/3]/point ; j--)
                {
                    Bars[0].SetPixel(i, j, Color.Red);
                }
                ++i;
                for (int j = Height - 1; j > Height - G[i/3]/point; j--)
                {
                    Bars[1].SetPixel(i, j, Color.Green);
                }
                ++i;
                for (int j = Height - 1; j > Height - B[i/3]/point; j--)
                {
                    Bars[2].SetPixel(i, j, Color.Blue); ;
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

            Bitmap[] Hist = RGBHist(res);
            pictureBox1.Image = Hist[0];
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            
            pictureBox2.Image = Hist[1];
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            
            pictureBox3.Image = Hist[2];
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }

       
    }
}
