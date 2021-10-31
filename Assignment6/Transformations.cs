using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment6
{
    class Transformations
    {
        private float[,] matrix = new float[4, 4];

        public float[,] Matrix { get { return matrix; } }

        public Transformations()
        {
            matrix = Identity().matrix;
        }

        public Transformations(float[,] matrix)
        {
            this.matrix = matrix;
        }

        public static Transformations RotateX(double angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Transformations(
                new float[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, cos, -sin, 0 },
                    { 0, sin, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformations RotateY(double angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Transformations(
                new float[,]
                {
                    { cos, 0, sin, 0 },
                    { 0, 1, 0, 0 },
                    { -sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformations RotateZ(double angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Transformations(
                new float[,]
                {
                    { cos, -sin, 0, 0 },
                    { sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformations RotateLine(Line line, double angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            float l = Math.Sign(line.pos2.X - line.pos1.X);
            float m = Math.Sign(line.pos2.Y - line.pos1.Y);
            float n = Math.Sign(line.pos2.Z - line.pos1.Z);
            float length = (float)Math.Sqrt(l * l + m * m + n * n);
            l /= length; m /= length; n /= length;
            return new Transformations(
                new float[,]
                {
                   { l * l + cos * (1 - l * l),   l * (1 - cos) * m + n * sin,   l * (1 - cos) * n - m * sin,  0  },
                   { l * (1 - cos) * m - n * sin,   m * m + cos * (1 - m * m),    m * (1 - cos) * n + l * sin,  0 },
                   { l * (1 - cos) * n + m * sin,  m * (1 - cos) * n - l * sin,    n * n + cos * (1 - n * n),   0 },
                   {            0,                            0,                             0,               1   }
                });

        }

        public static Transformations Scale(double fx, double fy, double fz)
        {
            return new Transformations(
                new float[,] {
                    { (float)fx, 0, 0, 0 },
                    { 0, (float)fy, 0, 0 },
                    { 0, 0, (float)fz, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformations Translate(double dx, double dy, double dz)
        {
            return new Transformations(
                new float[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { (float)dx, (float)dy, (float)dz, 1 },
                });
        }

        public static Transformations Identity()
        {
            return new Transformations(
                new float[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }


        public static Transformations ReflectX()
        {
            return Scale(-1, 1, 1);
        }

        public static Transformations ReflectY()
        {
            return Scale(1, -1, 1);
        }

        public static Transformations ReflectZ()
        {
            return Scale(1, 1, -1);
        }

        /*public static Transformations OrthographicXYProjection()
        {
            return Identity();
        }

        public static Transform OrthographicXZProjection()
        {
            return Identity() * RotateX(-Math.PI / 2);
        }

        public static Transform OrthographicYZProjection()
        {
            return Identity() * RotateY(Math.PI / 2);
        }

        public static Transform IsometricProjection()
        {
            return Identity() * RotateY(Math.PI / 4) * RotateX(-Math.PI / 4);
        }*/

        public static Transformations PerspectiveProjection()
        {
            return new Transformations(
                new float[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 0, 2 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Transformations operator *(Transformations t1, Transformations t2)
        {
            float[,] matrix = new float[4, 4];
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    matrix[i, j] = 0;
                    for (int k = 0; k < 4; ++k)
                        matrix[i, j] += t1.matrix[i, k] * t2.matrix[k, j];
                }
            return new Transformations(matrix);
        }
    }
}
