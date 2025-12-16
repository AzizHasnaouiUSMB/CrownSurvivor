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
        private int limiteEnemie = 5;
        private int secondes = 0;
        private int damage = 0;
        private int score = 0;
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

        private List<Image> enemies = new List<Image>();
        private double enemySpeed = 2;
        private int enemySpawnCounter = 0;
        private int enemySpawnInterval = 50; // nombre de ticks entre 2 spawns

        private int enemieSurLeTerrain = 0;
        private int limiteEnemie = 5;      // limite de base
        private int secondesEcoulees = 0;  // pour compter les secondes
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
            // déplacement du perso
            double x = Canvas.GetLeft(imgPerso);
            double y = Canvas.GetTop(imgPerso);

            if (gauche) x -= vitesse;
            if (droite) x += vitesse;
            if (haut) y -= vitesse;
            if (bas) y += vitesse;

            Canvas.SetLeft(imgPerso, x);
            Canvas.SetTop(imgPerso, y);

            // comptage du temps
            enemySpawnCounter++;
            // if (enemySpawnCounter >= 50) à revoir 
            
                enemySpawnCounter = 0;
                secondes++;

                if (secondes % 10 == 0)
                    SpawnEnemy();

                if (secondes % 120 == 0)
                    limiteEnemie++;
            

            // déplacement des ennemis
            MoveEnemiesToPlayer();
        }

        private void SpawnEnemy()
        {
            if (enemieSurLeTerrain >= limiteEnemie)
                return;
            Image e = new Image
                {
                    Width = 30,
                    Height = 30,
                    Stretch = Stretch.Uniform
                };

            Uri uri = new Uri("pack://application:,,,/image/ImZombie.png");
            e.Source = new BitmapImage(uri);

            ZoneJeu.Children.Add(e);
            Canvas.SetLeft(e, 0);
            Canvas.SetTop(e, 0);

                enemies.Add(e); ;
                enemieSurLeTerrain++;
            
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

        private void MoveEnemiesToPlayer()
        {
            double px = Canvas.GetLeft(imgPerso) + imgPerso.Width / 2;
            double py = Canvas.GetTop(imgPerso) + imgPerso.Height / 2;

            foreach (var e in enemies)
            {
                double ex = Canvas.GetLeft(e) + e.Width / 2;
                double ey = Canvas.GetTop(e) + e.Height / 2;    

                double dx = px - ex;
                double dy = py - ey;

                double length = Math.Sqrt(dx * dx + dy * dy);
                if (length == 0) continue;

                dx /= length;
                dy /= length;

                ex += dx * enemySpeed;
                ey += dy * enemySpeed;

                Canvas.SetLeft(e, ex - e.Width / 2);
                Canvas.SetTop(e, ey - e.Height / 2);
            }
        }
    }
}

    
