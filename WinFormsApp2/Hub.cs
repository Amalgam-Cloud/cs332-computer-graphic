using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Assignment2
{
    public partial class Hub : Form
    {
        public Hub()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            task3 task3Form = new task3();
            task3Form.Show();
        }

        private void Task1_Click(object sender, EventArgs e)
        {
            task1 task1Form = new task1();
            task1Form.Show();
        }
    }
}
