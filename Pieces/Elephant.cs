using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class Elephant : Piece
    {
        public Elephant(int x, int y, int teamModifier)
        {
            canCrossRiver = false;
            crossedRiver = false;
            this.x = x;
            this.y = y;
            this.teamModifier = teamModifier;
            this.alive = true;
        }

        public override bool[,] legalMoves(ref Board board)
        {
            bool[,] tempBoard;
            tempBoard = new bool[9, 10];
            if(this.x + 1 < 9 && this.y + 1 < 10 && this.x + 1 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y + 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, 2, 2);
                }
            }
            if (this.x + 1 < 9 && this.y - 1 < 10 && this.x + 1 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y - 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, 2, -2);
                }
            }
            if (this.x - 1 < 9 && this.y + 1 < 10 && this.x - 1 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y + 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, -2, 2);
                }
            }
            if (this.x - 1 < 9 && this.y - 1 < 10 && this.x - 1 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y - 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, -2, -2);
                }
            }
            return tempBoard;
        }
    }
}
