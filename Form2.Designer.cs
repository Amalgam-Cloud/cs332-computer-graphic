
namespace cs332ComputerGraphic
{
    partial class Form2
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
            this.REDpictureBox = new System.Windows.Forms.PictureBox();
            this.BLUEpictureBox = new System.Windows.Forms.PictureBox();
            this.GREENpictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.REDpictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BLUEpictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GREENpictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // REDpictureBox
            // 
            this.REDpictureBox.Location = new System.Drawing.Point(12, 12);
            this.REDpictureBox.Name = "REDpictureBox";
            this.REDpictureBox.Size = new System.Drawing.Size(372, 285);
            this.REDpictureBox.TabIndex = 0;
            this.REDpictureBox.TabStop = false;
            // 
            // BLUEpictureBox
            // 
            this.BLUEpictureBox.Location = new System.Drawing.Point(12, 303);
            this.BLUEpictureBox.Name = "BLUEpictureBox";
            this.BLUEpictureBox.Size = new System.Drawing.Size(372, 285);
            this.BLUEpictureBox.TabIndex = 1;
            this.BLUEpictureBox.TabStop = false;
            // 
            // GREENpictureBox
            // 
            this.GREENpictureBox.Location = new System.Drawing.Point(12, 594);
            this.GREENpictureBox.Name = "GREENpictureBox";
            this.GREENpictureBox.Size = new System.Drawing.Size(372, 285);
            this.GREENpictureBox.TabIndex = 2;
            this.GREENpictureBox.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 885);
            this.Controls.Add(this.GREENpictureBox);
            this.Controls.Add(this.BLUEpictureBox);
            this.Controls.Add(this.REDpictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.REDpictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BLUEpictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GREENpictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox REDpictureBox;
        private System.Windows.Forms.PictureBox BLUEpictureBox;
        private System.Windows.Forms.PictureBox GREENpictureBox;
    }
}