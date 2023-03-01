using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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
            this.moveBoard = new int[9, 10];
        }

        public override int[,] legalMoves(ref Board board, ref Dictionary<Piece, PictureBox> allPieces)
        {
            this.moveBoard = new int[9, 10];
            if (this.x - 1 < 9 && this.y + 2 < 10 && this.x - 1 >= 0 && this.y + 2 >= 0)
            {
                if (board.grid[this.x, this.y + 1].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, -1, 2);
                }
            }
            if (this.x + 1 < 9 && this.y + 2 < 10 && this.x + 1 >= 0 && this.y + 2 >= 0)
            {
                if (board.grid[this.x, this.y + 1].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, 1, 2);
                }
            }
            if (this.x + 2 < 9 && this.y + 1 < 10 && this.x + 2 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, 2, 1);
                }
            }
            if (this.x + 2 < 9 && this.y - 1 < 10 && this.x + 2 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, 2, -1);
                }
            }
            if (this.x + 1 < 9 && this.y - 2 < 10 && this.x + 1 >= 0 && this.y - 2 >= 0)
            {
                if (board.grid[this.x, this.y - 1].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, 1, -2);
                }
            }
            if (this.x - 1 < 9 && this.y - 2 < 10 && this.x - 1 >= 0 && this.y - 2 >= 0)
            {
                if (board.grid[this.x, this.y - 1].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, -1, -2);
                }
            }
            if (this.x - 2 < 9 && this.y - 1 < 10 && this.x - 2 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, -2, -1);
                }
            }
            if (this.x - 2 < 9 && this.y + 1 < 10 && this.x - 2 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y].occupied != true)
                {
                    moveCheck(ref board, ref allPieces, this.moveBoard, -2, 1);
                }
            }
            return this.moveBoard;
        }

        public override int[,] legalMovesBasic(ref Board board)
        {
            this.moveBoard = new int[9, 10];
            if (this.x - 1 < 9 && this.y + 2 < 10 && this.x - 1 >= 0 && this.y + 2 >= 0)
            {
                if (board.grid[this.x, this.y + 1].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, -1, 2);
                }
            }
            if (this.x + 1 < 9 && this.y + 2 < 10 && this.x + 1 >= 0 && this.y + 2 >= 0)
            {
                if (board.grid[this.x, this.y + 1].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, 1, 2);
                }
            }
            if (this.x + 2 < 9 && this.y + 1 < 10 && this.x + 2 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, 2, 1);
                }
            }
            if (this.x + 2 < 9 && this.y - 1 < 10 && this.x + 2 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x + 1, this.y].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, 2, -1);
                }
            }
            if (this.x + 1 < 9 && this.y - 2 < 10 && this.x + 1 >= 0 && this.y - 2 >= 0)
            {
                if (board.grid[this.x, this.y - 1].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, 1, -2);
                }
            }
            if (this.x - 1 < 9 && this.y - 2 < 10 && this.x - 1 >= 0 && this.y - 2 >= 0)
            {
                if (board.grid[this.x, this.y - 1].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, -1, -2);
                }
            }
            if (this.x - 2 < 9 && this.y - 1 < 10 && this.x - 2 >= 0 && this.y - 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, -2, -1);
                }
            }
            if (this.x - 2 < 9 && this.y + 1 < 10 && this.x - 2 >= 0 && this.y + 1 >= 0)
            {
                if (board.grid[this.x - 1, this.y].occupied != true)
                {
                    moveCheckBasic(ref board, this.moveBoard, -2, 1);
                }
            }
            return this.moveBoard;
        }
    }
}
