using System;
using System.Collections.Generic;
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

namespace CrownSurvivor
{
    /// <summary>
    /// Logique d'interaction pour UCParametres.xaml
    /// </summary>
    public partial class UCParametres : UserControl
    {


        public UCParametres()
        {
            InitializeComponent();
            butRetrecirEcran.Visibility = Visibility.Hidden;
        }

        public void butTestSon_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.nivSon = slidSon.Value / 100;
            MainWindow.SetVolumeMusique();
        }

        private void butRetourPara_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.nivSon = slidSon.Value / 100;
            MainWindow.SetVolumeMusique();
        }

        private void butGrandEcran_Click(object sender, RoutedEventArgs e)
        {
            butRetrecirEcran.Visibility = Visibility.Visible;
            butGrandEcran.Visibility = Visibility.Hidden;
        }

        private void butRetrecirEcran_Click(object sender, RoutedEventArgs e)
        {
            butGrandEcran.Visibility= Visibility.Visible;
            butRetrecirEcran.Visibility = Visibility.Hidden;
        }
    }
}
