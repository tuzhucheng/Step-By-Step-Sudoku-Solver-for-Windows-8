using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Step_by_Step_Sudoku_Solver
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Step_by_Step_Sudoku_Solver.Common.LayoutAwarePage
    {

        public static TextBox[,] TextBoxGrid = new TextBox[9, 9];
        public static TextBlock MessageBox = new TextBlock();
        public static Button solveButton = new Button();
        public static Button clearButton = new Button();
        public static ToggleSwitch StepToggle = new ToggleSwitch();
        public static Image Logo = new Image();
        SudokuGrid GridObject = new SudokuGrid();
        public static bool stepByStepEnabled = false;
        private ResourceLoader resLoader = new Windows.ApplicationModel.Resources.ResourceLoader();

        public void populateGrid()
        {
            Color f = new Color();
            f.A = 255;
            f.R = 0;
            f.B = 150;
            f.G = 0;

            GridObject.reset();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (TextBoxGrid[i, j].Text != "0" && TextBoxGrid[i, j].Text != "" && TextBoxGrid[i, j].Text != null)
                    {
                        GridObject.fillIn(GridObject.Grid[i, j], Int32.Parse(TextBoxGrid[i, j].Text));
                    }
                    else
                    {
                        GridObject.Grid[i, j].original = true;
                        TextBoxGrid[i, j].Foreground = new SolidColorBrush(f);
                    }
                }
            }
        }

        public bool catchInvalidType()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (TextBoxGrid[i, j].Text != "" && TextBoxGrid[i, j].Text != "1" && TextBoxGrid[i, j].Text != "2" && TextBoxGrid[i, j].Text != "3" && TextBoxGrid[i, j].Text != "4" && TextBoxGrid[i, j].Text != "5" && TextBoxGrid[i, j].Text != "6" && TextBoxGrid[i, j].Text != "7" && TextBoxGrid[i, j].Text != "8" && TextBoxGrid[i, j].Text != "9")
                        return false;
                }
            }

            return true; //All good!
        }

        public static void setLayoutDimensions()
        {
            Rect bounds = Window.Current.Bounds;
            double windowWidth = bounds.Width;
            double windowHeight = bounds.Height;

            //Initializing the 81 textboxes
            int boxSideDim = (int)(0.0439 * windowWidth);
            int borderThickness = (int)(Math.Ceiling(0.0167 * boxSideDim));
            Thickness commonThickness = new Thickness(borderThickness);
            int horizontalInc = boxSideDim, verticalInc = boxSideDim;
            //double horizontalPadding = 0.167 * boxSideDim, verticalPadding = 0.067 * boxSideDim;
            int horizontalMargin = 120, verticalMargin = 0;
            int whiteSpaceControlLeft = 120+9*boxSideDim+60;
            int fontSize = (int)boxSideDim / 2;

            for (int i = 0; i < 9; i++)
            {
                horizontalMargin = 120;
                for (int j = 0; j < 9; j++)
                {
                    //TextBoxGrid[i, j] = (TextBox)mainSection.FindName("_" + index.ToString());
                    TextBoxGrid[i, j].Width = TextBoxGrid[i, j].Height = boxSideDim;
                    TextBoxGrid[i, j].MinWidth = TextBoxGrid[i, j].MinHeight = boxSideDim;
                    TextBoxGrid[i, j].FontSize = fontSize;
                    //TextBoxGrid[i, j].Padding = new Thickness(horizontalPadding, verticalPadding, 0, 0);
                    TextBoxGrid[i, j].Margin = new Thickness(horizontalMargin, verticalMargin, 0, 0);
                    TextBoxGrid[i, j].BorderThickness = commonThickness;
                    horizontalMargin += horizontalInc;
                }
                verticalMargin += verticalInc;
            }

            //Initializing controls' dimensions
            StepToggle.FontSize = 0.25 * boxSideDim;
            StepToggle.Margin = new Thickness(whiteSpaceControlLeft, 0, 0, 0);
            StepToggle.Width = 0.143 * windowWidth;
            StepToggle.Height = 0.051 * windowWidth;

            solveButton.Margin = new Thickness(whiteSpaceControlLeft, 0.11 * windowHeight, 0, 0);
            solveButton.Width = 2 * boxSideDim;
            //solveButton.Height = 1.13 * boxSideDim;
            solveButton.FontSize = 0.25 * boxSideDim;

            clearButton.Margin = new Thickness(whiteSpaceControlLeft + solveButton.Width + 0.025 * windowWidth, 0.11 * windowHeight, 0, 0);
            clearButton.Width = 2 * boxSideDim;
            //clearButton.Height = 1.13 * boxSideDim;
            clearButton.FontSize = 0.25 * boxSideDim;

            MessageBox.Height = 0.6 * windowHeight;
            MessageBox.Width = windowWidth - whiteSpaceControlLeft - 0.05 * windowWidth;
            MessageBox.Margin = new Thickness(whiteSpaceControlLeft, 0.18 * windowHeight, 0, 0);
            MessageBox.FontSize = 0.5 * fontSize;
        }

        public MainPage()
        {
            this.InitializeComponent();
            freshSessionStart = false;

            Color c = new Color();
            c.A = 255;
            c.R = 168;
            c.B = 168;
            c.G = 168;

            int index = 0;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //TextBoxGrid[i, j] = (TextBox)mainSection.FindName("_" + index.ToString());
                    TextBoxGrid[i, j] = new TextBox();
                    TextBoxGrid[i, j].Style = (Style)(this.Resources["SudokuUnitTextBox"]);
                    TextBoxGrid[i, j].Name = "_" + index.ToString();
                    TextBoxGrid[i, j].GotFocus += new RoutedEventHandler(gridGotFocus);
                    TextBoxGrid[i, j].KeyDown += new KeyEventHandler(gridKeyDown);

                    if (j >= 3 && j <= 5)
                    {
                        if ((i >= 0 && i <= 2) || (i >= 6 && i <= 8))
                            TextBoxGrid[i, j].Background = new SolidColorBrush(c);
                    }
                    else
                    {
                        if (i >= 3 && i <= 5)
                            TextBoxGrid[i, j].Background = new SolidColorBrush(c);
                    }
                    mainSection.Children.Add(TextBoxGrid[i, j]);
                    index++;
                }
            }
            //((TextBox)mainSection.FindName("_0")).Focus(FocusState.Keyboard);

            //Adding static controls
            solveButton.Style = (Style)(this.Resources["SolveButton"]);
            solveButton.Click += new RoutedEventHandler(startSolve);
            clearButton.Style = (Style)(this.Resources["ClearButton"]);
            clearButton.Click += new RoutedEventHandler(clearGrid);
            StepToggle.Style = (Style)(this.Resources["StepToggle"]);
            StepToggle.Toggled += new RoutedEventHandler(changedStepByStep);
            mainSection.Children.Add(solveButton);
            mainSection.Children.Add(clearButton);
            mainSection.Children.Add(StepToggle);

            //Adding static MessageBox
            MessageBox.Style = (Style)(this.Resources["MessageBox"]);
            MessageBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            mainSection.Children.Add(MessageBox);

            Logo.Opacity = 0;
            Logo.Source = ImageFromRelativePath(this, "/Assets/SplashScreen.png");
            mainSection.Children.Add(Logo);
                
            setLayoutDimensions();
        }


            /// <summary>
            /// Populates the page with content passed during navigation.  Any saved state is also
            /// provided when recreating a page from a prior session.
            /// </summary>
            /// <param name="navigationParameter">The parameter value passed to
            /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
            /// </param>
            /// <param name="pageState">A dictionary of state preserved by this page during an earlier
            /// session.  This will be null the first time a page is visited.</param>
            protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
            {
                // Restore values stored in session state.
                if (pageState != null && pageState.ContainsKey("messageBoxText"))
                {
                    //MessageBox.Text = pageState["messageBoxText"].ToString();
                }

                // Restore values stored in app data.
                Windows.Storage.ApplicationDataContainer roamingSettings =
                    Windows.Storage.ApplicationData.Current.RoamingSettings;
                //if (roamingSettings.Values["StepToggle"].ToString().Equals("On"))
                //{
                //    StepToggle.IsOn = true;
                //    stepByStepEnabled = true;
                //    solveButton.Content = "Solve Next";
                //}
                //else
                //{
                //    StepToggle.IsOn = false;
                //    stepByStepEnabled = false ;
                //    solveButton.Content = "Solve";
                //}
                int index = 0;
                do
                {
                    for (int j = 0; j < 9; j++)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            if (roamingSettings.Values.ContainsKey(index.ToString()) && !roamingSettings.Values[index.ToString()].ToString().Equals("0"))
                            {
                                TextBoxGrid[j, k].Text = roamingSettings.Values[index.ToString()].ToString();
                            }
                            index++;
                        }
                    }
                } while (index < 81);
            }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            pageState["messageBoxText"] = MessageBox.Text;
            
        }

        private void startSolve(object sender, RoutedEventArgs e)
        {
            bool typeValid = catchInvalidType();
            if (typeValid)
            {
                populateGrid();
                bool noConflicts = GridObject.check('b');
                if (noConflicts)
                {
                    //Stores the initial grid in the cloud for user access later
                    Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    //if (stepByStepEnabled)
                    //    roamingSettings.Values["StepToggle"] = "On";
                    //else
                    //    roamingSettings.Values["StepToggle"] = "Off";
                    int index = 0;
                    do
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            for (int k = 0; k < 9; k++)
                            {
                                roamingSettings.Values[index.ToString()] = GridObject.Grid[j, k].value;
                                index++;
                            }
                        }
                    } while (index < 81);
                        

                    int feedback; //Not showing steps: 1) guess not invoked 2) guess invoked *** showing steps: 3) can be completed using deductive alone 4) must use inductive
                    if (stepByStepEnabled)
                        feedback = GridObject.solveSudokuStepByStep();
                    else
                        feedback = GridObject.solveSudoku();
                    if (feedback == 1 || feedback == 3)
                        messageBoxReplaceText(resLoader.GetString("msgSolved"));
                    else if (feedback == 2)
                        messageBoxReplaceText(resLoader.GetString("msgSolvedUsedInductive"));
                    else if (feedback == 4)
                        messageBoxReplaceText(resLoader.GetString("msgNoDeductions"));
                }
                else
                    messageBoxReplaceText(resLoader.GetString("msgConflicts"));

            }
            else
                messageBoxReplaceText(resLoader.GetString("msgNumbersOnly"));

        }

        private void clearGrid(object sender, RoutedEventArgs e)
        {
            GridObject.reset();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    TextBoxGrid[i, j].Text = "";
            }
            MessageBox.Text = "";

        }

        public static void messageBoxAddText(String text)
        {
            MessageBox.Text += text;
        }

        public static void messageBoxReplaceText(String text)
        {
            MessageBox.Text = text;
        }

        private void changedStepByStep(object sender, RoutedEventArgs e)
        {
            if (StepToggle.IsOn)
            {
                stepByStepEnabled = true;
                solveButton.Content = "Solve Next";
            }
            else
            {
                stepByStepEnabled = false;
                solveButton.Content = "Solve";
            }
        }

        private void gridGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void gridKeyDown(object sender, KeyRoutedEventArgs e) //Allows user to use arrow keys to navigate through grid
        {
            String name = ((TextBox)sender).Name;
            int nameInt;
            name = name.Remove(0, 1);
            nameInt = Convert.ToInt32(name);
            if (e.Key == Windows.System.VirtualKey.Left && nameInt != 0)
                nameInt--;
            else if (e.Key == Windows.System.VirtualKey.Right && nameInt != 80)
                nameInt++;
            else if (e.Key == Windows.System.VirtualKey.Up && nameInt > 8)
                nameInt -= 9;
            else if (e.Key == Windows.System.VirtualKey.Down && nameInt < 72)
                nameInt += 9;
            //else if ((int)e.Key >= 49 && (int)e.Key <= 57)
            //    ((TextBox)mainSection.FindName(name)).Text = (((int)e.Key) - 48).ToString(); //Change textbox value immediately
            name = String.Concat("_", nameInt.ToString());
            ((TextBox)mainSection.FindName(name)).Focus(FocusState.Keyboard);
        }

        public static void onSnapped()
        {
            Rect bounds = Window.Current.Bounds;
            double windowWidth = bounds.Width;
            double windowHeight = bounds.Height;

            MessageBox.Opacity = 0;
            solveButton.Opacity = 0;
            clearButton.Opacity = 0;
            StepToggle.Opacity = 0;
            MessageBox.Opacity = 0;
            Logo.Opacity = 100;
            if (windowWidth >= 620)
            {
                Logo.Width = 620;
                Logo.Height = 300;
            }
            else
            {
                Logo.Width = 0.8 * windowWidth;
                Logo.Height = 0.48 * Logo.Width;
            }
            Logo.Margin = new Thickness(windowWidth/2-Logo.Width/2, 10, 0, 0);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    TextBoxGrid[i, j].Opacity = 0;
            }
        }

        public static void onFullLandscape()
        {
            setLayoutDimensions();
            MessageBox.Opacity = 100;
            solveButton.Opacity = 100;
            clearButton.Opacity = 100;
            StepToggle.Opacity = 100;
            MessageBox.Opacity = 100;
            Logo.Opacity = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    TextBoxGrid[i, j].Opacity = 100;
            }
        }

        public static Windows.UI.Xaml.Media.Imaging.BitmapImage ImageFromRelativePath(FrameworkElement parent, string path)
        {
            var uri = new Uri(parent.BaseUri, path);
            Windows.UI.Xaml.Media.Imaging.BitmapImage bmp = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
            bmp.UriSource = uri;
            return bmp;
        }
    }
}
