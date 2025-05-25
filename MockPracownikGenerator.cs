using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
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

        public static Pracownik GeneratePracownik(Pracownik parent)
        {
            return new Pracownik(
                Choose(names),
                Choose(surnames),
                random.Next(1, 31),
                Math.Round(random.NextDouble() * 100) * 100 + 4000,
                Choose(roles),
                parent
            );
        }

        public static ObservableCollection<Pracownik> GenerateForest(int branchingFactor, int depth)
        {
            var forest = new ObservableCollection<Pracownik>();
            for (int i = 0; i < branchingFactor; i++)
            {
                forest.Add(GenerateTree(branchingFactor, depth, null));
            }
            return forest;
        }

        public static Pracownik GenerateTree(int branchingFactor, int depth, Pracownik parent)
        {
            var pracownik = GeneratePracownik(parent);
            if (depth > 0)
            {
                for (int i = 0; i < branchingFactor; i++)
                {
                    pracownik.Podwladni.Add(GenerateTree(branchingFactor, depth - 1, pracownik));
                }
            }
            return pracownik;
        }

        public static ObservableCollection<Pracownik> GenerateFlatStructure(int branchingFactor, int depth, int count = 20)
        {
            var all = new ObservableCollection<Pracownik>();

            for (int i = 0; i < count; i++)
            {
                all.Add(GenerateTree(branchingFactor, depth, null));
            }
            return all;
        }


    }
}
