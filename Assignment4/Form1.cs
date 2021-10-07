using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment4
{
    public partial class Form1 : Form
    {
        List<Primal> primals = new List<Primal>();
        Primal curr_primal;
        int counter = -1;
        bool LineMode;
        bool PoligonMode;
        bool DotMode;
        Point p1;
        Point p2;
        Graphics graph;
        Pen p;
        public Form1()
        {
            InitializeComponent();
            this.graph = pictureBox1.CreateGraphics();
            this.p = new Pen(Color.Black);
            this.p.Width = 2;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (LineMode || PoligonMode || DotMode)
            {
                bool IsAded = false;
                Control control = (Control)sender;
                if (this.p1.X == 0 && this.p1.Y == 0)
                {
                    this.p1 = control.PointToClient(new Point(Cursor.Position.X, MousePosition.Y));
                    if (DotMode)
                    {
                        Dot d = new Dot(p1);
                        primals.Add(d);
                        counter++;
                        listBox1.Items.Insert(counter, "Dot " + counter);
                        p1.X = 0;
                        p1.Y = 0;
                        IsAded = true;
                        for(int i = 0; i<this.primals.Count; ++i)
                        {
                            if(this.primals[i] is Poligon && (this.primals[i] as Poligon).IsDotInside(d))
                            {
                                primals.Add(new Line((this.primals[i] as Poligon).downSide.pos1, d));
                                this.primals.Last().Draw(this.graph, this.p);
                                counter++;
                                listBox1.Items.Insert(counter, "Inside Line " + counter);
                                primals.Add(new Line((this.primals[i] as Poligon).leftSide.pos1, d));
                                this.primals.Last().Draw(this.graph, this.p);
                                counter++;
                                listBox1.Items.Insert(counter, "Inside Line " + counter);
                                primals.Add(new Line((this.primals[i] as Poligon).upSide.pos1, d));
                                this.primals.Last().Draw(this.graph, this.p);
                                counter++;
                                listBox1.Items.Insert(counter, "Inside Line " + counter);
                                primals.Add(new Line((this.primals[i] as Poligon).rightSide.pos1, d));
                                this.primals.Last().Draw(this.graph, this.p);
                                counter++;
                                listBox1.Items.Insert(counter, "Inside Line " + counter);
                            }
                        }
                    }
                }
                else
                {
                    this.p2 = control.PointToClient(new Point(Cursor.Position.X, MousePosition.Y));
                    if (LineMode)
                    {
                        Line n_line = new Line(p1, p2);
                        int bound = this.primals.Count;
                        for(int i = 0; i<bound; ++i)
                        {
                            if(this.primals[i] is Line)
                            {
                                Dot cross_dot = Line.CrossingLines(n_line,this.primals[i] as Line);
                                if (cross_dot.pos.X != 0 || cross_dot.pos.Y != 0)
                                {
                                    primals.Add(cross_dot);
                                    counter++;
                                    listBox1.Items.Insert(counter, "Cross Dot " + counter);
                                    IsAded = true;
                                }
                            }
                        }
                        primals.Add(n_line);
                        counter++;
                        listBox1.Items.Insert(counter, "Line " + counter);
                        IsAded = true;
                    }
                    if (PoligonMode)
                    {
                        primals.Add(new Poligon(p1, p2));
                        counter++;
                        listBox1.Items.Insert(counter, "Polygon " + counter);
                        IsAded = true;
                    }
                    p1.X = 0;
                    p1.Y = 0;
                }
                if (IsAded)
                {
                    this.primals.Last().Draw(this.graph, this.p);
                }
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.graph.Clear(pictureBox1.BackColor);
            for (int i = 0; i < this.primals.Count; ++i)
            {
                if (i == listBox1.SelectedIndex)
                {
                    this.primals[i].Draw(this.graph, new Pen(Color.Red, this.p.Width));
                }
                else
                {
                    this.primals[i].Draw(this.graph, this.p);
                }
            }
            curr_primal = this.primals[listBox1.SelectedIndex];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.primals.Clear();
            listBox1.Items.Clear();
            this.graph.Clear(pictureBox1.BackColor);
            counter = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DotMode = true;
            this.PoligonMode = false;
            this.LineMode = false;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.LineMode = true;
            this.PoligonMode = false;
            this.DotMode = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.PoligonMode = true;
            this.LineMode = false;
            this.DotMode = false;
        }





        private double[,] MatrMult(double[,] m1, double[,] m2)
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

        List<Point> newPrim_points = new List<Point>();
        private void CalculateLine(Line l)
        {     
            double[,] point1 = new double[,] { { l.pos1.pos.X, l.pos1.pos.Y, 1.0 } };
            double[,] point2 = new double[,] { { l.pos2.pos.X, l.pos2.pos.Y, 1.0 } };
            double[,] res1 = MatrMult(point1, transMatr);
            double[,] res2 = MatrMult(point2, transMatr);
            newPrim_points.Add(new Point(Convert.ToInt32(Math.Round(res1[0, 0])), Convert.ToInt32(Math.Round(res1[0, 1]))));
            newPrim_points.Add(new Point(Convert.ToInt32(Math.Round(res2[0, 0])), Convert.ToInt32(Math.Round(res2[0, 1])))); 
        }

        Point rotatePoint;
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            rotatePoint = new Point((e as MouseEventArgs).X, (e as MouseEventArgs).Y);
        }

        double[,] transMatr;

        private void button5_Click(object sender, EventArgs e)
        {
            this.PoligonMode = false;
            this.LineMode = false;
            this.DotMode = false;


            if (comboBox1.Text == "Offset")
            {

                double dX = System.Convert.ToDouble(textBox1.Text);
                double dY = System.Convert.ToDouble(textBox2.Text);
                transMatr = new double[,] { { 1.0, 0, 0 }, { 0, 1.0, 0 }, { dX, dY, 1.0 } };

            }
            else if (comboBox1.Text == "Rotation") {
                textBox1.Text = rotatePoint.X.ToString();
                textBox2.Text = rotatePoint.Y.ToString();

                double c = System.Convert.ToDouble(textBox1.Text);
                double d = System.Convert.ToDouble(textBox2.Text);
                double ang = System.Convert.ToDouble(textBox3.Text) * Math.PI / 180;
                double cos = Math.Cos(ang);
                double sin = Math.Sin(ang);
                transMatr = new double[,] {
                {cos, sin, 0},
                {-sin, cos, 0},
                {cos*(-c)+d*sin+c, (-c)*sin-d*cos+d, 1}
            };
            }
            else if (comboBox1.Text == "Scaling")
            {
                double k = System.Convert.ToDouble(textBox4.Text);
                Point centre;
                if (curr_primal is Line)
                {
                    centre = new Point(
                        Math.Abs((curr_primal as Line).pos1.pos.X + (curr_primal as Line).pos2.pos.X) / 2,
                        Math.Abs((curr_primal as Line).pos1.pos.Y + (curr_primal as Line).pos2.pos.Y) / 2);
                }
                else if(curr_primal is Poligon)
                {
                    centre = new Point(
                        Math.Abs((curr_primal as Poligon).leftSide.pos1.pos.X + (curr_primal as Poligon).rightSide.pos1.pos.X) / 2,
                        Math.Abs((curr_primal as Poligon).leftSide.pos2.pos.Y + (curr_primal as Poligon).rightSide.pos2.pos.Y) / 2);
                }
                else
                {
                    centre = new Point(0, 0);
                }
                transMatr = new double[,] { { k, 0, 0 }, { 0, k, 0 }, { (1 - k) * centre.X, (1 - k) * centre.Y, 1 } };
            }

            if (curr_primal is Dot)
            {
                
                List<Dot> newPrim = new List<Dot>();
                double[,] point = new double[,] { { (curr_primal as Dot).pos.X, (curr_primal as Dot).pos.Y, 1.0 } };
                double[,] res = MatrMult(point, transMatr);
                newPrim.Add(new Dot(new Point(Convert.ToInt32(Math.Round(res[0, 0])), Convert.ToInt32(Math.Round(res[0, 1]))), true));
                this.graph.Clear(pictureBox1.BackColor);
                newPrim.First().Draw(graph, p);
            }
            else if (curr_primal is Line)
            {
                this.graph.Clear(pictureBox1.BackColor);
                CalculateLine(curr_primal as Line);
                (this.curr_primal as Line).pos1 = new Dot(newPrim_points.First());
                (this.curr_primal as Line).pos2 = new Dot(newPrim_points.Last());
                newPrim_points.Clear();
                /*Line l = new Line(newPrim_points.First(), newPrim_points.Last());
                l.Draw(graph, p);*/
                
            }
            else if (curr_primal is Poligon)
            {
                /*Line l1 = (curr_primal as Poligon).upSide;
                Line l2 = (curr_primal as Poligon).downSide;
                Line l3 = (curr_primal as Poligon).leftSide;
                Line l4 = (curr_primal as Poligon).rightSide;
                List<Line> ll = new List<Line>() { l1, l2, l3, l4 };
                this.graph.Clear(pictureBox1.BackColor);
                foreach (Line l in ll)
                {
                    OffsetLine(l);
                    Line drawLine = new Line(newPrim_points.First(), newPrim_points.Last());
                    drawLine.Draw(graph, p);
                    newPrim_points.Clear();
                }*/
                this.graph.Clear(pictureBox1.BackColor);
                Line l1 = (curr_primal as Poligon).upSide;
                CalculateLine(l1 as Line);
                (l1 as Line).pos1 = new Dot(newPrim_points.First());
                (l1 as Line).pos2 = new Dot(newPrim_points.Last());
                newPrim_points.Clear();
                Line l2 = (curr_primal as Poligon).downSide;
                CalculateLine(l2 as Line);
                (l2 as Line).pos1 = new Dot(newPrim_points.First());
                (l2 as Line).pos2 = new Dot(newPrim_points.Last());
                newPrim_points.Clear();
                Line l3 = (curr_primal as Poligon).leftSide;
                CalculateLine(l3 as Line);
                (l3 as Line).pos1 = new Dot(newPrim_points.First());
                (l3 as Line).pos2 = new Dot(newPrim_points.Last());
                newPrim_points.Clear();
                Line l4 = (curr_primal as Poligon).rightSide;
                CalculateLine(l4 as Line);
                (l4 as Line).pos1 = new Dot(newPrim_points.First());
                (l4 as Line).pos2 = new Dot(newPrim_points.Last());
                newPrim_points.Clear();
            }
            foreach(var elem in this.primals)
            {
                elem.Draw(this.graph,this.p);
            }
        }


    }
}
