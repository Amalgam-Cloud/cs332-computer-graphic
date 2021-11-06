using System;

namespace Assignment8
{
    public static class Transformer
    {
        public static double[,] MatrMult(double[,] m1, double[,] m2)
        {
            double[,] res = new double[m1.GetLength(0), m2.GetLength(1)];
            for (int i = 0; i < m1.GetLength(0); ++i)
                for (int j = 0; j < m2.GetLength(1); ++j)
                    for (int k = 0; k < m2.GetLength(0); k++)
                    {
                        res[i, j] += m1[i, k] * m2[k, j];
                    }
            return res;
        }
        public static double[,] RotateX(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, cos, sin, 0 },
                    { 0, -sin, cos, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public static double[,] RotateY(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return
                new double[,]
                {
                    { cos, 0, -sin, 0 },
                    { 0, 1, 0, 0 },
                    { sin, 0, cos, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public static double[,] RotateZ(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);
            return
                new double[,]
                {
                    { cos, sin, 0, 0 },
                    { -sin, cos, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public static double[,] Scale(double fx, double fy, double fz)
        {
            return
                new double[,] {
                    { fx, 0, 0, 0 },
                    { 0, fy, 0, 0 },
                    { 0, 0, fz, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public static double[,] Translate(Dot v)
        {
            return Translate(v.X, v.Y, v.Z);
        }

        public static double[,] Translate(double dx, double dy, double dz)
        {
            return
                new double[,]
                {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { dx, dy, dz, 1 },
                };
        }

        public static double[,] Identity()
        {
            return
                new double[,] {
                    { 1, 0, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 1 }
                };
        }

        public static double[,] OrthogonalProjection()
        {
            return Identity();
        }

        public static double[,] PerspectiveProjection(double left, double right, double bottom, double top, double near, double far)
        {
            var a = 2 * near / (right - left);
            var b = (right + left) / (right - left);
            var c = 2 * near / (top - bottom);
            var d = (top + bottom) / (top - bottom);
            var e = -(far + near) / (far - near);
            var f = -2 * far * near / (far - near);
            return
                new double[4, 4] {
                    { a, 0, 0, 0 },
                    { 0, c, 0, 0 },
                    { b, d, e, -1 },
                    { 0, 0, f, 0 }
                };
        }
    }
}
