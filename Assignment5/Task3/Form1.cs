using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Assignment5
{
    public partial class Form1 : Form
    {
        Spline curr_spline;
        Graphics graph;
        Pen p;
        List<Primal> primals = new List<Primal>();
        Primal curr_primal;
        int counter = -1;
        bool IsChoosed = false;
        int id = -1;
        public Form1()
        {
            InitializeComponent();
            this.curr_spline = new Spline();
            this.graph = pictureBox1.CreateGraphics();
            this.p = new Pen(Color.Black);
            this.p.Width = 2;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            if (((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Left && this.IsChoosed)
            {

            }
            if (((MouseEventArgs)e).Button == System.Windows.Forms.MouseButtons.Right && this.IsChoosed)
            {
                (this.curr_primal as Dot).Pos = control.PointToClient(new Point(Cursor.Position.X, MousePosition.Y));
                pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width, pictureBox1.Height));
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Dot d = new Dot(control.PointToClient(new Point(Cursor.Position.X, MousePosition.Y)));
            this.curr_spline.AddPoint(d);
            this.primals.Add(d);
            counter++;
            listBox1.Items.Insert(counter, "Poligon " + counter);
            pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width,pictureBox1.Height));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                this.IsChoosed = true;
                this.graph.Clear(pictureBox1.BackColor);
                Primal buffer = null;
                for (int i = 0; i < this.primals.Count; ++i)
                {
                    if (i == listBox1.SelectedIndex)
                    {
                        buffer = this.primals[i];
                    }
                    else
                    {
                        this.primals[i].Draw(this.graph, this.p);
                    }
                }
                this.curr_primal = this.primals[listBox1.SelectedIndex];
                pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width, pictureBox1.Height));
                buffer.Draw(this.graph, new Pen(Color.Red, this.p.Width));
            }
            else 
            {
                this.counter = -1;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            this.id = listBox1.SelectedIndex;
            listBox1.Items.RemoveAt(this.id);
            this.curr_spline.dots.RemoveAt(id);
            pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width, pictureBox1.Height));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.primals.Clear();
            this.curr_primal = null;
            this.curr_spline = new Spline();
            listBox1.Items.Clear();
            this.graph.Clear(pictureBox1.BackColor);
            counter = -1;
            this.IsChoosed = false;
        }
    }
}
