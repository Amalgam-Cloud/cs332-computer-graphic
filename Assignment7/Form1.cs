using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment7
{
    public partial class Form1 : Form
    {
        Primal cur_primal;
        Graphics graph;
        Pen p;

        public Form1()
        {
            InitializeComponent();
            this.graph = pictureBox1.CreateGraphics();
            this.p = new Pen(Color.Black);
            this.p.Width = 2;
            comboBox2.Items.Add("Аксонометрическая(Изометрическая)");
            comboBox2.Items.Add("Перспективная");
            comboBox1.Items.Add("(x * x) + (y * y)");
            comboBox1.Items.Add("(x * x) - (y * y)");
            comboBox1.Items.Add("x + y");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.graph.Clear(Color.White);
            Dot center = new Dot(0,0, 0);
            Line OX = new Line(center, new Dot(200, 0,0));
            Line OY = new Line(center, new Dot(0, 200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            Pen p1 = new Pen(Color.Red);
            Pen p2 = new Pen(Color.Blue);
            Pen p3 = new Pen(Color.Green);
            OX.Draw(graph, p1, comboBox2.SelectedItem.ToString(), pictureBox1.Height,pictureBox1.Width);
            OY.Draw(graph, p3, comboBox2.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p2, comboBox2.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            this.cur_primal = new Graphic_Grid((double)numericUpDown2.Value*200, (double)numericUpDown1.Value*150, (double)numericUpDown3.Value*150, (double)numericUpDown4.Value*150, (double)numericUpDown5.Value*150, comboBox1.Text);
            this.cur_primal.Draw(this.graph, this.p, comboBox2.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);


        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.graph.Clear(Color.White);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            (this.cur_primal as Graphic_Grid).Save();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            (this.cur_primal as Graphic_Grid).Load();
            this.graph.Clear(Color.White);
            Dot center = new Dot(0, 0, 0);
            Line OX = new Line(center, new Dot(200, 0, 0));
            Line OY = new Line(center, new Dot(0, 200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            Pen p1 = new Pen(Color.Red);
            Pen p2 = new Pen(Color.Blue);
            Pen p3 = new Pen(Color.Green);
            OX.Draw(graph, p1, comboBox2.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OY.Draw(graph, p3, comboBox2.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p2, comboBox2.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            this.cur_primal.Draw(this.graph, this.p, comboBox2.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);

        }
    }
}
