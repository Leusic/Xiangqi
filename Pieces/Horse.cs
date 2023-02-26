using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class Horse : Piece
    {
        public Horse(int x, int y, int teamModifier)
        {
            canCrossRiver = true;
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
            if (this.x - 1 < 9 && this.y + 2 < 10 && this.x - 1 >= 0 && this.y + 2 >= 0)
            {
                if (board.grid[this.x, this.y + 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, -1, 2);
                }
            }
            if (this.x + 1 < 9 && this.y + 2 < 10 && this.x + 1 >= 0 && this.y + 2 >= 0)
            {
                if (board.grid[this.x, this.y + 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, 1, 2);
                }
            }
            if (this.x + 2 < 9 && this.y + 1 < 10 && this.x + 2 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y].occupied != true)
                {
                    moveCheck(ref board, tempBoard, 2, 1);
                }
            }
            if (this.x + 2 < 9 && this.y - 1 < 10 && this.x + 2 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y].occupied != true)
                {
                    moveCheck(ref board, tempBoard, 2, -1);
                }
            }
            if (this.x + 1 < 9 && this.y - 2 < 10 && this.x + 1 >= 0 && this.y - 2 >= 0)
            {
                if (board.grid[this.x, this.y - 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, 1, -2);
                }
            }
            if (this.x - 1 < 9 && this.y - 2 < 10 && this.x - 1 >= 0 && this.y - 2 >= 0)
            {
                if (board.grid[this.x, this.y - 1].occupied != true)
                {
                    moveCheck(ref board, tempBoard, -1, -2);
                }
            }
            if (this.x - 2 < 9 && this.y - 1 < 10 && this.x - 2 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y].occupied != true)
                {
                    moveCheck(ref board, tempBoard, -2, -1);
                }
            }
            if (this.x - 2 < 9 && this.y + 1 < 10 && this.x - 2 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y].occupied != true)
                {
                    moveCheck(ref board, tempBoard, -2, 1);
                }
            }
            return tempBoard;
        }
    }
}
