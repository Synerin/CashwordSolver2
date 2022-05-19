using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CashwordSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // TODO: Add trigger to update set when WinningLetters is updated
        HashSet<char> matchSet; // Set of winning letters
        int GRID_SIZE; // Size of rows and cols in grid
        char[,] gridArray; // LetterGrid converted to a char array

        public MainWindow()
        {
            InitializeComponent();

            matchSet = new HashSet<char>();
            GRID_SIZE = LetterGrid.RowDefinitions.Count;
            gridArray = new char[GRID_SIZE, GRID_SIZE];

            InitializeLetterGrid();
        }

        /// <summary>
        /// Set up blank spaces for all cells in LetterGrid, which are to be filled in by the user
        /// </summary>
        public void InitializeLetterGrid()
        {
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    TextBox square = new()
                    {
                        MaxLength = 1, // Limit each cell to a length of one character
                        // Fill cell box
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    // Set square coordinates as (i, j)
                    Grid.SetRow(square, i);
                    Grid.SetColumn(square, j);

                    // Insert square
                    LetterGrid.Children.Add(square);
                }
            }
        }

        // TODO: Call method on button press
        public void TransferToGridArray()
        {
            int i = 0, j = 0;

            // Fill gridArray with the cells from LetterGrid
            foreach(var cell in LetterGrid.Children)
            {
                gridArray[i, j] = (char)cell;

                i++;

                // Loop around to the front
                if(i == GRID_SIZE)
                {
                    i = 0;
                    j++;
                }
            }
        }

        /// <summary>
        /// Check if word contains only characters in matchSet
        /// </summary>
        /// <param name="word">Word being checked against the winning letters</param>
        public bool VerifyWord(String word)
        {
            if (word.Length == 0) return false; // Check if word is empty (invalid)

            // Check that each char in word is valid
            foreach (char ch in word)
            {
                if (!matchSet.Contains(ch)) return false;
            }

            return true; // No invalid chars detected
        }

        /// <summary>
        /// Scans LetterGrid for any words to be considered for score calculation
        /// </summary>
        /// <returns>The number of valid words</returns>
        public int ScanGrid()
        {
            int result = 0;

            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    // TODO: Change Grid to DataGrid and/or figure out how to get child at (x, y)
                    bool lettersAbove = j > 0 ? LetterGrid.Get(i, j - 1) != null : false;
                    bool lettersBelow = j < GRID_SIZE - 1 ? LetterGrid.Get(i, j + 1) != null : false;
                    bool lettersLeft = i > 0 ? LetterGrid.Get(i - 1, j) != null : false;
                    bool lettersRight = i < GRID_SIZE - 1 ? LetterGrid.Get(i + 1, j) != null : false;

                    // Check if there is a valid word on the vertical axis
                    if (!lettersAbove && lettersBelow)
                    {
                        String verticalStr = VerticalScan(i, j);
                        if (VerifyWord(verticalStr)) result++;
                    }

                    // Check if there is a valid word on the horizontal axis
                    if (!lettersLeft && lettersRight)
                    {
                        String horizontalStr = HorizontalScan(i, j);
                        if(VerifyWord(horizontalStr)) result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Finds a word on the vertical axis of LetterGrid
        /// </summary>
        /// <param name="i">The beginning row of the word</param>
        /// <param name="j">The column of the word</param>
        /// <returns>The word found on column j</returns>
        public String VerticalScan(int i, int j)
        {
            StringBuilder sb = new();

            while(i < GRID_SIZE)
            {
                char ch = LetterGrid.Get(i, j); // Get letter at current position

                if (!Char.IsLetter(ch)) break; // End of word

                sb.Append(ch);
                i++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Finds a word on the horizontal axis of LetterGrid
        /// </summary>
        /// <param name="i">The row of the word</param>
        /// <param name="j">The beginning column of the word</param>
        /// <returns>The word found on row i</returns>
        public String HorizontalScan(int i, int j)
        {
            StringBuilder sb = new();

            while (j < GRID_SIZE)
            {
                char ch = LetterGrid.Get(i, j); // Get letter at current position

                if (!Char.IsLetter(ch)) break; // End of word

                sb.Append(ch);
                j++;
            }

            return sb.ToString();
        }
    }
}
