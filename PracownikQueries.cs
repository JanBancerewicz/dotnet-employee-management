using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnet
{
    public static class PracownikQueries
    {
        public class Zapytanie1Result
        {
            public double SUM_OF { get; set; }
            public string UPPERCASE { get; set; }
        }

        //Projekcja elementów kolekcji
        public static IEnumerable<Zapytanie1Result> Zapytanie1(IEnumerable<Pracownik> pracownicy)
        {
            return pracownicy
                .Where(p => IsOddGuid(p.ID))
                .Select(p => new Zapytanie1Result
                {
                    SUM_OF = p.Info.OcenaPracownika + p.Info.Premia,
                    UPPERCASE = p.Info.Wyksztalcenie.ToString().ToUpper()
                });
        }

        //Grupowanie wyników zapytania 1
        public static void Zapytanie2(IEnumerable<Pracownik> pracownicy)
        {
            var projected = Zapytanie1(pracownicy);

            var grouped = projected
                .GroupBy(x => x.UPPERCASE);  // Grupowanie po poziomie wykształcenia

            
            foreach (var group in grouped)
            {
                double avg = group.Average(x => x.SUM_OF);
                Console.WriteLine($"UPPERCASE: {group.Key}, Średnia SUM_OF: {avg:F2}");
            }
        }

        private static bool IsOddGuid(Guid id)
        {
            byte lastByte = id.ToByteArray().Last();
            return lastByte % 2 != 0;
        }
    }
}
