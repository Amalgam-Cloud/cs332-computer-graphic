using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Assignment6
{
    class Primal
    {
        public virtual void Draw(Graphics g, Pen p,string Project = " ", int h = 0, int w = 0) { }
        public static double[,] MatrMult(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];
            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }
            return res;
        }


    }
    class Dot : Primal
    {
        string Project { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool Visibility;
        public Dot(Point dot, bool Vis = true)
        {
            this.X = dot.X;
            this.Y = dot.Y;
            this.Z = 0;
            this.Visibility = Vis;
        }
        public Dot(int X,int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public override void Draw(Graphics g, Pen p, string Project, int h, int w)
        {
            if (!this.Visibility)
            {
                if (p.Color != Color.Black)
                {
                    Brush b = new SolidBrush(p.Color);
                    var n_p = this.AksonometrProject();
                    g.FillRectangle(b, n_p.X-2, n_p.Y-2, 5, 5);
                }
            }
            else
            {
                Brush b = new SolidBrush(p.Color);
                var n_p = this.AksonometrProject();
                g.FillRectangle(b, n_p.X - 3, n_p.Y - 3, 3, 3);
            }
        }
        public Dot AksonometrProject()
        {
            double[,] point1 = new double[,] { { this.X, this.Y, this.Z, 1.0 } };
            double[,] res1 = MatrMult(point1, MatrLibrary.RotateY(-Math.PI/8.0/*0.785398*/));
            double[,] res2 = MatrMult(res1, MatrLibrary.RotateX(Math.PI/4.0/*0.610865*/));
            var xx = Convert.ToInt32(Math.Round(res2[0, 0]));
            var yy = Convert.ToInt32(Math.Round(res2[0, 2]));
            var zz = Convert.ToInt32(Math.Round(res2[0, 1]));
            return new Dot(new Point(xx,yy));

        }
        public Dot PerspectiveProject()
        {
            double[,] point1 = new double[,] { { this.X, this.Y, this.Z, 1.0 } };
            double[,] res1 = MatrMult(point1, MatrLibrary.PerspectiveProject());
            var xx = (double)this.X / res1[0,3];
            var yy = (double)this.Y / res1[0, 3];
            //var xx = this.X;
            //var yy = this.Y;
            return new Dot(new Point(Convert.ToInt32(xx), Convert.ToInt32(yy)));

        }
        public Point GetPoint()
        {
            return new Point(this.X, this.Y);
        }

    }
    class Line:Primal
    {
        public Dot pos1 { get; set; }
        public Dot pos2 { get; set; }
        public static Dot CrossingLines(Line l1, Line l2)
        {
            /*y = Ax + B*/
            float a1 = (float)(l1.pos1.Y - l1.pos2.Y) / (float)(l1.pos1.X - l1.pos2.X);
            float b1 = l1.pos1.Y - a1 * l1.pos1.X;
            float a2 = (float)(l2.pos1.Y - l2.pos2.Y) / (float)(l2.pos1.X - l2.pos2.X);
            float b2 = l2.pos1.Y - a2 * l2.pos1.X;
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
            float min_l1_x = Math.Min(l1.pos1.X, l1.pos2.X);
            float min_l1_y = Math.Min(l1.pos1.Y, l1.pos2.Y);
            float max_l1_x = Math.Max(l1.pos1.X, l1.pos2.X);
            float max_l1_y = Math.Max(l1.pos1.Y, l1.pos2.Y);
            float min_l2_x = Math.Min(l2.pos1.X, l2.pos2.X);
            float min_l2_y = Math.Min(l2.pos1.Y, l2.pos2.Y);
            float max_l2_x = Math.Max(l2.pos1.X, l2.pos2.X);
            float max_l2_y = Math.Max(l2.pos1.Y, l2.pos2.Y);
            //L1 contain
            if (x < min_l1_x || x > max_l1_x || y < min_l1_y || y > max_l1_y)
                return new Dot(new Point(0, 0));
            //L2 contain
            if (x < min_l2_x || x > max_l2_x || y < min_l2_y || y > max_l2_y)
                return new Dot(new Point(0, 0));
            return new Dot(new Point(Convert.ToInt32(x), Convert.ToInt32(y)),false);

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

        public override void Draw(Graphics g,Pen p, string project,int h,int w)
        {
            if(project == "Аксонометрическая(Изометрическая)")
            {
                Dot n_p1 = pos1.AksonometrProject();
                Dot n_p2 = pos2.AksonometrProject();
                n_p1.X += w / 2;
                n_p1.Y += h / 2;
                n_p2.X += w / 2;
                n_p2.Y += h / 2;
                g.DrawLine(p, new Point(n_p1.X, n_p1.Y), new Point(n_p2.X,n_p2.Y));
            }
            if (project == "Перспективная")
            {
                Dot n_p1 = pos1.PerspectiveProject();
                Dot n_p2 = pos2.PerspectiveProject();
                n_p1.X += w / 2;
                n_p1.Y += h / 2;
                n_p2.X += w / 2;
                n_p2.Y += h / 2;
                g.DrawLine(p, new Point(n_p1.X, n_p1.Y), new Point(n_p2.X, n_p2.Y));
            }
        }

    }
    class Poligon : Primal
    {
        List<Line> lines;
        public Poligon(List<Line> lines)
        {
            this.lines = lines;
        }
        public Poligon()
        {
            this.lines = new List<Line>();
        }
        public void AddLine(Line l)
        {
            this.lines.Add(l);
        }
        public override void Draw(Graphics g, Pen p, string project, int h, int w)
        {
            foreach(Line l in this.lines)
            {
                l.Draw(g,p, project, h, w);
            }
        }
    }
    class Hexahedron : Primal
    {
        List<Poligon> sides { get; set; }
        public Hexahedron(int size)
        {
            this.sides = new List<Poligon>();
            Dot p1 = new Dot(0, 0, 0);
            Dot p2 = new Dot(size, 0, 0);
            Dot p3 = new Dot(size, 0, size);
            Dot p4 = new Dot(0, 0, size);
            Dot p5 = new Dot(0, -size, 0);
            Dot p6 = new Dot(size, -size, 0);
            Dot p7 = new Dot(size, -size, size);
            Dot p8 = new Dot(0, -size, size);
            //Bottom
            var side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p1));
            this.sides.Add(side);
            //Left
            side = new Poligon();
            side.AddLine(new Line(p1, p5));
            side.AddLine(new Line(p5, p8));
            side.AddLine(new Line(p8, p4));
            side.AddLine(new Line(p4, p1));
            this.sides.Add(side);
            //Front
            side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p6));
            side.AddLine(new Line(p6, p5));
            side.AddLine(new Line(p5, p1));
            this.sides.Add(side);
            //Right
            side = new Poligon();
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p7));
            side.AddLine(new Line(p7, p6));
            side.AddLine(new Line(p6, p2));
            this.sides.Add(side);
            //Back
            side = new Poligon();
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p8));
            side.AddLine(new Line(p8, p7));
            side.AddLine(new Line(p7, p3));
            this.sides.Add(side);
            //Top
            side = new Poligon();
            side.AddLine(new Line(p5, p6));
            side.AddLine(new Line(p6, p7));
            side.AddLine(new Line(p7, p8));
            side.AddLine(new Line(p8, p5));
            this.sides.Add(side);
        }
        public Hexahedron(List<Poligon> sides)
        {
            this.sides = sides;
        }
        public Hexahedron(Poligon bottom,Poligon left, Poligon front, Poligon right, Poligon back, Poligon top)
        {
            this.sides = new List<Poligon>();
            this.sides.Add(bottom);
            this.sides.Add(left);
            this.sides.Add(front);
            this.sides.Add(right);
            this.sides.Add(back);
            this.sides.Add(top);
        }
        public Hexahedron(Dot p1, Dot p2, Dot p3, Dot p4, Dot p5, Dot p6, Dot p7, Dot p8)
        {
            //Bottom
            this.sides = new List<Poligon>();
            var side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p1));
            this.sides.Add(side);
            //Left
            side = new Poligon();
            side.AddLine(new Line(p1, p5));
            side.AddLine(new Line(p5, p8));
            side.AddLine(new Line(p1, p4));
            side.AddLine(new Line(p4, p8));
            this.sides.Add(side);
            //Front
            side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p6));
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p6, p5));
            this.sides.Add(side);
            //Right
            side = new Poligon();
            side.AddLine(new Line(p2,p3));
            side.AddLine(new Line(p3, p7));
            side.AddLine(new Line(p7, p6));
            side.AddLine(new Line(p6, p2));
            this.sides.Add(side);
            //Back
            side = new Poligon();
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p8));
            side.AddLine(new Line(p8, p7));
            side.AddLine(new Line(p7, p3));
            this.sides.Add(side);
            //Top
            side = new Poligon();
            side.AddLine(new Line(p5, p6));
            side.AddLine(new Line(p6, p7));
            side.AddLine(new Line(p7, p8));
            side.AddLine(new Line(p8, p5));
            this.sides.Add(side);
        }
        public bool AddSide(Poligon side)
        {
            if(this.sides.Count <= 6)
            {
                this.sides.Add(side);
                return false;
            }
            return true;
        }
        public override void Draw(Graphics g, Pen p, string project, int h, int w)
        {
            foreach (Poligon pol in this.sides)
            {
                pol.Draw(g, p, project, h , w);
            }
        }
    }
    class Tetradedron : Primal
    {
        List<Poligon> sides { get; set; }
        public Tetradedron(int size)
        {
            this.sides = new List<Poligon>();
            Dot p1 = new Dot(0, -size, 0);
            Dot p2 = new Dot(size, 0, 0);
            Dot p3 = new Dot(0, 0, size);
            Dot p4 = new Dot(size, -size, size);
            //Bottom
            var side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p1));
            this.sides.Add(side);
            //Left
            side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p4));
            side.AddLine(new Line(p4, p1));
            this.sides.Add(side);
            //Back
            side = new Poligon();
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p2));
            this.sides.Add(side);
            //Right
            side = new Poligon();
            side.AddLine(new Line(p3, p1));
            side.AddLine(new Line(p1, p4));
            side.AddLine(new Line(p4, p3));
            this.sides.Add(side);
        }
        public Tetradedron(List<Poligon> sides)
        {
            this.sides = sides;
        }
        public Tetradedron(Poligon bottom, Poligon left, Poligon back,Poligon right)
        {
            this.sides = new List<Poligon>();
            this.sides.Add(bottom);
            this.sides.Add(left);
            this.sides.Add(back);
            this.sides.Add(right);
        }
        public bool AddSide(Poligon side)
        {
            if (this.sides.Count <= 4)
            {
                this.sides.Add(side);
                return false;
            }
            return true;
        }
        public override void Draw(Graphics g, Pen p, string project, int h, int w)
        {
            foreach (Poligon pol in this.sides)
            {
                pol.Draw(g, p, project, h, w);
            }
        }
    }
    class Octaedron : Primal
    {
        List<Poligon> sides { get; set; }
        public Octaedron(int size)
        {
            this.sides = new List<Poligon>();
            Dot p1 = new Dot(size/2, 0, size/2);
            Dot p2 = new Dot(0, -size/2, size / 2);
            Dot p3 = new Dot(size / 2, -size / 2, 0);
            Dot p4 = new Dot(size, -size / 2, size / 2);
            Dot p5 = new Dot(size / 2, -size / 2, size);
            Dot p6 = new Dot(size/2, -size, size / 2);
            //Bottom_Left
            var side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p5));
            side.AddLine(new Line(p5, p1));
            this.sides.Add(side);
            //Bottom_Front
            side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p1));
            this.sides.Add(side);
            //Bottom_Right
            side = new Poligon();
            side.AddLine(new Line(p1, p3));
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p1));
            this.sides.Add(side);
            //Bottom_Back
            side = new Poligon();
            side.AddLine(new Line(p1, p4));
            side.AddLine(new Line(p4, p5));
            side.AddLine(new Line(p5, p1));
            this.sides.Add(side);
            //Top_Left
            side = new Poligon();
            side.AddLine(new Line(p2, p5));
            side.AddLine(new Line(p5, p6));
            side.AddLine(new Line(p6, p2));
            this.sides.Add(side);
            //Top_Front
            side = new Poligon();
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p6));
            side.AddLine(new Line(p6, p2));
            this.sides.Add(side);
            //Top_Right
            side = new Poligon();
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p6));
            side.AddLine(new Line(p6, p3));
            this.sides.Add(side);
            //Top_Back
            side = new Poligon();
            side.AddLine(new Line(p4, p5));
            side.AddLine(new Line(p5, p6));
            side.AddLine(new Line(p6, p4));
            this.sides.Add(side);
        }
        public Octaedron(List<Poligon> sides)
        {
            this.sides = sides;
        }
        public Octaedron(Dot p1, Dot p2, Dot p3, Dot p4, Dot p5, Dot p6)
        {
            this.sides = new List<Poligon>();
            //Bottom_Left
            var side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p4));
            side.AddLine(new Line(p4, p1));
            //Bottom_Front
            side = new Poligon();
            side.AddLine(new Line(p1, p2));
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p1));
            this.sides.Add(side);
            this.sides.Add(side);
            //Bottom_Right
            side = new Poligon();
            side.AddLine(new Line(p2, p5));
            side.AddLine(new Line(p5, p1));
            side.AddLine(new Line(p1, p2));
            this.sides.Add(side);
            //Bottom_Back
            side = new Poligon();
            side.AddLine(new Line(p1, p4));
            side.AddLine(new Line(p4, p5));
            side.AddLine(new Line(p5, p1));
            this.sides.Add(side);
            //Top_Left
            side = new Poligon();
            side.AddLine(new Line(p3, p4));
            side.AddLine(new Line(p4, p6));
            side.AddLine(new Line(p6, p3));
            this.sides.Add(side);
            //Top_Front
            side = new Poligon();
            side.AddLine(new Line(p2, p3));
            side.AddLine(new Line(p3, p6));
            side.AddLine(new Line(p6, p2));
            this.sides.Add(side);
            //Top_Right
            side = new Poligon();
            side.AddLine(new Line(p5, p2));
            side.AddLine(new Line(p3, p6));
            side.AddLine(new Line(p6, p5));
            this.sides.Add(side);
            //Top_Back
            side = new Poligon();
            side.AddLine(new Line(p4, p5));
            side.AddLine(new Line(p5, p6));
            side.AddLine(new Line(p6, p4));
            this.sides.Add(side);
        }
        public Octaedron(Poligon bottom_left,Poligon bottom_front, Poligon bottom_right, Poligon bottom_back, Poligon top_left, Poligon top_front, Poligon top_right, Poligon top_back)
        {
            this.sides = new List<Poligon>();
            this.sides.Add(bottom_left);
            this.sides.Add(bottom_front);
            this.sides.Add(bottom_right);
            this.sides.Add(bottom_back);
            this.sides.Add(top_left);
            this.sides.Add(top_front);
            this.sides.Add(top_right);
            this.sides.Add(top_back);
        }
        public bool AddSide(Poligon side)
        {
            if (this.sides.Count <= 8)
            {
                this.sides.Add(side);
                return false;
            }
            return true;
        }
        public override void Draw(Graphics g, Pen p, string project, int h, int w)
        {
            foreach (Poligon pol in this.sides)
            { 
                pol.Draw(g, p, project, h, w);
            }
        }
    }
    class MatrLibrary
    {
        public static double[,] RotateY(double angle)
        {
            return new double[,]
                {
                    { Math.Cos(angle), 0, -Math.Sin(angle), 0 },
                    { 0, 1, 0, 0 },
                    { Math.Sin(angle), 0,Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 }
                };
        }
        public static double[,] RotateX(double angle)
        {
            return new double[,]
                {
                    {1, 0, 0, 0 },
                    { 0, Math.Cos(angle),  Math.Sin(angle), 0 },
                    {0, -Math.Sin(angle),Math.Cos(angle), 0 },
                    { 0, 0, 0, 1 }
                };
        }
        public static double[,] AksonometrProject(double angle)
        {
            return new double[,]
                {
                    {1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    {0, 0, 0, 0 },
                    { 0, 0, 0, 1 }
                };
        }
        public static double[,] PerspectiveProject()
        {
            return new double[,]
                {
                    {1, 0, 0, 0},
                    { 0, 0, 0, 0},
                    {0, 0, 1, 0.003 },
                    { 0, 0, 0, 1 }
                };
        }
    }
}
