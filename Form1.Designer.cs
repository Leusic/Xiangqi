
namespace Xiangqi
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RedSoldier2 = new System.Windows.Forms.PictureBox();
            this.RedSoldier1 = new System.Windows.Forms.PictureBox();
            this.BoardImage = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RedSoldier2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedSoldier1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardImage)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1113, 232);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 56);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RedSoldier2);
            this.panel1.Controls.Add(this.RedSoldier1);
            this.panel1.Controls.Add(this.BoardImage);
            this.panel1.Location = new System.Drawing.Point(1, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 800);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // RedSoldier2
            // 
            this.RedSoldier2.Image = ((System.Drawing.Image)(resources.GetObject("RedSoldier2.Image")));
            this.RedSoldier2.Location = new System.Drawing.Point(178, 485);
            this.RedSoldier2.Name = "RedSoldier2";
            this.RedSoldier2.Size = new System.Drawing.Size(60, 60);
            this.RedSoldier2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RedSoldier2.TabIndex = 3;
            this.RedSoldier2.TabStop = false;
            // 
            // RedSoldier1
            // 
            this.RedSoldier1.Image = ((System.Drawing.Image)(resources.GetObject("RedSoldier1.Image")));
            this.RedSoldier1.Location = new System.Drawing.Point(32, 485);
            this.RedSoldier1.Name = "RedSoldier1";
            this.RedSoldier1.Size = new System.Drawing.Size(60, 60);
            this.RedSoldier1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RedSoldier1.TabIndex = 2;
            this.RedSoldier1.TabStop = false;
            // 
            // BoardImage
            // 
            this.BoardImage.BackColor = System.Drawing.Color.Transparent;
            this.BoardImage.Image = ((System.Drawing.Image)(resources.GetObject("BoardImage.Image")));
            this.BoardImage.Location = new System.Drawing.Point(0, 0);
            this.BoardImage.Name = "BoardImage";
            this.BoardImage.Size = new System.Drawing.Size(720, 800);
            this.BoardImage.TabIndex = 0;
            this.BoardImage.TabStop = false;
            this.BoardImage.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 796);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RedSoldier2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RedSoldier1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox RedSoldier1;
        private System.Windows.Forms.PictureBox BoardImage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox RedSoldier2;
    }
}

