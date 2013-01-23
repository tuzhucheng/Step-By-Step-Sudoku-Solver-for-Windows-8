using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Step_by_Step_Sudoku_Solver
{
    public class List
    {
        public Node first;
        public Node last;
        public int counter;

        public List()
        {
            first = null;
            last = null;
            counter = 0;
        }

        public int getCounter() { return counter; }

        public void push(Unit[,] Grid, int inputValue, int guessH, int guessV)
        {
            Node newNode = new Node();
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    newNode.Snapshot[x, y].possibilityCounter = Grid[x, y].possibilityCounter;
                    newNode.Snapshot[x, y].value = Grid[x, y].value;
                    newNode.Snapshot[x, y].uncertain = Grid[x, y].uncertain;
                    for (int z = 0; z < 9; z++)
                        newNode.Snapshot[x, y].possibilities[z] = Grid[x, y].possibilities[z];
                }
            }
            newNode.guessHoriz = guessH;
            newNode.guessVert = guessV;
            newNode.guessValue = inputValue;

            if (counter == 0)
            {
                first = newNode;
                last = newNode;
            }
            else
                last.next = newNode;
            newNode.prev = last;
            last = newNode;
            counter++;
        }

        public void pop()
        {
            if (counter != 0)
            {
                if (counter != 1)
                {
                    last = last.prev;
                    last.next = null;
                }
                else
                {
                    first = null;
                    last = null;
                }
                counter--;
            }
        }

        public void undo(Unit[,] Grid)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Grid[x, y].possibilityCounter = last.Snapshot[x, y].possibilityCounter;
                    Grid[x, y].value = last.Snapshot[x, y].value;
                    Grid[x, y].uncertain = last.Snapshot[x, y].uncertain;
                    for (int z = 0; z < 9; z++)
                        Grid[x, y].possibilities[z] = last.Snapshot[x, y].possibilities[z];
                }
            }
            Grid[last.guessVert, last.guessHoriz].possibilities[last.guessValue - 1] = false;
            Grid[last.guessVert, last.guessHoriz].possibilityCounter--;
        }
    }
}
