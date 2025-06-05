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

        private string imie;
        public string Imie
        {
            get => imie;
            set
            {
                if (imie != value)
                {
                    imie = value;
                    OnPropertyChanged(nameof(Imie));
                }
            }
        }

        private string nazwisko;
        public string Nazwisko
        {
            get => nazwisko;
            set
            {
                if (nazwisko != value)
                {
                    nazwisko = value;
                    OnPropertyChanged(nameof(Nazwisko));
                }
            }
        }

        private int staz;
        public int Staz
        {
            get => staz;
            set
            {
                if (staz != value)
                {
                    staz = value;
                    OnPropertyChanged(nameof(Staz));
                }
            }
        }

        private double pensja;
        public double Pensja
        {
            get => pensja;
            set
            {
                if (pensja != value)
                {
                    pensja = value;
                    OnPropertyChanged(nameof(Pensja));
                }
            }
        }

        private string stanowisko;
        public string Stanowisko
        {
            get => stanowisko;
            set
            {
                if (stanowisko != value)
                {
                    stanowisko = value;
                    OnPropertyChanged(nameof(Stanowisko));
                }
            }
        }

        private PracownikInfo info;
        public PracownikInfo Info
        {
            get => info;
            set
            {
                if (info != value)
                {
                    info = value;
                    OnPropertyChanged(nameof(Info));
                }
            }
        }

        private EnhancedObservableCollection<Pracownik> podwladni;
        public EnhancedObservableCollection<Pracownik> Podwladni
        {
            get => podwladni;
            set
            {
                if (podwladni != value)
                {
                    podwladni = value;
                    OnPropertyChanged(nameof(Podwladni));
                }
            }
        }

        private Pracownik przelozony;
        public Pracownik Przelozony
        {
            get => przelozony;
            set
            {
                if (przelozony != value)
                {
                    przelozony = value;
                    OnPropertyChanged(nameof(Przelozony));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Pracownik(string imie, string nazwisko, int staz, double pensja, string stanowisko, Pracownik przelozony)
        {
            ID = Guid.NewGuid();
            Imie = imie;
            Nazwisko = nazwisko;
            Staz = staz;
            Pensja = pensja;
            Stanowisko = stanowisko;
            Info = new PracownikInfo();
            Podwladni = new EnhancedObservableCollection<Pracownik>();
            Przelozony = przelozony;
        }

        private Pracownik()
        {

        }

        public static Pracownik fromPracownikXml(XmlSerde.PracownikXml pXml, Pracownik? przelozony)
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(timestamp).CopyTo(bytes, 0);
            BitConverter.GetBytes(timestamp).CopyTo(bytes, 8);

            Pracownik p = new Pracownik
            {
                ID = new Guid(bytes),
                Imie = pXml.Imie,
                Nazwisko = pXml.Nazwisko,
                Staz = pXml.Staz,
                Pensja = pXml.Pensja,
                Stanowisko = pXml.Stanowisko,
                Info = pXml.Info,
                Podwladni = new EnhancedObservableCollection<Pracownik>(),
                Przelozony = przelozony
            };

            p.Podwladni = new EnhancedObservableCollection<Pracownik>(
                    pXml.Podwladni?.Select(nowy => fromPracownikXml(nowy, p)) ?? new List<Pracownik>()
                );


            return p;
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

            return $"{ind}ID: {ID}\n" +
                $"{ind}Imie: {Imie}\n" +
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

        public int GetLiczbaPodwladnych()
        {
            return GetPodwladniCount(new Dictionary<Pracownik, int>());
        }
    }
}
