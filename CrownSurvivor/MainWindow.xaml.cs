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
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
            sonTest.MediaOpened += SonTest_MediaOpened;
            InitSon();
        }

        private void SonTest_MediaOpened(object sender, EventArgs e)
        {
            // C'est ici que vous pouvez vous assurer que tout est prêt
            // Si vous le souhaitez, vous pouvez déclencher un message de confirmation ici
            // MessageBox.Show("Le son a été chargé et est prêt à être joué.");
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
            uc.butRetourJeu.Click += RetourVersDemarrage;
        }

        private void Quitter(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AfficherPara(object sender, RoutedEventArgs e)
        {
            UCParametres uc = new UCParametres();
            ZoneJeu.Content = uc;
            uc.butRetourPara.Click += RetourVersDemarrage;
            uc.butTestSon.Click += JouerSon;
        }

        private void JouerSon(object sender, RoutedEventArgs e)
        {
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
        }

        private void InitSon()
        {
                
            



    }
}
