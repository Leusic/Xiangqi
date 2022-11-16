using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class Guard : Piece
    {
        public Guard(int x, int y, int teamModifier)
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

            this.palaceMoveCheck(board, tempBoard, 1, 1);
            this.palaceMoveCheck(board, tempBoard, 1, -1);
            this.palaceMoveCheck(board, tempBoard, -1, 1);
            this.palaceMoveCheck(board, tempBoard, -1, -1);

            return tempBoard;
        }
    }
}
