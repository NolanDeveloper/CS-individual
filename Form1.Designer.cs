namespace DelaunayTriangulation
{
    partial class Form1
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.triangluationPlotter1 = new DelaunayTriangulation.TriangluationPlotter();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "shift+клик - удаляет точку";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "клик - добавляет точку";
            // 
            // triangluationPlotter1
            // 
            this.triangluationPlotter1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triangluationPlotter1.Location = new System.Drawing.Point(0, 0);
            this.triangluationPlotter1.Name = "triangluationPlotter1";
            this.triangluationPlotter1.Size = new System.Drawing.Size(777, 319);
            this.triangluationPlotter1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 319);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.triangluationPlotter1);
            this.Name = "Form1";
            this.Text = "Триангуляция методом Делоне";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TriangluationPlotter triangluationPlotter1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

