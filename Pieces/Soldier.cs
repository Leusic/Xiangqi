using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class Soldier : Piece
    {
        public Soldier(int x, int y, int teamModifier)
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
            if(this.crossedRiver == false){
                this.moveCheck(ref board, tempBoard, 0, (1 * teamModifier));
            }
            else
            {
                this.moveCheck(ref board, tempBoard, 0, (1 * teamModifier));
                this.moveCheck(ref board, tempBoard, -1, 0);
                this.moveCheck(ref board, tempBoard, 1, 0);
            }
            return tempBoard;
        }
    }
}
