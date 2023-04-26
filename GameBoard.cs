using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xiangqi.Properties;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Threading;
using System.Resources;
using System.Diagnostics;

namespace Xiangqi
{
    public partial class gameBoard : Form
    {
        Board board = new Board();

        //instances of all units
        //red team
        Soldier redSoldier1 = new Soldier(0, 6, -1);
        Soldier redSoldier2 = new Soldier(2, 6, -1);
        Soldier redSoldier3 = new Soldier(4, 6, -1);
        Soldier redSoldier4 = new Soldier(6, 6, -1);
        Soldier redSoldier5 = new Soldier(8, 6, -1);
        General redGeneral = new General(4, 9, -1);
        Guard redGuard1 = new Guard(3, 9, -1);
        Guard redGuard2 = new Guard(5, 9, -1);
        Elephant redElephant1 = new Elephant(2, 9, -1);
        Elephant redElephant2 = new Elephant(6, 9, -1);
        Horse redHorse1 = new Horse(1, 9, -1);
        Horse redHorse2 = new Horse(7, 9, -1);
        Chariot redChariot1 = new Chariot(0, 9, -1);
        Chariot redChariot2 = new Chariot(8, 9, -1);
        Cannon redCannon1 = new Cannon(1, 7, -1);
        Cannon redCannon2 = new Cannon(7, 7, -1);

        //black team
        Soldier blackSoldier1 = new Soldier(0, 3, 1);
        Soldier blackSoldier2 = new Soldier(2, 3, 1);
        Soldier blackSoldier3 = new Soldier(4, 3, 1);
        Soldier blackSoldier4 = new Soldier(6, 3, 1);
        Soldier blackSoldier5 = new Soldier(8, 3, 1);
        General blackGeneral = new General(4, 0, 1);
        Guard blackGuard1 = new Guard(3, 0, 1);
        Guard blackGuard2 = new Guard(5, 0, 1);
        Elephant blackElephant1 = new Elephant(2, 0, 1);
        Elephant blackElephant2 = new Elephant(6, 0, 1);
        Horse blackHorse1 = new Horse(1, 0, 1);
        Horse blackHorse2 = new Horse(7, 0, 1);
        Chariot blackChariot1 = new Chariot(0, 0, 1);
        Chariot blackChariot2 = new Chariot(8, 0, 1);
        Cannon blackCannon1 = new Cannon(1, 2, 1);
        Cannon blackCannon2 = new Cannon(7, 2, 1);

        //graveyards store the pieces that have been taken
        Dictionary<Piece, PictureBox> redGraveyard = new Dictionary<Piece, PictureBox>();
        Dictionary<Piece, PictureBox> blackGraveyard = new Dictionary<Piece, PictureBox>();

        //stores piece type names next to their generalised value, used for scoring moves
        Dictionary<String, int> pieceValues = new Dictionary<String, int>();

        //stores the green movement dots (valid moves)
        PictureBox[,] movementIcons = new PictureBox[9, 10];
        //stores the red movement crosses (invalid moves)
        PictureBox[,] movementCrosses = new PictureBox[9, 10];
        //stores all the pieces and their pictureboxes
        Dictionary<Piece, PictureBox> allPieces = new Dictionary<Piece, PictureBox>();
        //connects piece names to the pieces themselves
        Dictionary<String, Piece> nameToPiece = null;

        //stores the moves of the game, used for rollback
        List<String> moveLog = new List<String>();

        bool redInCheck = false;
        bool blackInCheck = false;

        //instance of the client class, used for network play
        Client client = null;

        //represents the mode of the game, 0 is new local game, 1 is local game loaded from save, 2 is networked game, 3 is new game against AI, 4 is loaded game against AI
        int modeCode;
        
        int playerTeam;

