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
        public Form1()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {

        }

        private void ApplyPrimitive_Click(object sender, EventArgs e)
        {

        }

        private void ApplyAffin_Click(object sender, EventArgs e)
        {

            if (without_colors)
                current_primitive.Draw_without_colors(graphics3D);
            else if (moreThanOneObj)
            {
                foreach (var obj in objects)
                    obj.Draw(graphics3D);
            }
            else
                current_primitive.Draw(graphics3D);

            e.Graphics.DrawImage(graphics3D.ColorBuffer, 0, 0);

        }
    }
}
