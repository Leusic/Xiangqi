using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class Piece
    {
        public bool canCrossRiver;
        public bool alive;
        //-1 is Red, 1 is Black
        public int teamModifier;
        public int y;
        public int x;

        //finds all legal moves for the piece and returns a boolean grid representing the board
        public virtual bool[,] legalMoves (Board board)
        {
            bool[,] legalMovesBoard = new bool[9, 10];
            return legalMovesBoard;
        }

        //checks if a move will collide with a friendly unit or go out of bounds and if not sets the move grid position to true
        public void moveCheck(Board board, bool[,] tempBoard, int xMod, int yMod)
        {
            if(this.x + xMod > 8 || this.y + yMod > 9 || this.x + xMod < 0 || this.y + yMod < 0)
            {
                ;
            }
            //checks if position is occupied
            else if (board.grid[this.x + xMod, this.y + yMod].occupied != true)
            {
                tempBoard[this.x + xMod, this.y + yMod] = true;
            }
            //if position is occupied, checks if the piece occupying it is friendly or not
            else if (board.grid[this.x + xMod, this.y + yMod].piece.teamModifier != this.teamModifier)
            {
                tempBoard[this.x + xMod, this.y + yMod] = true;
            }
        }
    }
}