        public gameBoard(Save loadedSave, int modeCode, Client client)
        {
            InitializeComponent();

            //placeholder value in the previous move log, necessary for checking if the other player has moved in networked games
            moveLog.Add("-");
            
            //takes in client that was created in the menu
            this.client = client;
            //game mode passed in from menu
            this.modeCode = modeCode;

            //if playing networked game
            if (modeCode == 2)
            {
                //assign teams to players
                client.assignTeams();
                playerTeam = client.myTeam;

                //signify player of their team
                if(playerTeam == -1)
                {
                    myTeamLabel.Text = "You are playing as Red";
                }
                else
                {
                    myTeamLabel.Text = "You are playing as Black";
                }
                Console.WriteLine("My address: " + client.myAddress);
                Console.WriteLine("Other address: " + client.otherAddress);

                //begin asynchronous turn checking method, runs in the background to check if the other player has taken their turn, or if they have disconnected
                checkForTurnChange();

                //disables rollback for networked games as this would enable a player to force their opponent to replay all their moves
                RollbackButton.Visible = false;
            }

            //if playing against AI
            if(modeCode == 3 || modeCode == 4)
            {
                //sets player team to red, AI plays as black
                myTeamLabel.Text = "You are playing as Red";
                playerTeam = -1;
            }

            //matches pieces to their pictureboxes
            allPieces = new Dictionary<Piece, PictureBox>
            {
                {redSoldier1 , RedSoldier1}, {redSoldier2, RedSoldier2}, {redSoldier3, RedSoldier3}, {redSoldier4, RedSoldier4}, {redSoldier5, RedSoldier5}, {redGeneral, RedGeneral}, {redGuard1, RedGuard1}, {redGuard2, RedGuard2},
                {redElephant1, RedElephant1 }, {redElephant2, RedElephant2}, {redHorse1, RedHorse1}, {redHorse2, RedHorse2}, {redChariot1, RedChariot1}, {redChariot2, RedChariot2}, {redCannon1, RedCannon1}, {redCannon2, RedCannon2},
                {blackSoldier1 , BlackSoldier1}, {blackSoldier2, BlackSoldier2}, {blackSoldier3, BlackSoldier3}, {blackSoldier4, BlackSoldier4}, {blackSoldier5, BlackSoldier5}, {blackGeneral, BlackGeneral}, {blackGuard1, BlackGuard1}, {blackGuard2, BlackGuard2},
                {blackElephant1, BlackElephant1 }, {blackElephant2, BlackElephant2}, {blackHorse1, BlackHorse1}, {blackHorse2, BlackHorse2}, {blackChariot1, BlackChariot1}, {blackChariot2, BlackChariot2}, {blackCannon1, BlackCannon1}, {blackCannon2, BlackCannon2},
            };

            //maps piece names to their pieces, used for loading saves
            nameToPiece = new Dictionary<String, Piece>
            {
                {"redSoldier1", redSoldier1}, {"redSoldier2", redSoldier2 }, {"redSoldier3", redSoldier3}, {"redSoldier4", redSoldier4 }, {"redSoldier5", redSoldier5}, {"redGeneral1", redGeneral}, {"redGuard1", redGuard1}, {"redGuard2", redGuard2},
                {"redElephant1", redElephant1 }, {"redElephant2", redElephant2}, {"redHorse1", redHorse1}, {"redHorse2", redHorse2}, {"redChariot1", redChariot1}, {"redChariot2", redChariot2}, {"redCannon1", redCannon1}, {"redCannon2", redCannon2},
                {"blackSoldier1", blackSoldier1}, {"blackSoldier2", blackSoldier2 }, {"blackSoldier3", blackSoldier3}, {"blackSoldier4", blackSoldier4 }, {"blackSoldier5", blackSoldier5}, {"blackGeneral1", blackGeneral}, {"blackGuard1", blackGuard1}, {"blackGuard2", blackGuard2},
                {"blackElephant1", blackElephant1 }, {"blackElephant2", blackElephant2}, {"blackHorse1", blackHorse1}, {"blackHorse2", blackHorse2}, {"blackChariot1", blackChariot1}, {"blackChariot2", blackChariot2}, {"blackCannon1", blackCannon1}, {"blackCannon2", blackCannon2}
            };

            //stores piece names next to their generalised values, used for AI move scoring
            pieceValues = new Dictionary<String, int>
            {
                {"Soldier", 2}, {"Chariot", 18}, {"Horse", 8}, {"Cannon", 9}, {"Guard", 2}, {"Elephant", 2}, {"General", 9999}
            };


            //if loading saved game
            if ((loadedSave != null) && (modeCode == 1))
            {
                //fetches pieces from save and their properties
                foreach (Piece i in loadedSave.pieces)
                {
                    Piece currentPiece = nameToPiece[i.name];
                    currentPiece.x = i.x;
                    currentPiece.y = i.y;
                    currentPiece.alive = i.alive;
                    currentPiece.name = i.name;

                    //regulates piece x and y values, if they go out of range initialisePiece will crash
                    if ((currentPiece.x > 10) || (currentPiece.y > 10))
                    {
                        currentPiece.x = 0;
                        currentPiece.y = 0;
                    }

                    //initialises instances of each piece in the save
                    initialisePiece(currentPiece, allPieces[currentPiece], currentPiece.x, currentPiece.y, currentPiece.name);

                    //restores taken units to their respective graveyards
                    if (currentPiece.alive == false)
                    {
                        currentPiece.x = 99; currentPiece.y = 99;
                        if (currentPiece.teamModifier == -1)
                        {
                            redGraveyard.Add(currentPiece, allPieces[currentPiece]);
                        }
                        if (currentPiece.teamModifier == 1)
                        {
                            blackGraveyard.Add(currentPiece, allPieces[currentPiece]);
                        }

                    }
                }
                //restore whether it was red or black's turn
                board.currentTurn = loadedSave.currentTurn;

                //runs updateTurn to update the UI to the correct turn
                if (board.currentTurn == 1)
                {
                    board.currentTurn = -1;
                    updateTurn();
                }
                if (board.currentTurn == -1)
                {
                    board.currentTurn = 1;
                    updateTurn();
                }

                //restore previous move log
                moveLog = loadedSave.rollbackMoves;
                updateMoveDisplay();

            }
            //if starting new game
            else
            {
                //start on red's turn
                board.currentTurn = -1;
                //initialising pieces
                //board pieces offset to match the board is (28,33) and there is 75 pixels between board positions
                //red team
                initialisePiece(redSoldier1, RedSoldier1, 0, 6, "redSoldier1");
                initialisePiece(redSoldier2, RedSoldier2, 2, 6, "redSoldier2");
                initialisePiece(redSoldier3, RedSoldier3, 4, 6, "redSoldier3");
                initialisePiece(redSoldier4, RedSoldier4, 6, 6, "redSoldier4");
                initialisePiece(redSoldier5, RedSoldier5, 8, 6, "redSoldier5");
                initialisePiece(redGeneral, RedGeneral, 4, 9, "redGeneral1");
                initialisePiece(redGuard1, RedGuard1, 3, 9, "redGuard1");
                initialisePiece(redGuard2, RedGuard2, 5, 9, "redGuard2");
                initialisePiece(redElephant1, RedElephant1, 2, 9, "redElephant1");
                initialisePiece(redElephant2, RedElephant2, 6, 9, "redElephant2");
                initialisePiece(redHorse1, RedHorse1, 1, 9, "redHorse1");
                initialisePiece(redHorse2, RedHorse2, 7, 9, "redHorse2");
                initialisePiece(redChariot1, RedChariot1, 0, 9, "redChariot1");
                initialisePiece(redChariot2, RedChariot2, 8, 9, "redChariot2");
                initialisePiece(redCannon1, RedCannon1, 1, 7, "redCannon1");
                initialisePiece(redCannon2, RedCannon2, 7, 7, "redCannon2");
                //black team
                initialisePiece(blackSoldier1, BlackSoldier1, 0, 3, "blackSoldier1");
                initialisePiece(blackSoldier2, BlackSoldier2, 2, 3, "blackSoldier2");
                initialisePiece(blackSoldier3, BlackSoldier3, 4, 3, "blackSoldier3");
                initialisePiece(blackSoldier4, BlackSoldier4, 6, 3, "blackSoldier4");
                initialisePiece(blackSoldier5, BlackSoldier5, 8, 3, "blackSoldier5");
                initialisePiece(blackGeneral, BlackGeneral, 4, 0, "blackGeneral1");
                initialisePiece(blackGuard1, BlackGuard1, 3, 0, "blackGuard1");
                initialisePiece(blackGuard2, BlackGuard2, 5, 0, "blackGuard2");
                initialisePiece(blackElephant1, BlackElephant1, 2, 0, "blackElephant1");
                initialisePiece(blackElephant2, BlackElephant2, 6, 0, "blackElephant2");
                initialisePiece(blackHorse1, BlackHorse1, 1, 0, "blackHorse1");
                initialisePiece(blackHorse2, BlackHorse2, 7, 0, "blackHorse2");
                initialisePiece(blackChariot1, BlackChariot1, 0, 0, "blackChariot1");
                initialisePiece(blackChariot2, BlackChariot2, 8, 0, "blackChariot2");
                initialisePiece(blackCannon1, BlackCannon1, 1, 2, "blackCannon1");
                initialisePiece(blackCannon2, BlackCannon2, 7, 2, "blackCannon2");
            }

            //distance in x and y from the top left corner of the board image to the top left corner of the playable area of the board
            int iconX = 47;
            int iconY = 47;

            //initialises all the movement dots (valid moves) and movement crosses (invalid moves) onto their positions on the board
            for (int i = 0; i < 9; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    //sets all dots and crosses as invisible to start as they will be made visible when they are showing possible moves
                    initialiseMovementCross(i, z, iconX, iconY);
                    movementCrosses[i, z].Visible = false;
                    initialiseMovementIcon(i, z, iconX, iconY);
                    movementIcons[i, z].Visible = false;
                    iconY += 75;
                }
                iconY = 47;
                iconX += 75;
            }

