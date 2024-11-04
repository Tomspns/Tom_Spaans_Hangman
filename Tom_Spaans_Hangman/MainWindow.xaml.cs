using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
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
        private string filePath = "C:\\Users\\SLAB27\\Source\\Repos\\Tomspns\\Tom_Spaans_Hangman\\Tom_Spaans_Hangman\\mots_hangman.txt"; // chemin de votre fichier
        private string currentWord; // mot à deviner
        private HashSet<string> guessedLetters = new HashSet<string>(); // lettres déjà devinées
        private int lives = 7; // Nombre de vies
        private DispatcherTimer timer; // Chronomètre
        private int timeLeft = 60; // Temps en secondes

        private List<string> hangmanImages = new List<string>() // liste des images
        {
            "1.png", "2.png", "3.png", "4.png", "5.png", "6.png", "7.png"
        };

        private string winSoundPath = "C:\\Users\\SLAB27\\Source\\Repos\\Tomspns\\Tom_Spaans_Hangman\\Tom_Spaans_Hangman\\victoire.wav"; // Chemin vers le son de victoire
        private string loseSoundPath = "C:\\Users\\SLAB27\\Source\\Repos\\Tomspns\\Tom_Spaans_Hangman\\Tom_Spaans_Hangman\\defaite.wav"; // Chemin vers le son de défaite

        public MainWindow()
        {
            InitializeComponent();
            currentWord = ChooseRandomWord().ToUpper(); // choisir un mot aléatoire
            UpdateDisplay(); // met à jour l'affichage
            StartTimer(); // démarre le chronomètre
        }

        private string ChooseRandomWord()
        {
            try
            {
                // lire tous les mots du fichier
                List<string> words = new List<string>(File.ReadAllLines(filePath));

                if (words.Count == 0)
                {
                    throw new Exception("Le fichier ne contient aucun mot."); // gère le cas où le fichier est vide
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
            timer = new DispatcherTimer(); // créer un chronomètre
            timer.Interval = TimeSpan.FromSeconds(1); // définir un intervalle de 1s
            timer.Tick += Timer_Tick;
            timer.Start(); // démarre le chronomètre
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--; // décrémente le temps
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
            Button btn = sender as Button; //récupère le bouton qui a été cliqué
            string btnContent = btn.Content.ToString(); // récupère le contenu du bouton
            btn.IsEnabled = false; // désactive le bouton après le clic

            if (!guessedLetters.Contains(btnContent))
            {
                guessedLetters.Add(btnContent); // ajouter la lettre 
                if (!currentWord.Contains(btnContent.ToUpper()))
                {
                    lives--; // Perd une vie si la lettre n'est pas dans le mot
                    MessageBox.Show($"Erreur! Il vous reste {lives} vies.");
                    UpdateHangmanImage(); // Mettre à jour l'image du pendu
                }

                if (lives <= 0)
                {
                    PlaySound(loseSoundPath); // Jouer le son de défaite
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
            StringBuilder displayWord = new StringBuilder(); // construire le mot à afficher
            bool allLettersGuessed = true; // vérifier si toutes les lettres ont été devinées

            foreach (char letter in currentWord)
            {
                if (guessedLetters.Contains(letter.ToString()))
                {
                    displayWord.Append(letter + " "); // afficher la lettre si devinée
                }
                else
                {
                    displayWord.Append("_ "); // afficher un underscore si non devinée
                    allLettersGuessed = false; // indiquer qu'il reste des lettres à deviner
                }
            }

            TB_Display.Text = displayWord.ToString().Trim();
            TB_Lives.Text = "Vies restantes: " + lives;

            if (allLettersGuessed)
            {
                PlaySound(winSoundPath); // Jouer le son de victoire
                MessageBox.Show("Félicitations ! Vous avez trouvé le mot : " + currentWord);
                ResetGame();
            }
        }

        private void PlaySound(string soundPath)
        {
            try
            {
                using (SoundPlayer player = new SoundPlayer(soundPath))
                {
                    player.PlaySync(); // joue le son et attend qu'il se termine
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la lecture du son : {ex.Message}"); // gère les erreurs de lecture
            }
        }

        private void ResetGame()
        {
            guessedLetters.Clear(); // réinitialiser les lettres devinées
            lives = 7; // réinitialiser le nombre de vies
            timeLeft = 60; // réinitialiser le temps
            currentWord = ChooseRandomWord().ToUpper(); // choisir un nouveau mot
            UpdateDisplay();
            TB_Timer.Text = "Temps restant: 60"; // réinitialiser l'affichage du temps
            StartTimer(); // redémarrer le chronomètre

            // réactive les lettres du clavier 
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
