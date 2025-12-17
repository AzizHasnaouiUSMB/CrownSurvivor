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

        private double currentHP;
        private double maxHP;

        double[] stats = new double[NB_STATS];

        private int _numeroImage;

        private List<Image> enemies = new List<Image>();
        private double enemySpeed = 2.5;
        private Dictionary<Image, double> enemyHp = new Dictionary<Image, double>(); //pour pouvoir donner des hp à plusieurs ennemies


        private List<Image> projectiles = new List<Image>();
        private double projectileSpeed = 10;
        private int tirCounter = 0;      // compteur pour la cadence de tir

        private double projectileDamageMultiplier = 1.0;
        private double projectileSpeedMultiplier = 1.0;
        private double luckyShotChance = 0.0;
        private double projectileHitboxMultiplier = 1.0;
        private bool slowEnemiesOnHit = false;

        private int wave = 1;                 // numéro de vague
        private double enemyHpMultiplier = 1; // PV ennemis (1.0 de base)

        public UCJeu(int numeroImage)
        {
            InitializeComponent();
            _numeroImage = numeroImage;

            Canvas.SetLeft(imgPerso, 100);
            Canvas.SetTop(imgPerso, 100);
            this.Loaded += (s, e) => this.Focus();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;
            timer.Start();

            string Chemin = $"/ImPerso/im{_numeroImage}.png";
            Uri path = new Uri($"pack://application:,,,{Chemin}");
            BitmapImage bitmap = new BitmapImage(path);
            imgPerso.Source = bitmap;

            // récupère les stats de base du perso
            stats = Personnage(_numeroImage);

            // PV de base
            maxHP = stats[STAT_PV];
            currentHP = maxHP;
            UpdateHealthBar();

            // reset des bonus
            projectileDamageMultiplier = 1.0;
            projectileSpeedMultiplier = 1.0;
            luckyShotChance = 0.0;
            projectileHitboxMultiplier = 1.0;
            slowEnemiesOnHit = false;

            // pouvoirs selon l'image tirée (1 à 5)
            switch (_numeroImage)
            {
                case 1:
                    projectileDamageMultiplier = 1.20;  // +20% dégâts
                    break;
                case 2:
                    projectileSpeedMultiplier = 1.20;   // +20% vitesse des tirs
                    break;
                case 3:
                    luckyShotChance = 0.01;             // 1% one-shot
                    break;
                case 4:
                    projectileHitboxMultiplier = 1.5;   // hitbox ×1,5
                    break;
                case 5:
                    slowEnemiesOnHit = true;            // ralentissement actif
                    break;
            }
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

            if (gauche) x -= vitesse;
            if (droite) x += vitesse;
            if (haut) y -= vitesse;
            if (bas) y += vitesse;

            // bordures de la map
            double maxX = Math.Max(0, ZoneJeu.ActualWidth - imgPerso.Width);
            double maxY = Math.Max(0, ZoneJeu.ActualHeight - imgPerso.Height);

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > maxX) x = maxX;
            if (y > maxY) y = maxY;

            Canvas.SetLeft(imgPerso, x);
            Canvas.SetTop(imgPerso, y);

            // ---- gestion du temps ----
            ticks++;

            if (ticks >= 50)
            {
                ticks = 0;
                secondes++;

                if (secondes % 1 == 0)
                    SpawnEnemy();

                if (secondes % 15 == 0)
                {
                    limiteEnemie++;
                    enemyHpMultiplier *= 1.2;
                    UpdateWaveText();
                    wave++;
                }

            }

            // BOUCLE DE JEU (à ne pas oublier)
            MoveEnemiesToPlayer();
            CheckPlayerCollisions();
            tirCounter++;
            if (tirCounter >= 25)
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
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(new Uri("pack://application:,,,/image/ImZombie.png"))
            };

            ZoneJeu.Children.Add(e);

            // s'assurer que la taille du Canvas est connue
            double maxX = Math.Max(0, ZoneJeu.ActualWidth - e.Width);
            double maxY = Math.Max(0, ZoneJeu.ActualHeight - e.Height);

            // coordonnés aléatoires dans la map
            double x = random.NextDouble() * maxX;
            double y = random.NextDouble() * maxY;

            Canvas.SetLeft(e, x);
            Canvas.SetTop(e, y);

            enemies.Add(e);
            enemieSurLeTerrain++;
            double baseHp = 15.0;                  // PV de base d'un zombie
            double hpAvecVague = baseHp * enemyHpMultiplier;
            enemyHp[e] = hpAvecVague;
        }

        public double[] Personnage(int numeroImage)
        {
            double[] s = new double[4];

            s[STAT_PV] = 100;
            s[STAT_ATTAQUE] = 10;
            s[STAT_DEFENSE] = 5;
            s[STAT_VITESSE] = 3;

            // bonus de stats de base selon le perso
            if (numeroImage == 1)
                s[STAT_VITESSE] *= 1.20;
            else if (numeroImage == 2)
                s[STAT_ATTAQUE] *= 1.20;

            return s;
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

                double x = Canvas.GetLeft(p) + dir.X * projectileSpeed * projectileSpeedMultiplier;
                double y = Canvas.GetTop(p) + dir.Y * projectileSpeed * projectileSpeedMultiplier;

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

        private void CheckPlayerCollisions()
        {
            Rect playerBox = GetPlayerHitbox();

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Image e = enemies[i];
                Rect enemyBox = GetEnemyHitbox(e);

                if (playerBox.IntersectsWith(enemyBox))
                {
                    double degatsRecus = 10.0 / Math.Max(1.0, stats[STAT_DEFENSE]);

                    currentHP -= degatsRecus;
                    if (currentHP < 0) currentHP = 0;
                    UpdateHealthBar();

                    ZoneJeu.Children.Remove(e);
                    enemies.RemoveAt(i);
                    enemieSurLeTerrain--;

                    if (currentHP <= 0)
                    {
                        GameOver();
                        break;
                    }
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
                        bool oneShot = random.NextDouble() < luckyShotChance;

                        double degats;
                        if (oneShot)
                        {
                            degats = enemyHp.ContainsKey(e) ? enemyHp[e] : 9999; // tue sûr
                        }
                        else
                        {
                            double degatsBase = stats[STAT_ATTAQUE] * projectileDamageMultiplier;
                            // tient compte de la vague
                            degats = degatsBase;
                        }

                        if (enemyHp.ContainsKey(e))
                        {
                            enemyHp[e] -= degats;
                            if (enemyHp[e] <= 0)
                            {
                                enemyHp.Remove(e);
                                ZoneJeu.Children.Remove(e);
                                enemies.RemoveAt(j);
                                enemieSurLeTerrain--;

                                score += (int)degats;  // récompense
                            }
                        }

                        // le projectile disparaît à l'impact
                        ZoneJeu.Children.Remove(p);
                        projectiles.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private Rect GetProjectileHitbox(Image p)
        {
            double x = Canvas.GetLeft(p);
            double y = Canvas.GetTop(p);
            double w = p.Width * projectileHitboxMultiplier;
            double h = p.Height * projectileHitboxMultiplier;

            x -= (w - p.Width) / 2;
            y -= (h - p.Height) / 2;

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


        private void UpdateHealthBar()
        {
            if (HpBar == null) return;

            double pourcentage = (currentHP / maxHP) * 100.0;
            if (pourcentage < 0) pourcentage = 0;
            if (pourcentage > 100) pourcentage = 100;

            HpBar.Value = pourcentage;

        }

        private void GameOver()
        {
            timer.Stop();
            MessageBox.Show("Game Over !");
            // plus tard : revenir à l’écran de démarrage
        }

        private void UpdateWaveText()
        {
            if (Vague != null)
                Vague.Content = $"Vague : {wave}";
        }
    }
}


