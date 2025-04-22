using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet
{
    public class Pracownik : INotifyPropertyChanged, IComparable<Pracownik>
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int Staz { get; set; }
        public double Pensja { get; set; }
        public string Stanowisko { get; set; }
        public ObservableCollection<Pracownik> Podwladni { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Pracownik(string imie, string nazwisko, int staz, double pensja, string stanowisko)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Staz = staz;
            Pensja = pensja;
            Stanowisko = stanowisko;
            Podwladni = new ObservableCollection<Pracownik>();
        }


        public int CompareTo(Pracownik other)
        {
            int cmp = Nazwisko.CompareTo(other.Nazwisko);
            if (cmp != 0) return cmp;
            cmp = Imie.CompareTo(other.Imie);
            if (cmp != 0) return cmp;
            cmp = Stanowisko.CompareTo(other.Stanowisko);
            if (cmp != 0) return cmp;
            return Staz.CompareTo(other.Staz);
        }



        public override string ToString()
        {
            return $"Pracownik{{ stanowisko = {Stanowisko}, nazwisko = {Nazwisko}, imie = {Imie}, staz = {Staz}, pensja = {Pensja} }}";
        }

        

        private int GetPodwladniCount(Dictionary<Pracownik, int> statistics)
        {
            int subCount = 0;
            foreach (var p in Podwladni)
            {
                subCount += 1;
                subCount += p.GetPodwladniCount(statistics);
            }

            statistics[this] = subCount;
            return subCount;
        }

        

        // Można dodać ręczne wywoływanie PropertyChanged przy zmianach, jeśli chcesz z czasem umożliwić edytowanie danych w GUI
    }
}
