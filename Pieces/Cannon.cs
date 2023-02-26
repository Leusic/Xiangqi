using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class Cannon : Piece
    {
        public Cannon(int x, int y, int teamModifier)
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
            bool screen = false;

            for (int i = 1; i < 10; i++)
            {
                if (this.y + i < 10 && this.y + i >= 0)
                {
                    if (board.grid[this.x, this.y + i].occupied == true && board.grid[this.x, this.y + i].piece.teamModifier != this.teamModifier && screen == true)
                    {
                        moveCheck(ref board, tempBoard, 0, i);
                        break;
                    }
                    if (board.grid[this.x, this.y + i].occupied == true)
                    {
                        screen = true;
                    }
                    else if (board.grid[this.x, this.y + i].occupied != true && screen == false)
                    {
                        moveCheck(ref board, tempBoard, 0, i);
                    }
                    if (board.grid[this.x, this.y + i].occupied != true && screen == true)
                    {
                        ;
                    }
                }
            }
            screen = false;
            for (int i = 1; i < 10; i++)
            {
                if (this.y - i < 10 && this.y - i >= 0)
                {
                    if (board.grid[this.x, this.y - i].occupied == true && board.grid[this.x, this.y - i].piece.teamModifier != this.teamModifier && screen == true)
                    {
                        moveCheck(ref board, tempBoard, 0, -i);
                        break;
                    }
                    if (board.grid[this.x, this.y - i].occupied == true)
                    {
                        screen = true;
                    }
                    else if (board.grid[this.x, this.y - i].occupied != true && screen == false)
                    {
                        moveCheck(ref board, tempBoard, 0, -i);
                    }
                    if (board.grid[this.x, this.y - i].occupied != true && screen == true)
                    {
                        ;
                    }             
                }
            }
            screen = false;
            for (int i = 1; i < 10; i++)
            {
                if (this.x + i < 9 && this.x + i >= 0)
                {
                    if (board.grid[this.x + i, this.y].occupied == true && board.grid[this.x + i, this.y].piece.teamModifier != this.teamModifier && screen == true)
                    {
                        moveCheck(ref board, tempBoard, i, 0);
                        break;
                    }
                    if (board.grid[this.x + i, this.y].occupied == true)
                    {
                        screen = true;
                    }
                    else if (board.grid[this.x + i, this.y].occupied != true && screen == false)
                    {
                        moveCheck(ref board, tempBoard, i, 0);
                    }
                    if (board.grid[this.x + i, this.y].occupied != true && screen == true)
                    {
                        ;
                    }
                }
            }
            screen = false;
            for (int i = 1; i < 10; i++)
            {
                if (this.x - i < 9 && this.x - i >= 0)
                {
                    if (board.grid[this.x - i, this.y].occupied == true && board.grid[this.x - i, this.y].piece.teamModifier != this.teamModifier && screen == true)
                    {
                        moveCheck(ref board, tempBoard, -i, 0);
                        break;
                    }
                    if (board.grid[this.x - i, this.y].occupied == true)
                    {
                        screen = true;
                    }
                    else if (board.grid[this.x - i, this.y].occupied != true && screen == false)
                    {
                        moveCheck(ref board, tempBoard, -i, 0);
                    }
                    if (board.grid[this.x - i, this.y].occupied != true && screen == true)
                    {
                        ;
                    }
                }
            }
            return tempBoard;
        }
    }
}
    
