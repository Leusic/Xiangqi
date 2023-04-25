using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Xiangqi
{
    public class Piece
    {
        public string name;
        
        //if the unit can cross the river
        public bool canCrossRiver;

        //if the unit has crossed the river
        public bool crossedRiver;

        public bool alive;
        //-1 is Red, 1 is Black
        public int teamModifier;
        public int y;
        public int x;
        public int[,] moveBoard;

        //finds all legal moves for the piece and returns a boolean grid representing the board
        public virtual int[,] legalMoves (ref Board board, ref Dictionary<Piece, PictureBox> allPieces)
        {
            int[,] legalMovesBoard = new int[9, 10];
            return legalMovesBoard;
        }

        //finds all legal moves for the piece without performing any check scanning (necessary to prevent endless loops in legalMoves)
        public virtual int[,] legalMovesBasic (ref Board board)
        {
            int[,] legalMovesBoard = new int[9, 10];
            return legalMovesBoard;
        }

        //checks if a move will collide with a friendly unit or go out of bounds and if not sets the move grid position to true
        //tempBoard stores the resulting moves (0 - Illegal move, 1 - Illegal move due to placing self into check, 2 - Legal Move)
        public void moveCheck(ref Board board, ref Dictionary<Piece, PictureBox> allPieces, int[,] tempBoard, int xMod, int yMod)
        {
            //checks if position is within the board
            if(this.x + xMod > 8 || this.y + yMod > 9 || this.x + xMod < 0 || this.y + yMod < 0)
            {
                return;
            }
            //checks if position is not occupied
            else if (board.grid[this.x + xMod, this.y + yMod].occupied != true)
            {
                tempBoard[this.x + xMod, this.y + yMod] = 2;
            }
            //if position is occupied, checks if the piece occupying it is friendly or not
            else if (board.grid[this.x + xMod, this.y + yMod].piece.teamModifier != this.teamModifier)
            {
                tempBoard[this.x + xMod, this.y + yMod] = 2;
            }
            //can unit cross river check
            if ((this.canCrossRiver == false) && ((this.y < 5 && (this.y + yMod) >= 5) || (this.y > 4 && (this.y + yMod) <= 4)))
            {
                tempBoard[this.x + xMod, this.y + yMod] = 0;
            }

            //if the position is valid run a checkscan:
            if (tempBoard[this.x + xMod, this.y + yMod] == 2)
            {
                //change current piece coordinates
                this.x = this.x + xMod;
                this.y = this.y + yMod;

                //makes it so that you can undo a piece if it is taken
                Piece undoPiece = null;

                //if a piece is taken, moves it out of play
                if (board.grid[this.x, this.y].occupied == true)
                {
                    undoPiece = board.grid[this.x, this.y].piece;
                    board.grid[this.x, this.y].piece.x = 99;
                    board.grid[this.x, this.y].piece.y = 99;
                }

                //temporarily move the piece to the new location
                board.grid[this.x, this.y].occupied = true;
                board.grid[this.x, this.y].piece = this;

                board.grid[this.x - xMod, this.y - yMod].occupied = false;
                board.grid[this.x - xMod, this.y - yMod].piece = null;

                //finds the x and y coords of the two generals
                int redGeneralX = 99, redGeneralY = 99, blackGeneralX = 99, blackGeneralY = 99, friendlyGeneralX = 99, friendlyGeneralY = 99;
                //gets the general for the piece's team
                foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if (i.Key.GetType().Name == "General")
                {
                        if(i.Key.teamModifier == -1)
                        {
                            redGeneralX = i.Key.x;
                            redGeneralY = i.Key.y;
                            if(i.Key.teamModifier == this.teamModifier)
                            {
                                friendlyGeneralX = i.Key.x;
                                friendlyGeneralY = i.Key.y;
                            }
                        }
                        if(i.Key.teamModifier == 1)
                        {
                            blackGeneralX = i.Key.x;
                            blackGeneralY = i.Key.y;
                            if (i.Key.teamModifier == this.teamModifier)
                            {
                                friendlyGeneralX = i.Key.x;
                                friendlyGeneralY = i.Key.y;
                            }
                        }
                }

                bool flyingGeneral = false;
                //flying general rule (if there are no intervening pieces between the generals across the board, one can take the other)
                if(redGeneralX == blackGeneralX)
                {
                    flyingGeneral = true;
                    //finds pieces that are between the two generals
                    foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if ((i.Key.GetType().Name != "General") && (i.Key.x == redGeneralX) && (i.Key.y > blackGeneralY) && (i.Key.y < redGeneralY))
                        {
                            flyingGeneral = false;
                            break;
                        }
                }

                if (flyingGeneral == true)
                {
                    tempBoard[this.x, this.y] = 1;
                }

                //checks if any enemy team units legal movements can hit the friendly general, if they can this move is illegal due to placing self into check
                foreach (KeyValuePair<Piece, PictureBox> i in allPieces) if ((i.Key.teamModifier != this.teamModifier) && (i.Key.legalMovesBasic(ref board)[friendlyGeneralX, friendlyGeneralY] == 2))
                    {
                        tempBoard[this.x, this.y] = 1;
                        break;
                    }

                //undo board changes
                if(undoPiece != null)
                {
                    undoPiece.x = this.x;
                    undoPiece.y = this.y;
                    board.grid[this.x, this.y].piece = undoPiece;
                }
                else
                {
                    board.grid[this.x, this.y].occupied = false;
                    board.grid[this.x, this.y].piece = null;
                }

                this.x = this.x - xMod;
                this.y = this.y - yMod;

                board.grid[this.x, this.y].occupied = true;
                board.grid[this.x, this.y].piece = this;
            }
        }

        //move check for units that cannot leave the palace (guards and the general), also runs check scans
        public void palaceMoveCheck(ref Board board, ref Dictionary<Piece, PictureBox> allPieces, int[,] tempBoard, int xMod, int yMod)
        {
            //standard move check
            this.moveCheck(ref board, ref allPieces, tempBoard, xMod, yMod);
            //reperform out of bounds check
            if (this.x + xMod > 8 || this.y + yMod > 9 || this.x + xMod < 0 || this.y + yMod < 0)
            {
                return;
            }
            if (this.teamModifier == -1)
            {
                if ((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod < 7))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = 0;
                }
            }
            if (this.teamModifier == 1)
            {
                if ((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod > 2))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = 0;
                }
            }
        }

        //checks if a move puts a unit out of board bounds, or collides with a friendly unit or if the unit is attempting to cross the river and cannot (does not run check scans)
        public void moveCheckBasic(ref Board board, int[,] tempBoard, int xMod, int yMod)
        {
            //checks if position is within the board
            if (this.x + xMod > 8 || this.y + yMod > 9 || this.x + xMod < 0 || this.y + yMod < 0)
            {
                return;
            }
            //checks if position is not occupied
            else if (board.grid[this.x + xMod, this.y + yMod].occupied != true)
            {
                tempBoard[this.x + xMod, this.y + yMod] = 2;
            }
            //if position is occupied, checks if the piece occupying it is friendly or not
            else if (board.grid[this.x + xMod, this.y + yMod].piece.teamModifier != this.teamModifier)
            {
                tempBoard[this.x + xMod, this.y + yMod] = 2;
            }
            //can unit cross river check
            if ((this.canCrossRiver == false) && ((this.y < 5 && (this.y + yMod) >= 5) || (this.y > 4 && (this.y + yMod) <= 4)))
            {
                tempBoard[this.x + xMod, this.y + yMod] = 0;
            }
        }

        //palaceMoveCheck but without check scans
        public void palaceMoveCheckBasic(ref Board board, int[,] tempBoard, int xMod, int yMod)
        {
            //standard move check
            this.moveCheckBasic(ref board, tempBoard, xMod, yMod);
            //reperform out of bounds check
            if (this.x + xMod > 8 || this.y + yMod > 9 || this.x + xMod < 0 || this.y + yMod < 0)
            {
                return;
            }
            if (this.teamModifier == -1)
            {
                if ((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod < 7))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = 0;
                }
            }
            if (this.teamModifier == 1)
            {
                if ((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod > 2))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = 0;
                }
            }
        }
    }
}
