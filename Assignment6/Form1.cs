using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment6
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
            comboBox1.Items.Add("Аксонометрическая(Изометрическая)");
            comboBox1.Items.Add("Перспективная");
            comboBox2.Items.Add("Тетраэдр");
            comboBox2.Items.Add("Октаэдр");
            comboBox2.Items.Add("Гексэдр");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dot center = new Dot(0,0, 0);
            Line OX = new Line(center, new Dot(200, 0,0));
            Line OY = new Line(center, new Dot(0, -200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            OX.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height,pictureBox1.Width);
            OY.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            this.cur_primal.Draw(this.graph, this.p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);


        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.graph.Clear(Color.White);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedItem.ToString())
            {
                case "Тетраэдр":
                    this.cur_primal = new Tetradedron(100);
                    break;
                case "Октаэдр":
                    this.cur_primal = new Octaedron(100);
                    break;
                case "Гексэдр":
                    this.cur_primal = new Hexahedron(100);
                    break;

            }
        }
    }
}
