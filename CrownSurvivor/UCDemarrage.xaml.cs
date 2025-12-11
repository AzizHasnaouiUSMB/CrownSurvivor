using System;
using System.Collections.Generic;
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

namespace CrownSurvivor
{
    /// <summary>
    /// Logique d'interaction pour UCDemarrage.xaml
    /// </summary>
    public partial class UCDemarrage : UserControl
    {
        private Random random = new Random();
        private List<BitmapImage> tirableSprites;
        private static readonly string[] SpriteFileNames =
        {
            "Im1.png",
            "Im2.png",
            "Im3.png",
            "Im4.png",
            "Im5.png",
            "Im6.png"
        };

        public UCDemarrage()
        {
            InitializeComponent();
            // 1. Charger toutes les images au démarrage
            tirableSprites = LoadAllSprites();

            // Afficher une image par défaut au début
            if (tirableSprites.Count > 0)
            {
                ResultImage.Source = tirableSprites[0];
            }
        }

        private void butTirage_Click(object sender, RoutedEventArgs e)
        {
            if (tirableSprites.Count > 0)
            {
                int randomIndex = random.Next(0, tirableSprites.Count);

                ResultImage.Source = tirableSprites[randomIndex];
            }
            else
            {
                MessageBox.Show("Aucun sprite n'a pu être chargé pour le tirage.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private List<BitmapImage> LoadAllSprites()
        {
            var sprites = new List<BitmapImage>();

            foreach (string fileName in SpriteFileNames)
            {
                try
                {
                    // URI pour charger une ressource (l'image intégrée)
                    // Le chemin est relatif au dossier du projet.
                    Uri uri = new Uri($"/{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name};component/{fileName}", UriKind.Relative);

                    BitmapImage image = new BitmapImage(uri);
                    sprites.Add(image);
                }
                catch (Exception ex)
                {
                    // Gérer les erreurs de chargement si un fichier est manquant
                    MessageBox.Show($"Erreur lors du chargement de l'image {fileName}: {ex.Message}", "Erreur de chargement", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return sprites;
        }

        private void butPara_Click(object sender, RoutedEventArgs e)
        {
            UCRegles reglesJeuUC = new UCRegles();

            //this.ConteneurDynamique.Content = reglesJeuUC;
        }
    }
}
