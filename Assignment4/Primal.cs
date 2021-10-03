using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Assignment4
{
    class Primal
    {
        public virtual void Draw(Graphics g, Pen p) { }

    }
    class Dot:Primal
    {
        public Point pos { get; set; }
        public bool Visibility;
        public Dot(Point dot, bool Vis = true)
        {
            this.pos = dot;
            this.Visibility = Vis;
        }
        public override void Draw(Graphics g, Pen p)
        {
            if (!this.Visibility)
            {
                if (p.Color != Color.Black)
                {
                    Brush b = new SolidBrush(p.Color);
                    g.FillRectangle(b, this.pos.X-2, this.pos.Y-2, 5, 5);
                }
            }
            else
            {
                Brush b = new SolidBrush(p.Color);
                g.FillRectangle(b, this.pos.X - 3, this.pos.Y - 3, 3, 3);
            }
        }

    }
    class Line:Primal
    {
        public Dot pos1 { get; set; }
        public Dot pos2 { get; set; }
        public static Dot CrossingLines(Line l1, Line l2)
        {
            /*y = Ax + B*/
            float a1 = (float)(l1.pos1.pos.Y - l1.pos2.pos.Y) / (float)(l1.pos1.pos.X - l1.pos2.pos.X);
            float b1 = l1.pos1.pos.Y - a1 * l1.pos1.pos.X;
            float a2 = (float)(l2.pos1.pos.Y - l2.pos2.pos.Y) / (float)(l2.pos1.pos.X - l2.pos2.pos.X);
            float b2 = l2.pos1.pos.Y - a2 * l2.pos1.pos.X;
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
            float min_l1_x = Math.Min(l1.pos1.pos.X, l1.pos2.pos.X);
            float min_l1_y = Math.Min(l1.pos1.pos.Y, l1.pos2.pos.Y);
            float max_l1_x = Math.Max(l1.pos1.pos.X, l1.pos2.pos.X);
            float max_l1_y = Math.Max(l1.pos1.pos.Y, l1.pos2.pos.Y);
            float min_l2_x = Math.Min(l2.pos1.pos.X, l2.pos2.pos.X);
            float min_l2_y = Math.Min(l2.pos1.pos.Y, l2.pos2.pos.Y);
            float max_l2_x = Math.Max(l2.pos1.pos.X, l2.pos2.pos.X);
            float max_l2_y = Math.Max(l2.pos1.pos.Y, l2.pos2.pos.Y);
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
            return (this.pos2.pos.X - this.pos1.pos.X) * (d.pos.Y - this.pos1.pos.Y) - (this.pos2.pos.Y - this.pos1.pos.Y) * (d.pos.X - this.pos1.pos.X) > 0;
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
        public override void Draw(Graphics g,Pen p)
        {
            g.DrawLine(p,pos1.pos,pos2.pos);
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
        public override void Draw(Graphics g, Pen p)
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
}
