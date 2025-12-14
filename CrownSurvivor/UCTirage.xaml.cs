using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Logique d'interaction pour UCTirage.xaml
    /// </summary>
    public partial class UCTirage : UserControl

    {
        private static readonly BitmapImage[] Sprite = new BitmapImage[6];

    private readonly Random random = new Random();

        public UCTirage()
        {
            InitializeComponent();
            butJouer.IsEnabled = false;
            butJouer.Visibility = Visibility.Hidden;
            butTirage.Visibility = Visibility.Visible;

        }

        private void butTirage_Click(object sender, RoutedEventArgs e)
        {
            int numeroImage = random.Next(1, Sprite.Length+1);
            Console.WriteLine(numeroImage);
            string Chemin = $"/ImPerso/im{numeroImage}.png";
            Console.WriteLine(Chemin);
            Uri path = new Uri($"pack://application:,,,{Chemin}");
            BitmapImage bitmap = new BitmapImage(path);
            imgPerso.Source = bitmap;

            butTirage.IsEnabled = false;
            butJouer.IsEnabled = true;
            butTirage.Visibility= Visibility.Hidden;
            butJouer.Visibility = Visibility.Visible;
            
        }

    }
}
