using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Assignment2
{
    public partial class task1 : Form
    {
        Graphics gr;
        Bitmap bmp;
        Point coord;
        Boolean isDown;
        Point prev;
        Color color = Color.Black;

        void FloodFill(Point p)
        {
            if (!pictureBox1.ClientRectangle.Contains(p))
                return;
            Color currC = GetColor(p);
            Color pervC = currC;        
            if (currC != color && currC == pervC)
            {
                Point leftP = p;
                Point rightP = p;
                while (currC == pervC && pictureBox1.ClientRectangle.Contains(p))
                {
                    leftP.X -= 1;
                    currC = GetColor(leftP);
                }
                leftP.X += 1;
                currC = GetColor(p);
                while (currC == pervC && pictureBox1.ClientRectangle.Contains(p))
                {
                    rightP.X += 1;
                    currC = GetColor(rightP);
                }
                rightP.X -= 1;
                Pen pen = new Pen(color, 1);
                gr.DrawLine(pen, leftP, rightP);
                for (int i = leftP.X; i <= rightP.X; ++i)
                {
                    Point upP = new Point(i, p.Y + 1);
                    Color curC = GetColor(upP);
                    if (curC.ToArgb() != Color.Black.ToArgb() && curC.ToArgb() != color.ToArgb() && pictureBox1.ClientRectangle.Contains(upP))
                        FloodFill(upP);
                }
                for (int i = leftP.X; i <= rightP.X; ++i)
                {
                    Point downP = new Point(i, p.Y - 1);
                    Color curC = GetColor(downP);
                    if (curC.ToArgb() != Color.Black.ToArgb() && curC.ToArgb() != color.ToArgb() && pictureBox1.ClientRectangle.Contains(downP))
                        FloodFill(downP);
                }
            }
        }

        Color GetColor(Point p)
        {
            if (pictureBox1.ClientRectangle.Contains(p))
                return ((Bitmap)pictureBox1.Image).GetPixel(p.X, p.Y);
            else
                return Color.Black;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDown = true;
            prev = e.Location;
            coord = e.Location;
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (radioButton2.Checked && isDown && prev != Point.Empty)
            {
                Pen p = new Pen(color, 1);
                gr.DrawLine(p, prev, e.Location);
                prev = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDown = false;
            prev = Point.Empty;
            coord = Point.Empty;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                FloodFill(coord);
            }
            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            c.ShowDialog();
            color = c.Color;
        }


        public task1()
        {
            InitializeComponent();
            bmp = new Bitmap(Width, Height);
            pictureBox1.Image = bmp;
            gr = Graphics.FromImage(pictureBox1.Image);
        }

        
    }
}
