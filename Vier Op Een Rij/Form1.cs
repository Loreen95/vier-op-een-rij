using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vier_Op_Een_Rij
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            startBord();
        }

        Brush[,] arrayKleuren = new Brush[6, 7];
        Pen tekenpotlood = new Pen(Color.Black, 5);
        bool boolBeurt = true;
        bool muntVallen;
        int intKolom, intRij;
        int intHuidigeRij;

        private void startBord()
        {
            for (int rij = 0; rij < arrayKleuren.GetLength(0); rij++)
            {
                for (int kolom = 0; kolom < arrayKleuren.GetLength(1); kolom++)
                {
                    arrayKleuren[rij, kolom] = Brushes.White;
                }
            }
        }

        private void pBordDraw(object sender, PaintEventArgs e)
        {
            Graphics bord = e.Graphics;
            for (int rij = 0; rij < arrayKleuren.GetLength(0); rij++)
            {
                for (int kolom = 0; kolom < arrayKleuren.GetLength(1); kolom++)
                {
                    bord.DrawEllipse(tekenpotlood, 10 + kolom * 107, 10 + rij * 107, 100, 100);
                    bord.FillEllipse(arrayKleuren[rij, kolom], 10 + kolom * 107, 10 + rij * 107, 100, 100);
                }
            }
        }
        // ZONDER VAL-TIMER
        /* private void beurtBijhouden()
         {
             if (boolBeurt == true && arrayKleuren[intRij, intKolom] == Brushes.White)
             {
                 if (intRij == 5)
                 {
                     arrayKleuren[intRij, intKolom] = Brushes.Red; 
                     boolBeurt = false;

                 }
                 else if (arrayKleuren[intRij + 1, intKolom] != Brushes.White)
                 {
                     arrayKleuren[intRij, intKolom] = Brushes.Red; 
                     boolBeurt = false;
                 }
             }
             else if (boolBeurt == false && arrayKleuren[intRij, intKolom] == Brushes.White)
             {
                 if (intRij == 5)
                 {
                     arrayKleuren[intRij, intKolom] = Brushes.Yellow; 
                     boolBeurt = true;
                 }
                 else if (arrayKleuren[intRij + 1, intKolom] != Brushes.White)
                 {
                     arrayKleuren[intRij, intKolom] = Brushes.Yellow; 
                     boolBeurt = true;
                 }
             } 
             pBord.Invalidate();
         } */

        private void pBord_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = e.X.ToString();
            label2.Text = e.Y.ToString();
            label3.Text = (e.X / 100).ToString();
            label4.Text = (e.Y / 100).ToString();
        }

        int huidigeRij = 0;
        int intHuidigeKolom = 0;
        bool beurtTimer;

        private void coinFall_Tick(object sender, EventArgs e)
        {
            #region timerTick
            muntVallen = true;
            if (!beurtTimer)
            {
                arrayKleuren[intHuidigeRij, intHuidigeKolom] = Brushes.Red;
            }

            else
            {
                arrayKleuren[intHuidigeRij, intHuidigeKolom] = Brushes.Yellow;
            }
            if (intHuidigeRij > 0)
            {
                arrayKleuren[intHuidigeRij - 1, intHuidigeKolom] = Brushes.White;
            }

            if (intHuidigeRij == 5)
            {
                coinFall.Stop();
                winCheck();
                muntVallen = false;
                beurtTimer = !beurtTimer;
            }

            else if (arrayKleuren[intHuidigeRij + 1, intHuidigeKolom] != Brushes.White)
            {
                coinFall.Stop();
                winCheck();
                muntVallen = false;
                beurtTimer = !beurtTimer;
            }

            intHuidigeRij++;
            pBord.Invalidate();
            #endregion
        }

        private void checkRij(Brush kleur)
        {
            int intTellerRij = 0;
            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (arrayKleuren[j, i] == kleur)
                    {
                        intTellerRij++;
                        if (intTellerRij == 4)
                        {
                            if (boolBeurt == false)
                            {
                                MessageBox.Show("De winnaar is Geel!");
                                Application.Restart();
                            }
                            else
                            {
                                MessageBox.Show("De winnaar is Rood!");
                                Application.Restart();
                            }
                        }
                    }
                    else
                    {
                        intTellerRij = 0;
                    }
                }
            }
        }

        private void checkKolom(Brush kleur)
        {
            int intTellerKolom = 0;
            for (int j = 0; j < 7; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (arrayKleuren[i, j] == kleur)
                    {
                        intTellerKolom++;
                        if (intTellerKolom == 4)
                        {
                            if (boolBeurt == false)
                            {
                                MessageBox.Show("De winnaar is Geel!");
                                Application.Restart();
                            }
                            else
                            {
                                MessageBox.Show("De winnaar is Rood!");
                                Application.Restart();
                            }
                        }
                    }
                    else
                    {
                        intTellerKolom = 0;
                    }
                }
            }
        }

        private void winCheck()
        {
            boolBeurt = !boolBeurt;
            pBord.Invalidate();

            // Horizontaal
            checkRij(Brushes.Red);
            checkRij(Brushes.Yellow);

            // Verticaal
            checkKolom(Brushes.Red);
            checkKolom(Brushes.Yellow);
        }

        private void pBord_MouseDown(object sender, MouseEventArgs e)
        {
            if (!muntVallen)
            {
                intKolom = (e.X + 90 - (e.X / 100) * 5) / 100 - 1;
                //intRij = (e.Y + 90 - (e.Y / 100) * 5) / 100 - 1;

                coinFall.Start();
                intHuidigeRij = 0;
                intHuidigeKolom = intKolom;
                pBord.Invalidate();
            }
            else
            {
                MessageBox.Show("Wacht even.");
            }
            winCheck();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void spelregelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wanneer een speler 4 van dezelfde kleur op een rij heeft horizontaal of verticaal");
        }

        private void overToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Geschreven door Lisa Hakhoff, 2022");
        }

        private void nieuwSpelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}