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

using System.Windows.Threading;
//rajout 

namespace CrownSurvivor
{   
    /// <summary>
    /// Logique d'interaction pour UCJeu.xaml
    /// </summary>
    public partial class UCJeu : UserControl
    {
        bool goleft, goright, goup, godown;
        int vitesse = 10;
        int speed = 12;
        
        DispatcherTimer gameTimer = new DispatcherTimer();
        public UCJeu()
        {
            InitializeComponent();
            Canvas.SetLeft(Perso, 100);
            Canvas.SetTop(Perso, 100);
            Perso.Focusable = true;
            this.Focus();
        }

        private void KeysIsDown(object sender, KeyEventArgs e)
        {

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {

        }
    }
}
