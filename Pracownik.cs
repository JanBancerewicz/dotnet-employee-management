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
        public Guid ID { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public int Staz { get; set; }
        public double Pensja { get; set; }
        public string Stanowisko { get; set; }
        public PracownikInfo Info { get; set; }
        public ObservableCollection<Pracownik> Podwladni { get; set; }
        

        public Pracownik Przelozony { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Pracownik(string imie, string nazwisko, int staz, double pensja, string stanowisko, Pracownik przelozony)
        {
            ID = Guid.NewGuid();
            Imie = imie;
            Nazwisko = nazwisko;
            Staz = staz;
            Pensja = pensja;
            Stanowisko = stanowisko;
            Info = new PracownikInfo();
            Podwladni = new ObservableCollection<Pracownik>();
            Przelozony = przelozony;
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
            return $"Pracownik{{ stanowisko = {Stanowisko}, nazwisko = {Nazwisko}, imie = {Imie}, staz = {Staz}, pensja = {Pensja}, premia = {Info.Premia}, wykształcenie = {Info.Wyksztalcenie} }}";
        }

        public string GetDetailsString(int indent)
        {
            string ind = string.Concat(Enumerable.Repeat(" ", indent * 4));
            string podwl = Podwladni.Count == 0 ?
                $"{ind}    [Brak]\n"
                : string.Concat(Podwladni.Select(x => x.GetDetailsString(indent + 1)).ToArray());

            return $"{ind}Imie: {Imie}\n" +
                $"{ind}Nazwisko: {Nazwisko}\n" +
                $"{ind}Stanowisko: {Stanowisko}\n" +
                $"{ind}Staż: {Staz}\n" +
                $"{ind}Pensja: {Pensja}\n" +
                $"{ind}Premia: {Info.Premia}\n" +
                $"{ind}Ocena: {Info.OcenaPracownika}\n" +
                $"{ind}Wykształcenie: {Info.Wyksztalcenie}\n" +
                $"{ind}Podwładni:\n" +
                $"{podwl}";
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
    }
}
