using System;
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
    public partial class MainWindow : Window
    {
        private UCTirage _ucTirage;
        public MainWindow()
        {
            InitializeComponent();
            AfficheDemarrage();
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
            UCTirage uc = new UCTirage();
            ZoneJeu.Content = uc;
            uc.butJouer.Click += AfficherJeu;
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

            
    }
}
