using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet
{
    public enum PoziomWyksztalcenia
    {
        Podstawowe,
        Srednie,
        Zawodowe,
        Wyższe
    }

    public class PracownikInfo : INotifyPropertyChanged
    {
        private int ocena;
        public int OcenaPracownika
        {
            get => ocena;
            set
            {
                if (ocena != value)
                {
                    ocena = value;
                    OnPropertyChanged(nameof(OcenaPracownika));
                }
            }
        }

        private double premia;
        public double Premia
        {
            get => premia;
            set
            {
                if (premia != value)
                {
                    premia = value;
                    OnPropertyChanged(nameof(Premia));
                }
            }
        }

        private PoziomWyksztalcenia wyksztalcenie;
        public PoziomWyksztalcenia Wyksztalcenie
        {
            get => wyksztalcenie;
            set
            {
                if (wyksztalcenie != value)
                {
                    wyksztalcenie = value;
                    OnPropertyChanged(nameof(Wyksztalcenie));
                }
            }
        }

        public PracownikInfo()
        {
            Random r = new Random();
            OcenaPracownika = r.Next(1, 11);
            Premia = Math.Round(r.NextDouble() * 100) * 10;
            Wyksztalcenie = (PoziomWyksztalcenia)r.Next(0, 4);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }


}