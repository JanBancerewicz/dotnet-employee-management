using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnet
{
    public static class MockPracownikGenerator
    {
        private static string[] names = { "Stasiu", "Wiesiu", "Marcin", "Włodek", "Kamil", "Jan", "Leon", "Gabriel" };
        private static string[] surnames = { "Kowalski", "Nazwiskowy", "Doe", "Skrzypczyński", "Nowak" };
        private static string[] roles = { "Sprzedawca", "Manager", "Dostawca", "Szef", "Produkt na półce", "Sprzątacz" };

        private static Random random = new Random();

        private static T Choose<T>(T[] array)
        {
            return array[random.Next(array.Length)];
        }

        public static Pracownik GeneratePracownik()
        {
            return new Pracownik(
            Choose(names),
            Choose(surnames),
            random.Next(1, 31),
            Math.Round(random.NextDouble() * 100) * 100 + 4000,
            Choose(roles)
    );
        }

        public static HashSet<Pracownik> GenerateForest(int branchingFactor, int depth)
        {
            var forest = new HashSet<Pracownik>();
            for (int i = 0; i < branchingFactor; i++)
            {
                forest.Add(GenerateTree(branchingFactor, depth));
            }
            return forest;
        }

        public static Pracownik GenerateTree(int branchingFactor, int depth)
        {
            var pracownik = GeneratePracownik();
            if (depth > 0)
            {
                for (int i = 0; i < branchingFactor; i++)
                {
                    pracownik.Podwladni.Add(GenerateTree(branchingFactor, depth - 1));
                }
            }
            return pracownik;
        }
    }
}
