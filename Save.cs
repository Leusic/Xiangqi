using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    public class Save
    {
        public List<Piece> pieces;
        public int currentTurn;
        public List<String> rollbackMoves;
        public Save(List<Piece> pPieces, int pCurrentTurn, List<String> pRollbackMoves )
        {
            pieces = pPieces;
            currentTurn = pCurrentTurn;
            rollbackMoves = pRollbackMoves;
        }
    }
}
