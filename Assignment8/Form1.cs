using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Math;
using System.IO;


namespace Assignment8
{
    public partial class Form1 : Form
    {
        Primal current_primitive;
        bool without_colors = false;
        private Camera camera;

        List<Primal> objects = new List<Primal>();
        bool moreThanOneObj = false;
        int p1 = 0;
        int p2 = 0;
        Graphics graph;

        public Form1()
        {
            InitializeComponent();
            current_primitive = new Tetrahedron(1);
            double[,] projection = Transformer.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
            camera = new Camera(new Dot(1, 1, 1), Math.PI / 4, -Math.PI / 4, projection);
            graph = pictureBox1.CreateGraphics();
            ProjectionComboBox.SelectedItem = ProjectionComboBox.Items[0];
            PrimitiveComboBox.SelectedItem = PrimitiveComboBox.Items[0];
            
        }

        private void Scale()
        {
            double scalingX = (double)numericUpDown1.Value;
            double scalingY = (double)numericUpDown2.Value;
            double scalingZ = (double)numericUpDown3.Value;
            current_primitive.ApplyTo(Transformer.Scale(scalingX, scalingY, scalingZ));
            pictureBox1.Refresh();
        }
        private void Rotate()
        {
            double rotatingX = ((double)numericUpDown4.Value) / 180 * Math.PI;
            double rotatingY = ((double)numericUpDown5.Value) / 180 * Math.PI;
            double rotatingZ = ((double)numericUpDown6.Value) / 180 * Math.PI;
            current_primitive.ApplyTo(Transformer.MatrMult(Transformer.MatrMult(Transformer.RotateX(rotatingX)
                ,Transformer.RotateY(rotatingY)),
                Transformer.RotateZ(rotatingZ)));
            pictureBox1.Refresh();
        }
        private void Translate()
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            current_primitive.ApplyTo(Transformer.Translate(translatingX, translatingY, translatingZ));
            pictureBox1.Refresh();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
      {
                double delta = 0.3;
                switch (keyData)
                {
                    case Keys.W: camera.pos *= Transformer.Translate(0.1 * camera.Forward()); break;
                    case Keys.A: camera.pos *= Transformer.Translate(0.1 * camera.Left()); break;
                    case Keys.S: camera.pos *= Transformer.Translate(0.1 * camera.Back()); break;
                    case Keys.D: camera.pos *= Transformer.Translate(0.1 * camera.Right()); break;
                    case Keys.Left: camera.phi += delta; break;
                    case Keys.Right: camera.phi -= delta; break;
                    case Keys.Up: camera.theta += delta; break;
                    case Keys.Down: camera.theta -= delta; break;
                }
                pictureBox1.Refresh();
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ApplyAffin_Click(object sender, EventArgs e)
        {
            Scale();
            Rotate();
            Translate();
            pictureBox1.Invalidate();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            (current_primitive as Graphic_Grid).Save();
            /*SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //current_primitive.Save(saveDialog.FileName);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }*/
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            (this.current_primitive as Graphic_Grid).Load();
            pictureBox1.Refresh();
        }
        private void ApplyPrimitive_Click(object sender, EventArgs e)
        {
            moreThanOneObj = false;
            without_colors = false;
            objects.Clear();
            switch (PrimitiveComboBox.SelectedItem.ToString())
            {
                case "Тетраэдр":
                    {
                        current_primitive = new Tetrahedron(1);
                        break;
                    }
                case "(x * x) + (y * y)":
                    {
                        current_primitive = new Graphic_Grid(-1, 1, -1, 1, 0.05, "(x * x) + (y * y)");
                        break;
                    }
                case "x + y":
                    {
                        current_primitive = new Graphic_Grid(-1,1, -1, 1, 0.05, "x + y");
                        break;
                    }
                case "| (x * x) - (y * y) | ":
                    {
                        current_primitive = new Graphic_Grid(-1, 1, -1, 1, 0.05, "| (x * x) - (y * y) | ");
                        break;
                    }
                default:
                    {
                        current_primitive = new Tetrahedron(1);
                        break;
                    }
            }

            double[,] projection = Transformer.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
            switch (ProjectionComboBox.SelectedItem.ToString())
            {
                case "Перспективная":
                    {
                        projection = Transformer.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
                        camera = new Camera(new Dot(1, 1, 1), Math.PI / 4, -Math.PI / 4, projection);
                        break;
                    }
                case "Ортогональная XY":
                    {
                        camera = new Camera(new Dot(0, 0, 0), 0, 0, Transformer.MatrMult(Transformer.RotateX(Math.PI / 2),Transformer.OrthogonalProjection()));
                        break;
                    }
                case "Ортогональная XZ":
                    {
                        camera = new Camera(new Dot(0, 0, 0), 0, 0, Transformer.OrthogonalProjection());
                        break;
                    }
                case "Ортогональная YZ":
                    {
                        camera = new Camera(new Dot(0, 0, 0), 0, 0, Transformer.MatrMult(Transformer.RotateY(-Math.PI / 2),Transformer.OrthogonalProjection()));
                        break;
                    }
                default:
                    {
                        projection = Transformer.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
                        break;
                    }
            }
            pictureBox1.Invalidate();
        }
        private void DrawWithoutColors_Click(object sender, EventArgs e)
        {
            double vi =1.0;
            double vj = 1.0;
            double vk = 1.0;
            without_colors = true;
            pictureBox1.Refresh();
            pictureBox1.Invalidate();
        }
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
                if (current_primitive == null)
                    return;
                var OX = new Line(new Dot(0,0,0),new Dot(1,0,0));
                var OY = new Line(new Dot(0, 0, 0), new Dot(0, 1, 0));
                var OZ = new Line(new Dot(0, 0, 0), new Dot(0, 0, 1));
                OX.Draw(e.Graphics, camera.TrueProjection(), pictureBox1.Width, pictureBox1.Height, camera.pos, new Pen(Color.Red,3));
                OY.Draw(e.Graphics, camera.TrueProjection(), pictureBox1.Width, pictureBox1.Height, camera.pos, new Pen(Color.Green,3));
                OZ.Draw(e.Graphics, camera.TrueProjection(), pictureBox1.Width, pictureBox1.Height, camera.pos, new Pen(Color.Blue,3));
                current_primitive.Draw(e.Graphics, camera.TrueProjection(), pictureBox1.Width, pictureBox1.Height, camera.pos,new Pen(Color.Black,2));

        }
    }
}
