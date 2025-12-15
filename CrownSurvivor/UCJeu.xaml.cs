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

using System.Windows.Threading;
//rajout 

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



        const int STAT_PV = 0;
        const int STAT_ATTAQUE = 1;
        const int STAT_DEFENSE = 2;
        const int STAT_VITESSE = 3;
        const int NB_STATS = 4;

        double[] stats = new double[NB_STATS];

        private int _numeroImage;

        public UCJeu(int numeroImage)
        {   
            InitializeComponent();
            _numeroImage = numeroImage;
           

            // Position de départ du perso
            Canvas.SetLeft(imgPerso, 100);
            Canvas.SetTop(imgPerso, 100);

            // Focus clavier
            this.Loaded += (s, e) => this.Focus();

            // Boucle de jeu
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20); // ~50 fois par seconde
            timer.Tick += Timer_Tick;
            timer.Start();

            string Chemin = $"/ImPerso/im{_numeroImage}.png";
            Console.WriteLine(Chemin);
            Uri path = new Uri($"pack://application:,,,{Chemin}");
            BitmapImage bitmap = new BitmapImage(path);
            imgPerso.Source = bitmap;
            stats = Personnage(_numeroImage);
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
            double x = Canvas.GetLeft(imgPerso);
            double y = Canvas.GetTop(imgPerso);

            // déplacement fixe à chaque tick
            if (gauche) x -= vitesse;
            if (droite) x += vitesse;
            if (haut) y -= vitesse;
            if (bas) y += vitesse;

            Canvas.SetLeft(imgPerso, x);
            Canvas.SetTop(imgPerso, y);
        }

        private void Enemie_Spawn()
        { ImageBrush enemiesprite = new ImageBrush();

            enemieSurLeTerrain = random.Next(1, 6);

        }

        public double[] Personnage(int _numeroImage)
        {
            double[] stats = new double[4];  // PV, Attaque, Défense, Vitesse

            stats[0] = 100;   // PV
            stats[1] = 10;    // Attaque
            stats[2] = 5;     // Défense
            stats[3] = 3;     // Vitesse

            if (_numeroImage == 1)
            {
                stats[3] = stats[3] * 1.20;
            }

            else if (_numeroImage == 2)
            {
                stats[1] = stats[1] * 1.20;
            }

            else if (_numeroImage == 3)
            {

            }

            else if (_numeroImage == 4)
            {


            }

            else if (_numeroImage == 5)
            {

            }
            else
            {

            }

            return stats;
        }


    }
}
    
