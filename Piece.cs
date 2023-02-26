using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Xiangqi
{
    class Piece
    {
        public string name;
        public bool canCrossRiver;
        public bool crossedRiver;
        public bool alive;
        //-1 is Red, 1 is Black
        public int teamModifier;
        public int y;
        public int x;

        //finds all legal moves for the piece and returns a boolean grid representing the board
        public virtual bool[,] legalMoves (ref Board board)
        {
            bool[,] legalMovesBoard = new bool[9, 10];
            return legalMovesBoard;
        }

        //checks if a move will collide with a friendly unit or go out of bounds and if not sets the move grid position to true
        public void moveCheck(ref Board board, bool[,] tempBoard, int xMod, int yMod)
        {
            //checks if position is within the board
            if(this.x + xMod > 8 || this.y + yMod > 9 || this.x + xMod < 0 || this.y + yMod < 0)
            {
                return;
            }
            //checks if position is not occupied
            else if (board.grid[this.x + xMod, this.y + yMod].occupied != true)
            {
                tempBoard[this.x + xMod, this.y + yMod] = true;
            }
            //if position is occupied, checks if the piece occupying it is friendly or not
            else if (board.grid[this.x + xMod, this.y + yMod].piece.teamModifier != this.teamModifier)
            {
                tempBoard[this.x + xMod, this.y + yMod] = true;
            }
            //can unit cross river check
            if ((this.canCrossRiver == false) && ((this.y < 5 && (this.y + yMod) >= 5) || (this.y > 4 && (this.y + yMod) <= 4)))
            {
                tempBoard[this.x + xMod, this.y + yMod] = false;
            }

            //if the position is valid run a checkscan:
            if (tempBoard[this.x + xMod, this.y + yMod] == true)
            {
                //store the current board so it can be returned back to later
                //will run a checkscan on the board as if the piece was moved
                Board oldBoard = board;

                //update if the piece crossed the river
                if ((this.y < 5 && (this.y + yMod) >= 5) || ((this).y > 4 && (this.y + yMod) <= 4))
                {
                    this.crossedRiver = true;
                }
                //sets the piece that is taken to be dead
                if (board.grid[x + xMod, y + yMod].occupied == true)
                {
                    board.grid[x + xMod, y + yMod].piece.alive = false;
                }

                //change current piece coordinates
                this.x = x + xMod;
                this.y = y + yMod;

                //if a piece is taken, moves it out of play
                if (board.grid[x + xMod, y = yMod].occupied == true)
                {
                    board.grid[x + xMod, y + yMod].piece.x = 99;
                    board.grid[x = xMod, y + yMod].piece.y = 99;
                }

                //temporarily move the piece to the new location
                board.grid[x + xMod, y + yMod].occupied = true;
                board.grid[x + xMod, y + yMod].piece = this;
            }

        }


        public void palaceMoveCheck(ref Board board, bool[,] tempBoard, int xMod, int yMod)
        {
            //standard move check
            this.moveCheck(ref board, tempBoard, xMod, yMod);
            //reperform out of bounds check
            if (this.x + xMod > 8 || this.y + yMod > 9 || this.x + xMod < 0 || this.y + yMod < 0)
            {
                return;
            }
            if (this.teamModifier == -1)
            {
                if ((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod < 7))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = false;
                }
            }
            if (this.teamModifier == 1)
            {
                if ((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod > 2))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = false;
                }
            }
        }
    }
}
