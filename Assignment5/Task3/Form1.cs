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
                Dot old_pos = new Dot((this.curr_primal as Dot).Pos);
                (this.curr_primal as Dot).Pos = ((MouseEventArgs)e).Location;
                var x_move = old_pos.Pos.X - (this.curr_primal as Dot).Pos.X;
                var y_move = old_pos.Pos.Y - (this.curr_primal as Dot).Pos.Y;
                if ((this.curr_primal as Dot).suprim == null)
                {
                    var b1_x = (this.curr_primal as Dot).buddy.Pos.X - x_move;
                    var b1_y = (this.curr_primal as Dot).buddy.Pos.Y - y_move;
                    var b2_x = (this.curr_primal as Dot).buddy.buddy.Pos.X - x_move;
                    var b2_y = (this.curr_primal as Dot).buddy.buddy.Pos.Y - y_move;
                    (this.curr_primal as Dot).buddy.Pos = new Point(b1_x,b1_y);
                    (this.curr_primal as Dot).buddy.buddy.Pos = new Point(b2_x,b2_y);
                }
                else
                {
                    var b1_x = (this.curr_primal as Dot).buddy.Pos.X + x_move;
                    var b1_y = (this.curr_primal as Dot).buddy.Pos.Y + y_move;
                    (this.curr_primal as Dot).buddy.Pos = new Point(b1_x, b1_y);
                }
                pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width, pictureBox1.Height),listBox1,this.primals);
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            Dot piv_p = new Dot(((MouseEventArgs)e).Location);
            Dot add_point_1 = new Dot(new Point(piv_p.Pos.X-20, piv_p.Pos.Y-20));
            Dot add_point_2 = new Dot(new Point(piv_p.Pos.X + 20, piv_p.Pos.Y + 20));
            add_point_1.buddy = add_point_2;
            add_point_1.suprim = piv_p;
            add_point_2.suprim = piv_p;
            add_point_2.buddy = add_point_1;
            piv_p.buddy = add_point_1;
            this.curr_spline.AddPoint(piv_p);
            this.primals.Add(piv_p);
            counter++;
            listBox1.Items.Insert(counter, "Pivot_Point " + counter);
            this.curr_spline.AddPoint(add_point_1);
            this.primals.Add(add_point_1);
            counter++;
            listBox1.Items.Insert(counter, "Additional_Point " + counter);
            this.curr_spline.AddPoint(add_point_2);
            this.primals.Add(add_point_2);
            counter++;
            listBox1.Items.Insert(counter, "Additional_Point " + counter);
            pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width,pictureBox1.Height),listBox1,this.primals);
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
                pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width, pictureBox1.Height),listBox1,this.primals);
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
            pictureBox1.Image = curr_spline.DrawBit(this.graph, p, new Bitmap(pictureBox1.Width, pictureBox1.Height),listBox1,this.primals);
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
