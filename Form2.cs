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

        public Form2(Bitmap res)
        {
            InitializeComponent();
            Bitmap[] RGB = ConvertToRGB(res);
            REDpictureBox.Image = RGB[0];
            REDpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            GREENpictureBox.Image = RGB[1];
            GREENpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            BLUEpictureBox.Image = RGB[2];
            BLUEpictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }

       
    }
}
