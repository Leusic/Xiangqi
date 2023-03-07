using System;
using System.Collections.Generic;
using System.Text;

namespace Xiangqi
{
    public class Board
    {
        public Cell[,] grid;

        public Board()
        {
            grid = new Cell[9,10];
            for(int i = 0; i < 9; i++)
            {
                for(int z = 0; z < 10; z++)
                {
                    grid[i, z] = new Cell(false);
                }
            }
        }
    }
}
