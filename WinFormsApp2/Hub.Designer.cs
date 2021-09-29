
namespace Assignment2
{
    partial class Hub
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Task1 = new System.Windows.Forms.Button();
            this.Task2 = new System.Windows.Forms.Button();
            this.Task3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Task1
            // 
            this.Task1.Location = new System.Drawing.Point(365, 106);
            this.Task1.Name = "Task1";
            this.Task1.Size = new System.Drawing.Size(75, 23);
            this.Task1.TabIndex = 0;
            this.Task1.Text = "Task 1";
            this.Task1.UseVisualStyleBackColor = true;
            // 
            // Task2
            // 
            this.Task2.Location = new System.Drawing.Point(365, 135);
            this.Task2.Name = "Task2";
            this.Task2.Size = new System.Drawing.Size(75, 23);
            this.Task2.TabIndex = 1;
            this.Task2.Text = "Task 2";
            this.Task2.UseVisualStyleBackColor = true;
            // 
            // Task3
            // 
            this.Task3.Location = new System.Drawing.Point(365, 164);
            this.Task3.Name = "Task3";
            this.Task3.Size = new System.Drawing.Size(75, 23);
            this.Task3.TabIndex = 2;
            this.Task3.Text = "Task 3";
            this.Task3.UseVisualStyleBackColor = true;
            this.Task3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Hub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Task3);
            this.Controls.Add(this.Task2);
            this.Controls.Add(this.Task1);
            this.Name = "Hub";
            this.Text = "Hub";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Task1;
        private System.Windows.Forms.Button Task2;
        private System.Windows.Forms.Button Task3;
    }
}