            //updates the UI graveyard
            updateGraveyard();
        }

        //(networked game) checks if the other player has moved or if they have disconnected
        private async void checkForTurnChange()
        {
            //runs continuously in the background
            while (true)
            {
                Console.WriteLine("Checking for turn change...");
                try
                {
                    //if the client has a different move logged than the current local move log, a move has been taken by the other player
                    if (client.lastMove != moveLog[moveLog.Count - 1])
                    {
                        //converts client move codes to coordinate integers
                        int startX = client.lastMove[0] - 65;
                        int startY = client.lastMove[1] - '0';
                        startY = 9 - startY;
                        int endX = client.lastMove[2] - 65;
                        int endY = client.lastMove[3] - '0';
                        endY = 9 - endY;

                        //use the start x and y to find the piece that needs to be moved and then it's picturebox
                        Piece pieceToMove = board.grid[startX, startY].piece;

                        int xDiff = endX - startX;
                        int yDiff = endY - startY;
                        PictureBox pictureBoxToMove = allPieces[pieceToMove];

                        //move the piece the other player moved and adjust the board accordingly
                        pieceToMove.x = endX;
                        pieceToMove.y = endY;
                        board.grid[startX, startY].occupied = false;
                        board.grid[startX, startY].piece = null;
                        pictureBoxToMove.Left += xDiff * 75;
                        pictureBoxToMove.Top += yDiff * 75;

                        //taking a piece moves the taken piece to its team graveyard
                        if (board.grid[endX, endY].occupied == true)
                        {
                            board.grid[endX, endY].piece.alive = false;
                            if (board.grid[endX, endY].piece.teamModifier == -1)
                            {
                                redGraveyard.Add(board.grid[endX, endY].piece, allPieces[board.grid[endX, endY].piece]);
                            }
                            else
                            {
                                blackGraveyard.Add(board.grid[endX, endY].piece, allPieces[board.grid[endX, endY].piece]);
                            }
                            updateGraveyard();
                        }

                        board.grid[endX, endY].occupied = true;
                        board.grid[endX, endY].piece = pieceToMove;

                        //update local move log, update UI and perform a check scan
                        moveLog.Add(client.lastMove);
                        updateMoveDisplay();
                        updateTurn();
                        UnshowMoves();
                        checkScan();
                        saveGameStatusLabel.Text = "-";
                    }
                }
                catch
                {

                }
                //if the other player loses connection, stop turn checking and prompt the player to return to menu
                if(client.checkConnection() == false)
                {
                    CheckTextbox.Text = "Lost Connection :(";
                    MenuButton.BringToFront();
                    TurnTextbox.SendToBack();
                    foreach (KeyValuePair<Piece, PictureBox> i in allPieces)
                    {
                        i.Key.alive = false;
                    }
                    client.haltProcess = true;
                    break;
                }
                //perform a turn check every interval of this amount
                await Task.Delay(100);
            }
        }

        //update turn UI and switch current turn
        private void updateTurn()     
        {
            board.currentTurn = -board.currentTurn;
            if (board.currentTurn == -1)
            {
                TurnTextbox.BackColor = Color.IndianRed;
                TurnTextbox.Text = "Red's Turn";
            }
            else
            {
                TurnTextbox.BackColor = Color.DarkGray;
                TurnTextbox.Text = "Black's Turn";
            }
        }

