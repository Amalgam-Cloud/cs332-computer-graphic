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
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.graph = pictureBox1.CreateGraphics();
            this.p = new Pen(Color.Black);
            this.p.Width = 2;
            if (LineMode || PoligonMode || DotMode)
            {
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
                    }
                }
                else
                {
                    this.p2 = control.PointToClient(new Point(Cursor.Position.X, MousePosition.Y));
                    if(LineMode)
                    {
                        primals.Add(new Line(p1, p2));
                        counter++;
                        listBox1.Items.Insert(counter, "Line " + counter);
                    }
                    if(PoligonMode)
                    {
                        primals.Add(new Poligon(p1, p2));
                        counter++;
                        listBox1.Items.Insert(counter, "Polygon " + counter);
                    }
                    p1.X = 0;
                    p1.Y = 0;
                }
            }
            if (this.primals.Count != 0)
            {
                foreach (var elem in this.primals)
                {
                    elem.Draw(this.graph, this.p);
                }
            }
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
