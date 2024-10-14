using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Tom_Spaans_Hangman
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentWord = "EXAMPLE"; // Exemple de mot pour le jeu
        private HashSet<string> guessedLetters = new HashSet<string>();

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        private void BTN_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string btnContent = btn.Content.ToString();
            btn.IsEnabled = false;

            if (!guessedLetters.Contains(btnContent))
            {
                guessedLetters.Add(btnContent);
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            // Met à jour l'affichage du TextBox en fonction des lettres devinées
            StringBuilder displayWord = new StringBuilder();
            foreach (char letter in currentWord)
            {
                if (guessedLetters.Contains(letter.ToString()))
                {
                    displayWord.Append(letter + " ");
                }
                else
                {
                    displayWord.Append("_ ");
                }
            }

            TB_Display.Text = displayWord.ToString().Trim();
        }
    }
}