        //return to menu
        private void MenuButton_Click(object sender, EventArgs e)
        {
            //restarts the game in order to fully close the network socket.
            System.Diagnostics.Process.Start(Process.GetCurrentProcess().MainModule.FileName); // to start new instance of application
            Environment.Exit(0);
            Application.Exit();

            MainMenu mainMenu = new MainMenu();
            this.Hide();
            mainMenu.ShowDialog();
            this.Close();
        }

        //changes the UI to it's game over status, displaying the winner and disabling unit movement
        private void gameOver()
        {
            CheckmateTextbox.BringToFront();
            WinningTeamTextbox.BringToFront();
            MenuButton.BringToFront();
            RollbackButton.Visible = false;
            TurnTextbox.Text = "Game Over!";
            TurnTextbox.BackColor = Color.Tan;
            CheckTextbox.SendToBack();
            saveGameButton.Visible = false;
            foreach (KeyValuePair<Piece, PictureBox> i in allPieces)
            {
                i.Key.alive = false;
            }
        }

        //performs a check scan, displaying whether red or black is in check
        private void checkScan()
        {
            //by default display that no team is in check
            redInCheck = false; blackInCheck = false;
            CheckTextbox.Text = "Neither team in check";
            bool possibleMove = false;
            //is black in check?
            //checks if any red units legal movements can hit the black general
            foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if ((i.Key.teamModifier == -1) && (i.Key.legalMovesBasic(ref board)[blackGeneral.x, blackGeneral.y] == 2))
                {
                    blackInCheck = true;
                    CheckTextbox.Text = "Black in check!";
                    possibleMove = false;
                    //runs a legal moves check on all black units to see if there is any move that can be made to escape check
                    foreach (KeyValuePair<Piece, PictureBox> z in allPieces) if (z.Key.teamModifier == 1)
                        {
                            int[,] moveBoard = z.Key.legalMoves(ref board, ref allPieces);
                            for (int a = 0; a < 8; a++)
                            {
                                for (int b = 0; b < 9; b++)
                                {
                                    if (moveBoard[a, b] == 2)
                                    {
                                        possibleMove = true;
                                    }
                                }
                            }
                        }
                    //if no move can be made to escape check, red wins
                    if (possibleMove == false)
                    {
                        WinningTeamTextbox.Text = "Red Wins!";
                        gameOver();
                    }
                    break;
                }
            //is red in check?
            //checks if any black units legal movements can hit the red general
            foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if ((i.Key.teamModifier == 1) && (i.Key.legalMovesBasic(ref board)[redGeneral.x, redGeneral.y] == 2))
                {
                    redInCheck = true;
                    CheckTextbox.Text = "Red in check!";
                    possibleMove = false;
                    //runs a legal moves check on all black units to see if there is any move that can be made to escape check
                    foreach (KeyValuePair<Piece, PictureBox> z in allPieces) if (z.Key.teamModifier == -1)
                        {
                            int[,] moveBoard = z.Key.legalMoves(ref board, ref allPieces);
                            for (int a = 0; a < 8; a++)
                            {
                                for (int b = 0; b < 9; b++)
                                {
                                    if (moveBoard[a, b] == 2)
                                    {
                                        possibleMove = true;
                                    }
                                }
                            }
                        }
                    //if no move can be made to escape check, black wins
                    if (possibleMove == false)
                    {
                        WinningTeamTextbox.Text = "Black Wins!";
                        gameOver();
                    }
                    break;
                }
        }

        //updates the UI graveyard with all taken pieces, making sure all are visible
        private void updateGraveyard()
        {
            int count = 0;
            int y = 33;
            int x = 745;
            foreach (KeyValuePair<Piece, PictureBox> i in blackGraveyard)
            {
                if (count > 3)
                {
                    count = 0;
                    x += 75;
                    y = 33;
                }
                i.Value.Location = new Point(x, y);
                y += 75;
                count++;
            }
            count = 0;
            y = 433;
            x = 745;
            foreach (KeyValuePair<Piece, PictureBox> i in redGraveyard)
            {
                if (count > 3)
                {
                    count = 0;
                    x += 75;
                    y = 433;
                }
                i.Value.Location = new Point(x, y);
                y += 75;
                count++;
            }
        }

