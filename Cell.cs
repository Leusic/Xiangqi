﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    //represents one position on the game board
    public class Cell
    {
        public bool occupied;
        public Piece piece;

        public Cell(bool pOccupied)
        {
            occupied = pOccupied;
        }
    }
}
