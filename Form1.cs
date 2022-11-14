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
        PictureBox[,] movementIcons = new PictureBox[9, 10];

        public Form1()
        {
            //board pieces offset to match the board is (28,33) and there is 75 pixels between board positions.
            InitializeComponent();
            RedSoldier1.Parent = BoardImage;
            RedSoldier1.BackColor = Color.Transparent;
            //480
            RedSoldier1.Location = new Point(28, 480);
            RedSoldier1.MouseClick += (sender, EventArgs) => { ShowMoves(sender, EventArgs, redSoldier1, RedSoldier1); }; ;
            board.grid[0, 6].occupied = true;
            board.grid[0, 6].piece = redSoldier1;

            int iconX = 47;
            int iconY = 47;
            for(int i = 0; i < 9; i++)
            {
                for(int z = 0; z < 10;  z++)
                {
                    movementIcons[i, z] = new PictureBox();
                    movementIcons[i, z].Height = 25;
                    movementIcons[i, z].Width = 25;
                    movementIcons[i, z].Location = new Point(iconX, iconY);
                    movementIcons[i, z].Parent = BoardImage;
                    movementIcons[i, z].Image = Image.FromFile(@"C:\Users\Max\Documents\Y3 Uni\Honours Stage Project\Code\Xiangqi\Images\greenDot.png");
                    movementIcons[i, z].BackColor = Color.White;
                    this.Controls.Add(movementIcons[i, z]);
                    movementIcons[i, z].SendToBack();
                    iconY += 75;
                }
                iconY = 47;
                iconX += 75;
            }
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
            //RedSoldier1.Top += 75;
            bool[,] moveBoard;
            moveBoard = piece.legalMoves(board);
            for (int i = 0; i < 9; i++)
            {
                for(int z = 0; z < 10; z++)
                {
                    if(moveBoard[i,z] == true)
                    {
                        movementIcons[i,z].BringToFront();
                        int a = i;
                        int b = z;
                        movementIcons[i, z].MouseClick += (sender, EventArgs) => { MoveUnit(sender, EventArgs, a, b, piece, pictureBox); };
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
            int xDiff = x - piece.x;
            int yDiff = y - piece.y;
            piece.x = x;
            piece.y = y;
            pictureBox.Left += xDiff * 75;
            pictureBox.Top += yDiff * 75;
            UnshowMoves();
        }
    }
}
