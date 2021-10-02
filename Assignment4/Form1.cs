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
                        primals.Add(new Dot(p1));
                        counter++;
                        listBox1.Items.Insert(counter, "Dot " + counter);
                        p1.X = 0;
                        p1.Y = 0;
                        IsAded = true;
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
    }
}
