using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment8
{
    class Transform
    {
        public static Matrix RotateX(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new Matrix(
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, cos, sin, 0 },
                    { 0, -sin, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix RotateY(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new Matrix(
                new double[,]
                {
                    { cos, 0, -sin, 0 },
                    { 0, 1, 0, 0 },
                    { sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix RotateZ(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return new Matrix(
                new double[,]
                {
                    { cos, sin, 0, 0 },
                    { -sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix Scale(double fx, double fy, double fz)
        {
            return new Matrix(
                new double[,] {
                    { fx, 0, 0, 0 },
                    { 0, fy, 0, 0 },
                    { 0, 0, fz, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix Translate(Vector v)
        {
            return Translate(v.X, v.Y, v.Z);
        }

        public static Matrix Translate(double dx, double dy, double dz)
        {
            return new Matrix(
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { dx, dy, dz, 1 },
                });
        }

        public static Matrix Identity()
        {
            return new Matrix(
                new double[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                });
        }

        public static Matrix OrthogonalProjection()
        {
            return Identity();
        }

        public static Matrix PerspectiveProjection(double left, double right, double bottom, double top, double near, double far)
        {
            var a = 2 * near / (right - left);
            var b = (right + left) / (right - left);
            var c = 2 * near / (top - bottom);
            var d = (top + bottom) / (top - bottom);
            var e = -(far + near) / (far - near);
            var f = -2 * far * near / (far - near);
            return new Matrix(
                new double[4, 4] {
                    { a, 0, 0, 0 },
                    { 0, c, 0, 0 },
                    { b, d, e, -1 },
                    { 0, 0, f, 0 }
                });
        }
    }


    public class Matrix
    {
        private double[,] data;

        public double this[int i, int j]
        {
            get { return data[i, j]; }
            set { data[i, j] = value; }
        }

        public Matrix()
        {
            data = new double[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public Matrix(double[,] data)
        {
            Debug.Assert(4 == data.GetLength(0));
            Debug.Assert(4 == data.GetLength(1));
            this.data = data;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            double[,] data = new double[4, 4];
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                {
                    data[i, j] = 0;
                    for (int k = 0; k < 4; ++k)
                        data[i, j] += m1[i, k] * m2[k, j];
                }
            return new Matrix(data);
        }

        public Matrix Transpose()
        {
            double[,] data = new double[4, 4];
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 4; ++j)
                    data[i, j] = this[j, i];
            return new Matrix(data);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    builder.Append(data[i, j]);
                    builder.Append(' ');
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
