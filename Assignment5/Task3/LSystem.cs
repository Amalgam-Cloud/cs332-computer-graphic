using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Assignment5
{
    public partial class LSystem : Form
    {
        Graphics g;
        string axiom;
        double angle;
        string direct;
        int n;
        Dictionary<char, string> rules;
        List<Tuple<double, double, double, double>> branch;

        public LSystem()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            rules = new Dictionary<char, string>();
            branch = new List<Tuple<double, double, double, double>>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text files(*.txt)|*.txt";
            if (open.ShowDialog() == DialogResult.OK) {
                string fname = open.FileName;
                string[] flines = File.ReadAllLines(fname);
                string[] parameters = flines[0].Split(' ');

                axiom = parameters[0];
                angle = Convert.ToDouble(parameters[1]);
                direct = parameters[2];

                rules.Clear();
                string[] rule;
                for (int i = 1; i < flines.Length; ++i)
                {
                    rule = flines[i].Split('>');
                    rules[Convert.ToChar(rule[0])] = rule[1];
                }
            }
        }

        string CalculatePath()
        {
            string prev = axiom;
            string next = axiom;
            int c = 0;
            while (c < n)
            {
                prev = next;
                next = "";
                for (int i = 0; i < prev.Length; ++i)
                {
                    if (rules.ContainsKey(prev[i]))
                        next += rules[prev[i]];
                    else
                        next += prev[i];
                }
                c++;
            }
            return next;
        }

        void DrawFractal(string path)
        {
            List<Point> points = new List<Point>();
            List<Tuple<double, double, double, double>> fractal = new List<Tuple<double, double, double, double>>();
            
            double x = 0, y = 0, dx = 0, dy = 0;

            switch (direct)
            {
                case "up":
                    x = pictureBox1.Width / 2;
                    dy = pictureBox1.Height / Math.Pow(10, n);
                    break;

                case "down":
                    x = pictureBox1.Width / 2;
                    y = pictureBox1.Height;
                    dy = -(pictureBox1.Height / Math.Pow(10, n));
                    break;

                case "left":
                    y = pictureBox1.Height / 2;
                    dx = pictureBox1.Width / Math.Pow(10, n);
                    break;

                case "right":
                    x = pictureBox1.Width;
                    y = pictureBox1.Height / 2;
                    dx = -(pictureBox1.Width / Math.Pow(10, n));
                    break;

                default: 
                    break;
            }

            double maxX = -1, maxY = -1, minX = 10000, minY = 10000;
            points.Add(new Point(Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y))));

            double xx, yy;
            for (int i = 0; i < path.Length; ++i)
            {
                switch (path[i])
                {
                    case 'F':
                        fractal.Add(new Tuple<double, double, double, double>(x, y, x + dx, y + dy)); 
                        x += dx;
                        y += dy;
                        if (x > maxX)
                            maxX = x;
                        if (y > maxY)
                            maxY = y;
                        if (x < minX)
                            minX = x;
                        if (y < minY)
                            minY = y;
                        points.Add(new Point(Convert.ToInt32(Math.Round(x)), Convert.ToInt32(Math.Round(y))));
                        break;

                    case '+':
                        xx = dx;
                        yy = dy;
                        dx = xx * Math.Cos(angle * Math.PI / 180) - yy * Math.Sin(angle * Math.PI / 180);
                        dy = xx * Math.Sin(angle * Math.PI / 180) + yy * Math.Cos(angle * Math.PI / 180);
                        break;

                    case '-':
                        xx = dx;
                        yy = dy;
                        dx = xx * Math.Cos(-angle * Math.PI / 180) - yy * Math.Sin(-angle * Math.PI / 180);
                        dy = xx * Math.Sin(-angle * Math.PI / 180) + yy * Math.Cos(-angle * Math.PI / 180);
                        break;

                    case '[':
                        branch.Add(new Tuple<double, double, double, double>(x, y, dx, dy));
                        break;

                    case ']':
                        Tuple<double, double, double, double> coords = branch.Last();
                        x = coords.Item1;
                        y = coords.Item2;
                        dx = coords.Item3;
                        dy = coords.Item4;
                        branch.Remove(coords);
                        break;

                    default: 
                        break;
                }
            }

            double K = Math.Max(maxX - minX, maxY -minY);

            foreach (var fp in fractal)
                g.DrawLine(new Pen(Color.Black),
                    (float)((maxX - fp.Item1) / K * pictureBox1.Width),
                    (float)((maxY - fp.Item2) / K * pictureBox1.Height),
                    (float)((maxX - fp.Item3) / K * pictureBox1.Width),
                    (float)((maxY - fp.Item4) / K * pictureBox1.Height));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            n = Convert.ToInt32(textBox1.Text);
            DrawFractal(CalculatePath());
            pictureBox1.Invalidate();
        }
    }
}
