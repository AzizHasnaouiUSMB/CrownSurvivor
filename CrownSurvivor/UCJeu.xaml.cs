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
using static System.Formats.Asn1.AsnWriter;
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
        private int ticks = 0;
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


        private List<Image> projectiles = new List<Image>();
        private double projectileSpeed = 10;
        private int tirCounter = 0;      // compteur pour la cadence de tir

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

            // ---- gestion du temps ----
            ticks++;

            if (ticks >= 50) // 50 ticks ≈ 1 seconde
            {
                ticks = 0;
                secondes++;

                // toutes les 2 secondes -> spawn
                if (secondes % 2 == 0)
                    SpawnEnemy();

                // toutes les 30 secondes -> +1 à la limite
                if (secondes % 30 == 0)
                    limiteEnemie++;
            }

            // déplacement des ennemis
            MoveEnemiesToPlayer();

            // tir auto
            tirCounter++;
            if (tirCounter >= 25) // toutes les ~0,5 s
            {
                tirCounter = 0;
                SpawnAutoShot();
            }
            MoveProjectiles();
            CheckProjectileCollisions();
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

        private Rect GetPlayerHitbox()
        {
            double x = Canvas.GetLeft(imgPerso);
            double y = Canvas.GetTop(imgPerso);
            double w = imgPerso.Width;
            double h = imgPerso.Height;

            return new Rect(x, y, w, h);
        }
        private Rect GetEnemyHitbox(Image e)
        {
            double x = Canvas.GetLeft(e);
            double y = Canvas.GetTop(e);
            double w = e.Width;
            double h = e.Height;

            return new Rect(x, y, w, h);
        }

        private void SpawnAutoShot()
        {
            // pas d'ennemi -> pas de tir
            Image cible = GetClosestEnemy();
            if (cible == null)
                return;

            // centre du joueur
            double px = Canvas.GetLeft(imgPerso) + imgPerso.Width / 2;
            double py = Canvas.GetTop(imgPerso) + imgPerso.Height / 2;

            // centre de l'ennemi le plus proche
            double ex = Canvas.GetLeft(cible) + cible.Width / 2;
            double ey = Canvas.GetTop(cible) + cible.Height / 2;

            // direction joueur -> ennemi
            double dx = ex - px;
            double dy = ey - py;
            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length == 0) return;
            dx /= length;
            dy /= length;
            Vector dir = new Vector(dx, dy);

            // créer le projectile
            Image proj = new Image
            {
                Width = 10,
                Height = 10,
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(new Uri("pack://application:,,,/image/ImBalle.png"))
            };

            // l'ajouter dans la zone de jeu, à la position du joueur
            ZoneJeu.Children.Add(proj);
            Canvas.SetLeft(proj, px - proj.Width / 2);
            Canvas.SetTop(proj, py - proj.Height / 2);

            // stocker la direction dans Tag pour MoveProjectiles()
            proj.Tag = dir;

            // mémoriser ce projectile dans la liste
            projectiles.Add(proj);
        }

        private void MoveProjectiles()
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                Image p = projectiles[i];
                Vector dir = (Vector)p.Tag;

                double x = Canvas.GetLeft(p) + dir.X * projectileSpeed;
                double y = Canvas.GetTop(p) + dir.Y * projectileSpeed;

                Canvas.SetLeft(p, x);
                Canvas.SetTop(p, y);

                if (x > ZoneJeu.ActualWidth + 20 ||
                    x < -20 ||
                    y > ZoneJeu.ActualHeight + 20 ||
                    y < -20)
                {
                    ZoneJeu.Children.Remove(p);
                    projectiles.RemoveAt(i);
                }
            }
        }

        private void CheckProjectileCollisions()
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                Image p = projectiles[i];
                Rect projBox = GetProjectileHitbox(p);

                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    Image e = enemies[j];
                    Rect enemyBox = GetEnemyHitbox(e);

                    if (projBox.IntersectsWith(enemyBox))
                    {
                        // dégâts / score
                        score += 10;

                        // supprimer projectile et ennemi
                        ZoneJeu.Children.Remove(p);
                        ZoneJeu.Children.Remove(e);
                        projectiles.RemoveAt(i);
                        enemies.RemoveAt(j);
                        enemieSurLeTerrain--;

                        break; // on arrête de tester ce projectile
                    }
                }
            }
        }
        private Rect GetProjectileHitbox(Image p)
        {
            double x = Canvas.GetLeft(p);
            double y = Canvas.GetTop(p);
            double w = p.Width;
            double h = p.Height;

            return new Rect(x, y, w, h);
        }
        private Image GetClosestEnemy()
        {
            if (enemies.Count == 0)
                return null;

            double px = Canvas.GetLeft(imgPerso) + imgPerso.Width / 2;
            double py = Canvas.GetTop(imgPerso) + imgPerso.Height / 2;

            Image closest = null;
            double bestDist2 = double.MaxValue; // distance au carré

            foreach (var e in enemies)
            {
                double ex = Canvas.GetLeft(e) + e.Width / 2;
                double ey = Canvas.GetTop(e) + e.Height / 2;

                double dx = ex - px;
                double dy = ey - py;
                double dist2 = dx * dx + dy * dy;

                if (dist2 < bestDist2)
                {
                    bestDist2 = dist2;
                    closest = e;
                }
            }

            return closest;
        }

    }
}



