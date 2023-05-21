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
using System.Threading;
using System.Media;

namespace Puissance4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
 

        const int VIDE = 0;
        const int JAUNE = 1;
        const int ROUGE = 2;

        /*Grille du jeu, 0 : vide , 1 : jaune, 2 : rouge*/
        int[,] Grille = new int[7, 6];
        /*Tableau contenant tous les cercles de l'interface*/
        Ellipse[,] GrilleElements;

        SoundPlayer clac = new SoundPlayer(@"C:\Users\15thi\Music\soundvs\clacp4.wav");
        SoundPlayer vict = new SoundPlayer(@"C:\Users\15thi\Music\soundvs\victp4.wav");
        SoundPlayer newg = new SoundPlayer(@"C:\Users\15thi\Music\soundvs\newgp4.wav");


        int currentPlayer;
        bool GameOver = false;
        bool BlocageSurbrillance = false;
        public MainWindow()
        {
            InitializeComponent();
            GrilleElements = new Ellipse[7, 6] { { c00, c01, c02, c03, c04, c05 }, { c10, c11, c12, c13, c14, c15 }, { c20, c21, c22, c23, c24, c25 }, { c30, c31, c32, c33, c34, c35 }, { c40, c41, c42, c43, c44, c45 }, { c50, c51, c52, c53, c54, c55 }, { c60, c61, c62, c63, c64, c65 } };
            InitGrille();
        }
        void InitGrille() 
        {
            GameOver = false;
            currentPlayer = JAUNE;
            ccurrent.Fill = Brushes.Yellow;
            lvictoire.Content = "";
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++) 
                {
                    Grille[i, j] = VIDE;
                    GrilleElements[i, j].Fill = Brushes.White;
                    GrilleElements[i, j].Stroke = Brushes.Black;
                }
            }
            newg.Play();
        }

        void Victory(Ellipse e1, Ellipse e2, Ellipse e3, Ellipse e4, int player)
        {
            e1.Stroke = Brushes.White;
            e2.Stroke = Brushes.White;
            e3.Stroke = Brushes.White;
            e4.Stroke = Brushes.White;


            if (player == JAUNE)
            {
                lvictoire.Content = "Victoire du joueur JAUNE";
            } else
            {
                lvictoire.Content = "Victoire du joueur ROUGE";
            }

            vict.Play();

            GameOver = true;
        }
        
        int checkVictoire(int player)
        {
            /*Check des lignes*/
            for (int _row = 0; _row < 6;  _row++)
            {
                for (int _col = 0; _col < 4; _col++)
                {
                    if ((Grille[_col, _row] == player) && (Grille[_col + 1, _row] == player) && (Grille[_col + 2, _row] == player) && (Grille[_col + 3, _row] == player))
                    {
                        Victory(GrilleElements[_col, _row], GrilleElements[_col + 1, _row], GrilleElements[_col + 2, _row], GrilleElements[_col + 3, _row], currentPlayer);
                        break;
                    }
                }
            }
            /*Check des colonnes*/
            for (int _col = 0; _col < 7; _col++) 
            {
                for (int _row = 0; _row < 3; _row++)
                {
                    if ((Grille[_col, _row] == player) && (Grille[_col, _row + 1] == player) && (Grille[_col, _row + 2] == player) && (Grille[_col, _row + 3] == player))
                    {
                        Victory(GrilleElements[_col, _row], GrilleElements[_col, _row + 1], GrilleElements[_col, _row + 2], GrilleElements[_col, _row + 3], currentPlayer);
                        break;
                    }
                }
            }

            /**/
            for(int _col= 0; _col < 4; _col ++)
            {
                for (int _row = 0; _row < 3; _row++)
                {
                    if ((Grille[_col, _row] == player) && (Grille[_col + 1, _row + 1] == player) && (Grille[_col + 2, _row + 2] == player) && (Grille[_col + 3, _row + 3] == player))
                    {
                        Victory(GrilleElements[_col, _row], GrilleElements[_col + 1, _row + 1], GrilleElements[_col + 2, _row + 2], GrilleElements[_col + 3, _row + 3], currentPlayer);
                        break;
                    }
                }
            }

            for (int _col = 0; _col < 3; _col++)    
            {
                for (int _row = 3; _row < 6; _row++)
                {
                    if ((Grille[_col, _row] == player) && (Grille[_col + 1, _row - 1] == player) && (Grille[_col + 2, _row - 2] == player) && (Grille[_col + 3, _row - 3] == player))
                    {
                        Victory(GrilleElements[_col, _row], GrilleElements[_col + 1, _row - 1], GrilleElements[_col + 2, _row - 2], GrilleElements[_col + 3, _row - 3], currentPlayer);
                        break;
                    }
                }
            }

            return 0;
        }

        void RemplissageJeton(int col, int row)
        {
            if (currentPlayer == JAUNE)
            {
                Grille[col, row] = JAUNE;
                GrilleElements[col, row].Fill = Brushes.Yellow;
            }
            else
            {
                Grille[col, row] = ROUGE;
                GrilleElements[col, row].Fill = Brushes.Red;
            }
        }

        async void DescenteJeton(int Col)
        {
            Ellipse olderR;
            bool descente = false;

            if (GameOver) return;

            BlocageSurbrillance = true;
            SurbrillanceColonne(Col, false, true);

            /*Blocage du jeu*/
            Game.IsEnabled = false;

            /*On parcourt toutes les lignes de la colonne*/
            for (int i = 5; i >= 0; i--)
            {
                /**/
                if (descente == true) {
                    olderR = GrilleElements[Col, i +1];
                    olderR.Fill = Brushes.White;
                    Grille[Col, i + 1] = VIDE;
                }

                descente = false;
                /*Si la case est vide, on la remplit*/
                if (Grille[Col, i] == VIDE)
                {
                    RemplissageJeton(Col, i);

                    /*Si on est pas tout en bas*/
                    if (i > 0)
                    {
                        /*Si la case en dessous est vide*/
                        if (Grille[Col, i - 1] == VIDE)
                        {
                            descente = true;
                        }
                    }
                   
                }
                /*Délai de 0.1s*/
                await Task.Delay(75);
            }

            clac.Play();

            /*On vérifie si puissance 4*/
            int test = checkVictoire(currentPlayer);

            /*Changement de joueur*/
            if (currentPlayer == JAUNE)
            {
                currentPlayer = ROUGE;
                ccurrent.Fill = Brushes.Red;
            }
            else
            {
                currentPlayer = JAUNE;
                ccurrent.Fill = Brushes.Yellow;
            }

            /*Déblocage du jeu*/
            Game.IsEnabled = true;
            BlocageSurbrillance = false;
        }

        void SurbrillanceColonne(int Col, bool Active, bool Force)
        {
            if ((!Force) && (((GameOver) || (Grille[Col, 5] != VIDE) || (BlocageSurbrillance)))) return;

            for(int row=0;row < 6; row++)
            {
                if (!Active)
                {
                    GrilleElements[Col, row].Stroke = Brushes.Black;
                }
                else
                {
                    GrilleElements[Col, row].Stroke = Brushes.White;
                }
            }
        }

        

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            InitGrille();
        }

        private void c_MouseEnter(object sender, MouseEventArgs e)
        {
            SurbrillanceColonne((int) Char.GetNumericValue(((Ellipse)sender).Name[1]), true, false);
        }

        private void c_MouseLeave(object sender, MouseEventArgs e)
        {
            SurbrillanceColonne((int) Char.GetNumericValue(((Ellipse)sender).Name[1]), false, false);
        }

        private void c_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DescenteJeton((int)Char.GetNumericValue(((Ellipse)sender).Name[1]));
        }
    }
}
