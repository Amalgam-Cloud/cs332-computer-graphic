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
        bool without_colors = false;
        private Camera camera;

        List<Primal> objects = new List<Primal>();
        bool moreThanOneObj = false;

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

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {

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

            var x = new Vector(1, 0, 0);
            var y = new Vector(0, 1, 0);
            var z = new Vector(0, 0, 1);
            graphics3D.DrawLine(new Vector(0, 0, 0), x, new Pen(Color.Red, 2));
            graphics3D.DrawPoint(x, Color.Red);
            graphics3D.DrawLine(new Vector(0, 0, 0), y, new Pen(Color.Green, 2));
            graphics3D.DrawPoint(y, Color.Green);
            graphics3D.DrawLine(new Vector(0, 0, 0), z, new Pen(Color.Blue, 2));
            graphics3D.DrawPoint(z, Color.Blue);

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

        }
    }
}
