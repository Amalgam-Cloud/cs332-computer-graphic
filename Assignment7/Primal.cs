using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace Assignment7
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
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool Visibility;
        public Dot(Point dot, bool Vis = true)
        {
            this.X = dot.X;
            this.Y = dot.Y;
            this.Z = 0;
            this.Visibility = Vis;
        }
        public Dot(double X, double Y, double Z)
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
                    g.FillRectangle(b, (int)n_p.X-2, (int)n_p.Y-2, 5, 5);
                }
            }
            else
            {
                Brush b = new SolidBrush(p.Color);
                var n_p = this.AksonometrProject();
                g.FillRectangle(b, (int)n_p.X - 3, (int)n_p.Y - 3, 3, 3);
            }
        }
        public Dot AksonometrProject()
        {
            double[,] point1 = new double[,] { { this.X, this.Y, this.Z, 1.0 } };
            double[,] res1 = MatrMult(point1, MatrLibrary.RotateY(-135));
            double[,] res2 = MatrMult(res1, MatrLibrary.RotateX(-65));
            double[,] res3 = MatrMult(res2, MatrLibrary.RotateY(0));
            var xx = Convert.ToInt32(Math.Round(res3[0, 0]));
            var yy = Convert.ToInt32(Math.Round(res3[0, 2]));
            var zz = Convert.ToInt32(Math.Round(res3[0, 1]));
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
            return new Point((int)this.X, (int)this.Y);
        }

    }
    class Line:Primal
    {
        public Dot pos1 { get; set; }
        public Dot pos2 { get; set; }
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
            if (project == "Аксонометрическая(Изометрическая)")
            {
                Dot n_p1 = pos1.AksonometrProject();
                Dot n_p2 = pos2.AksonometrProject();
                n_p1.X += w / 2;
                n_p1.Y += h / 2 - 2 * (n_p1.Y);
                n_p2.X += w / 2;
                n_p2.Y += h / 2 - 2 * (n_p2.Y);
                g.DrawLine(p, new Point((int)n_p1.X, (int)n_p1.Y), new Point((int)n_p2.X,(int)n_p2.Y));
            }
            if (project == "Перспективная")
            {
                Dot n_p1 = pos1.PerspectiveProject();
                Dot n_p2 = pos2.PerspectiveProject();
                n_p1.X += w / 2;
                if (n_p1.Y > 0)
                {
                    n_p1.Y *= -1;
                }
                n_p1.Y += h / 2;
                n_p2.X += w / 2;
                if (n_p2.Y > 0)
                {
                    n_p2.Y *= -1;
                }
                n_p2.Y += h / 2;
                g.DrawLine(p, new Point((int)n_p1.X, (int)n_p1.Y), new Point((int)n_p2.X, (int)n_p2.Y));
            }
        }

    }
    class Graphic_Grid : Primal
    {
        List<List<Dot>> grid { get; set; }
        double X0 { get; set; }
        double X1 { get; set; }
        double Y0 { get; set; }
        double Y1 { get; set; }
        double step { get; set; }
        List<Dot> dots { get; set; }
        List<Graphic_Line> lines { get; set; }
        private string Func;
        private double F(double x, double y)
        {
            switch (this.Func)
            {
                case "(x * x) + (y * y)":
                    {
                        return Math.Sqrt((x * x) + (y * y));
                    }
                case "| (x * x) - (y * y) | ":
                    {
                        return Math.Sqrt(Math.Abs((x * x) - (y * y)));
                    }
                case "x + y":
                    {
                        return x + y;
                    }
                default:
                    {
                        return Math.Sqrt(Math.Abs((x * x) - (y * y)));
                    }
            }
        }
        class Graphic_Line:Primal
        {
            public List<Dot> dots { get; set; }
            public List<Line> lines { get; set; }
            public Graphic_Line()
            {
                this.dots = new List<Dot>();
                this.lines = new List<Line>();
            }
            public void Add_Dot(Dot d)
            {
                if (this.dots.Count != 0)
                {
                    this.lines.Add(new Line(this.dots.Last(),d));
                }
                this.dots.Add(d);
            }
            public override void Draw(Graphics g, Pen p, string project, int h, int w)
            {
                foreach (var line in this.lines)
                {
                    line.Draw(g, p, project, h, w);
                }
            }
        }
        public Graphic_Grid(double x0, double x1, double y0, double y1, double step, string func)
        {
            this.dots = new List<Dot>();
            this.lines = new List<Graphic_Line>();
            this.X0 = x0;
            this.X1 = x1;
            this.Y0 = y0;
            this.Y1 = y1;
            this.step = step;
            this.Func = func;
            dots.Add(new Dot(0, 0, F(0, 0)));

            for (double i = this.X0; i < this.X1; i += step)
            {
                var l = new Graphic_Line();
                for (double j = this.Y0; j < this.Y1; j += step)
                {
                    l.Add_Dot(new Dot(i, F(i, j), j));
                    
                }
                this.lines.Add(l);
            }
        }
        public override void Draw(Graphics g, Pen p, string project, int h, int w)
        {
            foreach (var line in this.lines)
            {
                line.Draw(g, p, project, h, w);
            }
        }
        public void Save()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)| *.obj | Text files(*.txt) | *.txt | All files(*.*) | *.* ";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string buffer = "";

                    buffer += "Plor_Grid" + "\r\n";

                    int num_line = 1;
                    int num_dot = 1;
                    foreach (Graphic_Line line in this.lines)
                    {
                        buffer += "Line " + num_line + "\r\n";
                        foreach (Dot dot in line.dots)
                        {
                            buffer += "Dot #" + num_dot;
                            buffer += "\r\n";
                            buffer += dot.X + " ";
                            buffer += dot.Y + " ";
                            buffer += dot.Z;
                            buffer += "\r\n";
                            ++num_dot;
                        }
                        ++num_line;
                    }
                    System.IO.File.WriteAllText(saveDialog.FileName, buffer);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void Load()
        {
            this.lines = new List<Graphic_Line>();
            OpenFileDialog loadDialog = new OpenFileDialog();
            loadDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (loadDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<Dot> points = new List<Dot>();
                    Graphic_Line l = new Graphic_Line();
                    Dot d;
                    bool flag = false;
                    bool f_line = true;
                    var str = System.IO.File.ReadAllLines(loadDialog.FileName);
                    foreach (var line in str)
                    {
                        if(flag)
                        {
                            var pos = line.Split(' ');
                            l.Add_Dot(new Dot(double.Parse(pos[0]), double.Parse(pos[1]), double.Parse(pos[2])));
                            flag = false;

                        }
                        if (line[0] == 'L')
                        {
                            if(!f_line)
                            {
                                this.lines.Add(l);
                            }
                            l = new Graphic_Line();
                            flag = false;
                            f_line = false;
                        }
                        if(line[0] == 'D')
                        {
                            flag = true;
                        }
                    }
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно прочитать файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Console.WriteLine();
        }
    }
    class MatrLibrary
    {
        public static double[,] RotateY(double angle)
        {
            angle = (Math.PI / 180) * angle;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return new double[,]
                {
                    { cos, 0, sin, 0 },
                    { 0, 1, 0, 0 },
                    { -sin, 0,cos, 0 },
                    { 0, 0, 0, 1 }
                };
        }
        public static double[,] RotateX(double angle)
        {
            angle = (Math.PI / 180) * angle;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return new double[,]
                {
                    {1, 0, 0, 0 },
                    { 0, cos, -sin, 0 },
                    {0, sin,cos, 0 },
                    { 0, 0, 0, 1 }
                };
        }
        public static double[,] RotateZ(double angle)
        {
            angle = (Math.PI / 180) * angle;
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            return
                new double[,]
                {
                    { cos, -sin, 0, 0 },
                    { sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
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
