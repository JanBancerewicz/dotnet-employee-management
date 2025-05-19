using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dotnet
{
    public static class XmlSerde
    {
        public class PracownikXml
        {
            public string Imie { get; set; }
            public string Nazwisko { get; set; }
            public int Staz { get; set; }
            public double Pensja { get; set; }
            public string Stanowisko { get; set; }
            public PracownikInfo Info { get; set; }
            public List<PracownikXml> Podwladni { get; set; }
        }

        public static void SavePracownicyToXml(ObservableCollection<Pracownik> pracownicy, string filePath)
        {
            List<PracownikXml> simplifiedList = pracownicy
                .Select(ToXmlDto)
                .ToList();

            var serializer = new XmlSerializer(typeof(List<PracownikXml>));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, simplifiedList);
            }

            Console.WriteLine("saved");
        }

        public static ObservableCollection<Pracownik> LoadPracownicyFromXml(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<PracownikXml>));
            using (var reader = new StreamReader(filePath))
            {
                var xmlList = (List<PracownikXml>)serializer.Deserialize(reader);
                return new ObservableCollection<Pracownik>(
                    xmlList.Select(FromXmlDto).ToList()
                );
            }
        }

        // Converts full Pracownik -> PracownikXml
        private static PracownikXml ToXmlDto(Pracownik p)
        {
            return new PracownikXml
            {
                Imie = p.Imie,
                Nazwisko = p.Nazwisko,
                Staz = p.Staz,
                Pensja = p.Pensja,
                Stanowisko = p.Stanowisko,
                Info = p.Info,
                Podwladni = p.Podwladni?.Select(ToXmlDto).ToList()
            };
        }

        private static Pracownik FromXmlDto(PracownikXml pXml)
        {
            Pracownik? root = null;
            return Pracownik.fromPracownikXml(pXml, root);
        }
    }
}
