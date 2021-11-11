using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment8
{
    class Camera
    {
        public Vector Position { get; set; }
        public double Fi { get; set; }
        public double Theta { get; set; }
        public Matrix Projection { get; set; }

        public Vector Forward { get { return new Vector(-Math.Sin(Fi), Math.Sin(Theta), -Math.Cos(Fi)); } }
        public Vector Left { get { return new Vector(-Math.Sin(Fi + Math.PI / 2), 0, -Math.Cos(Fi + Math.PI / 2)); } }
        public Vector Up { get { return Vector.CrossProduct(Forward, Left); } }
        public Vector Right { get { return -Left; } }
        public Vector Backward { get { return -Forward; } }
        public Vector Down { get { return -Up; } }

        public Matrix ViewProjection
        {
            get
            {
                return Transform.Translate(-Position)
                    * Transform.RotateY(-Fi)
                    * Transform.RotateX(-Theta)
                    * Projection;
            }
        }

        public Camera(Vector position, double angleY, double angleX, Matrix projection)
        {
            Position = position;
            Fi = angleY;
            Theta = angleX;
            Projection = projection;
        }
    }
}
