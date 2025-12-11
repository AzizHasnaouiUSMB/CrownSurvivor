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
            uc.butRegles.Click += AfficherRegles;
        }

        private void AfficherRegles(object sender, RoutedEventArgs e)
        {
            UCRegles uc = new UCRegles();
            ZoneJeu.Content = uc;
        }

    }
}
