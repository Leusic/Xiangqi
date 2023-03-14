﻿using System;
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
using System.Threading.Tasks;

namespace Xiangqi
{
    public partial class MainMenu : Form
    {
        Save loadedSave = null;
        gameBoard GameBoard = null;
        Client client = null;

        public MainMenu()
        {
            InitializeComponent();
            waitingLabel.Text = " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(loadedSave == null)
            {
                GameBoard = new gameBoard(null, 0, null);
            }
            if(loadedSave != null)
            {
                GameBoard = new gameBoard(loadedSave, 1, null);
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
            if (client == null)
            {
                client = new Client();
                new Thread(() =>
                {
                    client.runServer();
                }).Start();
            }

            while (client.otherAddress == null)
            {
                connectionLabel.Text = "Seaching for player...";
                Application.DoEvents();
                searchForPlayer();
            }

            if(client.otherAddress != null)
            {
                GameBoard = new gameBoard(null, 2, client);
                this.Hide();
                GameBoard.ShowDialog();
                this.Close();
            }
            else
            {
                Console.WriteLine("Server could not be found");
            }
        }

        private async void searchForPlayer()
        {
            Console.WriteLine("Searching for player on network");
            client.findPlayer();
            await Task.Delay(500);
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
