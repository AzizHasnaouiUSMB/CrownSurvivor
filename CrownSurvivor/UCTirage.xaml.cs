using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CrownSurvivor
{
    /// <summary>
    /// Logique d'interaction pour UCTirage.xaml
    /// </summary>
  
public partial class UCTirage : Window
    {
        
        private static readonly string[] Sprite =
       {
            "Im1.png",
            "Im2.png",
            "Im3.png",
            "Im4.png",
            "Im5.png",
            "Im6.png"
        };

        public UCTirage()
        {
            InitializeComponent();
        }
        private readonly Random random = new Random();
        private void butTiragePerso_Click(object sender, RoutedEventArgs e)
        {
            int TiragePers = random.Next(Sprite.Length);
            string fileName = Sprite[TiragePers];
            string imagePath = "Images/" + fileName;
            Uri imageUri = new Uri(imagePath, UriKind.Relative);
            BitmapImage bitmap = new BitmapImage(imageUri);
            //imgPerso.Source = bitmap;
        }
    }
}
