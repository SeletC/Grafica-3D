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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grafica_3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Scena scena;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //Gazon
            Gazon gazon = new Gazon(new Vector3D(0, -0.2, 0), 0, new Vector3D(200, 0.1, 200));
            Copac copac1 = new Copac(new Vector3D(-36, 2, -10), 0, new Vector3D(0.8, 2, 0.8));
            Copac copac2 = new Copac(new Vector3D(-38, 3, 24), 0, new Vector3D(1, 3, 1));
            Copac copac3 = new Copac(new Vector3D(38, 2, -6), 0, new Vector3D(0.6, 2, 0.6));
            Copac copac4 = new Copac(new Vector3D(36, 2, 14), 0, new Vector3D(1, 2, 1));

            //Foisor
            Pereti_ext foisor1 = new Pereti_ext(new Vector3D(5, 5, 50), 0, new Vector3D(8, 5, 8));
            Pereti_ext foisor2 = new Pereti_ext(new Vector3D(5, 5, 50), 15, new Vector3D(8, 5, 8));
            Pereti_ext foisor3 = new Pereti_ext(new Vector3D(5, 5, 50), 30, new Vector3D(8, 5, 8));
            Pereti_ext foisor4 = new Pereti_ext(new Vector3D(5, 5, 50), 45, new Vector3D(8, 5, 8));
            Pereti_ext foisor5 = new Pereti_ext(new Vector3D(5, 5, 50), 60, new Vector3D(8, 5, 8));
            Pereti_ext foisor6 = new Pereti_ext(new Vector3D(5, 5, 50), 75, new Vector3D(8, 5, 8));

            //Garaj
            Pereti_ext garaj_ext = new Pereti_ext(new Vector3D(0, 5, -18), 0, new Vector3D(12, 5, 6));
            Pereti_int garaj_int = new Pereti_int(new Vector3D(0, 5, -18), 0, new Vector3D(11.9, 4.9, 5.9));
            usaGaraj intrare_garaj = new usaGaraj(new Vector3D(-12, 3, -18), 0, new Vector3D(0.2, 3, 4));
            Usa usa_garaj = new Usa(new Vector3D(8, 3, -12), 90, new Vector3D(2, 3, 0.2));
            Geam geam_garaj = new Geam(new Vector3D(12, 5, -18), 0, new Vector3D(0.2, 2, 2));

            //Etajul 1
            Pereti_ext primul_etaj = new Pereti_ext(new Vector3D(6, 5, 6), 0, new Vector3D(18, 5, 18));
            Pereti_int primul_etaj_int = new Pereti_int(new Vector3D(6, 5, 6), 0, new Vector3D(17.9, 4.9, 17.9));
            Pereti_int Bucatarie = new Pereti_int(new Vector3D(16, 5,16), 0, new Vector3D(7.9, 4.9, 7.9));
            Pereti_int Baie = new Pereti_int(new Vector3D(18, 5, -6), 0, new Vector3D(5.9, 4.9, 5.9));
            Pereti_int Living = new Pereti_int(new Vector3D(-2, 5, 16), 0, new Vector3D(9.9, 4.9, 7.9));
            Usa usa_intrare = new Usa(new Vector3D(-12, 3, 5), 0, new Vector3D(0.2, 3, 2));
            Usa usa_baie = new Usa(new Vector3D(18, 3, 0), 90, new Vector3D(2, 3, 0.2));
            Usa usa_bucatarie = new Usa(new Vector3D(12, 3, 8), 90, new Vector3D(2, 3, 0.2));
            Usa usa_living = new Usa(new Vector3D(4, 3, 8), 90, new Vector3D(2, 3, 0.2));
            Geam geam_baie = new Geam(new Vector3D(24, 5, -6), 0, new Vector3D(0.2, 2, 2));
            Geam geam_bucatarie1 = new Geam(new Vector3D(24, 5, 16), 0, new Vector3D(0.2, 2, 2));
            Geam geam_bucatarie2 = new Geam(new Vector3D(16, 5, 24), 90, new Vector3D(2, 2, 0.2));
            Geam geam_living1 = new Geam(new Vector3D(-12, 5, 16), 0, new Vector3D(0.2, 2, 2));
            Geam geam_living2 = new Geam(new Vector3D(0, 5, 24), 90, new Vector3D(2, 2, 0.2));
            Geam geam_primul_etaj = new Geam(new Vector3D(-12, 5, -6), 0, new Vector3D(0.2, 2, 2));

            ObiectXAML sofa = new ObiectXAML("sofa", new Vector3D(-4, 0, 11), 0, new Vector3D(1.5, 1.5, 1.5));
            ObiectXAML tv = new ObiectXAML("tv", new Vector3D(-4, 1, 22), 180, new Vector3D(1.5, 1.5, 1.5));
            ObiectXAML table = new ObiectXAML("table", new Vector3D(18, 0, 14), 180, new Vector3D(2, 2, 3));

            //Etajul 2
            Pereti_ext al_doilea_etaj = new Pereti_ext(new Vector3D(6, 15, 6), 0, new Vector3D(18, 5, 18));
            Pereti_int al_doilea_etaj_int = new Pereti_int(new Vector3D(6, 15, 6), 0, new Vector3D(17.9, 4.9, 17.9));
            Pereti_int Baie_Etaj = new Pereti_int(new Vector3D(18, 15, -6), 0, new Vector3D(5.9, 4.9, 5.9));
            Pereti_int Dormitor1 = new Pereti_int(new Vector3D(15, 15, 16), 0, new Vector3D(8.9, 4.9, 7.9));
            Pereti_int Dormitor2 = new Pereti_int(new Vector3D(-3, 15, 16), 0, new Vector3D(8.9, 4.9, 7.9));
            Usa usa_baie_etaj = new Usa(new Vector3D(18, 13, 0), 90, new Vector3D(2, 3, 0.2));
            Usa usa_dormitor1 = new Usa(new Vector3D(10, 13, 8), 90, new Vector3D(2, 3, 0.2));
            Usa usa_dormitor2 = new Usa(new Vector3D(2, 13, 8), 90, new Vector3D(2, 3, 0.2));
            Geam geam1_al_doilea_etaj = new Geam(new Vector3D(-12, 15, -6), 0, new Vector3D(0.2, 2, 2));
            Geam geam2_al_doilea_etaj = new Geam(new Vector3D(0, 15, -12), 90, new Vector3D(2, 2, 0.2));
            Geam geam1_Dormitor1 = new Geam(new Vector3D(-12, 15, 16), 0, new Vector3D(0.2, 2, 2));
            Geam geam2_Dormitor1 = new Geam(new Vector3D(0, 15, 24), 90, new Vector3D(2, 2, 0.2));
            Geam geam1_Dormitor2 = new Geam(new Vector3D(24, 15, 16), 0, new Vector3D(0.2, 2, 2));
            Geam geam2_Dormitor2 = new Geam(new Vector3D(16, 15, 24), 90, new Vector3D(2, 2, 0.2));
            Geam geam_baie_etaj = new Geam(new Vector3D(24, 15, -6), 0, new Vector3D(0.2, 2, 2));

            //Acoperis
            Acoperis Acoperis = new Acoperis(new Vector3D(6, 23.6, 6), 90, new Vector3D(22, 5, 22));
            Geam geam1_acoperis = new Geam(new Vector3D(-12.6, 23.6, 6), 0, new Vector3D(0.2, 2, 2));
            Geam geam2_acoperis = new Geam(new Vector3D(24.6, 23.6, 6), 0, new Vector3D(0.2, 2, 2));


            scena.adauga_element_scena(gazon);
            scena.adauga_element_scena(copac1);
            scena.adauga_element_scena(copac2);
            scena.adauga_element_scena(copac3);
            scena.adauga_element_scena(copac4);

            scena.adauga_element_scena(foisor1);
            scena.adauga_element_scena(foisor2);
            scena.adauga_element_scena(foisor3);
            scena.adauga_element_scena(foisor4);
            scena.adauga_element_scena(foisor5);
            scena.adauga_element_scena(foisor6);

            scena.adauga_element_scena(garaj_ext);
            scena.adauga_element_scena(garaj_int);
            scena.adauga_element_scena(intrare_garaj);
            scena.adauga_element_scena(usa_garaj);
            scena.adauga_element_scena(geam_garaj);


            scena.adauga_element_scena(primul_etaj);
            scena.adauga_element_scena(primul_etaj_int);
            scena.adauga_element_scena(Bucatarie);
            scena.adauga_element_scena(Baie);
            scena.adauga_element_scena(Living);
            scena.adauga_element_scena(usa_intrare);
            scena.adauga_element_scena(usa_baie);
            scena.adauga_element_scena(usa_bucatarie);
            scena.adauga_element_scena(usa_living);
            scena.adauga_element_scena(geam_baie);
            scena.adauga_element_scena(geam_bucatarie1);
            scena.adauga_element_scena(geam_bucatarie2);
            scena.adauga_element_scena(geam_living1);
            scena.adauga_element_scena(geam_living2);
            scena.adauga_element_scena(geam_primul_etaj);
            scena.adauga_element_scena(sofa);
            scena.adauga_element_scena(tv);
            scena.adauga_element_scena(table);


            scena.adauga_element_scena(al_doilea_etaj);
            scena.adauga_element_scena(al_doilea_etaj_int);
            scena.adauga_element_scena(Baie_Etaj);
            scena.adauga_element_scena(Dormitor1);
            scena.adauga_element_scena(Dormitor2);
            scena.adauga_element_scena(usa_baie_etaj);
            scena.adauga_element_scena(usa_dormitor1);
            scena.adauga_element_scena(usa_dormitor2);
            scena.adauga_element_scena(geam1_al_doilea_etaj);
            scena.adauga_element_scena(geam2_al_doilea_etaj);
            scena.adauga_element_scena(geam1_Dormitor1);
            scena.adauga_element_scena(geam2_Dormitor1);
            scena.adauga_element_scena(geam1_Dormitor2);
            scena.adauga_element_scena(geam2_Dormitor2);
            scena.adauga_element_scena(geam_baie_etaj);


            scena.adauga_element_scena(Acoperis);
            scena.adauga_element_scena(geam1_acoperis);
            scena.adauga_element_scena(geam2_acoperis);


            scena.afiseaza_elemente();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            scena=new Scena(MainViewport);
        }
        void apasa_tasta(Key key)
        {
            /*switch (key)
            {
                case Key.Up:
                    muta_camera_sus();
                    break;
                case Key.Down:
                    muta_camera_jos();
                    break;
                case Key.Left:
                    muta_camera_stanga();
                    break;
                case Key.Right:
                    muta_camera_dreapta();
                    break;
                case Key.Add:
                case Key.OemPlus:
                    apropie_camera();
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    departeaza_camera();
                    break;
            }*/
        }

        private void lumina_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scena.valoare_lumina((byte)lumina.Value);
        }
        private void MainViewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //apasat = true;
            //punct = e.GetPosition(this);
        }

        private void MainViewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //apasat = false;
        }

        private void MainViewport_MouseMove(object sender, MouseEventArgs e)
        {/*
            if (apasat)
            {
                Point pCurent = e.GetPosition(this);
                if (pCurent.X > punct.X)
                {
                    muta_camera_stanga();
                }
                else if (pCurent.X < punct.X)
                {
                    muta_camera_dreapta();
                }
                else if (pCurent.Y > punct.Y)
                {
                    muta_camera_sus();
                }
                else if (pCurent.Y < punct.Y)
                {
                    muta_camera_jos();
                }
                punct = pCurent;
            }*/
        }
        private void slider_Theta_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scena.modifica_Theta(slider_Theta.Value * Math.PI / 180);
        }

        private void slider_Phi_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scena.modifica_Phi(slider_Phi.Value * Math.PI / 180);
        }
        private void zoom_in_Click(object sender, RoutedEventArgs e)
        {
            scena.apropie_camera();
        }

        private void zoom_out_Click(object sender, RoutedEventArgs e)
        {
            scena.departeaza_camera();
        }
    }
}
