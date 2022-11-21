using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xiangqi.Properties;

namespace Xiangqi
{
    public partial class Form1 : Form
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

        int currentTurn = -1;
        Dictionary<Piece,PictureBox> redGraveyard = new Dictionary<Piece, PictureBox>();
        Dictionary<Piece, PictureBox> blackGraveyard = new Dictionary<Piece, PictureBox>();


        PictureBox[,] movementIcons = new PictureBox[9, 10];
        Dictionary<Piece,PictureBox> allPieces = new Dictionary<Piece,PictureBox>();

        public Form1()
        {
            InitializeComponent();
            //initialising pieces
            //board pieces offset to match the board is (28,33) and there is 75 pixels between board positions.
            initialisePiece(redSoldier1, RedSoldier1, 0, 6);
            initialisePiece(redSoldier2, RedSoldier2, 2, 6);
            initialisePiece(redSoldier3, RedSoldier3, 4, 6);
            initialisePiece(redSoldier4, RedSoldier4, 6, 6);
            initialisePiece(redSoldier5, RedSoldier5, 8, 6);
            initialisePiece(redGeneral, RedGeneral, 4, 9);
            initialisePiece(redGuard1, RedGuard1, 3, 9);
            initialisePiece(redGuard2, RedGuard2, 5, 9);
            initialisePiece(redElephant1, RedElephant1, 2, 9);
            initialisePiece(redElephant2, RedElephant2, 6, 9);
            initialisePiece(redHorse1, RedHorse1, 1, 9);
            initialisePiece(redHorse2, RedHorse2, 7, 9);
            initialisePiece(redChariot1, RedChariot1, 0, 9);
            initialisePiece(redChariot2, RedChariot2, 8, 9);
            initialisePiece(redCannon1, RedCannon1, 1, 7);
            initialisePiece(redCannon2, RedCannon2, 7, 7);

            initialisePiece(blackSoldier1, BlackSoldier1, 0, 3);
            initialisePiece(blackSoldier2, BlackSoldier2, 2, 3);
            initialisePiece(blackSoldier3, BlackSoldier3, 4, 3);
            initialisePiece(blackSoldier4, BlackSoldier4, 6, 3);
            initialisePiece(blackSoldier5, BlackSoldier5, 8, 3);
            initialisePiece(blackGeneral, BlackGeneral, 4, 0);
            initialisePiece(blackGuard1, BlackGuard1, 3, 0);
            initialisePiece(blackGuard2, BlackGuard2, 5, 0);
            initialisePiece(blackElephant1, BlackElephant1, 2, 0);
            initialisePiece(blackElephant2, BlackElephant2, 6, 0);
            initialisePiece(blackHorse1, BlackHorse1, 1, 0);
            initialisePiece(blackHorse2, BlackHorse2, 7, 0);
            initialisePiece(blackChariot1, BlackChariot1, 0, 0);
            initialisePiece(blackChariot2, BlackChariot2, 8, 0);
            initialisePiece(blackCannon1, BlackCannon1, 1, 2);
            initialisePiece(blackCannon2, BlackCannon2, 7, 2);

            int iconX = 47;
            int iconY = 47;
            for(int i = 0; i < 9; i++)
            {
                for(int z = 0; z < 10;  z++)
                {
                    initialiseMovementIcon(i, z, iconX, iconY);
                    movementIcons[i, z].SendToBack();
                    iconY += 75;
                }
                iconY = 47;
                iconX += 75;
            }
        }

        private void updateTurn()
        {
            currentTurn = -currentTurn;
            if (currentTurn == -1)
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

        private void updateGraveyard()
        {
            int count = 0;
            int y = 33;
            int x = 745;
            foreach(KeyValuePair<Piece,PictureBox> i in blackGraveyard)
            {
                if(count > 3)
                {
                    count = 0;
                    x += 75;
                    y = 33;
                }
                i.Value.Location = new Point(x,y);
                y += 75;
                count++;
            }
            count = 0;
            y = 433;
            x = 745;
            foreach (KeyValuePair < Piece,PictureBox> i in redGraveyard)
            {
                if (count > 3)
                {
                    count = 0;
                    x += 75;
                    y = 433;
                }
                i.Value.Location = new Point(x,y);
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
            movementIcons[i, z].BackColor = Color.White;
            this.Controls.Add(movementIcons[i, z]);
        }

        private void initialisePiece(Piece piece, PictureBox pictureBox, int x, int y)
        {
            pictureBox.Parent = BoardImage;
            pictureBox.BackColor = Color.Transparent;
            board.grid[x, y].occupied = true;
            board.grid[x, y].piece = piece;
            pictureBox.Location = new Point(28 + (x * 75), 33 + (y * 75));
            pictureBox.MouseClick += (sender, EventArgs) => { ShowMoves(sender, EventArgs, piece, pictureBox); }; ;
            allPieces.Add(piece,pictureBox);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Board_Click(object sender, EventArgs e)
        {

        }

        private void ShowMoves(object sender, EventArgs e, Piece piece, PictureBox pictureBox)
        {
            if (piece.teamModifier == currentTurn) {
                UnshowMoves();
                //RedSoldier1.Top += 75;
                bool[,] moveBoard;
                moveBoard = piece.legalMoves(board);
                for (int i = 0; i < 9; i++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if (moveBoard[i, z] == true)
                        {
                            int a = i;
                            int b = z;
                            //removes event handlers of previously used buttons so that multiple pieces dont move at once.
                            int x = movementIcons[i, z].Location.X;
                            int y = movementIcons[i, z].Location.Y;
                            movementIcons[i, z].Dispose();
                            movementIcons[i, z] = null;
                            initialiseMovementIcon(i, z, x, y);
                            movementIcons[i, z].BringToFront();
                            movementIcons[i, z].MouseClick += (sender, EventArgs) => { MoveUnit(sender, EventArgs, a, b, piece, pictureBox); };
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
                    movementIcons[i, z].SendToBack();
                }
            }
        }

        private void MoveUnit(object sender, EventArgs e, int x, int y, Piece piece, PictureBox pictureBox)
        {
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
            if(board.grid[x,y].occupied == true)
            {
                board.grid[x, y].piece.alive = false;
                if(board.grid[x,y].piece.teamModifier == -1)
                {
                    redGraveyard.Add(board.grid[x, y].piece, allPieces[board.grid[x, y].piece]);
                }
                else
                {
                    blackGraveyard.Add(board.grid[x, y].piece, allPieces[board.grid[x, y].piece]);
                }
                updateGraveyard();
            }
            piece.x = x;
            piece.y = y;
            board.grid[x, y].occupied = true;
            board.grid[x, y].piece = piece;
            pictureBox.Left += xDiff * 75;
            pictureBox.Top += yDiff * 75;
            UnshowMoves();
            updateTurn();
        }
    }
}
