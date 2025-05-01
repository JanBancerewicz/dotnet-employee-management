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
using System.Windows.Shapes;

namespace dotnet
{
    /// <summary>
    /// Logika interakcji dla klasy AddPodwladnyWindow.xaml
    /// </summary>
    public partial class AddPodwladnyWindow : Window
    {
        Pracownik przelozony;

        public AddPodwladnyWindow(Pracownik pracownik)
        {
            this.przelozony = pracownik;
            InitializeComponent();
        }

        private string ValidateInput()
        {
            if (ImieInput.Text.Trim().Length < 2)
                return "Imie powinno mieć przynajmniej 2 znaki!";
            
            if (NazwiskoInput.Text.Trim().Length < 2)
                return "Nazwisko powinno mieć przynajmniej 2 znaki!";
            
            if (StanowiskoInput.Text.Trim().Length < 2)
                return "Stanowisko powinno mieć przynajmniej 2 znaki!";

            try { int staz = Int32.Parse(StazInput.Text.Trim()); }
            catch { return "Staż powinien być liczbą całkowitą!"; }

            try { int pensja = Int32.Parse(PensjaInput.Text.Trim()); }
            catch { return "Pensja powinna być liczbą całkowitą!"; }

            return "";
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            string errMsg = ValidateInput();
            if (errMsg.Length > 0)
            {
                MessageBox.Show(errMsg);
                return;
            }

            Pracownik p = new Pracownik(
                ImieInput.Text.Trim(), 
                NazwiskoInput.Text.Trim(), 
                Int32.Parse(StazInput.Text.Trim()),
                Int32.Parse(PensjaInput.Text.Trim()),
                StanowiskoInput.Text.Trim(),
                przelozony
            );

            przelozony.Podwladni.Add(p);

            Close();
        }

        private void Generuj_Click(object sender, RoutedEventArgs e)
        {
            var generated = MockPracownikGenerator.GeneratePracownik(null);
            ImieInput.Text = generated.Imie;
            NazwiskoInput.Text = generated.Nazwisko;
            StazInput.Text = "" + generated.Staz;
            StanowiskoInput.Text = generated.Stanowisko;
            PensjaInput.Text = "" + generated.Pensja;
        }
    }
}
