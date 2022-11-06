using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    class Board
    {
        Cell[,] board;

        public Board()
        {
            board = new Cell[9,10];
            Soldier soldier;
            soldier = new Soldier();
            board[0, 0].piece = soldier;
        }
    }
}
