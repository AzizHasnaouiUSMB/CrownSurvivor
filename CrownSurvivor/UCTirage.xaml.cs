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

        private static readonly string[] TabDescription =
            [
                "Vitesse de déplacement : le personnage se déplace 20% plus vite que d'habitude",
                "Dégâts augmentés : les projectiles infligent +20% de dégats ",
                "Vitesse de tir : la vitesse des projectiles 20% plus rapide",
                "Tir chanceux (One-Shot) : un projectile à 1% de chance de tuer un ennemi instantanément",
                "Dégâts de zone : La hitbox du projectile est 50% plus grosse ",
                "Ralentissement : chaque ennemi toucher à ça vitesse diminuer de 20%",
            ];


    private readonly Random random = new Random();
    public int NumeroImageTiree { get; private set; }

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
            NumeroImageTiree = numeroImage;
            Console.WriteLine(numeroImage);
            string Chemin = $"/ImPerso/im{numeroImage}.png";
            Console.WriteLine(Chemin);
            Uri path = new Uri($"pack://application:,,,{Chemin}");
            BitmapImage bitmap = new BitmapImage(path);
            imgPerso.Source = bitmap;

            butTirage.IsEnabled = false;
            butJouer.IsEnabled = true;
            butTirage.Visibility = Visibility.Hidden;
            butJouer.Visibility = Visibility.Visible;

            description.Text = TabDescription[numeroImage];
            
        }

    }
}
