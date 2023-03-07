using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    public class Cell
    {
        public bool occupied;
        public Piece piece;

        public Cell(bool pOccupied)
        {
            occupied = pOccupied;
        }
         
        public Cell(bool pOccupied, Piece pPiece)
        {
            occupied = pOccupied;
            piece = pPiece;
        }
    }
}
