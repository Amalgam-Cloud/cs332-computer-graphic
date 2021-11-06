using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment9
{
    public partial class Form1 : Form
    {
        static Random rnd = new Random();
        public int p1 = 0;
        public int p2 = 0;
        Graphics graph;
        List<Color> palet = new List<Color>();
        public Form1()
        {
            InitializeComponent();
            this.graph = pictureBox1.CreateGraphics();
            comboBox1.Items.Add("cos(x^2+y^2)/(x^2+y^2+2)") ;
            comboBox1.Items.Add("x^2+y^2");
            this.palet.Add(Color.Red);
            this.palet.Add(Color.Orange);
            this.palet.Add(Color.Yellow);
            this.palet.Add(Color.Green);
            this.palet.Add(Color.Cyan);
            this.palet.Add(Color.Blue);
            this.palet.Add(Color.DarkMagenta);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GraphicFloatingHorizont(this.graph, -5, 5, -5, 5, 50,comboBox1.Text);
        }
        private double Distance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        private double F(string s,double x,double y)
        {
            switch(s)
            {
                case "cos(x^2+y^2)/(x^2+y^2+2)":
                    return Math.Cos(x* x + y * y) / (x * x + y * y + 2);
                    break;
                case "x^2+y^2":
                    return x * x + y * y;
                    break;
                default: return -1;
            }
        }
        private void GraphicFloatingHorizont(Graphics graphics, float X0, float X1, float Y0, float Y1, int countSplit, string s)
        {
            int scale = 50;

            // верхний и нижний горизонты
            float[] maxG = new float[pictureBox1.Width];
            float[] minG = new float[pictureBox1.Width];
            for (int i = 0; i < pictureBox1.Width; i++)
            {
                maxG[i] = float.MinValue;
                minG[i] = float.MaxValue;
            }

            float dy = (Y1 - Y0) / countSplit;

            float angleX = (float)p1 / 10;
            float angleY = (float)p2 / 10;

            var pen = new Pen(Color.Black);
            pen.Width = 1;

            for (float y = Y0; y < Y1; y += dy)
            {
                Point lastPoint = new Point(0, 0);
                for (float bmpX = -pictureBox1.Width / 2; bmpX < pictureBox1.Width / 2; bmpX++)
                {
                    float x = (bmpX + X0) / scale;

                    int screenX = (int)(bmpX + pictureBox1.Width / 2);

                    if (screenX >= pictureBox1.Width || screenX <= 0)
                        continue;

                    float z = (float)((float)Math.Cos(angleY) * (float)Math.Cos(angleX) * y - (float)Math.Sin(angleX) * x + (float)Math.Sin(angleY) * F(s, (float)Math.Cos(angleX) * y - (float)Math.Sin(angleX) * x, (float)Math.Sin(angleX) * y + (float)Math.Cos(angleX) * x));
                    int screenY = (int)(z * scale + pictureBox1.Height / 2);
                    if (screenY >= pictureBox1.Height || screenY <= 0)
                        continue;

                    if (z < minG[screenX])
                    {
                        minG[screenX] = z;
                        pen.Color = /*Color.DarkGreen*/Color.Green;
                        Point curPoint = new Point(screenX, screenY);
                        if (Distance(curPoint, lastPoint) < 5)
                        {
                            graphics.DrawLine(pen, lastPoint, new Point(screenX, screenY));
                        }
                        lastPoint = new Point(screenX, screenY);
                    }
                    if (z > maxG[screenX])
                    {
                        maxG[screenX] = z;
                        
                        pen.Color = Color.Black;
                        Point curPoint = new Point(screenX, screenY);
                        if (Distance(curPoint, lastPoint) < 5)
                        {
                            graphics.DrawLine(pen, lastPoint, curPoint);
                        }
                        lastPoint = new Point(screenX, screenY);
                    }

                }

            }
        }
        private void Separate()
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.W: p1 +=3; break;
                case Keys.S: p1 -=3; break;
                case Keys.D: p2 +=3; break;
                case Keys.A: p2 -=3; break;
            }
            graph.Clear(Color.White);
            GraphicFloatingHorizont(this.graph, (float)numericUpDown1.Value, (float)numericUpDown2.Value, (float)numericUpDown3.Value, (float)numericUpDown4.Value, 50, comboBox1.Text);

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
