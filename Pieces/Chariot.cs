using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Xiangqi
{
    class Chariot : Piece
    {
        public Chariot(int x, int y, int teamModifier)
        {
            canCrossRiver = true;
            crossedRiver = false;
            this.x = x;
            this.y = y;
            this.teamModifier = teamModifier;
            this.alive = true;
            this.moveBoard = new int[9, 10];
        }

        public override int[,] legalMoves(ref Board board, ref Dictionary<Piece, PictureBox> allPieces)
        {
            this.moveBoard = new int[9, 10];
            for (int i = 1; i < 10; i++)
            {
                if (this.y + i < 10 && this.y + i >= 0)
                {
                    if (board.grid[this.x, this.y + i].occupied != true)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, 0, i);
                    }
                    else if (board.grid[this.x, this.y + i].piece.teamModifier != this.teamModifier)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, 0, i);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 1; i < 10; i++)
            {
                if (this.y - i < 10 && this.y - i >= 0)
                {
                    if (board.grid[this.x, this.y - i].occupied != true)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, 0, -i);
                    }
                    else if (board.grid[this.x, this.y - i].piece.teamModifier != this.teamModifier)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, 0, -i);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 1; i < 9; i++)
            {
                if (this.x + i < 9 && this.x + i >= 0)
                {
                    if (board.grid[this.x + i, this.y].occupied != true)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, i, 0);
                    }
                    else if (board.grid[this.x + i, this.y].piece.teamModifier != this.teamModifier)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, i, 0);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 1; i < 9; i++)
            {
                if (this.x - i < 9 && this.x - i >= 0)
                {

                    if (board.grid[this.x - i, this.y].occupied != true)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, -i, 0);
                    }
                    else if (board.grid[this.x - i, this.y].piece.teamModifier != this.teamModifier)
                    {
                        moveCheck(ref board, ref allPieces, this.moveBoard, -i, 0);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return this.moveBoard;
        }

        public override int[,] legalMovesBasic(ref Board board)
        {
            this.moveBoard = new int[9, 10];
            for (int i = 1; i < 10; i++)
            {
                if (this.y + i < 10 && this.y + i >= 0)
                {
                    if (board.grid[this.x, this.y + i].occupied != true)
                    {
                        moveCheckBasic(ref board, this.moveBoard, 0, i);
                    }
                    else if (board.grid[this.x, this.y + i].piece.teamModifier != this.teamModifier)
                    {
                        moveCheckBasic(ref board, this.moveBoard, 0, i);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 1; i < 10; i++)
            {
                if (this.y - i < 10 && this.y - i >= 0)
                {
                    if (board.grid[this.x, this.y - i].occupied != true)
                    {
                        moveCheckBasic(ref board, this.moveBoard, 0, -i);
                    }
                    else if (board.grid[this.x, this.y - i].piece.teamModifier != this.teamModifier)
                    {
                        moveCheckBasic(ref board, this.moveBoard, 0, -i);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 1; i < 9; i++)
            {
                if (this.x + i < 9 && this.x + i >= 0)
                {
                    if (board.grid[this.x + i, this.y].occupied != true)
                    {
                        moveCheckBasic(ref board, this.moveBoard, i, 0);
                    }
                    else if (board.grid[this.x + i, this.y].piece.teamModifier != this.teamModifier)
                    {
                        moveCheckBasic(ref board, this.moveBoard, i, 0);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            for (int i = 1; i < 9; i++)
            {
                if (this.x - i < 9 && this.x - i >= 0)
                {

                    if (board.grid[this.x - i, this.y].occupied != true)
                    {
                        moveCheckBasic(ref board, this.moveBoard, -i, 0);
                    }
                    else if (board.grid[this.x - i, this.y].piece.teamModifier != this.teamModifier)
                    {
                        moveCheckBasic(ref board, this.moveBoard, -i, 0);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return this.moveBoard;
        }
    }
}
