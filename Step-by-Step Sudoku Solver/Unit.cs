using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Step_by_Step_Sudoku_Solver
{
    public class Unit
    {
        public bool[] possibilities = new bool[9];
        public int possibilityCounter;
        public int value;
        public bool uncertain;
        public bool original;

        public Unit()
        {
            value = 0;
            possibilityCounter = 9;
            for (int x = 0; x < 9; x++)
                possibilities[x] = true;
            uncertain = true;
            original = false;
        }
    }
}
