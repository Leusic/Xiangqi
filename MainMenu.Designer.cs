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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.loadSaveButton = new System.Windows.Forms.Button();
            this.joinGameButton = new System.Windows.Forms.Button();
            this.loadLabel = new System.Windows.Forms.Label();
            this.unloadSaveButton = new System.Windows.Forms.Button();
            this.waitingLabel = new System.Windows.Forms.Label();
            this.connectionLabel = new System.Windows.Forms.Label();
            this.playAgainstAIButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // localGameButton
            // 
            this.localGameButton.Location = new System.Drawing.Point(343, 189);
            this.localGameButton.Name = "localGameButton";
            this.localGameButton.Size = new System.Drawing.Size(90, 23);
            this.localGameButton.TabIndex = 0;
            this.localGameButton.Text = "Start Game";
            this.localGameButton.UseVisualStyleBackColor = true;
            this.localGameButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(301, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 89);
            this.label1.TabIndex = 1;
            this.label1.Text = "象棋";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(296, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 62);
            this.label2.TabIndex = 2;
            this.label2.Text = "Xiangqi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(691, 426);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Max Trotter, 2023";
            // 
            // loadSaveButton
            // 
            this.loadSaveButton.Location = new System.Drawing.Point(328, 218);
            this.loadSaveButton.Name = "loadSaveButton";
            this.loadSaveButton.Size = new System.Drawing.Size(126, 23);
            this.loadSaveButton.TabIndex = 4;
            this.loadSaveButton.Text = "Load Saved Game";
            this.loadSaveButton.UseVisualStyleBackColor = true;
            this.loadSaveButton.Click += new System.EventHandler(this.loadSaveButton_Click);
            // 
            // joinGameButton
            // 
            this.joinGameButton.Location = new System.Drawing.Point(328, 276);
            this.joinGameButton.Name = "joinGameButton";
            this.joinGameButton.Size = new System.Drawing.Size(126, 23);
            this.joinGameButton.TabIndex = 5;
            this.joinGameButton.Text = "Join Network Game";
            this.joinGameButton.UseVisualStyleBackColor = true;
            this.joinGameButton.Click += new System.EventHandler(this.joinGameButton_Click);
            // 
            // loadLabel
            // 
            this.loadLabel.Location = new System.Drawing.Point(312, 352);
            this.loadLabel.Name = "loadLabel";
            this.loadLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.loadLabel.Size = new System.Drawing.Size(161, 27);
            this.loadLabel.TabIndex = 7;
            this.loadLabel.Text = "Current Game: New Game";
            this.loadLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // unloadSaveButton
            // 
            this.unloadSaveButton.Location = new System.Drawing.Point(328, 247);
            this.unloadSaveButton.Name = "unloadSaveButton";
            this.unloadSaveButton.Size = new System.Drawing.Size(126, 23);
            this.unloadSaveButton.TabIndex = 8;
            this.unloadSaveButton.Text = "Unload Saved Game";
            this.unloadSaveButton.UseVisualStyleBackColor = true;
            this.unloadSaveButton.Click += new System.EventHandler(this.unloadSaveButton_Click);
            // 
            // waitingLabel
            // 
            this.waitingLabel.Location = new System.Drawing.Point(460, 303);
            this.waitingLabel.Name = "waitingLabel";
            this.waitingLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.waitingLabel.Size = new System.Drawing.Size(161, 27);
            this.waitingLabel.TabIndex = 9;
            this.waitingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // connectionLabel
            // 
            this.connectionLabel.Location = new System.Drawing.Point(460, 303);
            this.connectionLabel.Name = "connectionLabel";
            this.connectionLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.connectionLabel.Size = new System.Drawing.Size(161, 27);
            this.connectionLabel.TabIndex = 10;
            this.connectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playAgainstAIButton
            // 
            this.playAgainstAIButton.Location = new System.Drawing.Point(326, 307);
            this.playAgainstAIButton.Name = "playAgainstAIButton";
            this.playAgainstAIButton.Size = new System.Drawing.Size(130, 23);
            this.playAgainstAIButton.TabIndex = 11;
            this.playAgainstAIButton.Text = "Start game versus AI";
            this.playAgainstAIButton.UseVisualStyleBackColor = true;
            this.playAgainstAIButton.Click += new System.EventHandler(this.playAgainstAIButton_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.playAgainstAIButton);
            this.Controls.Add(this.connectionLabel);
            this.Controls.Add(this.unloadSaveButton);
            this.Controls.Add(this.loadLabel);
            this.Controls.Add(this.joinGameButton);
            this.Controls.Add(this.loadSaveButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.localGameButton);
            this.Controls.Add(this.waitingLabel);
            this.Name = "MainMenu";
            this.Text = "Xiangqi Menu";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button localGameButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button loadSaveButton;
        private System.Windows.Forms.Button joinGameButton;
        private System.Windows.Forms.Label loadLabel;
        private System.Windows.Forms.Button unloadSaveButton;
        private System.Windows.Forms.Label waitingLabel;
        private System.Windows.Forms.Label connectionLabel;
        private System.Windows.Forms.Button playAgainstAIButton;
    }
}