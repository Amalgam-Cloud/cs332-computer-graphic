using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Assignment5
{
    class Primal
    {
        public virtual void Draw(Graphics g, Pen p, Bitmap b = null) { }

    }
    class Dot:Primal
    {
        public Point Pos { get; set; }
        public bool Visibility { get; set; }
        public Dot buddy { get; set; }
        public Dot suprim { get; set; }
        public Dot(Point dot, bool Vis = true)
        {
            this.Pos = dot;
            this.Visibility = Vis;
        }
        public Dot()
        {
            this.Pos = new Point(0,0);
            this.Visibility = true;
            this.buddy = null;
        }
        public override void Draw(Graphics g, Pen p, Bitmap b = null)
        {
            if (!this.Visibility)
            {
                if (p.Color != Color.Black)
                {
                    Brush br = new SolidBrush(p.Color);
                    g.FillRectangle(br, this.Pos.X-2, this.Pos.Y-2, 5, 5);
                }
            }
            else
            {
                Brush br = new SolidBrush(p.Color);
                g.FillRectangle(br, this.Pos.X - 3, this.Pos.Y - 3, 3, 3);
            }
            if(this.suprim != null)
            {
                p.Width = 1;
                g.DrawLine(p, this.Pos, this.buddy.Pos);
            }
        }
        public static Dot operator+(Dot d1, Dot d2)
        {
            return new Dot(new Point(d1.Pos.X + d2.Pos.X, d1.Pos.Y + d2.Pos.Y));
        }
        public static Dot operator-(Dot d1, Dot d2)
        {
            return new Dot(new Point(d1.Pos.X - d2.Pos.X, d1.Pos.Y - d2.Pos.Y));
        }
    }
    class Line:Primal
    {
        public Dot pos1 { get; set; }
        public Dot pos2 { get; set; }
        public static Dot CrossingLines(Line l1, Line l2)
        {
            /*y = Ax + B*/
            float a1 = (float)(l1.pos1.Pos.Y - l1.pos2.Pos.Y) / (float)(l1.pos1.Pos.X - l1.pos2.Pos.X);
            float b1 = l1.pos1.Pos.Y - a1 * l1.pos1.Pos.X;
            float a2 = (float)(l2.pos1.Pos.Y - l2.pos2.Pos.Y) / (float)(l2.pos1.Pos.X - l2.pos2.Pos.X);
            float b2 = l2.pos1.Pos.Y - a2 * l2.pos1.Pos.X;
            double x = Math.Round((float)(b2 - b1) / (float)(a1 - a2));
            double y = Math.Round(a1 * x + b1);
            if (l1.pos1 == l2.pos1 || l1.pos1 == l2.pos1)
            {
                return l1.pos1;
            }
            if (l1.pos2 == l2.pos1 || l1.pos2 == l2.pos2)
            {
                return l1.pos2;
            }
            if (a1 == a2)
                return new Dot(new Point(0,0));
            float min_l1_x = Math.Min(l1.pos1.Pos.X, l1.pos2.Pos.X);
            float min_l1_y = Math.Min(l1.pos1.Pos.Y, l1.pos2.Pos.Y);
            float max_l1_x = Math.Max(l1.pos1.Pos.X, l1.pos2.Pos.X);
            float max_l1_y = Math.Max(l1.pos1.Pos.Y, l1.pos2.Pos.Y);
            float min_l2_x = Math.Min(l2.pos1.Pos.X, l2.pos2.Pos.X);
            float min_l2_y = Math.Min(l2.pos1.Pos.Y, l2.pos2.Pos.Y);
            float max_l2_x = Math.Max(l2.pos1.Pos.X, l2.pos2.Pos.X);
            float max_l2_y = Math.Max(l2.pos1.Pos.Y, l2.pos2.Pos.Y);
            //L1 contain
            if (x < min_l1_x || x > max_l1_x || y < min_l1_y || y > max_l1_y)
                return new Dot(new Point(0, 0));
            //L2 contain
            if (x < min_l2_x || x > max_l2_x || y < min_l2_y || y > max_l2_y)
                return new Dot(new Point(0, 0));
            return new Dot(new Point(Convert.ToInt32(x), Convert.ToInt32(y)),false);

        }
        public bool IsUnerlined(Dot d)
        {
            return (this.pos2.Pos.X - this.pos1.Pos.X) * (d.Pos.Y - this.pos1.Pos.Y) - (this.pos2.Pos.Y - this.pos1.Pos.Y) * (d.Pos.X - this.pos1.Pos.X) > 0;
        }
        public Line(Point p1,Point p2)
        {
            this.pos1 = new Dot(p1);
            this.pos2 = new Dot(p2);
        }
        public Line(Dot d1, Dot d2)
        {
            this.pos1 = d1;
            this.pos2 = d2;
        }
        public override void Draw(Graphics g,Pen p, Bitmap b = null)
        {
            g.DrawLine(p,pos1.Pos,pos2.Pos);
        }
    }
    class Poligon : Primal
    {
        public Line upSide { get; set; }
        public Line downSide { get; set; }
        public Line leftSide { get; set; }
        public Line rightSide { get; set; }
        public Poligon(Point p1, Point p2)
        {
            List<Point> points = new List<Point>();
            if (p1.X < p2.X && p1.Y > p2.Y)
            {
                points.Add(p1);
                points.Add(p2);
            }
            else
            {
                points.Add(p2);
                points.Add(p1);
            }
            Point uprightCorner = new Point(points[1].X, points[0].Y);
            Point downleftCorner = new Point(points[0].X, points[1].Y);
            this.upSide = new Line(points[0],uprightCorner);
            this.rightSide = new Line(uprightCorner,points[1]);
            this.downSide = new Line(points[1],downleftCorner);
            this.leftSide = new Line(downleftCorner,points[0]);
        }
        public override void Draw(Graphics g, Pen p, Bitmap b = null)
        {
            this.upSide.Draw(g,p);
            this.rightSide.Draw(g,p);
            this.downSide.Draw(g,p);
            this.leftSide.Draw(g,p);
        }
        public bool IsDotInside(Dot d)
        {
            bool p1 = this.upSide.IsUnerlined(d);
            bool p2 = this.rightSide.IsUnerlined(d);
            bool p3 = this.downSide.IsUnerlined(d);
            bool p4 = this.leftSide.IsUnerlined(d);
            return  p1 && p2 && p3 && p4;
        }
    }
    class Spline :Primal
    {
        static Random rnd = new Random();
        public List<Dot> dots { get; set; }
        bool ready;
        List<Color> palet = new List<Color>();
        public Spline()
        {
            this.dots = new List<Dot>();
            this.ready = false;
        }
        public Spline(Dot d)
        {
            this.dots = new List<Dot>();
            dots.Add(d);
            this.palet.Add(Color.Red);
            this.palet.Add(Color.Orange);
            this.palet.Add(Color.Yellow);
            this.palet.Add(Color.Green);
            this.palet.Add(Color.Cyan);
            this.palet.Add(Color.Blue);
            this.palet.Add(Color.DarkMagenta);
        }
        public void AddPoint(Dot d)
        {
            dots.Add(d);
            if(this.palet.Count == 0)
            {
                this.palet.Add(Color.Red);
                this.palet.Add(Color.Orange);
                this.palet.Add(Color.Yellow);
                this.palet.Add(Color.Green);
                this.palet.Add(Color.Cyan);
                this.palet.Add(Color.Blue);
                this.palet.Add(Color.DarkMagenta);
            }
            if(dots.Count >= 3)
            {
                this.ready = true;
            }
        }
        private Bitmap SplineDraw(Dot p0, Dot p1, Dot p2, Dot p3, Color cl, Bitmap b)
        {
            Dot[] v = new Dot[4] { p0, p1, p2, p3 };
            int[,] m = new int[4, 4] { { 1, -3, 3, -1 }, { 0, 3, -6, 3 }, { 0, 0, 3, -3 }, { 0, 0, 0, 1 } };
            float[] t = new float[4];
            Point[] mult = new Point[4] { new Point(0,0), new Point(0, 0), new Point(0, 0), new Point(0, 0) };
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j <= 3; ++j)
                {
                    mult[i].Y += v[j].Pos.Y * m[j, i];
                    mult[i].X += v[j].Pos.X * m[j, i];
                }
            }
            for (float T = 0; T <= 1; T += (float)0.0001)
            {
                PointF res = new PointF(0, 0);
                t[0] = 1;
                t[1] = T;
                t[2] = T * T;
                t[3] = t[2] * T;
                for (int i = 0; i < 4; ++i)
                {
                    res.X += mult[i].X * t[i];
                    res.Y += mult[i].Y * t[i];
                }
                b.SetPixel((int)res.X, (int)res.Y, cl);
            }
            return b;
        }
        public Bitmap DrawBit(Graphics g, Pen p, Bitmap b, ListBox lb, List<Primal> lst)
        {
            g = Graphics.FromImage(b);
            Dot start = null;
            Dot end = null;
            foreach (var dot in this.dots)
            {
                dot.Draw(g,p);
            }
            if (this.dots.Count >= 4)
            {
                //Сплайн
                start = this.dots[0];
                for (int i = 3; i <= this.dots.Count - 3; i += 3)
                {
                    b = SplineDraw(this.dots[i - 3], this.dots[i-1], this.dots[i + 1], this.dots[i], this.palet[rnd.Next(this.palet.Count)], b);
                }

            }
            return b;

        }

    }
}
