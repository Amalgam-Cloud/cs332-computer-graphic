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
    public partial class Form1 : Form
    {

        public Bitmap cur_img;
        string img_path;
        public Form1()
        {
            InitializeComponent();
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 255;
            trackBar2.Minimum = 0;
            trackBar2.Maximum = 100;
            trackBar3.Minimum = 0;
            trackBar3.Maximum = 100;

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(this.cur_img != null)
                PictureHSVTransform();
        }
        public static void RGBtoHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1.0 - (1.0 * min / max);
            value = max / 255.0;
        }
        public static Color HSVtoRGB(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));
            if (v > 255)
            {
                v = 255;
            }
            if (v < 0)
            {
                v = 0;
            }
            if (p > 255)
            {
                p = 255;
            }
            if (p < 0)
            {
                p = 0;
            }
            if (q > 255)
            {
                q = 255;
            }
            if (q < 0)
            {
                q = 0;
            }
            if (t > 255)
            {
                t = 255;
            }
            if (t < 0)
            {
                t = 0;
            }

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
        public void PictureHSVTransform()
        {
            int x, y;
            double h, s, v;
            double c_h = trackBar1.Value;
            double c_s = (double)trackBar2.Value / 100.0;
            double c_v = (double)trackBar3.Value / 100.0;
            Bitmap new_img = (Bitmap)this.cur_img.Clone();
            for (x = 0; x < new_img.Width; x++)
            {
                for (y = 0; y < new_img.Height; y++)
                {
                    Color pColor = new_img.GetPixel(x, y);
                    RGBtoHSV(pColor, out h, out s, out v);
                    Color newColor = HSVtoRGB(h + c_h, s + c_s, v + c_v);
                    new_img.SetPixel(x, y, newColor);
                }
            }
            pictureBox1.Image = new_img;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap p = new Bitmap(pictureBox1.Image);
            p.Save(this.img_path.Substring(0, this.img_path.Length - 4) + "_edit.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    this.img_path = open.FileName;
                    this.cur_img = new Bitmap(this.img_path);
                }
            }
            pictureBox1.Image = this.cur_img;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }



        //.......... RGB CONVERT .......... //

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(this.cur_img); 
            form2.Show();
        }
    }
}
