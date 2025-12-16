using System;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrownSurvivor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window
    {

        private MediaPlayer sonTest = new MediaPlayer();
        private UCTirage _ucTirage;
        public static double nivSon = 50;
        private static MediaPlayer musique;

        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
            InitSon();
            InitMusiqueAccueil();
        }

        private void AfficheDemarrage()
        {
            // crée et charge l'écran de démarrage
            UCDemarrage uc = new UCDemarrage();

            // associe l'écran au conteneur
            ZoneJeu.Content = uc;
            uc.butQuitter.Click += Quitter;
            uc.butRegles.Click += AfficherRegles;
            uc.butPara.Click += AfficherPara;
            uc.butJouer.Click += AfficherJeu;
            uc.butTirageP.Click += AfficherTirrage;
        }

        private void AfficherTirrage(object sender, RoutedEventArgs e)
        {
            _ucTirage = new UCTirage();            // << on mémorise l’instance
            ZoneJeu.Content = _ucTirage;
            _ucTirage.butJouer.Click += AfficherJeu;
        }

        private void AfficherJeu(object sender, RoutedEventArgs e)
        {
            int numero = 1; // valeur par défaut si pas passé par tirage

            // si on vient de UCTirage, on récupère NumeroImageTiree
            if (_ucTirage != null)
            {
                numero = _ucTirage.NumeroImageTiree;
            }

            UCJeu uc = new UCJeu(numero);          // on passe le numéro ici
            ZoneJeu.Content = uc;
            uc.butRetourJeu.Click += RetourDemarragePlusMusique;

            InitMusiqueJeu();
        }

        private void RetourDemarragePlusMusique(object sender, RoutedEventArgs e)
        {
            musique.Stop();
            InitMusiqueAccueil();
            AfficheDemarrage();
            
        }

        private void Quitter(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AfficherPara(object sender, RoutedEventArgs e)
        {
            UCParametres uc = new UCParametres();
            ZoneJeu.Content = uc;
            uc.butRetrecirEcran.Visibility = Visibility.Hidden;
            uc.butRetourPara.Click += RetourVersDemarrage;
            uc.butTestSon.Click += JouerSon;
            uc.slidSon.Value = nivSon;
            uc.butGrandEcran.Click += PasserEnGrandEcran;
            uc.butRetrecirEcran.Click += PasserEnPetitEcran;
        }

        private void PasserEnPetitEcran(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
        }

        private void PasserEnGrandEcran(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
        }
        

        private void JouerSon(object sender, RoutedEventArgs e)
        {
            sonTest.Volume = nivSon;
            Console.WriteLine(nivSon *100);
            sonTest.Position = TimeSpan.Zero;
            sonTest.Play();
            
        }

        private void AfficherRegles(object sender, RoutedEventArgs e)
        {
            UCRegles uc = new UCRegles();
            ZoneJeu.Content = uc;
            uc.butRetourRegles.Click += RetourVersDemarrage;
        }

        private void RetourVersDemarrage(object sender, RoutedEventArgs e)
        {
            AfficheDemarrage();
            InitMusiqueAccueil();
        }

        private void InitSon()
        {
            sonTest.Open(new Uri("sons/SonTest.wav", UriKind.Relative));
        }

        private void InitMusiqueAccueil()
        {
            musique = new MediaPlayer();
            musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/MusiqueFondAccueil.mp3"));
            musique.MediaEnded += RelanceMusique;
            musique.Volume = nivSon;
            musique.Play();
            Console.WriteLine("Okay");
        }

        private void InitMusiqueJeu()
        {
            musique.Stop();
            musique = new MediaPlayer();
            musique.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/MusiqueFondJeu.mp3"));
            musique.MediaEnded += RelanceMusique;
            musique.Volume = nivSon;
            musique.Play();
            Console.WriteLine("Oui");
        }

        private void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }

        public static void SetVolumeMusique()
        {
            if (musique != null)
                musique.Volume = nivSon;
        }

    }
}
