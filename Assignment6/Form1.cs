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
            comboBox3.Items.Add("OX");
            comboBox3.Items.Add("OY");
            comboBox3.Items.Add("OZ");
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

        public void Offset()
        {
            float X, Y, Z;
            try
            {
                X = (float)Convert.ToDouble(textBox1.Text);
                Y = (float)Convert.ToDouble(textBox2.Text);
                Z = (float)Convert.ToDouble(textBox3.Text);
            }
            catch
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
            cur_primal.CalcNew(Transformations.Translate(X, Y, Z));
        }

        public void Rotate()
        {
            float X, Y, Z;
            try
            {
                X = (float)Convert.ToDouble(textBox4.Text);
                Y = (float)Convert.ToDouble(textBox5.Text);
                Z = (float)Convert.ToDouble(textBox6.Text);
            }
            catch
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
            cur_primal.CalcNew(Transformations.RotateX(X) * Transformations.RotateY(Y) * Transformations.RotateZ(Z));
        }

        public void Scale()
        {
            float X, Y, Z;
            try
            {
                X = (float)Convert.ToDouble(textBox7.Text);
                Y = (float)Convert.ToDouble(textBox8.Text);
                Z = (float)Convert.ToDouble(textBox9.Text);
            }
            catch
            {
                X = 1;
                Y = 1;
                Z = 1;
            }
            cur_primal.CalcNew(Transformations.Scale(X,Y,Z));
        }

        private void Reflect()
        {
            switch (comboBox3.SelectedItem.ToString())
            {
                case "OX":
                    {
                        cur_primal.CalcNew(Transformations.ReflectX());
                        break;
                    }
                case "OY":
                    {
                        cur_primal.CalcNew(Transformations.ReflectY());
                        break;
                    }
                case "OZ":
                    {
                        cur_primal.CalcNew(Transformations.ReflectZ());
                        break;
                    }
                default:
                    {
                        cur_primal.CalcNew(Transformations.ReflectX());
                        break;
                    }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            Offset();
            Rotate();
            Scale();
            Dot center = new Dot(0, 0, 0);
            Line OX = new Line(center, new Dot(200, 0, 0));
            Line OY = new Line(center, new Dot(0, -200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            OX.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OY.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            cur_primal.Draw(this.graph, this.p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            float D;
            try
            {
                D = (float)Convert.ToDouble(numericUpDown1.Value)/10;
            }
            catch
            {
                D = 0;
            }
            button3_Click(sender, e);
            cur_primal.CalcNew(Transformations.Scale(D, D, D));
            Dot center = new Dot(0, 0, 0);
            Line OX = new Line(center, new Dot(200, 0, 0));
            Line OY = new Line(center, new Dot(0, -200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            OX.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OY.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);

            cur_primal.Draw(this.graph, this.p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            Reflect();
            Dot center = new Dot(0, 0, 0);
            Line OX = new Line(center, new Dot(200, 0, 0));
            Line OY = new Line(center, new Dot(0, -200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            OX.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OY.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            cur_primal.Draw(this.graph, this.p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            double X, Y, Z;
            try
            {
                X = (float)numericUpDown2.Value / 180 * Math.PI;
                Y = (float)numericUpDown3.Value / 180 * Math.PI;
                Z = (float)numericUpDown4.Value / 180 * Math.PI;
            }
            catch
            {
                X = 0;
                Y = 0;
                Z = 0;
            }
            cur_primal.CalcNew(Transformations.RotateX(X) * Transformations.RotateY(Y) * Transformations.RotateZ(Z));
            Dot center = new Dot(0, 0, 0);
            Line OX = new Line(center, new Dot(200, 0, 0));
            Line OY = new Line(center, new Dot(0, -200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            OX.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OY.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            cur_primal.Draw(this.graph, this.p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            int X1 = (int)numericUpDown5.Value;
            int Y1 = (int)numericUpDown7.Value;
            int Z1 = (int)numericUpDown9.Value;

            int X2 = (int)numericUpDown6.Value;
            int Y2 = (int)numericUpDown8.Value;
            int Z2 = (int)numericUpDown10.Value;

            Line l = new Line(new Dot(X1, Y1, Z1), new Dot(X2, Y2, Z2));

            double ang = (double)numericUpDown11.Value / 180 * Math.PI;

            cur_primal.CalcNew(Transformations.RotateLine(l, ang));
            Dot center = new Dot(0, 0, 0);
            Line OX = new Line(center, new Dot(200, 0, 0));
            Line OY = new Line(center, new Dot(0, -200, 0));
            Line OZ = new Line(center, new Dot(0, 0, 200));
            OX.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OY.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            OZ.Draw(graph, p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
            cur_primal.Draw(this.graph, this.p, comboBox1.SelectedItem.ToString(), pictureBox1.Height, pictureBox1.Width);
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)| *.obj | Text files(*.txt) | *.txt | All files(*.*) | *.* ";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string info = "";
                    info += cur_primal.ToString() + "\r\n";

                    Dot[] ps = cur_primal.Points();
                    List<Poligon> vs = cur_primal.Verges();


                    foreach (Dot d in ps)
                    {
                        info += "\r\n";
                        info += d.X + " ";
                        info += d.Y + " ";
                        info += d.Z;
                    }
                    info += "\r\n";

                    foreach (Poligon p in vs)
                    {
                        info += "\r\n";
                        for (int i = 0; i < p.d.Count; ++i)
                        {
                            info += p.d[i].X + " " + p.d[i].Y + " " + p.d[i].Z;
                            info += "\r\n";
                        }

                    }
                    
                    System.IO.File.WriteAllText(saveDialog.FileName, info);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

    }
}
