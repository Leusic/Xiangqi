using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class General : Piece
    {
        public General(int x, int y, int teamModifier)
        {
            canCrossRiver = false;
            crossedRiver = false;
            this.x = x;
            this.y = y;
            this.teamModifier = teamModifier;
            this.alive = true;
        }

        public override bool[,] legalMoves(Board board)
        {
            bool[,] tempBoard;
            tempBoard = new bool[9, 10];

            this.generalMoveCheck(board, tempBoard, 0, 1);
            this.generalMoveCheck(board, tempBoard, 0, -1);
            this.generalMoveCheck(board, tempBoard, -1, 0);
            this.generalMoveCheck(board, tempBoard, 1, 0);

            return tempBoard;
        }

        public void generalMoveCheck(Board board, bool[,] tempBoard, int xMod, int yMod)
        {
            this.moveCheck(board, tempBoard, xMod, yMod);
            if(this.teamModifier == -1)
            {
                if((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod < 7))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = false;
                }
            }
            if(this.teamModifier == 1)
            {
                if ((this.x + xMod < 3) || (this.x + xMod > 5) || (this.y + yMod > 2))
                {
                    tempBoard[this.x + xMod, this.y + yMod] = false;
                }
            }
        }
    }
}
