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
        private Random random = new Random();
        private int enemieSurLeTerrain = 0;
        private int damage = 0;
        private int score = 0;

        private double vitesseEnemie = 2;
        private double vitesse = 5;

        // Indiquent si la touche correspondante est actuellement enfoncée.
        private bool gauche, droite, haut, bas;

        private readonly DispatcherTimer timer;

        public UCJeu()
        {
            InitializeComponent();

            // Position de départ du perso
            Canvas.SetLeft(Perso, 100);
            Canvas.SetTop(Perso, 100);

            // Focus clavier
            this.Loaded += (s, e) => this.Focus();

            // Boucle de jeu
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20); // ~50 fois par seconde
            timer.Tick += Timer_Tick;
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
            double x = Canvas.GetLeft(Perso);
            double y = Canvas.GetTop(Perso);

            // déplacement fixe à chaque tick
            if (gauche) x -= vitesse;
            if (droite) x += vitesse;
            if (haut) y -= vitesse;
            if (bas) y += vitesse;

            Canvas.SetLeft(Perso, x);
            Canvas.SetTop(Perso, y);
        }

        private void Enemie_Spawn()
        { ImageBrush enemiesprite = new ImageBrush();

            enemieSurLeTerrain = random.Next(1, 6);

            switch (enemieSurLeTerrain)
            { 


            } 
        } 
    } 
}
    
