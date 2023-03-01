using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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
            this.moveBoard = new int[9, 10]; 
        }

        public override int[,] legalMoves(ref Board board, ref Dictionary<Piece, PictureBox> allPieces)
        {
            this.moveBoard = new int[9, 10];
            this.palaceMoveCheck(ref board, ref allPieces, this.moveBoard, 1, 1);
            this.palaceMoveCheck(ref board, ref allPieces, this.moveBoard, 1, -1);
            this.palaceMoveCheck(ref board, ref allPieces, this.moveBoard, -1, 1);
            this.palaceMoveCheck(ref board, ref allPieces, this.moveBoard, -1, -1);

            return this.moveBoard;
        }

        public override int[,] legalMovesBasic(ref Board board)
        {
            this.moveBoard = new int[9, 10];
            this.palaceMoveCheckBasic(ref board, this.moveBoard, 1, 1);
            this.palaceMoveCheckBasic(ref board, this.moveBoard, 1, -1);
            this.palaceMoveCheckBasic(ref board, this.moveBoard, -1, 1);
            this.palaceMoveCheckBasic(ref board, this.moveBoard, -1, -1);

            return this.moveBoard;
        }
    }
}
