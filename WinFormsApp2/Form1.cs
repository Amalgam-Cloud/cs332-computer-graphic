using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Assignment2
{
    public partial class Form1 : Form
    {
        int count1 = 0;
        int count2 = 0;
        Point x1,x2,y1,y2;
        
        
        public Form1()
        {
            InitializeComponent();
        }

        public static void Swap(ref int x,ref int y)
        {
            int t;
            t = x;
            x = y;
            y = t;
        }

        void Bresenham(Point point1, Point point2)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Bitmap field = new Bitmap(pictureBox1.Image);

            Color color = Color.Black;

            int x0 = point1.X;
            int y0 = point1.Y;
            int x1 = point2.X;
            int y1 = point2.Y;

            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0); 

            if (steep)
            {
                Swap(ref x0,ref  y0); 
                Swap(ref x1, ref y1);
            }
           
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0,ref y1);
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            double error = dy+1 /dx+1; 
            int ystep = (y0 < y1) ? 1 : -1; 
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                field.SetPixel(x, y, color);
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            pictureBox1.Image = field;
        }

        private void Wu(Point point1, Point point2)
        {

            pictureBox2.Image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Bitmap field = new Bitmap(pictureBox2.Image);

            Color color = Color.Black;

            int x0 = point1.X;
            int y0 = point1.Y;
            int x1 = point2.X;
            int y1 = point2.Y;

            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            field.SetPixel(x0, y0, color); // Эта функция автоматом меняет координаты местами в зависимости от переменной steep
            field.SetPixel(x1, y1, color); // Последний аргумент — интенсивность в долях единицы
            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy / dx;
            float y = y0 + gradient;
            for (var x = x0 + 1; x <= x1 - 1; x++)
            {
                field.SetPixel(x, (int)y, Color.FromArgb(255-(int)Math.Round(1 - (y - (int)y)),0,0,0));
                field.SetPixel(x, (int)y + 1, Color.FromArgb(255-(int)Math.Round(y - (int)y),0,0,0));
                y += gradient;
            }
            pictureBox2.Image = field;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            count1++;
            Control control = (Control)sender;
            if (count1 == 1)
            {
                var old_point = new Point(Cursor.Position.X,MousePosition.Y);
                var n_point = control.PointToClient(old_point);
                x1 = n_point;
            }
            if (count1 == 2)
            {
                var old_point = new Point(Cursor.Position.X, MousePosition.Y);
                var n_point = control.PointToClient(old_point);
                y1 = n_point;
                Bresenham(x1, y1);
                count1 = 0;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            count2++;
            Control control = (Control)sender;
            if (count2 == 1)
            {
                var old_point = new Point(Cursor.Position.X, MousePosition.Y);
                var n_point = control.PointToClient(old_point);
                x2 = n_point;
            }
            if (count2 == 2)
            {
                var old_point = new Point(Cursor.Position.X, MousePosition.Y);
                var n_point = control.PointToClient(old_point);
                y2 = n_point;
                Wu(x2, y2);
                count2 = 0;
            }
        }
    }
}
