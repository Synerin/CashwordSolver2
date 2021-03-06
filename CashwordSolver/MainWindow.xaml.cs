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
                        CharacterCasing = CharacterCasing.Upper, // Automatically capitalize letter input
                        // Fill cell box
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        BorderThickness = new Thickness(0),
                        Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5CC3F0"))
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
        /// <summary>
        /// Insert the values of the cells in LetterGrid to the gridArray char array
        /// </summary>
        public void TransferToGridArray()
        {
            int i = 0, j = 0;

            // Fill gridArray with the cells from LetterGrid
            foreach(TextBox cell in LetterGrid.Children)
            {
                try
                {
                    gridArray[i, j] = Char.Parse(cell.Text); // Can parse a char from the cell at (i, j)
                }
                catch
                {
                    gridArray[i, j] = ' '; // Could not parse a char, set index (i, j) to a blank space
                }

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
                    bool lettersAbove = j > 0 && Char.IsLetter(gridArray[i, j - 1]);
                    bool lettersBelow = j < GRID_SIZE - 1 && Char.IsLetter(gridArray[i, j + 1]);
                    bool lettersLeft = i > 0 && Char.IsLetter(gridArray[i - 1, j]);
                    bool lettersRight = i < GRID_SIZE - 1 && Char.IsLetter(gridArray[i + 1, j]);

                    // Check if there is a valid word on the vertical axis
                    if (!lettersAbove && lettersBelow)
                    {
                        String verticalStr = VerticalScan(i, j);
                        if (VerifyWord(verticalStr))
                        {
                            result++;
                        }
                    }

                    // Check if there is a valid word on the horizontal axis
                    if (!lettersLeft && lettersRight)
                    {
                        String horizontalStr = HorizontalScan(i, j);
                        if(VerifyWord(horizontalStr))
                        {
                            result++;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Finds a word on the vertical axis of LetterGrid
        /// </summary>
        /// <param name="i">The column of the word</param>
        /// <param name="j">The beginning row of the word</param>
        /// <returns>The word found on column i</returns>
        public String VerticalScan(int i, int j)
        {
            StringBuilder sb = new();

            while(j < GRID_SIZE)
            {
                char ch = gridArray[i, j]; // Get letter at current position

                if (!Char.IsLetter(ch)) break; // End of word

                sb.Append(ch);
                j++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Finds a word on the horizontal axis of LetterGrid
        /// </summary>
        /// <param name="i">The beginning column of the word</param>
        /// <param name="j">The row of the word</param>
        /// <returns>The word found on row i</returns>
        public String HorizontalScan(int i, int j)
        {
            StringBuilder sb = new();

            while (i < GRID_SIZE)
            {
                char ch = gridArray[i, j]; // Get letter at current position

                if (!Char.IsLetter(ch)) break; // End of word

                sb.Append(ch);
                i++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Check if word contains only characters in matchSet
        /// </summary>
        /// <param name="word">Word being checked against the winning letters</param>
        public bool VerifyWord(String word)
        {
            if (word.Length < 2) return false; // Check if word is too short (invalid)

            // Check that each char in word is valid
            foreach (char ch in word)
            {
                if (!matchSet.Contains(ch)) return false;
            }

            return true; // No invalid chars detected
        }

        /// <summary>
        /// Set up remaining components and report score
        /// </summary>
        private void Submit(object sender, RoutedEventArgs e) {
            // Add winning letters to matchSet
            foreach(char ch in WinningLetters.Text)
            {
                matchSet.Add(ch);
            }

            // Set up gridArray
            TransferToGridArray();

            // Scan grid for matching words
            int matchingWords = ScanGrid();

            // Report score
            Score.Text = "" + matchingWords;
        }
    }
}
