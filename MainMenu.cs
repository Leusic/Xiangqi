using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.AxHost;

namespace Xiangqi
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gameBoard GameBoard = new gameBoard(null);
            this.Hide();
            GameBoard.ShowDialog();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void loadSaveButton_Click(object sender, EventArgs e)
        {
            Board loadedBoard = null;
            TextReader reader = null;
            try
            {
                String sPath = System.AppDomain.CurrentDomain.BaseDirectory;
                reader = new StreamReader(sPath);
                var fileContents = reader.ReadToEnd();
                loadedBoard = JsonConvert.DeserializeObject<Board>(fileContents);
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }
            gameBoard GameBoard = new gameBoard(null);
            this.Hide();
            GameBoard.ShowDialog();
            this.Close();
        }
    }
}
