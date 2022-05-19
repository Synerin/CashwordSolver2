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
        HashSet<char> matchSet = new HashSet<char>(); // Set of winning letters
        public MainWindow()
        {
            InitializeComponent();
            InitializeLetterGrid();
            
        }

        /// <summary>
        /// Set up blank spaces for all cells in LetterGrid,
        /// which are to be filled in by the user
        /// </summary>
        public void InitializeLetterGrid()
        {
            for (int i = 0; i < LetterGrid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < LetterGrid.ColumnDefinitions.Count; j++)
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
        
        /// <summary>
        /// Check if word contains only characters in matchSet
        /// </summary>
        /// <param name="word"> Word being checked against the winning letters </param>
        public bool VerifyWord(String word)
        {
            if(word.Length == 0) return false; // Check if word is empty (invalid)

            // Check that each char in word is valid
            foreach(char ch in word)
            {
                if(!matchSet.Contains(ch)) return false;
            }

            return true; // No invalid chars detected
        }
    }
}