        //initialises a movement dot (valid move)
        private void initialiseMovementIcon(int i, int z, int x, int y)
        {
            movementIcons[i, z] = new PictureBox();
            movementIcons[i, z].Height = 25;
            movementIcons[i, z].Width = 25;
            movementIcons[i, z].Location = new Point(x, y);
            movementIcons[i, z].Parent = BoardImage;
            movementIcons[i, z].Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "//Images//greenDot.png");
            movementIcons[i, z].BackColor = Color.Transparent;
        }

        //initialises a movement cross (invalid move)
        private void initialiseMovementCross(int i, int z, int x, int y)
        {
            movementCrosses[i, z] = new PictureBox();
            movementCrosses[i, z].Height = 25;
            movementCrosses[i, z].Width = 25;
            movementCrosses[i, z].Location = new Point(x, y);
            movementCrosses[i, z].Parent = BoardImage;
            movementCrosses[i, z].Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "//Images//redCross.png");
            movementCrosses[i, z].BackColor = Color.Transparent;
        }

        //initialises a piece, places the piece on board and moves the picturebox
        private void initialisePiece(Piece piece, PictureBox pictureBox, int x, int y, string name)
        {
            piece.name = name;
            pictureBox.Parent = BoardImage;
            pictureBox.BackColor = Color.Transparent;
            board.grid[x, y].occupied = true;
            board.grid[x, y].piece = piece;
            pictureBox.Location = new Point(28 + (x * 75), 33 + (y * 75));
            pictureBox.MouseClick += (sender, EventArgs) => { ShowMoves(sender, EventArgs, piece, pictureBox); }; ;
        }

        //displays movement dots and crosses representing where a piece's valid and invalid moves are
        private void ShowMoves(object sender, EventArgs e, Piece piece, PictureBox pictureBox)
        {
            //only runs if it is the piece's turn
            if ((piece.alive == true) && (piece.teamModifier == board.currentTurn))
            {
                //if in networked mode, check that the client's team matches the current turn's team
                if ((((modeCode == 2) || (modeCode == 3) || (modeCode == 4)) && (board.currentTurn == playerTeam)) || ((modeCode != 2) && (modeCode != 3) && (modeCode != 4)))
                {
                    //clears any previous movement icons
                    UnshowMoves();
                    int[,] moveBoard;
                    //gets legal moves for the piece
                    moveBoard = piece.legalMoves(ref board, ref allPieces);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            //if move is valid, display a dot at the move location
                            if (moveBoard[i, z] == 2)
                            {
                                int a = i;
                                int b = z;
                                int x = movementIcons[i, z].Location.X;
                                int y = movementIcons[i, z].Location.Y;
                                //removes event handlers of previously used buttons so that multiple pieces dont move at once.
                                movementIcons[i, z].Dispose();
                                movementIcons[i, z] = null;
                                initialiseMovementIcon(i, z, x, y);
                                movementIcons[i, z].Visible = true;
                                movementIcons[i, z].BringToFront();
                                movementIcons[i, z].MouseClick += (sender, EventArgs) => { MoveUnit(sender, EventArgs, a, b, piece, pictureBox); };
                            }
                            //if move is invalid, display a cross at the move location
                            if (moveBoard[i, z] == 1)
                            {
                                int a = i;
                                int b = z;
                                int x = movementCrosses[i, z].Location.X;
                                int y = movementCrosses[i, z].Location.Y;
                                //removes event handlers of previously used buttons so that multiple pieces dont move at once.
                                movementCrosses[i, z].Dispose();
                                movementCrosses[i, z] = null;
                                initialiseMovementCross(i, z, x, y);
                                movementCrosses[i, z].Visible = true;
                                movementCrosses[i, z].BringToFront();
                            }
                        }
                    }
                }
            }
        }

        //hides all movement icons
        private void UnshowMoves()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    movementIcons[i, z].Visible = false;
                    movementCrosses[i, z].Visible = false;
                }
            }
        }

        //updates the move log display
        private void updateMoveDisplay()
        {
            if (board.currentTurn == 1)
            {
                try
                {
                    moveLabel2.Text = moveLog[moveLog.Count - 1].Substring(0, 4);
                }
                catch
                {
                    moveLabel2.Text = "-";
                }
                try
                {
                    moveLabel1.Text = moveLog[moveLog.Count - 2].Substring(0, 4);
                }
                catch
                {
                    moveLabel1.Text = "-";
                }
                try
                {
                    moveLabel3.Text = moveLog[moveLog.Count - 4].Substring(0, 4);
                }
                catch
                {
                    moveLabel3.Text = "-";
                }
                try
                {
                    moveLabel4.Text = moveLog[moveLog.Count - 3].Substring(0, 4);
                }
                catch
                {
                    moveLabel4.Text = "-";
                }
            }
            if (board.currentTurn == -1)
            {
                try
                {
                    moveLabel1.Text = moveLog[moveLog.Count - 1].Substring(0, 4);
                }
                catch
                {
                    moveLabel1.Text = "-";
                }
                try
                {
                    moveLabel2.Text = "-";
                }
                catch
                {
                    moveLabel2.Text = "-";
                }
                try
                {
                    moveLabel3.Text = moveLog[moveLog.Count - 3].Substring(0, 4);
                }
                catch
                {
                    moveLabel3.Text = "-";
                }
                try
                {
                    moveLabel4.Text = moveLog[moveLog.Count - 2].Substring(0, 4);
                }
                catch
                {
                    moveLabel4.Text = "-";
                }
            }
        }

        //gets the code for a piece, used for describing pieces in move codes
        private List<char> getCodeFromPiece(Piece piece)
        {
            List<char> charCode = new List<char>();
            String pieceName = piece.name;

            //team
            if (pieceName.Contains("red"))
            {
                charCode.Add('R');
            }
            if (pieceName.Contains("black"))
            {
                charCode.Add('B');
            }

            //type of piece
            switch (piece.GetType().Name)
            {
                case "Chariot":
                    charCode.Add('R');
                    break;
                case "Cannon":
                    charCode.Add('C');
                    break;
                case "Elephant":
                    charCode.Add('E');
                    break;
                case "Guard":
                    charCode.Add('A');
                    break;
                case "Horse":
                    charCode.Add('H');
                    break;
                case "Soldier":
                    charCode.Add('P');
                    break;
                default:
                    charCode.Add('K');
                    break;
            }
            charCode.Add(pieceName[pieceName.Length - 1]);
            return charCode;
        }

        //when playing against AI, decides which move the AI will take
        private void AIMoveUnit()
        {
            //stores the current best moveScore, starts at -9999 because sometimes the AI will have to choose between several 'bad' moves
            int bestMoveScore = -9999;
            int bestMoveX = 99;
            int bestMoveY = 99;

            Piece bestMovePiece = null;
            PictureBox bestMovePictureBox = null;
            foreach (KeyValuePair<Piece, PictureBox> p in allPieces)
            {
                //only acts on pieces that are on the AI's team and are currently alive
                if(p.Key.alive == true && p.Key.teamModifier == 1)
                {
                    int[,] moveBoard = p.Key.legalMoves(ref board, ref allPieces);
                    //gets valid moves
                    for (int i = 0; i < 9; i++)
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            if (moveBoard[i, z] == 2)
                            {
                                int moveScore = 0;
                                //store where the piece started so the board can be restored later
                                int startX = p.Key.x;
                                int startY = p.Key.y;

                                board.grid[p.Key.x, p.Key.y].occupied = false;
                                board.grid[p.Key.x, p.Key.y].piece = null;

                                //change current piece coordinates
                                p.Key.x = i;
                                p.Key.y = z;

                                //makes it so that you can undo the piece change after calculating a move
                                Piece undoPiece = null;

                                //if a piece is taken, moves it out of play
                                if (board.grid[p.Key.x, p.Key.y].occupied == true)
                                {
                                    undoPiece = board.grid[p.Key.x, p.Key.y].piece;

                                    //adds potential taken piece score to moveScore
                                    moveScore = moveScore + pieceValues[undoPiece.GetType().Name];
                                    if((undoPiece.GetType().Name == "Soldier") && undoPiece.crossedRiver == true)
                                    {
                                        moveScore = moveScore + 2;
                                    }

                                    board.grid[p.Key.x, p.Key.y].piece.x = 99;
                                    board.grid[p.Key.x, p.Key.y].piece.y = 99;
                                }

                                //temporarily move the piece to the new location
                                board.grid[p.Key.x, p.Key.y].occupied = true;
                                board.grid[p.Key.x, p.Key.y].piece = p.Key;

                                //move weighting
                                //check if the move would put the unit in immediate danger, and if so deduct from the move score the value of the current unit
                                foreach (KeyValuePair<Piece, PictureBox> e in allPieces) if (e.Key.teamModifier == -1){
                                        int[,] enemyMoves = e.Key.legalMoves(ref board, ref allPieces);
                                        if (enemyMoves[p.Key.x,p.Key.y] == 2)
                                        {
                                            moveScore = moveScore - pieceValues[p.Key.GetType().Name];
                                            //check if ai unit is a soldier and if it has crossed the river, increase it's value
                                            if((p.Key.GetType().Name == "Soldier") && (p.Key.crossedRiver == true))
                                            {
                                                moveScore = moveScore - 2;
                                            }
                                            break;
                                        }
                                }

                                //if calculated move has the highest score so far, log it.
                                if(moveScore > bestMoveScore)
                                {                  
                                    bestMoveScore = moveScore;
                                    bestMoveX = p.Key.x;
                                    bestMoveY = p.Key.y;
                                    bestMovePiece = p.Key;
                                    bestMovePictureBox = p.Value;                                    
                                }

                                //undo board changes
                                if (undoPiece != null)
                                {
                                    undoPiece.x = p.Key.x;
                                    undoPiece.y = p.Key.y;
                                    board.grid[p.Key.x, p.Key.y].piece = undoPiece;
                                }
                                else
                                {
                                    board.grid[p.Key.x, p.Key.y].occupied = false;
                                    board.grid[p.Key.x, p.Key.y].piece = null;
                                }

                                p.Key.x = startX;
                                p.Key.y = startY;

                                board.grid[startX, startY].occupied = true;
                                board.grid[startX, startY].piece = p.Key;

                            }
                            }
                        }
                    }
            }
            //update the board and UI with the decided move (if one with a positive score is possible)
            if (bestMoveScore > 0)
            {
                MoveUnit(null, null, bestMoveX, bestMoveY, bestMovePiece, bestMovePictureBox);
            }
            //if there are no positive rating moves to take, try and move towards the player general
            else
            {
                int playerGeneralX = 99;
                int playerGeneralY = 99;
                //find the player's general x and y value.
                foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if (i.Key.GetType().Name == "General")
                    { 
                        if (i.Key.teamModifier == -1)
                        {
                            playerGeneralX = i.Key.x;
                            playerGeneralY = i.Key.y;
                        }
                    }
                int combinedGeneralDiff = 99;
                bestMoveX = 99;
                bestMoveY = 99;
                foreach (KeyValuePair<Piece, PictureBox> p in allPieces)
                {
                    //only acts on pieces that are on the AI's team and are currently alive
                    if (p.Key.alive == true && p.Key.teamModifier == 1)
                    {
                        int[,] moveBoard = p.Key.legalMoves(ref board, ref allPieces);
                        //gets valid moves
                        for (int i = 0; i < 9; i++)
                        {
                            for (int z = 0; z < 10; z++)
                            {
                                if (moveBoard[i, z] == 2)
                                {
                                    //store where the piece started so the board can be restored later
                                    int startX = p.Key.x;
                                    int startY = p.Key.y;

                                    board.grid[p.Key.x, p.Key.y].occupied = false;
                                    board.grid[p.Key.x, p.Key.y].piece = null;

                                    //change current piece coordinates
                                    p.Key.x = i;
                                    p.Key.y = z;

                                    //makes it so that you can undo the piece change after calculating a move
                                    Piece undoPiece = null;

                                    //if a piece is taken, moves it out of play
                                    if (board.grid[p.Key.x, p.Key.y].occupied == true)
                                    {
                                        undoPiece = board.grid[p.Key.x, p.Key.y].piece;

                                        board.grid[p.Key.x, p.Key.y].piece.x = 99;
                                        board.grid[p.Key.x, p.Key.y].piece.y = 99;
                                    }

                                    board.grid[p.Key.x, p.Key.y].occupied = true;
                                    board.grid[p.Key.x, p.Key.y].piece = p.Key;

                                    //find the distance between the piece moved and the player general
                                    int generalDiffX = Math.Abs(playerGeneralX - p.Key.x);
                                    int generalDiffY = Math.Abs(playerGeneralY - p.Key.y);
                                    //if this move results in the shortest distance between the piece and the general, mark it as the best move
                                    if ((generalDiffX + generalDiffY) < combinedGeneralDiff)
                                    {
                                        combinedGeneralDiff = generalDiffX + generalDiffY;
                                        bestMoveX = p.Key.x;
                                        bestMoveY = p.Key.y;
                                        bestMovePiece = p.Key;
                                        bestMovePictureBox = p.Value;
                                    }
                                    //undo board changes
                                    if (undoPiece != null)
                                    {
                                        undoPiece.x = p.Key.x;
                                        undoPiece.y = p.Key.y;
                                        board.grid[p.Key.x, p.Key.y].piece = undoPiece;
                                    }
                                    else
                                    {
                                        board.grid[p.Key.x, p.Key.y].occupied = false;
                                        board.grid[p.Key.x, p.Key.y].piece = null;
                                    }

                                    p.Key.x = startX;
                                    p.Key.y = startY;

                                    board.grid[startX, startY].occupied = true;
                                    board.grid[startX, startY].piece = p.Key;
                                }
                            }
                        }
                    }
                }
                //update the board and UI with the decided move
                MoveUnit(null, null, bestMoveX, bestMoveY, bestMovePiece, bestMovePictureBox);
            }
        }

        //changes the board position of a unit to it's new position and updates it's visible position
        private void MoveUnit(object sender, EventArgs e, int x, int y, Piece piece, PictureBox pictureBox)
        {
            //characters for logging the move
            List<char> moveChars = new List<char>();
            moveChars.Add((char)(piece.x + 65));
            moveChars.Add((char)(9 - piece.y + '0'));
            moveChars.Add((char)(x + 65));
            moveChars.Add((char)(9 - y + '0'));

            //clears the position the piece is moving from
            board.grid[piece.x, piece.y].occupied = false;
            board.grid[piece.x, piece.y].piece = null;
            int xDiff = x - piece.x;
            int yDiff = y - piece.y;
            //checks if the river is crossed because if it is a soldier it's movement patterns change
            if ((piece.y < 5 && (piece.y + yDiff) >= 5) || (piece.y > 4 && (piece.y + yDiff) <= 4))
            {
                piece.crossedRiver = true;
            }
            //taking a piece moves the taken piece to its team graveyard
            if (board.grid[x, y].occupied == true)
            {
                board.grid[x, y].piece.alive = false;
                if (board.grid[x, y].piece.teamModifier == -1)
                {
                    redGraveyard.Add(board.grid[x, y].piece, allPieces[board.grid[x, y].piece]);
                }
                else
                {
                    blackGraveyard.Add(board.grid[x, y].piece, allPieces[board.grid[x, y].piece]);
                }
                //adds movement to the move log
                moveChars.AddRange(getCodeFromPiece(board.grid[x, y].piece));
                updateGraveyard();
            }
            piece.x = x;
            piece.y = y;
            //moves piece that was in the position away from interacting with any other pieces (functionally killing it)
            if (board.grid[x, y].occupied == true)
            {
                board.grid[x, y].piece.x = 99;
                board.grid[x, y].piece.y = 99;
            }
            //moves piece into it's new position
            board.grid[x, y].occupied = true;
            board.grid[x, y].piece = piece;
            pictureBox.Left += xDiff * 75;
            pictureBox.Top += yDiff * 75;
            UnshowMoves();
            string moveString = new string(moveChars.ToArray());
            moveLog.Add(moveString);
            //sends turn to the other player if playing networked
            if (modeCode == 2)
            {
                client.sendTurn(moveString);
            }
            updateMoveDisplay();
            updateTurn();
            checkScan();
            //if playing against AI, prompt AI to make it's move
            if ((modeCode == 3 || modeCode == 4) && (board.currentTurn == 1))
            {
                AIMoveUnit();
            }
            //refresh save game label as board is in a new state that would need to be saved
            saveGameStatusLabel.Text = "-";
        }

        //rolls back the last move
        private void RollbackButton_Click(object sender, EventArgs e)
        {
            int numOfRollbacks = 1;
            //if playing against ai, rollback the ais move along with the players to prevent game freeze
            if (modeCode == 3 || modeCode == 4)
            {
                numOfRollbacks = 2;
            }
            //do not allow rollback if playing a networked game
            if (modeCode != 2)
            {
                for(int t = 0; t <= numOfRollbacks; t++)
                {
                    try
                    {
                        //retrieve last move and get the starting and ending X and Y values from it
                        String rollbackMove = moveLog[moveLog.Count - 1];
                        int startX = rollbackMove[0] - 65;
                        int startY = rollbackMove[1] - '0';
                        startY = 9 - startY;
                        int endX = rollbackMove[2] - 65;
                        int endY = rollbackMove[3] - '0';
                        endY = 9 - endY;

                        //use the end x and y to find the piece that needs to be moved and then it's picturebox
                        Piece pieceToMove = board.grid[endX, endY].piece;

                        int xDiff = endX - startX;
                        int yDiff = endY - startY;
                        PictureBox pictureBoxToMove = allPieces[pieceToMove];

                        //move the piece to where it was before it moved, and update the board accordingly
                        pieceToMove.x = startX;
                        pieceToMove.y = startY;
                        board.grid[startX, startY].occupied = true;
                        board.grid[startX, startY].piece = pieceToMove;
                        pictureBoxToMove.Left -= xDiff * 75;
                        pictureBoxToMove.Top -= yDiff * 75;

                        //if soldier is returning to before it crossed the river
                        if (pieceToMove.GetType().Name == "Soldier")
                        {
                            if ((pieceToMove.teamModifier == -1) && (pieceToMove.y >= 5))
                            {
                                pieceToMove.crossedRiver = false;
                            }
                            else if (pieceToMove.y <= 4)
                            {
                                pieceToMove.crossedRiver = false;
                            }
                        }

                        //if a piece was taken on the move in question (the move code will have additional characters if this is the case)
                        if (rollbackMove.Length > 4)
                        {
                            //translates the code for the taken piece from the end of the move code and retrieves the piece and the picturebox for it
                            Piece revivedPiece = getTakenPieceFromCode(rollbackMove);
                            PictureBox revivedPictureBox = allPieces[revivedPiece];
                            revivedPiece.alive = true;
                            revivedPiece.x = endX;
                            revivedPiece.y = endY;
                            //removes the piece from it's team's graveyard
                            if (revivedPiece.teamModifier == -1)
                            {
                                redGraveyard.Remove(revivedPiece);
                            }
                            else
                            {
                                blackGraveyard.Remove(revivedPiece);
                            }
                            //resets the position of the now revived piece's picturebox and updates the board
                            revivedPictureBox.Top = 33;
                            revivedPictureBox.Left = 28;
                            revivedPictureBox.Top += endY * 75;
                            revivedPictureBox.Left += endX * 75;

                            board.grid[endX, endY].piece = revivedPiece;
                        }
                        //if no piece was taken on the turn being rolled back, update the board to empty the cell the piece is now leaving
                        else
                        {
                            board.grid[endX, endY].occupied = false;
                            board.grid[endX, endY].piece = null;
                        }

                        //remove the rolled back move from the movelog and update the move display, the current turn, bring forward the check textbox incase you are rolling back from a end of game state and run a checkscan
                        moveLog.Remove(rollbackMove);
                        updateMoveDisplay();
                        updateTurn();
                        CheckTextbox.BringToFront();
                        UnshowMoves();
                        checkScan();
                        saveGameStatusLabel.Text = "-";
                    }
                    catch { }
                }               
            }
        }

        //saves the game to a text file called XiangqiSave.txt
        private void saveGameButton_Click(object sender, EventArgs e)
        {
            //converts save to a json object in order to save it to a file
            TextWriter writer = null;
            List<Piece> piecesToWrite = new List<Piece>();
            try
            {
                foreach (KeyValuePair<Piece, PictureBox> i in allPieces)
                {
                    piecesToWrite.Add(i.Key);
                }
                Save saveObject = new Save(piecesToWrite, board.currentTurn, moveLog);
                var fileContent = JsonConvert.SerializeObject(saveObject);
                String sPath = System.AppDomain.CurrentDomain.BaseDirectory;
                writer = new StreamWriter(sPath = "XiangqiSave.txt", false);
                writer.Write(fileContent);
                saveGameStatusLabel.Text = "Game Saved!";
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }

            }
        }

        //extracts the piece that was taken on a move from a move code
        private Piece getTakenPieceFromCode(String moveCode)
        {
            Piece revivedPiece = null;
            if (moveCode[4] == 'R')
            {
                switch (moveCode[5])
                {
                    case 'R':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = redChariot1;
                                break;
                            default:
                                revivedPiece = redChariot2;
                                break;
                        }
                        break;
                    case 'C':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = redCannon1;
                                break;
                            default:
                                revivedPiece = redCannon2;
                                break;
                        }
                        break;
                    case 'E':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = redElephant1;
                                break;
                            default:
                                revivedPiece = redElephant2;
                                break;
                        }
                        break;
                    case 'A':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = redGuard1;
                                break;
                            default:
                                revivedPiece = redGuard2;
                                break;
                        }
                        break;
                    case 'H':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = redHorse1;
                                break;
                            default:
                                revivedPiece = redHorse2;
                                break;
                        }
                        break;
                    case 'P':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = redSoldier1;
                                break;
                            case '2':
                                revivedPiece = redSoldier2;
                                break;
                            case '3':
                                revivedPiece = redSoldier3;
                                break;
                            case '4':
                                revivedPiece = redSoldier4;
                                break;
                            default:
                                revivedPiece = redSoldier5;
                                break;
                        }
                        break;
                    default:
                        revivedPiece = redGeneral;
                        break;
                }
            }
            if (moveCode[4] == 'B')
            {
                switch (moveCode[5])
                {
                    case 'R':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = blackChariot1;
                                break;
                            default:
                                revivedPiece = blackChariot2;
                                break;
                        }
                        break;
                    case 'C':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = blackCannon1;
                                break;
                            default:
                                revivedPiece = blackCannon2;
                                break;
                        }
                        break;
                    case 'E':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = blackElephant1;
                                break;
                            default:
                                revivedPiece = blackElephant2;
                                break;
                        }
                        break;
                    case 'A':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = blackGuard1;
                                break;
                            default:
                                revivedPiece = blackGuard2;
                                break;
                        }
                        break;
                    case 'H':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = blackHorse1;
                                break;
                            default:
                                revivedPiece = blackHorse2;
                                break;
                        }
                        break;
                    case 'P':
                        switch (moveCode[6])
                        {
                            case '1':
                                revivedPiece = blackSoldier1;
                                break;
                            case '2':
                                revivedPiece = blackSoldier2;
                                break;
                            case '3':
                                revivedPiece = blackSoldier3;
                                break;
                            case '4':
                                revivedPiece = blackSoldier4;
                                break;
                            default:
                                revivedPiece = blackSoldier5;
                                break;
                        }
                        break;
                    default:
                        revivedPiece = blackGeneral;
                        break;
                }
            }
            return revivedPiece;
        }
    }
}
