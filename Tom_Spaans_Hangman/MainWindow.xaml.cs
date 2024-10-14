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
        private List<string> words = new List<string>()
        {
            "arbre" , "maison" , "ordinateur" , "voiture" , "chien" , "chat" , "ciel" , "plage" , "montagne" , "fleur"
        };

        private string currentWord;
        private HashSet<string> guessedLetters = new HashSet<string>();
        private int lives = 5; // Nombre de vies

        public MainWindow()
        {
            Random random = new Random();
            currentWord = words[random.Next(words.Count)].ToUpper();
            InitializeComponent();
            UpdateDisplay();
        }

        public void DisplayWord()
        {
            foreach (char letter in currentWord)
            {
                if (guessedLetters.Contains(letter.ToString()))
                {
                    Console.Write(letter + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine();
        }

        private void BTN_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string btnContent = btn.Content.ToString();
            btn.IsEnabled = false;

            if (!guessedLetters.Contains(btnContent))
            {
                guessedLetters.Add(btnContent);
                if (!currentWord.Contains(btnContent.ToUpper()))
                {
                    lives--; // Perd une vie
                    MessageBox.Show($"Erreur! Il vous reste {lives} vies.");
                }

                if (lives <= 0)
                {
                    MessageBox.Show("Vous avez perdu! Le mot était: " + currentWord);
                    // Réinitialiser le jeu ou fermer
                    ResetGame();
                }

                else
                {
                    UpdateDisplay();
                }
            }
        }

        private void UpdateDisplay()
        {
            // Met à jour l'affichage du TextBox en fonction des lettres devinées
            StringBuilder displayWord = new StringBuilder();
            bool allLettersGuessed = true; // Pour vérifier si toutes les lettres ont été devinées

            foreach (char letter in currentWord)
            {
                if (guessedLetters.Contains(letter.ToString()))
                {
                    displayWord.Append(letter + " ");
                }
                else
                {
                    displayWord.Append("_ ");
                    allLettersGuessed = false; // Une lettre n'a pas encore été devinée
                }
            }

            TB_Display.Text = displayWord.ToString().Trim();
            TB_Lives.Text = "Vies restantes: " + lives; // Met à jour l'affichage des vies

            if (allLettersGuessed)
            {
                MessageBox.Show("Félicitations ! Vous avez trouvé le mot : " + currentWord);
                ResetGame(); // Réinitialise le jeu
            }
        }

        private void ResetGame()
        {
            guessedLetters.Clear();
            lives = 5; // Réinitialise les vies
            Random random = new Random();
            currentWord = words[random.Next(words.Count)].ToUpper();
            UpdateDisplay();
        }
    }
}
