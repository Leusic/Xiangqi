namespace Xiangqi
{
    partial class MainMenu
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
            this.localGameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // localGameButton
            // 
            this.localGameButton.Location = new System.Drawing.Point(316, 12);
            this.localGameButton.Name = "localGameButton";
            this.localGameButton.Size = new System.Drawing.Size(126, 23);
            this.localGameButton.TabIndex = 0;
            this.localGameButton.Text = "Play Local Game";
            this.localGameButton.UseVisualStyleBackColor = true;
            this.localGameButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.localGameButton);
            this.Name = "MainMenu";
            this.Text = "Xiangqi Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button localGameButton;
    }
}