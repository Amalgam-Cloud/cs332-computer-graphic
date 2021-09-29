using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment2
{

    public partial class task3 : Form
    {
        private static int CompareByPoint(Point x, Point y)
        {
            return x.Y.CompareTo(y.Y);
        }
        int count = 0;
        Point x;
        Point y;
        Point z;
        private Point p1, p2;
        List<Point> p1List = new List<Point>();
        List<Point> p2List = new List<Point>();
        public class PointComp : IComparer<Point>
        {
            // Compares by Height, Length, and Width.
            public int Compare(Point x, Point y)
            {
                return x.Y.CompareTo(y.Y);
            }
        }
        public task3()
        {
            InitializeComponent();
        }

        private void task3_Load(object sender, EventArgs e)
        {
        }
        void TriangleMaker(Point x, Point y, Point z, Color cl1, Color cl2, Color cl3)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Bitmap field = new Bitmap(pictureBox1.Image);
            List<Point> triangle = new List<Point>(); ;
            triangle.Add(x);
            triangle.Add(y);
            triangle.Add(z);
            triangle.Sort(CompareByPoint);
            Color curr_color = cl1;
            int bound = triangle[1].Y;
            double ch_R = (cl1.R - cl2.R) / (double)(triangle[2].Y - 1 - bound+1);
            double ch_G = (cl1.G - cl2.G) / (double)(triangle[2].Y - 1 - bound+1);
            double ch_B = (cl1.B - cl2.B) / (double)(triangle[2].Y - 1 - bound+1);
            double cr_R = ch_R;
            double cr_G = ch_G;
            double cr_B = ch_B;
            //double color_change =(double)255/(double)(triangle[2].Y - triangle[1].Y+3);
            for (int i = triangle[2].Y-1;i >=bound; --i)
            {
                int wall_1 = ((i- triangle[0].Y)* (triangle[2].X - triangle[0].X)) /(triangle[2].Y-triangle[0].Y) + triangle[0].X;
                int wall_2 = ((i - triangle[1].Y) * (triangle[2].X - triangle[1].X)) / (triangle[2].Y - triangle[1].Y) + triangle[1].X;
                int left_wall, right_wall;
                if(wall_1 > wall_2)
                {
                    left_wall = wall_2;
                    right_wall = wall_1;
                }
                else
                {
                    left_wall = wall_1;
                    right_wall = wall_2;
                }
                for (int j = left_wall+1;j < right_wall;++j)
                {
                    field.SetPixel(j,i, curr_color);
                }
                curr_color = Color.FromArgb(cl1.R - (int)cr_R, cl1.G - (int)cr_G, cl1.B - (int)cr_B);
                cr_R += ch_R;
                cr_G += ch_G;
                cr_B += ch_B;
            }
            curr_color = cl3;
            ch_R = (cl3.R - cl2.R) / (double)(bound - triangle[0].Y+1);
            ch_G = (cl3.G - cl2.G) / (double)(bound - triangle[0].Y+1);
            ch_B = (cl3.B - cl2.B) / (double)(bound - triangle[0].Y+1);
            cr_R = ch_R;
            cr_G = ch_G;
            cr_B = ch_B;
            //curr_color = 255;
            for (int i = triangle[0].Y; i <= bound; ++i)
            {
                int wall_1 = ((i - triangle[0].Y) * (triangle[2].X - triangle[0].X)) / (triangle[2].Y - triangle[0].Y)+ triangle[0].X;
                int wall_2 = ((i - triangle[0].Y) * (triangle[1].X - triangle[0].X)) / (triangle[1].Y - triangle[0].Y)+ triangle[0].X;
                int left_wall, right_wall;
                if (wall_1 > wall_2)
                {
                    left_wall = wall_2;
                    right_wall = wall_1;
                }
                else
                {
                    left_wall = wall_1;
                    right_wall = wall_2;
                }
                for (int j = left_wall + 1; j < right_wall; ++j)
                {
                    field.SetPixel(j, i, curr_color);
                }
                curr_color = Color.FromArgb(cl3.R - (int)cr_R, cl3.G - (int)cr_G, cl3.B - (int)cr_B);
                cr_R += ch_R;
                cr_G += ch_G;
                cr_B += ch_B;
            }
            pictureBox1.Image = field;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            count++;
            Control control = (Control)sender;
            if (count == 1)
            {
                var old_point = new Point(Cursor.Position.X, MousePosition.Y);
                var n_point = control.PointToClient(old_point);
                x = n_point;
            }
            if (count == 2)
            {
                var old_point = new Point(Cursor.Position.X, MousePosition.Y);
                var n_point = control.PointToClient(old_point);
                y = n_point;
            }
            if (count == 3)
            {
                var old_point = new Point(Cursor.Position.X, MousePosition.Y);
                var n_point = control.PointToClient(old_point);
                z = n_point;
                TriangleMaker(x, y, z, Color.Red,Color.DarkMagenta,Color.Blue);
                count = 0;
            }
        }
    }
}