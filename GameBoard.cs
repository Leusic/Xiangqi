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

namespace Xiangqi
{
    public partial class gameBoard : Form
    {
        Board board = new Board();

        //piece definitions
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

        Dictionary<Piece, PictureBox> redGraveyard = new Dictionary<Piece, PictureBox>();
        Dictionary<Piece, PictureBox> blackGraveyard = new Dictionary<Piece, PictureBox>();


        PictureBox[,] movementIcons = new PictureBox[9, 10];
        PictureBox[,] movementCrosses = new PictureBox[9, 10];
        Dictionary<Piece, PictureBox> allPieces = new Dictionary<Piece, PictureBox>();
        Dictionary<String, Piece> nameToPiece = null;

        List<String> moveLog = new List<String>();

        bool redInCheck = false;
        bool blackInCheck = false;

        public gameBoard(Save loadedSave, int modeCode)
        {
            InitializeComponent();

            allPieces = new Dictionary<Piece, PictureBox>
            {
                {redSoldier1 , RedSoldier1}, {redSoldier2, RedSoldier2}, {redSoldier3, RedSoldier3}, {redSoldier4, RedSoldier4}, {redSoldier5, RedSoldier5}, {redGeneral, RedGeneral}, {redGuard1, RedGuard1}, {redGuard2, RedGuard2},
                {redElephant1, RedElephant1 }, {redElephant2, RedElephant2}, {redHorse1, RedHorse1}, {redHorse2, RedHorse2}, {redChariot1, RedChariot1}, {redChariot2, RedChariot2}, {redCannon1, RedCannon1}, {redCannon2, RedCannon2},
                {blackSoldier1 , BlackSoldier1}, {blackSoldier2, BlackSoldier2}, {blackSoldier3, BlackSoldier3}, {blackSoldier4, BlackSoldier4}, {blackSoldier5, BlackSoldier5}, {blackGeneral, BlackGeneral}, {blackGuard1, BlackGuard1}, {blackGuard2, BlackGuard2},
                {blackElephant1, BlackElephant1 }, {blackElephant2, BlackElephant2}, {blackHorse1, BlackHorse1}, {blackHorse2, BlackHorse2}, {blackChariot1, BlackChariot1}, {blackChariot2, BlackChariot2}, {blackCannon1, BlackCannon1}, {blackCannon2, BlackCannon2},
            };

            //maps piece names to picture boxes, used for loading saves
            nameToPiece = new Dictionary<String, Piece>
            {
                {"redSoldier1", redSoldier1}, {"redSoldier2", redSoldier2 }, {"redSoldier3", redSoldier3}, {"redSoldier4", redSoldier4 }, {"redSoldier5", redSoldier5}, {"redGeneral1", redGeneral}, {"redGuard1", redGuard1}, {"redGuard2", redGuard2},
                {"redElephant1", redElephant1 }, {"redElephant2", redElephant2}, {"redHorse1", redHorse1}, {"redHorse2", redHorse2}, {"redChariot1", redChariot1}, {"redChariot2", redChariot2}, {"redCannon1", redCannon1}, {"redCannon2", redCannon2},
                {"blackSoldier1", blackSoldier1}, {"blackSoldier2", blackSoldier2 }, {"blackSoldier3", blackSoldier3}, {"blackSoldier4", blackSoldier4 }, {"blackSoldier5", blackSoldier5}, {"blackGeneral1", blackGeneral}, {"blackGuard1", blackGuard1}, {"blackGuard2", blackGuard2},
                {"blackElephant1", blackElephant1 }, {"blackElephant2", blackElephant2}, {"blackHorse1", blackHorse1}, {"blackHorse2", blackHorse2}, {"blackChariot1", blackChariot1}, {"blackChariot2", blackChariot2}, {"blackCannon1", blackCannon1}, {"blackCannon2", blackCannon2}
            };


            //if loading saved game
            if(loadedSave != null)
            {
                foreach(Piece i in loadedSave.pieces)
                {
                    Piece currentPiece = nameToPiece[i.name];
                    currentPiece.x = i.x;
                    currentPiece.y = i.y;
                    currentPiece.alive = i.alive;
                    currentPiece.name = i.name;

                    //
                    if((currentPiece.x > 10) || (currentPiece.y > 10))
                    {
                        currentPiece.x = 0;
                        currentPiece.y = 0;
                    }
                    initialisePiece(currentPiece, allPieces[currentPiece], currentPiece.x, currentPiece.y, currentPiece.name);
                    if(currentPiece.alive == false)
                    {
                        currentPiece.x = 99; currentPiece.y = 99;
                        if(currentPiece.teamModifier == -1)
                        {
                            redGraveyard.Add(currentPiece, allPieces[currentPiece]);
                        }
                        if (currentPiece.teamModifier == 1)
                        {
                            blackGraveyard.Add(currentPiece, allPieces[currentPiece]);
                        }

                    }
                }
                board.currentTurn = loadedSave.currentTurn;
                if(board.currentTurn == 1)
                {
                    board.currentTurn = -1;
                    updateTurn();
                }
                if (board.currentTurn == -1){
                    board.currentTurn = 1;
                    updateTurn();
                }
                moveLog = loadedSave.rollbackMoves;
                updateMoveDisplay();

            }
            //if starting new game
            else
            {
                board.currentTurn = -1;
                //initialising pieces
                //board pieces offset to match the board is (28,33) and there is 75 pixels between board positions.
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
                moveLog.Add("-");
            }

            int iconX = 47;
            int iconY = 47;
            for (int i = 0; i < 9; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    initialiseMovementCross(i, z, iconX, iconY);
                    movementCrosses[i, z].Visible = false;
                    initialiseMovementIcon(i, z, iconX, iconY);
                    movementIcons[i, z].Visible = false;
                    iconY += 75;
                }
                iconY = 47;
                iconX += 75;
            }
            updateGraveyard();
        }

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

