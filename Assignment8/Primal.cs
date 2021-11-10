using AffineTransformationsIn3D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Assignment8
{
    public class Primal
    {
        public List<Dot> Dots { get; set; }
        public List<Line> Grani { get; set; }



        public Dot Center()
        {
            Dot center = new Dot();
            foreach (var v in Dots)
            {
                center.X += v.X;
                center.Y += v.Y;
                center.Z += v.Z;
            }
            center.X /= Dots.Count;
            center.Y /= Dots.Count;
            center.Z /= Dots.Count;
            return center;

        }

        public Primal(Tuple<List<Dot>, List<Line>> data) : this(data.Item1, data.Item2)
        {
        }
        public Primal()
        {

        }

        public Primal(List<Dot> vertices, List<Line> verges)
        {
            Dots = vertices;
            Grani = verges;
        }
        public virtual void Draw(Graphics graphics, double[,] transformation, double width, double height, Dot pos, Pen p)
        {
        }

        public void ApplyTo(double[,] transformation)
        {
            for (int i = 0; i < Dots.Count; ++i)
            {
                var container = new double[4] { Dots[i].X, Dots[i].Y, Dots[i].Z, Dots[i].W };
                double[] res = new double[4];
                for (int j = 0; j < 4; ++j)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        res[j] += container[k] * transformation[k, j];
                    }
                }
                Dots[i].X = res[0];
                Dots[i].Y = res[1];
                Dots[i].Z = res[2];
                Dots[i].W = res[3];
            }
        }
 
    }
    public class Dot
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }
        public Dot(Point dot)
        {
            this.X = dot.X;
            this.Y = dot.Y;
            this.Z = 0;
            this.W = 1;
        }
        public Dot(double X, double Y, double Z, double W = 1)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }
        public Dot()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.W = 0;
        }
        public static Dot operator *(Dot d, double[,] m2)
        {
            var container = new double[4] { d.X, d.Y, d.Z, d.W };
            double[] res = new double[4];
            for (int j = 0; j < 4; ++j)
            {
                for (int k = 0; k < 4; k++)
                {
                    res[j] += container[k] * m2[k, j];
                }
            }
            return new Dot(res[0], res[1], res[2], res[3]);
        }
        public static Dot operator *(double[,] m2, Dot d)
        {
            var container = new double[4] { d.X, d.Y, d.Z, 1 };
            double[] res = new double[4];
            for (int j = 0; j < 4; ++j)
            {
                for (int k = 0; k < 4; k++)
                {
                    res[j] += m2[k, j] * container[k];
                }
            }
            return new Dot(res[0], res[1], res[2], res[3]);
        }
        public static Dot operator *(double m2, Dot d)
        {
            return new Dot(d.X * m2, d.Y * m2, d.Z * m2, d.W * m2);
        }
        public static Dot operator *(Dot d, double m2)
        {
            return new Dot(d.X * m2, d.Y * m2, d.Z * m2, d.W * m2);
        }
        public static Dot VectorMult(Dot u, Dot v)
        {
            return new Dot(
                u.Y * v.Z - u.Y * v.Y,
                u.Z * v.X - u.X * v.Y,
                u.X * v.Y - u.Y * v.X);
        }
        public Point GetPoint()
        {
            return new Point((int)this.X, (int)this.Y);
        }
    }
    public class Line
    {
        public Dot pos1 { get; set; }
        public Dot pos2 { get; set; }
        public Line()
        {
            this.pos1 = new Dot();
            this.pos2 = new Dot();
        }
        public Line(Dot d)
        {
            this.pos1 = d;
            this.pos2 = d;
        }
        public Line(Dot d1, Dot d2)
        {
            this.pos1 = d1;
            this.pos2 = d2;
        }
        public void Draw(Graphics graphics, double[,] transformation, double width, double height, Dot pos, Pen p)
        {
            var t = SpaceToNormalized(this.pos1,transformation);
            if (t.Z < -1 || t.Z > 1) 
                return;
            var A = NormalizedToScreen(t,width,height);
            var u = SpaceToNormalized(this.pos2, transformation);
            if (u.Z < -1 || u.Z > 1) 
                return;
            var B = NormalizedToScreen(u,width,height);
            graphics.DrawLine(p, (float)A.X, (float)A.Y, (float)B.X, (float)B.Y);
        }
        public static Dot SpaceToNormalized(Dot d, double[,] Transformation)
        {
            return Normalize(d * Transformation);
        }
        public static  Dot Normalize(Dot d)
        {
            return new Dot(d.X / d.W, d.Y / d.W, d.Z / d.W);
        }
        public static Dot NormalizedToScreen(Dot v,double Width,double Height)
        {
            return new Dot(
                (v.X + 1) / 2 * Width,
                (-v.Y + 1) / 2 * Height,
                v.Z);
        }
    }
    public class Tetrahedron : Primal
    {
        public Tetrahedron(double size)
            : base(Construct(size))
        {
        }

        private static Tuple<List<Dot>, List<Line>> Construct(double size)
        {
            var vertices = new List<Dot>();
            var lines = new List<Line>();
            double h = Math.Sqrt(2.0 / 3.0) * size;
            vertices.Add(new Dot(-size / 2, 0, h / 3));
            vertices.Add(new Dot(0, 0, -h * 2 / 3));
            vertices.Add(new Dot(size / 2, 0, h / 3));
            vertices.Add(new Dot(0, h, 0));

            lines.Add(new Line(vertices[0], vertices[1]));
            lines.Add(new Line(vertices[0], vertices[2]));
            lines.Add(new Line(vertices[0], vertices[3]));

            lines.Add(new Line(vertices[1], vertices[2]));
            lines.Add(new Line(vertices[1], vertices[3]));

            lines.Add(new Line(vertices[2], vertices[3]));

            return new Tuple<List<Dot>, List<Line>>(vertices, lines);
        }
        public override void Draw(Graphics graphics, double[,] transformation, double width, double height, Dot pos, Pen p)
        {
            foreach (Line line in this.Grani)
            {
                line.Draw(graphics, transformation, width, height, pos, p);
                //graphics.DrawLine(line.pos1, line.pos2, new Pen(Color.Black));
            }
        }
    }
    class Layers
    {
        List<List<Dot>> layers { get; set; }
        public void Add_Point(Dot p)
        {
            //layers.FindIndex(p.Z
        }

    }
    class Graphic_Grid: Primal
    {
        List<List<Dot>> grid { get; set; }
        double X0 { get; set; }
        double X1 { get; set; }
        double Y0 { get; set; }
        double Y1 { get; set; }
        double step { get; set; }
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
        class Graphic_Line: Primal
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
                    this.lines.Add(new Line(this.dots.Last(), d));
                }
                this.dots.Add(d);
            }
            public override void Draw(Graphics graphics, double[,] transformation, double width, double height, Dot pos, Pen p)
            {
                foreach (var line in this.lines)
                {
                    line.Draw(graphics, transformation, width, height, pos, p);
                }
            }
            /*public void Rotate(int angle_X, int angle_Y, int angle_Z)
            {
                foreach (Dot d in this.dots)
                {
                    d.Rotate(angle_X, angle_Y, angle_Z);
                }
            }*/
        }
       
        public Graphic_Grid(double x0, double x1, double y0, double y1, double step, string func)
        {
            this.Dots = new List<Dot>();
            this.lines = new List<Graphic_Line>();
            this.X0 = x0;
            this.X1 = x1;
            this.Y0 = y0;
            this.Y1 = y1;
            this.step = step;
            this.Func = func;
            for (double i = x0; i < x1; i += step)
            {
                var l = new Graphic_Line();
                for (double j = y0; j < y1; j += step)
                {
                    var d = new Dot(j, F(i, j), i);
                    l.Add_Dot(d);
                    Dots.Add(d);

                }
                lines.Add(l);
            }
        }
        public override void Draw(Graphics graphics, double[,] transformation, double width, double height, Dot pos, Pen p)
        {
            foreach (var line in this.lines)
            {
                line.Draw(graphics, transformation,width, height, pos, p);
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
            this.Dots = new List<Dot>();
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
                        if (flag)
                        {
                            var pos = line.Split(' ');
                            var dott = new Dot(double.Parse(pos[0]), double.Parse(pos[1]), double.Parse(pos[2]));
                            l.Add_Dot(dott);
                            this.Dots.Add(dott);
                            flag = false;

                        }
                        if (line[0] == 'L')
                        {
                            if (!f_line)
                            {
                                this.lines.Add(l);
                            }
                            l = new Graphic_Line();
                            flag = false;
                            f_line = false;
                        }
                        if (line[0] == 'D')
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

        public int[][] GraniNew { get; set; }

        public virtual void Draw_without_colors(Graph3D graphics)
        {

            //foreach(var vertex in Vertices)
            //{
            //    graphics.DrawPoint(vertex, Color.Black);
            //}

            foreach (var verge in GraniNew)
            {
                Dot p1 = Dots[verge[0]];
                Dot p2 = Dots[verge[1]];
                Dot p3 = Dots[verge[2]];

                double[,] matrix = new double[2, 3];
                matrix[0, 0] = p2.X - p1.X;
                matrix[0, 1] = p2.Y - p1.Y;
                matrix[0, 2] = p2.Z - p1.Z;
                matrix[1, 0] = p3.X - p1.X;
                matrix[1, 1] = p3.Y - p1.Y;
                matrix[1, 2] = p3.Z - p1.Z;

                double ni = matrix[0, 1] * matrix[1, 2] - matrix[0, 2] * matrix[1, 1];
                double nj = matrix[0, 2] * matrix[1, 0] - matrix[0, 0] * matrix[1, 2];
                double nk = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                double d = -(ni * p1.X + nj * p1.Y + nk * p1.Z);

                Dot pp = new Dot(p1.X + ni, p1.Y + nj, p1.Z + nk);
                double val1 = ni * pp.X + nj * pp.Y + nk * pp.Z + d;
                double val2 = ni * Center().X + nj * Center().Y + nk * Center().Z + d;

                if (val1 * val2 > 0)
                {
                    ni = -ni;
                    nj = -nj;
                    nk = -nk;
                }

                if (ni * (-graphics.CamPosition.X) + nj * (-graphics.CamPosition.Y) + nk * (-graphics.CamPosition.Z) + ni * p1.X + nj * p1.Y + nk * p1.Z < 0)
                {
                    graphics.DrawPoint(Dots[verge[0]], Color.Black);
                    for (int i = 1; i < verge.Length; ++i)
                    {
                        graphics.DrawPoint(Dots[verge[i]], Color.Black);
                        graphics.DrawLine(Dots[verge[i - 1]], Dots[verge[i]]);
                    }
                    graphics.DrawLine(Vertices[verge[verge.Length - 1]], Vertices[verge[0]]);
                }
            }
        }
    }
}
