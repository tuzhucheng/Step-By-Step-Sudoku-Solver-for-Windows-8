using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Step_by_Step_Sudoku_Solver
{
    public class Node
    {
        public Unit[,] Snapshot = new Unit[9, 9];
        public Node next; //Pointers?
        public Node prev;
        public int guessValue;
        public int guessHoriz;
        public int guessVert;

        public Node()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                    Snapshot[x, y] = new Unit();
            }
            guessValue = 0;
            guessHoriz = 0;
            guessVert = 0;
            next = null;
            prev = null;
        }
    }
}