        private void MenuButtonClick(object sender, EventArgs e)
        {
            MainMenu mainMenu = new MainMenu();
            this.Hide();
            mainMenu.ShowDialog();
            this.Close();
        }

        private void gameOver()
        {
            CheckmateTextbox.BringToFront();
            WinningTeamTextbox.BringToFront();
            MenuButton.BringToFront();
            TurnTextbox.Text = "Game Over!";
            TurnTextbox.BackColor = Color.Tan;
            CheckTextbox.SendToBack();
            foreach (KeyValuePair<Piece, PictureBox> i in allPieces)
            {
                i.Key.alive = false;
            }
        }

        private void unGameOver()
        {
            CheckmateTextbox.SendToBack();
            WinningTeamTextbox.SendToBack();
            MenuButton.SendToBack();
            CheckTextbox.BringToFront();
        }

        private void checkScan()
        {
            redInCheck = false; blackInCheck = false;
            CheckTextbox.Text = "Neither team in check";
            bool possibleMove = false;
            //is black in check
            //checks if any enemy team units legal movements can hit the friendly general, if they can this move is illegal due to placing self into check
            foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if ((i.Key.teamModifier == -1) && (i.Key.legalMovesBasic(ref board)[blackGeneral.x, blackGeneral.y] == 2))
                {
                    blackInCheck = true;
                    CheckTextbox.Text = "Black in check!";
                    possibleMove = false;
                    foreach (KeyValuePair<Piece, PictureBox> z in allPieces) if (z.Key.teamModifier == 1)
                        {
                            int[,] moveBoard = z.Key.legalMoves(ref board, ref allPieces);
                            for(int a = 0; a < 8; a++)
                            {
                                for(int b = 0; b < 9; b++)
                                {
                                    if (moveBoard[a, b] == 2)
                                    {
                                        possibleMove = true;
                                    }
                                }
                            }
                        }
                    if(possibleMove == false)
                    {
                        WinningTeamTextbox.Text = "Red Wins!";
                        gameOver();
                    }
                    break;
                }
            //is red in check
            foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if ((i.Key.teamModifier == 1) && (i.Key.legalMovesBasic(ref board)[redGeneral.x, redGeneral.y] == 2))
                {
                    redInCheck = true;
                    CheckTextbox.Text = "Red in check!";
                    possibleMove = false;
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
                    if(possibleMove == false)
                    {
                        WinningTeamTextbox.Text = "Black Wins!";
                        gameOver();
                    }
                    break;
                }
        }

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

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void ShowMoves(object sender, EventArgs e, Piece piece, PictureBox pictureBox)
        {
            if (piece.alive == true)
            {
                if (piece.teamModifier == board.currentTurn)
                {
                    UnshowMoves();
                    int[,] moveBoard;
                    moveBoard = piece.legalMoves(ref board, ref allPieces);
                    for (int i = 0; i < 9; i++)
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            if (moveBoard[i, z] == 2)
                            {
                                int a = i;
                                int b = z;
                                //removes event handlers of previously used buttons so that multiple pieces dont move at once.
                                int x = movementIcons[i, z].Location.X;
                                int y = movementIcons[i, z].Location.Y;
                                movementIcons[i, z].Dispose();
                                movementIcons[i, z] = null;
                                initialiseMovementIcon(i, z, x, y);
                                movementIcons[i, z].Visible = true;
                                movementIcons[i, z].BringToFront();
                                movementIcons[i, z].MouseClick += (sender, EventArgs) => { MoveUnit(sender, EventArgs, a, b, piece, pictureBox); };
                            }
                            if (moveBoard[i,z] == 1)
                            {
                                int a = i;
                                int b = z;
                                //removes event handlers of previously used buttons so that multiple pieces dont move at once.
                                int x = movementCrosses[i, z].Location.X;
                                int y = movementCrosses[i, z].Location.Y;
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

        //converts movement strings to the corresponding piece and the x and y movement it made (for going backwards through moves)
        //move strings are formatted as the x and y the piece started at and the x and y the piece ended at
        private List<char> getPieceCode(Piece piece)
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

        private void MoveUnit(object sender, EventArgs e, int x, int y, Piece piece, PictureBox pictureBox)
        {
            //characters for logging the move
            List<char> moveChars = new List<char>();
            moveChars.Add((char)(piece.x + 65));
            moveChars.Add((char)(9 - piece.y + '0'));
            moveChars.Add((char)(x + 65));
            moveChars.Add((char)(9 - y + '0'));

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
                moveChars.AddRange(getPieceCode(board.grid[x, y].piece));
                updateGraveyard();
            }
            piece.x = x;
            piece.y = y;
            if (board.grid[x,y].occupied == true)
            {
                board.grid[x, y].piece.x = 99;
                board.grid[x, y].piece.y = 99;
            }
            board.grid[x, y].occupied = true;
            board.grid[x, y].piece = piece;
            pictureBox.Left += xDiff * 75;
            pictureBox.Top += yDiff * 75;
            UnshowMoves();
            string moveString = new string(moveChars.ToArray());
            moveLog.Add(moveString);
            updateMoveDisplay();
            updateTurn();
            checkScan();
            saveGameStatusLabel.Text = "";
        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void RollbackButton_Click(object sender, EventArgs e)
        {
            try
            {
                String rollbackMove = moveLog[moveLog.Count - 1];
                int startX = rollbackMove[0] - 65;
                int startY = rollbackMove[1] - '0';
                startY = 9 - startY;
                int endX = rollbackMove[2] - 65;
                int endY = rollbackMove[3] - '0';
                endY = 9 - endY;
                Piece pieceToMove = board.grid[endX, endY].piece;
                int xDiff = endX - startX;
                int yDiff = endY - startY;
                PictureBox pictureBoxToMove = allPieces[pieceToMove];

                pieceToMove.x = startX;
                pieceToMove.y = startY;
                board.grid[startX, startY].occupied = true;
                board.grid[startX, startY].piece = pieceToMove;
                pictureBoxToMove.Left -= xDiff * 75;
                pictureBoxToMove.Top -= yDiff * 75;

                //if a piece was taken on the move in question
                if (rollbackMove.Length > 4)
                {
                    Piece revivedPiece = getTakenPieceFromCode(rollbackMove);
                    PictureBox revivedPictureBox = allPieces[revivedPiece];
                    revivedPiece.alive = true;
                    if (revivedPiece.teamModifier == -1)
                    {
                        redGraveyard.Remove(revivedPiece);
                    }
                    else
                    {
                        blackGraveyard.Remove(revivedPiece);
                    }
                    revivedPictureBox.Top = 33;
                    revivedPictureBox.Left = 28;
                    revivedPictureBox.Top += endY * 75;
                    revivedPictureBox.Left += endX * 75;

                    board.grid[endX, endY].piece = revivedPiece;
                }
                else
                {
                    board.grid[endX, endY].occupied = false;
                    board.grid[endX, endY].piece = null;
                }
                moveLog.Remove(rollbackMove);
                updateMoveDisplay();
                updateTurn();
                CheckTextbox.BringToFront();
                UnshowMoves();
                checkScan();
                saveGameStatusLabel.Text = "";
            }
            catch { }
        }

        private void saveGameButton_Click(object sender, EventArgs e)
        {
            //uses xml serialisation to serialize the board and save it to a file
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
                if(writer != null)
                {
                    writer.Close();
                }

            }
        }

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
