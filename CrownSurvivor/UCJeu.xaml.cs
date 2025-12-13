using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;

namespace CrownSurvivor
{
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {
        private double vitesse = 200; //pixels par seconde
        private bool gauche, droite, haut, bas; // Indiquent si la touche correspondante est actuellement enfoncée.
        private readonly Stopwatch chrono = new Stopwatch();
        private readonly DispatcherTimer timer;
        public UCJeu()
        {
            InitializeComponent();

            Canvas.SetLeft(Perso, 100);
            Canvas.SetTop(Perso, 100);
            //Place le personnage à sa position de départ (coordonnées 100, 100) sur le Canvas. 
            //SetLeft contrôle la position horizontale, SetTop la verticale.
            this.Loaded += (s, e) => this.Focus();
            //Quand le UserControl est chargé et affiché, on lui donne le focus clavier.
            //Sinon, les événements KeyDown / KeyUp n’arriveraient pas dans UCJeu_KeyDown / UCJeu_KeyUp.​
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20); // ~50 fois par seconde
            timer.Tick += Timer_Tick;
            chrono.Start();
            timer.Start();


        }

        private void UCJeu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) gauche = true;
            if (e.Key == Key.D) droite = true;
            if (e.Key == Key.Z) haut = true;
            if (e.Key == Key.S) bas = true;


        }

        private void UCJeu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q) gauche = false;
            if (e.Key == Key.D) droite = false;
            if (e.Key == Key.Z) haut = false;
            if (e.Key == Key.S) bas = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // temps écoulé depuis le dernier tick (en secondes)
            double dt = chrono.Elapsed.TotalSeconds;
            chrono.Restart();

            double x = Canvas.GetLeft(Perso);
            double y = Canvas.GetTop(Perso);

            if (gauche) x -= vitesse * dt;
            if (droite) x += vitesse * dt;
            if (haut) y -= vitesse * dt;
            if (bas) y += vitesse * dt;

            Canvas.SetLeft(Perso, x);
            Canvas.SetTop(Perso, y);
        }
    }
}
