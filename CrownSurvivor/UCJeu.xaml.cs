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
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {
        private double vitesse = 10;
        public UCJeu()
        {
            InitializeComponent();

            Canvas.SetLeft(Perso, 100);
            Canvas.SetTop(Perso, 100);


            this.Loaded += (s, e) => this.Focus();
        }

        private void UCJeu_KeyDown(object sender, KeyEventArgs e)
        {
            if (Perso == null) return;

            double x = Canvas.GetLeft(Perso); // Récupère la position X actuelle
            double y = Canvas.GetTop(Perso);      // Récupère la position Y actuelle

            if (e.Key == Key.Q) x -= vitesse;    // Q : gauche  (on diminue X)
            if (e.Key == Key.D) x += vitesse;    // D : droite  (on augmente X)
            if (e.Key == Key.Z) y -= vitesse;    // Z : haut    (on diminue Y)
            if (e.Key == Key.S) y += vitesse;    // S : bas     (on augmente Y)


            Canvas.SetLeft(Perso, x); // Applique la nouvelle position X
            Canvas.SetTop(Perso, y);    // Applique la nouvelle position Y
        }
    }
}
