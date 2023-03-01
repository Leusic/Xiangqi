using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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
            this.moveBoard = new int[9, 10];
        }

        public override int[,] legalMoves(ref Board board, ref Dictionary<Piece, PictureBox> allPieces)   
        {
            this.moveBoard = new int[9, 10];
            if(this.crossedRiver == false){
                this.moveCheck(ref board, ref allPieces, this.moveBoard, 0, (1 * teamModifier));
            }
            else
            {
                this.moveCheck(ref board, ref allPieces, this.moveBoard, 0, (1 * teamModifier));
                this.moveCheck(ref board, ref allPieces, this.moveBoard, -1, 0);
                this.moveCheck(ref board, ref allPieces, this.moveBoard, 1, 0);
            }
            return this.moveBoard;
        }

        public override int[,] legalMovesBasic(ref Board board)
        {
            this.moveBoard = new int[9, 10];
            if (this.crossedRiver == false)
            {
                this.moveCheckBasic(ref board, this.moveBoard, 0, (1 * teamModifier));
            }
            else
            {
                this.moveCheckBasic(ref board, this.moveBoard, 0, (1 * teamModifier));
                this.moveCheckBasic(ref board, this.moveBoard, -1, 0);
                this.moveCheckBasic(ref board, this.moveBoard, 1, 0);
            }
            return this.moveBoard;
        }
    }
}
