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

            this.palaceMoveCheck(board, tempBoard, 0, 1);
            this.palaceMoveCheck(board, tempBoard, 0, -1);
            this.palaceMoveCheck(board, tempBoard, -1, 0);
            this.palaceMoveCheck(board, tempBoard, 1, 0);

            return tempBoard;
        }
    }
}
