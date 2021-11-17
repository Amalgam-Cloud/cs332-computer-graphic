using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment8
{
    public partial class Form1 : Form
    {

        Primal current_primitive;
        private Camera camera;

        List<Primal> objects = new List<Primal>();
        bool moreThanOneObj = false;

        private static double ToRad(double deg)
        {
            return deg / 180 * Math.PI;
        }
        private void Scale()
        {
            double scalingX = (double)numericUpDown1.Value;
            double scalingY = (double)numericUpDown2.Value;
            double scalingZ = (double)numericUpDown3.Value;
            current_primitive.Apply(Transform.Scale(scalingX, scalingY, scalingZ));
            pictureBox1.Refresh();
        }

        private void Rotate()
        {
            double rotatingX = ToRad((double)numericUpDown4.Value);
            double rotatingY = ToRad((double)numericUpDown5.Value);
            double rotatingZ = ToRad((double)numericUpDown6.Value);
            current_primitive.Apply(Transform.RotateX(rotatingX)
                * Transform.RotateY(rotatingY)
                * Transform.RotateZ(rotatingZ));
            pictureBox1.Refresh();
        }

        private void Translate()
        {
            double translatingX = (double)numericUpDown7.Value;
            double translatingY = (double)numericUpDown8.Value;
            double translatingZ = (double)numericUpDown9.Value;
            current_primitive.Apply(Transform.Translate(translatingX, translatingY, translatingZ));
            pictureBox1.Refresh();
        }
        public Form1()
        {
            InitializeComponent();
            current_primitive = new Tetrahedron(1);
            Matrix projection = Transform.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
            camera = new Camera(new Vector(1, 1, 1), Math.PI / 4, -Math.PI / 4, projection);
            ProjectionComboBox.SelectedItem = ProjectionComboBox.Items[0];
            PrimitiveComboBox.SelectedItem = PrimitiveComboBox.Items[0];
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            double delta = 0.3;
            switch (keyData)
            {
                case Keys.W: camera.Position *= Transform.Translate(0.1 * camera.Forward); break;
                case Keys.A: camera.Position *= Transform.Translate(0.1 * camera.Left); break;
                case Keys.S: camera.Position *= Transform.Translate(0.1 * camera.Backward); break;
                case Keys.D: camera.Position *= Transform.Translate(0.1 * camera.Right); break;
                case Keys.Left: camera.Fi += delta; break;
                case Keys.Right: camera.Fi -= delta; break;
                case Keys.Up: camera.Theta += delta; break;
                case Keys.Down: camera.Theta -= delta; break;
            }
            pictureBox1.Refresh();
            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    current_primitive.Save(saveDialog.FileName);
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно сохранить файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Object Files(*.obj)|*.obj|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                current_primitive = new Primal(openDialog.FileName);
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyPrimitive_Click(object sender, EventArgs e)
        {
            moreThanOneObj = false;
            objects.Clear();
            switch (PrimitiveComboBox.SelectedItem.ToString())
            {
                case "Тетраэдр":
                    {
                        current_primitive = new Tetrahedron(1);
                        break;
                    }
                case "Гексаэдр":
                    {
                        current_primitive = new Hexahedron(1);
                        break;
                    }
                default:
                    {
                        current_primitive = new Tetrahedron(1);
                        break;
                    }
            }

            Matrix projection = Transform.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
            switch (ProjectionComboBox.SelectedItem.ToString())
            {
                case "Перспективная":
                    {
                        projection = Transform.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
                        camera = new Camera(new Vector(1, 1, 1), Math.PI / 4, -Math.PI / 4, projection);
                        break;
                    }
                case "Ортогональная XY":
                    {
                        camera = new Camera(new Vector(0, 0, 0), 0, 0, Transform.RotateX(Math.PI / 2) * Transform.OrthogonalProjection());
                        break;
                    }
                case "Ортогональная XZ":
                    {
                        camera = new Camera(new Vector(0, 0, 0), 0, 0, Transform.OrthogonalProjection());
                        break;
                    }
                case "Ортогональная YZ":
                    {
                        camera = new Camera(new Vector(0, 0, 0), 0, 0, Transform.RotateY(-Math.PI / 2) * Transform.OrthogonalProjection());
                        break;
                    }
                default:
                    {
                        projection = Transform.PerspectiveProjection(-0.1, 0.1, -0.1, 0.1, 0.1, 20);
                        break;
                    }
            }
            pictureBox1.Invalidate();
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.White);
            if (current_primitive == null)
                return;

            var graphics3D = new Graphic(e.Graphics, camera.ViewProjection, pictureBox1.Width, pictureBox1.Height, camera.Position);
            /*
            var x = new Vector(1, 0, 0);
            var y = new Vector(0, 1, 0);
            var z = new Vector(0, 0, 1);
            graphics3D.DrawLine(new Vector(0, 0, 0), x, new Pen(Color.Red, 2));
            graphics3D.DrawPoint(x, Color.Red);
            graphics3D.DrawLine(new Vector(0, 0, 0), y, new Pen(Color.Green, 2));
            graphics3D.DrawPoint(y, Color.Green);
            graphics3D.DrawLine(new Vector(0, 0, 0), z, new Pen(Color.Blue, 2));
            graphics3D.DrawPoint(z, Color.Blue);*/

            if (moreThanOneObj)
            {
                foreach (var obj in objects)
                    obj.Draw(graphics3D);
            }
            else
                current_primitive.Draw(graphics3D);

            e.Graphics.DrawImage(graphics3D.ColorBuffer, 0, 0);

        }

        private void ApplyAffin_Click(object sender, EventArgs e)
        {
            Scale();
            Rotate();
            Translate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            moreThanOneObj = true;
            current_primitive = new Hexahedron(1);
            current_primitive.Apply(Transform.Translate(0, 0.20, -0.40));
            objects.Add(current_primitive);
            current_primitive = new Tetrahedron(1);
            current_primitive.Apply(Transform.Translate(0, 0, 0.80));
            objects.Add(current_primitive);
            pictureBox1.Invalidate();
        }
    }
}
