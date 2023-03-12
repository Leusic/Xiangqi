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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Xiangqi
{
    public partial class MainMenu : Form
    {
        Save loadedSave = null;
        gameBoard GameBoard = null;

        public MainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(loadedSave == null)
            {
                GameBoard = new gameBoard(null, 0);
            }
            if(loadedSave != null)
            {
                GameBoard = new gameBoard(loadedSave, 1);
            }
            this.Hide();
            GameBoard.ShowDialog();
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void loadSaveButton_Click(object sender, EventArgs e)
        {
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
            if(loadedSave != null)
            {
                loadLabel.Text = "Current Game: Previous Save";
            }
        }

        private void joinGameButton_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.findServer();
            //gameBoard GameBoard = new gameBoard(null, )
        }

        private void hostGameButton_Click(object sender, EventArgs e)
        {
            //runs the server in parallel
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Server.runServer();
            }).Start();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        private void unloadSaveButton_Click(object sender, EventArgs e)
        {
            loadedSave = null;
            loadLabel.Text = "Current Game: New Game";
        }
    }
}
