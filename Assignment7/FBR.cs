using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Assignment7
{
    class FBR 
    {
        private List<Dot> points = new List<Dot>();

        private List<Line> lines = new List<Line>();

        public List<Dot> Points { get { return points; } }
        public List<Line> Lines { get { return lines; } }

        public RotationFigure(List<Dot> d, List<Line> l, int count_start_points)
        {
            points = d;
            lines = l;
        }

        public RotationFigure(List<Dot> p, int axis, int density)
        {
            if (axis < 0 || axis > 2)
                return;

            points.AddRange(p);
            List<Dot> rotatedPoints = new List<Dot>();
            Func<double, Transformations> rotation;

            switch (axis)
            {
                case 0: rotation = Transformations.RotateX; break;
                case 1: rotation = Transformations.RotateY; break;
                default: rotation = Transformations.RotateZ; break;
            }

            for (int i = 1; i < density - 1; ++i)
            {
                double angle = 2 * Math.PI * i / (density - 1);
                foreach (var point in p)
                    rotatedPoints.Add(point.Transform(rotation(angle)));
                points.AddRange(rotatedPoints);
                rotatedPoints.Clear();
            }
            var n = p.Count;
            for (int i = 0; i < density - 1; ++i)
                for (int j = 0; j < n - 1; ++j)
                    lines.Add(new Line(new List<Dot> {
                        points[i * n + j], points[(i + 1) % (density - 1) * n + j],
                        points[(i + 1) % (density - 1) * n + j + 1], points[i * n + j + 1] }));
        }

        public void CalcNew(Transformations t)
        {
            foreach (var line in Lines)
                    line.CalcNew(t);
        }

        public void Draw(Graphics g,Pen p, string projection, int width, int height)
        {
            foreach (var Verge in Lines)
                Verge.Draw(g,p, projection, width, height);
        }

        override public string ToString()
        {
            return "Rotation Figure";
        }
    }
}
