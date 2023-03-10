using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
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
            gameBoard GameBoard = new gameBoard(null, 0);
            this.Hide();
            GameBoard.ShowDialog();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void loadSaveButton_Click(object sender, EventArgs e)
        {
            Save loadedSave = null;
            TextReader reader = null;
            try
            {
                String sPath = System.AppDomain.CurrentDomain.BaseDirectory + "XiangqiSave.txt";
                reader = new StreamReader(sPath);
                var fileContents = reader.ReadToEnd();
                loadedSave = JsonConvert.DeserializeObject<Save>(fileContents);
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }
            gameBoard GameBoard = new gameBoard(loadedSave, 1);
            //this.Hide();
            GameBoard.ShowDialog();
            //this.Close();
        }

        private void joinGameButton_Click(object sender, EventArgs e)
        {

        }

        private void hostGameButton_Click(object sender, EventArgs e)
        {

        }
    }
}
