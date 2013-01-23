using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Step_by_Step_Sudoku_Solver
{
    public class SudokuGrid
    {
        public int newFills = 0;
        public int colSlider = 0;
        public int rowSlider = 0;
        public int turns = 1;
        public List Actions = new List();
        public int guessHorizLoc = 0;
        public int guessVertLoc = 0;
        public int guessedValue = 0;
        public Unit[,] Grid = new Unit[9, 9];
        public bool displayed = false; //Debugging -> toggle display indicator //Not currently used

        public SudokuGrid()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                    Grid[x, y] = new Unit();
            }
        }

        public int[,] debugDisplay() //Display grid for debugging //Not currently used
        {
            int[,] miniGrid = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    miniGrid[i, j] = Grid[i, j].value;
            }
            return miniGrid;
        }

        public void fillIn(Unit Temp, int value)
        {
            Temp.value = value;
            for (int x = 0; x < 9; x++)
            {
                if (x != value - 1)
                    Temp.possibilities[x] = false;
            }
            Temp.possibilityCounter = 1;
            Temp.uncertain = false;
        }

        public void eliminate(Unit compareTo, Unit current) //Eliminate choices in current based on value of compareTo
        {
            int eliminatedChoice = 0;
            if (compareTo.possibilityCounter == 1)
            {
                eliminatedChoice = compareTo.value;
                if (current.possibilities[eliminatedChoice - 1])
                {
                    current.possibilities[eliminatedChoice - 1] = false;
                    current.possibilityCounter--;
                    newFills++;
                }
            }
        }

        //Eliminate using the idea that nothing can repeat in the same row, column, square
        //i.e. values already present in the same row, column, square is eliminated from possibilities of
        //the current Unit.
        public int eliminateChoices(int rowMarker, int colMarker)
        {
            int rowSlider = 0, colSlider = 0;
            if (Grid[rowMarker, colMarker].possibilityCounter != 1)
            {
                //Eliminating values based on row
                //Eliminating values on the left
                colSlider = colMarker;
                while (colSlider != 0)
                {
                    colSlider--;
                    eliminate(Grid[rowMarker, colSlider], Grid[rowMarker, colMarker]);
                }
                //Eliminating values on the right
                colSlider = colMarker;
                while (colSlider != 8)
                {
                    colSlider++;
                    eliminate(Grid[rowMarker, colSlider], Grid[rowMarker, colMarker]);
                }

                //Eliminating values based on column
                //Eliminating values above
                rowSlider = rowMarker;
                while (rowSlider != 0)
                {
                    rowSlider--;
                    eliminate(Grid[rowSlider, colMarker], Grid[rowMarker, colMarker]);
                }
                //Eliminating values below
                rowSlider = rowMarker;
                while (rowSlider != 8)
                {
                    rowSlider++;
                    eliminate(Grid[rowSlider, colMarker], Grid[rowMarker, colMarker]);
                }

                //Eliminating within 3x3 square
                if (colMarker % 3 == 0)
                {
                    //Pos1
                    if (rowMarker % 3 == 0)
                    {
                        eliminate(Grid[rowMarker + 1, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker + 2], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 2, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 2, colMarker + 2], Grid[rowMarker, colMarker]);
                    }
                    //Pos2
                    else if ((rowMarker - 1) % 3 == 0)
                    {
                        eliminate(Grid[rowMarker - 1, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker + 2], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker + 2], Grid[rowMarker, colMarker]);
                    }
                    //Pos3
                    else if ((rowMarker - 2) % 3 == 0)
                    {
                        eliminate(Grid[rowMarker - 2, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 2, colMarker + 2], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker + 2], Grid[rowMarker, colMarker]);
                    }
                }
                else if ((colMarker - 1) % 3 == 0)
                {
                    //Pos4
                    if (rowMarker % 3 == 0)
                    {
                        eliminate(Grid[rowMarker + 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 2, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 2, colMarker + 1], Grid[rowMarker, colMarker]);
                    }
                    //Pos5
                    else if ((rowMarker - 1) % 3 == 0)
                    {
                        eliminate(Grid[rowMarker - 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker + 1], Grid[rowMarker, colMarker]);
                    }
                    //Pos6
                    else if ((rowMarker - 2) % 3 == 0)
                    {
                        eliminate(Grid[rowMarker - 2, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 2, colMarker + 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker + 1], Grid[rowMarker, colMarker]);
                    }
                }
                else if ((colMarker - 2) % 3 == 0)
                {
                    //Pos7
                    if (rowMarker % 3 == 0)
                    {
                        eliminate(Grid[rowMarker + 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker - 2], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 2, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 2, colMarker - 2], Grid[rowMarker, colMarker]);
                    }
                    //Pos8
                    else if ((rowMarker - 1) % 3 == 0)
                    {
                        eliminate(Grid[rowMarker - 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker - 2], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker + 1, colMarker - 2], Grid[rowMarker, colMarker]);
                    }
                    //Pos9
                    else if ((rowMarker - 2) % 3 == 0)
                    {
                        eliminate(Grid[rowMarker - 2, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 2, colMarker - 2], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker - 1], Grid[rowMarker, colMarker]);
                        eliminate(Grid[rowMarker - 1, colMarker - 2], Grid[rowMarker, colMarker]);
                    }
                }

                if (Grid[rowMarker, colMarker].possibilityCounter == 1)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        if (Grid[rowMarker, colMarker].possibilities[x])
                        {
                            Grid[rowMarker, colMarker].value = x + 1;
                            newFills++;
                            if (Step_by_Step_Sudoku_Solver.MainPage.stepByStepEnabled) //Giving reasons in the step-by-step option
                            {
                                MainPage.messageBoxReplaceText("Let's take a look at row " + (rowMarker + 1) + " and column " + (colMarker + 1) + ".\n");
                                for (int num = 1; num < 10; num++)
                                {
                                    if (num != x+1)
                                    {
                                        bool reasonGiven = false;
                                        //Scans across the row to see if num is found
                                        for (int j = 0; j < 9; j++)
                                        {
                                            if (num == Grid[rowMarker, j].value)
                                            {
                                                MainPage.messageBoxAddText(num + " cannot be here since it is already in column " + (j + 1) + " of the same row.\n");
                                                System.Diagnostics.Debug.WriteLine(num + " cannot be here since it is already in column " + (j + 1) + " of the same row.\n");
                                                reasonGiven = true;
                                            }
                                        }
                                        //Scans across the column to see if num is found
                                        if (!reasonGiven)
                                        {
                                            for (int j = 0; j < 9; j++)
                                            {
                                                if (num == Grid[j, colMarker].value)
                                                {
                                                    MainPage.messageBoxAddText(num + " cannot be here since it is already in row " + (j + 1) + " of the same column.\n");
                                                    System.Diagnostics.Debug.WriteLine(num + " cannot be here since it is already in row " + (j + 1) + " of the same column.\n");
                                                    reasonGiven = true;
                                                }
                                            }
                                        }
                                        if (!reasonGiven)
                                        {
                                            MainPage.messageBoxAddText(num + " cannot be here since it is already in the same 3x3 sub-grid.\n");
                                            System.Diagnostics.Debug.WriteLine(num + " cannot be here since it is already in the same 3x3 sub-grid.\n");
                                        }
                                    }
                                }
                                MainPage.messageBoxAddText("Therefore, due to elimination, " + (x+1) + " must be placed in row " + (rowMarker + 1) + " and column " + (colMarker + 1) + ".\n");
                            }
                            return 1; //Returns 1 if able to reduce possibilityCounter to one single value
                        }
                    }
                    //Grid[rowMarker,colMarker].uncertain = false;
                }
            }
            return 0;
        }

        public void onlyChoiceElimination(int rowMarker, int colMarker, char identifier)
        {
            int rowSlider = 0;
            int colSlider = 0;
            int totalPosLoc = 0;

            for (int x = 1; x < 10; x++)
            {
                rowSlider = rowMarker;
                colSlider = colMarker;
                totalPosLoc = 0;
                if (identifier == 's')
                {
                    for (int rowRev = 0; rowRev < 3; rowRev++)
                    {
                        for (int colRev = 0; colRev < 3; colRev++)
                        {
                            if (Grid[rowMarker - rowRev, colMarker - colRev].possibilities[x - 1] == true)
                            {
                                rowSlider = rowMarker - rowRev;
                                colSlider = colMarker - colRev;
                                totalPosLoc++;
                            }
                        }
                    }
                }
                else if (identifier == 'r')
                {
                    for (int colRev = 0; colRev < 9; colRev++)
                    {
                        if (Grid[rowMarker, colMarker - colRev].possibilities[x - 1] == true)
                        {
                            colSlider = colMarker - colRev;
                            totalPosLoc++;
                        }
                    }
                }
                else if (identifier == 'c')
                {
                    for (int rowRev = 0; rowRev < 9; rowRev++)
                    {
                        if (Grid[rowMarker - rowRev, colMarker].possibilities[x - 1] == true)
                        {
                            rowSlider = rowMarker - rowRev;
                            totalPosLoc++;
                        }
                    }
                }

                if (totalPosLoc == 1 && Grid[rowSlider, colSlider].possibilityCounter != 1)
                {
                    fillIn(Grid[rowSlider, colSlider], x);
                    /*String identifierName;
                    if (identifier == 's')
                        identifierName = "3x3 square";
                    else if (identifier == 'r')
                        identifierName = "row";
                    else
                        identifierName = "column";*/
                    //MainPage.messageBoxAddText(x + " must be in row " + (rowSlider+1) + " and column " + (colSlider+1) + " since this is the only place in the " + identifierName + " where we can put " + x + ".\n");
                    newFills++;
                    eliminateChoices(rowSlider, colSlider);
                }
            }
        }

        public int onlyChoiceEliminationStepByStep(int rowMarker, int colMarker, char identifier)
        {
            int rowSlider = 0;
            int colSlider = 0;
            int totalPosLoc = 0;

            for (int x = 1; x < 10; x++)
            {
                rowSlider = rowMarker;
                colSlider = colMarker;
                totalPosLoc = 0;
                if (identifier == 's')
                {
                    for (int rowRev = 0; rowRev < 3; rowRev++)
                    {
                        for (int colRev = 0; colRev < 3; colRev++)
                        {
                            if (Grid[rowMarker - rowRev, colMarker - colRev].possibilities[x - 1] == true)
                            {
                                rowSlider = rowMarker - rowRev;
                                colSlider = colMarker - colRev;
                                totalPosLoc++;
                            }
                        }
                    }
                }
                else if (identifier == 'r')
                {
                    for (int colRev = 0; colRev < 9; colRev++)
                    {
                        if (Grid[rowMarker, colMarker - colRev].possibilities[x - 1] == true)
                        {
                            colSlider = colMarker - colRev;
                            totalPosLoc++;
                        }
                    }
                }
                else if (identifier == 'c')
                {
                    for (int rowRev = 0; rowRev < 9; rowRev++)
                    {
                        if (Grid[rowMarker - rowRev, colMarker].possibilities[x - 1] == true)
                        {
                            rowSlider = rowMarker - rowRev;
                            totalPosLoc++;
                        }
                    }
                }

                if (totalPosLoc == 1 && Grid[rowSlider, colSlider].possibilityCounter != 1)
                {
                    fillIn(Grid[rowSlider, colSlider], x);
                    String identifierName;
                    if (identifier == 's')
                        identifierName = "3x3 sub-grid";
                    else if (identifier == 'r')
                        identifierName = "row";
                    else
                        identifierName = "column";
                    MainPage.messageBoxReplaceText(x + " must be in row " + (rowSlider + 1) + " and column " + (colSlider + 1) + " since this is the only place in the " + identifierName + " where we can put a " + x + " without conflict.\n");
                    newFills++;
                    return 1;
                    //eliminateChoices(rowSlider, colSlider);
                }
            }
            return 0;
        }

        public bool check(char task)
        {
            bool[] filled = new bool[9];

            //Checking for completion
            if (task == 'a')
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (Grid[x, y].value == 0)
                            return false;
                    }
                }
            }

            //Checking for conflicts/flaws
            if (task == 'b')
            {
                for (int x = 0; x < 9; x++)
                    filled[x] = false;
                //Checking row;
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (Grid[x, y].possibilityCounter == 1)
                        {
                            if (filled[Grid[x, y].value - 1] == false)
                                filled[Grid[x, y].value - 1] = true;
                            else
                                return false; //Conflict arises
                        }
                    }
                    for (int z = 0; z < 9; z++)
                        filled[z] = false;
                }

                for (int x = 0; x < 9; x++)
                    filled[x] = false;
                //Checking column;
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (Grid[y, x].possibilityCounter == 1)
                        {
                            if (filled[Grid[y, x].value - 1] == false)
                                filled[Grid[y, x].value - 1] = true;
                            else
                                return false; //Conflict arises
                        }
                    }
                    for (int z = 0; z < 9; z++)
                        filled[z] = false;
                }

                for (int x = 0; x < 9; x++)
                    filled[x] = false;
                //Checking square;
                for (int x = 0; x < 9; x = x + 3)
                {
                    for (int y = 0; y < 9; y = y + 3)
                    {
                        for (int a = 0; a < 3; a++)
                        {
                            for (int b = 0; b < 3; b++)
                            {
                                if (Grid[x + a, y + b].possibilityCounter == 1)
                                {
                                    if (filled[Grid[x + a, y + b].value - 1] == false)
                                        filled[Grid[x + a, y + b].value - 1] = true;
                                    else
                                        return false; //Conflict arises
                                }
                            }
                        }
                        for (int z = 0; z < 9; z++)
                            filled[z] = false;
                    }
                }
            }

            return true;
        }

        public void deductiveReasoning()
        {
            do
            {
                newFills = 0;
                for (int rowMarker = 0; rowMarker < 9; rowMarker++)
                {
                    for (int colMarker = 0; colMarker < 9; colMarker++)
                        eliminateChoices(rowMarker, colMarker);
                }

                for (int rowMarker = 0; rowMarker < 9; rowMarker++)
                {
                    for (int colMarker = 0; colMarker < 9; colMarker++)
                    {
                        //Elimination based on only possible location in 3x3 square
                        if ((colMarker == 2 || colMarker == 5 || colMarker == 8) && (rowMarker == 2 || rowMarker == 5 || rowMarker == 8))
                            onlyChoiceElimination(rowMarker, colMarker, 's');

                        //Elimination based on only possible location in row
                        if (colMarker == 8)
                            onlyChoiceElimination(rowMarker, colMarker, 'r');

                        //Elimination based on only possible location in col
                        if (rowMarker == 8)
                            onlyChoiceElimination(rowMarker, colMarker, 'c');
                    }
                }
                turns++;
            } while (newFills != 0);
        }

        public int deductiveReasoningStepByStep()
        {
            newFills = 0;
            for (int rowMarker = 0; rowMarker < 9; rowMarker++)
            {
                for (int colMarker = 0; colMarker < 9; colMarker++)
                {
                    if (eliminateChoices(rowMarker, colMarker) == 1)
                        return 1; //Returns 1 when there's a new fill
                }
            }

            for (int rowMarker = 0; rowMarker < 9; rowMarker++)
            {
                for (int colMarker = 0; colMarker < 9; colMarker++)
                {
                    //Elimination based on only possible location in 3x3 square
                    if ((colMarker == 2 || colMarker == 5 || colMarker == 8) && (rowMarker == 2 || rowMarker == 5 || rowMarker == 8))
                    {
                        if (onlyChoiceEliminationStepByStep(rowMarker, colMarker, 's') == 1)
                            return 1;
                    }

                    //Elimination based on only possible location in row
                    if (colMarker == 8)
                    {
                        if (onlyChoiceEliminationStepByStep(rowMarker, colMarker, 'r') == 1)
                            return 1;
                    }

                    //Elimination based on only possible location in col
                    if (rowMarker == 8)
                    {
                        if (onlyChoiceEliminationStepByStep(rowMarker, colMarker, 'c') == 1)
                            return 1;
                    }
                }
            }
            turns++;
            if (newFills == 0)
                MainPage.messageBoxAddText("Deductive reasoning completed.\n");
            return 0;
        }

        public void inductiveReasoning()
        {
            bool finishedFill = false;
            bool conflict = false;
            bool guessFound = false;
            do
            {
                //Choosing random choice
                guessVertLoc = 0;
                guessHorizLoc = 0;
                guessFound = false;
                while (guessVertLoc < 9 && !guessFound)
                {
                    while (guessHorizLoc < 9 && !guessFound)
                    {
                        if (Grid[guessVertLoc, guessHorizLoc].uncertain)
                        {
                            int validChoice = 0;
                            while (validChoice < 9 && !guessFound)
                            {
                                if (Grid[guessVertLoc, guessHorizLoc].possibilities[validChoice])
                                {
                                    //Saving snapshot before guessing	
                                    guessedValue = validChoice + 1;
                                    Actions.push(Grid, guessedValue, guessHorizLoc, guessVertLoc);
                                    fillIn(Grid[guessVertLoc, guessHorizLoc], validChoice + 1);
                                    guessFound = true;
                                }
                                if (!guessFound)
                                    validChoice++;
                            }
                        }
                        if (!guessFound)
                            guessHorizLoc++;
                    }
                    if (!guessFound)
                    {
                        guessHorizLoc = 0;
                        guessVertLoc++;
                    }
                }
                deductiveReasoning();

                //Checking for conflicts
                if (!check('b'))
                {
                    Actions.undo(Grid);
                    Actions.pop();
                }
                //Checking for completion
                else
                {
                    finishedFill = check('a');
                    if (!finishedFill)
                    {
                        for (int x = 0; x < 9; x++)
                        {
                            for (int y = 0; y < 9; y++)
                            {
                                if (Grid[x, y].value == 0 && Grid[x, y].possibilityCounter == 0)
                                    conflict = true;
                            }
                        }
                        if (conflict)
                        {
                            Actions.undo(Grid);
                            Actions.pop();
                            conflict = false;
                        }
                    }
                }

                if (Grid[guessVertLoc, guessHorizLoc].possibilityCounter == 0)
                {
                    Actions.undo(Grid);
                    Actions.pop();
                }

            } while (!finishedFill);

            while (Actions.counter > 0) //Frees up memory used to store Snapshots
            {
                Actions.pop();
            }
        }

        public int solveSudoku()
        {
            int guessInvoked = 1;
            deductiveReasoning();

            if (!check('a'))
            {
                guessInvoked = 2;
                inductiveReasoning();
            }

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    MainPage.TextBoxGrid[i, j].Text = Grid[i, j].value.ToString();
            }

            return guessInvoked;

        }

        public int solveSudokuStepByStep()
        {
            //int guessInvoked = 0; //0: Not completed, 3: completed w/o inductive, 4: inductive required
            if (check('a'))
                return 3;
            if (deductiveReasoningStepByStep() == 1)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (Grid[i, j].value != 0)
                            MainPage.TextBoxGrid[i, j].Text = Grid[i, j].value.ToString();
                    }
                }
                return 0;
            }
            else
                return 4;

            /*if (!check('a'))
            {
                guessInvoked = true;
                inductiveReasoning();
            }*/

        }

        public void reset()
        {
            newFills = 0;
            colSlider = 0; rowSlider = 0;
            turns = 1;
            //Action doesn't need to be resetted since in inductive Reasoning all snapshots are popped in the end
            guessHorizLoc = 0; guessVertLoc = 0; guessedValue = 0;

            Color d = new Color();
            d.A = 255;
            d.R = 0;
            d.B = 0;
            d.G = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Grid[i, j].value = 0;
                    Grid[i, j].possibilityCounter = 9;
                    Grid[i, j].uncertain = true;
                    Grid[i, j].original = false;
                    for (int x = 0; x < 9; x++)
                        Grid[i, j].possibilities[x] = true;
                    MainPage.TextBoxGrid[i, j].Foreground = new SolidColorBrush(d);
                }
            }
        }
    }
}
