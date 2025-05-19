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

    public class PracownikInfo
	{
        public int OcenaPracownika { get; set; } // z zakresu 1-10
        public double Premia { get; set; } // mniejsza niz pensja
        public PoziomWyksztalcenia Wyksztalcenie { get; set; }

        public PracownikInfo()
        {
            Random r = new Random();
            OcenaPracownika = r.Next(1, 11); // 1–10
            Premia = Math.Round(r.NextDouble() * 100) * 10;
            Wyksztalcenie = (PoziomWyksztalcenia)r.Next(0, 4);
        }
    }
}