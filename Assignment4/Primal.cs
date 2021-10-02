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
        public Dot(Point dot)
        {
            this.pos = dot;
        }
        public override void Draw(Graphics g, Pen p)
        {
            Brush b = new SolidBrush(Color.Black);
            g.FillRectangle(b, this.pos.X-3,this.pos.Y-3,3,3);
        }

    }
    class Line:Primal
    {
        Dot pos1 { get; set; }
        Dot pos2 { get; set; }
        public Line(Point p1,Point p2)
        {
            this.pos1 = new Dot(p1);
            this.pos2 = new Dot(p2);
        }
        public override void Draw(Graphics g,Pen p)
        {
            g.DrawLine(p,pos1.pos,pos2.pos);
        }
    }
    class Poligon : Primal
    {
        Line upSide { get; set; }
        Line downSide { get; set; }
        Line leftSide { get; set; }
        Line rightSide { get; set; }
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
    }
}
