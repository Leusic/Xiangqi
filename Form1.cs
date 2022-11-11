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

        public Form1()
        {
            //board pieces offset to match the board is (28,33) and there is 75 pixels between board positions.
            InitializeComponent();
            pictureBox2.Parent = BoardImage;
            pictureBox2.BackColor = Color.Transparent;
            RedSoldier1.Parent = BoardImage;
            RedSoldier1.BackColor = Color.Transparent;
            RedSoldier1.Location = new Point(28, 480);
            RedSoldier1.MouseClick += new MouseEventHandler(MoveUnit);
            board.grid[0, 6].occupied = true;
            board.grid[0, 6].piece = redSoldier1;
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

        private void MoveUnit(object sender, EventArgs e)
        {
            //RedSoldier1.Top += 75;
            bool[,] moveBoard;
            moveBoard = redSoldier1.legalMoves(board);
            //counts how many icons need to be displayed.
            int count = 0;
            for(int i = 0; i < 9; i++)
            {
                for(int z = 0; z < 10; z++)
                {
                    if(moveBoard[i,z] == true)
                    {
                        count++;
                    }
                }
            }
            PictureBox moveIcon = new PictureBox();
        }
    }
}
