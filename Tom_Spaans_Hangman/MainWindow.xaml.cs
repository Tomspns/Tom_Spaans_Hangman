using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace Tom_Spaans_Hangman
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filePath = "C:\\Users\\SLAB27\\Source\\Repos\\Tomspns\\Tom_Spaans_Hangman\\Tom_Spaans_Hangman\\mots_hangman.txt"; // Remplacez par le chemin de votre fichier
        private string currentWord;
        private HashSet<string> guessedLetters = new HashSet<string>();
        private int lives = 7; // Nombre de vies
        private DispatcherTimer timer; // Chronomètre
        private int timeLeft = 60; // Temps en secondes

        private List<string> hangmanImages = new List<string>()
        {
            "1.png", "2.png", "3.png", "4.png", "5.png", "6.png", "7.png"
        };

        public MainWindow()
        {
            InitializeComponent();
            currentWord = ChooseRandomWord().ToUpper(); // Choisir un mot aléatoire
            UpdateDisplay();
            StartTimer(); // Démarrer le chronomètre
        }

        private string ChooseRandomWord()
        {
            try
            {
                // Lire tous les mots du fichier
                List<string> words = new List<string>(File.ReadAllLines(filePath));

                if (words.Count == 0)
                {
                    throw new Exception("Le fichier ne contient aucun mot.");
                }

                // Choisir un mot aléatoire
                Random random = new Random();
                int index = random.Next(words.Count);
                return words[index].Trim(); // Retourner le mot sans espaces superflus
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Le fichier spécifié est introuvable.");
                return "ERREUR"; // Retourner un mot d'erreur
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
                return "ERREUR"; // Retourner un mot d'erreur
            }
        }

        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            TB_Timer.Text = "Temps restant: " + timeLeft;

            if (timeLeft <= 0)
            {
                timer.Stop();
                MessageBox.Show("Temps écoulé ! Le mot était: " + currentWord);
                ResetGame();
            }
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
                    UpdateHangmanImage();
                }

                if (lives <= 0)
                {
                    MessageBox.Show("Vous avez perdu! Le mot était: " + currentWord);
                    ResetGame();
                }
                else
                {
                    UpdateDisplay();
                }
            }
        }

        private void UpdateHangmanImage()
        {
            if (lives >= 0 && lives < hangmanImages.Count)
            {
                HangmanImage.Source = new BitmapImage(new Uri(hangmanImages[6 - lives], UriKind.Relative));
            }
        }

        private void UpdateDisplay()
        {
            StringBuilder displayWord = new StringBuilder();
            bool allLettersGuessed = true;

            foreach (char letter in currentWord)
            {
                if (guessedLetters.Contains(letter.ToString()))
                {
                    displayWord.Append(letter + " ");
                }
                else
                {
                    displayWord.Append("_ ");
                    allLettersGuessed = false;
                }
            }

            TB_Display.Text = displayWord.ToString().Trim();
            TB_Lives.Text = "Vies restantes: " + lives;

            if (allLettersGuessed)
            {
                MessageBox.Show("Félicitations ! Vous avez trouvé le mot : " + currentWord);
                ResetGame();
            }
        }

        private void ResetGame()
        {
            guessedLetters.Clear();
            lives = 7;
            timeLeft = 60; // Réinitialiser le temps
            currentWord = ChooseRandomWord().ToUpper(); // Choisir un nouveau mot
            UpdateDisplay();
            TB_Timer.Text = "Temps restant: 60"; // Réinitialiser l'affichage du temps
            StartTimer(); // Redémarrer le chronomètre
            foreach (UIElement element in Zone_keyboard.Children)
            {
                if (element is Button button)
                {
                    button.IsEnabled = true; // Réactive le bouton
                }
            }
        }
    }
}
