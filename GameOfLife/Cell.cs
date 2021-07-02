using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Cell
    {
        // Fields
        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsAlive { get; private set; }

        // Constructor
        public Cell(int x, int y, bool isAlive)
        {
            X = x;
            Y = y;
            isAlive = IsAlive;
        }
        
    }
}
