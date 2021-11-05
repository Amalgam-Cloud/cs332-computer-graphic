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

            OpenFileDialog loadDialog = new OpenFileDialog();
            loadDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (loadDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    button3_Click(sender, e);
                    Dot[] ps1 = new Dot[8];
                    Dot[] ps2 = new Dot[4];
                    Dot[] ps3 = new Dot[6];
                    List<Poligon> verges = new List<Poligon>();

                    string str = System.IO.File.ReadAllText(loadDialog.FileName).Replace("\r\n", "!");
                    string[] info = str.Split('!');

                    string type = info[0].Replace("Assignment6.", "");
                    
                    if (type == "Hexahedron")
                    {
                        int i = 2; 
                        while (i < 10)
                        {
                            Console.WriteLine(info[i]);
                            string[] coordinates = info[i].Split(' ');

                            float x = (float)double.Parse(coordinates[0]);
                            float y = (float)double.Parse(coordinates[1]);
                            float z = (float)double.Parse(coordinates[2]);
                            ps1[i-2]= new Dot(x, y, z);
                            ++i;
                        }
                        type = "Гексэдр";
                    }
                    if(type == "Tetradedron")
                    {
                        int i = 2;
                        while (i < 6)
                        {
                            Console.WriteLine(info[i]);
                            string[] coordinates = info[i].Split(' ');

                            float x = (float)double.Parse(coordinates[0]);
                            float y = (float)double.Parse(coordinates[1]);
                            float z = (float)double.Parse(coordinates[2]);
                            ps2[i - 2] = new Dot(x, y, z);
                            ++i;
                        }
                        type = "Тетраэдр";
                    }
                    if (type == "Octaedron")
                    {
                        int i = 2;
                        while (i < 8)
                        {
                            Console.WriteLine(info[i]);
                            string[] coordinates = info[i].Split(' ');

                            float x = (float)double.Parse(coordinates[0]);
                            float y = (float)double.Parse(coordinates[1]);
                            float z = (float)double.Parse(coordinates[2]);
                            ps3[i - 2] = new Dot(x, y, z);
                            ++i;
                        }
                        type = "Октаэдр";
                    }
                   
                    if (type == "Гексэдр")
                    {
                        cur_primal = new Hexahedron(ps1[0], ps1[1], ps1[2], ps1[3], ps1[4], ps1[5], ps1[6], ps1[7]);
                    }
                    if (type == "Тетраэдр")
                    {
                        cur_primal = new Tetradedron(ps2[0], ps2[1], ps2[2], ps2[3]);
                    }
                    if (type == "Октаэдр")
                    {
                        cur_primal = new Octaedron(ps3[0], ps3[1], ps3[2], ps3[3], ps3[4], ps3[5]);
                    }
                    Dot center = new Dot(0, 0, 0);
                    Line OX = new Line(center, new Dot(200, 0, 0));
                    Line OY = new Line(center, new Dot(0, -200, 0));
                    Line OZ = new Line(center, new Dot(0, 0, 200));
                    OX.Draw(graph, p, "Аксонометрическая(Изометрическая)", pictureBox1.Height, pictureBox1.Width);
                    OY.Draw(graph, p, "Аксонометрическая(Изометрическая)", pictureBox1.Height, pictureBox1.Width);
                    OZ.Draw(graph, p, "Аксонометрическая(Изометрическая)", pictureBox1.Height, pictureBox1.Width);
                    cur_primal.Draw(this.graph, this.p, "Аксонометрическая(Изометрическая)", pictureBox1.Height, pictureBox1.Width);
                    
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

    }
}
