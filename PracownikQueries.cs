using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnet
{
    public static class PracownikQueries
    {
        //Projekcja elementów kolekcji
        public static IEnumerable<dynamic> Zapytanie1(IEnumerable<Pracownik> pracownicy)
        {
            var result = from p in pracownicy
                         where IsOddGuid(p.ID)
                         select new
                         {
                             SUM_OF = (double)p.Info.OcenaPracownika + p.Info.Premia,
                             UPPERCASE = p.Info.Wyksztalcenie.ToString().ToUpper()
                         };

            return result;
        }

        //Grupowanie wyników zapytania 1
        public static void Zapytanie2(IEnumerable<Pracownik> pracownicy)
        {
            var projected = Zapytanie1(pracownicy);

            var grouped = 
                from item in projected
                group item by item.UPPERCASE into g
                select new
                {
                    UPPERCASE = g.Key,
                    AVG = g.Average(x => (double)x.SUM_OF)
                };


            foreach (var group in grouped)
            {
                Console.WriteLine($"UPPERCASE: {group.UPPERCASE}, Średnia SUM_OF: {group.AVG:F2}");
            }
        }

        private static bool IsOddGuid(Guid id)
        {
            byte lastByte = id.ToByteArray().Last();
            return lastByte % 2 != 0;
        }
    }
}
