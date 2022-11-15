using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xiangqi
{
    public partial class Form1 : Form
    {
        Board board = new Board();
        Soldier redSoldier1 = new Soldier(0,6,-1);
        Soldier redSoldier2 = new Soldier(2, 6, -1);
        PictureBox[,] movementIcons = new PictureBox[9, 10];
        Dictionary<Piece,PictureBox> allPieces = new Dictionary<Piece,PictureBox>();

        public Form1()
        {
            //board pieces offset to match the board is (28,33) and there is 75 pixels between board positions.
            InitializeComponent();
            initialisePiece(redSoldier1, RedSoldier1, 0, 6);
            initialisePiece(redSoldier2, RedSoldier2, 2, 6);

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

        private void initialiseMovementIcon(int i, int z, int x, int y)
        {
            movementIcons[i, z] = new PictureBox();
            movementIcons[i, z].Height = 25;
            movementIcons[i, z].Width = 25;
            movementIcons[i, z].Location = new Point(x, y);
            movementIcons[i, z].Parent = BoardImage;
            movementIcons[i, z].Image = Image.FromFile(@"C:\Users\Max\Documents\Y3 Uni\Honours Stage Project\Code\Xiangqi\Images\greenDot.png");
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ShowMoves(object sender, EventArgs e, Piece piece, PictureBox pictureBox)
        {
            UnshowMoves();
            //RedSoldier1.Top += 75;
            bool[,] moveBoard;
            moveBoard = piece.legalMoves(board);
            for (int i = 0; i < 9; i++)
            {
                for(int z = 0; z < 10; z++)
                {
                    if(moveBoard[i,z] == true)
                    {
                        int a = i;
                        int b = z;
                        //removes event handlers of previously used buttons so that multiple pieces dont move at once.
                        int x = movementIcons[i, z].Location.X;
                        int y = movementIcons[i, z].Location.Y;
                        movementIcons[i, z].Dispose();
                        movementIcons[i, z] = null;
                        initialiseMovementIcon(i,z,x,y);
                        movementIcons[i, z].BringToFront();
                        movementIcons[i, z].MouseClick += (sender, EventArgs) => { MoveUnit(sender, EventArgs, a, b, piece, pictureBox); };
                    }
                }
            }
        }

        private void UnshowMoves()
        {
            foreach (KeyValuePair<Piece, PictureBox> pair in allPieces)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        movementIcons[i, z].SendToBack();
                        movementIcons[i, z].MouseClick -= (sender, EventArgs) => { MoveUnit(sender, EventArgs, i, z, pair.Key, pair.Value); };
                    }
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
            piece.x = x;
            piece.y = y;
            board.grid[x, y].occupied = true;
            board.grid[x, y].piece = piece;
            pictureBox.Left += xDiff * 75;
            pictureBox.Top += yDiff * 75;
            UnshowMoves();
        }
    }
}
