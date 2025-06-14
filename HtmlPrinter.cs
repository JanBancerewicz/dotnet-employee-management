using System.Xml.Linq;

namespace dotnet
{
    public static class HtmlPrinter
    {
        public static XDocument GenerateHtmlTable(IEnumerable<Pracownik> pracownicy, int depth = 0)
        {
            XDocument htmlDoc = new XDocument(
                new XDocumentType("html", null, null, null),
                new XElement("html",
                    new XElement("body",
                        new XElement("table",
                            new XAttribute("border", "1"),
                            new XElement("thead",
                                new XElement("tr",
                                    new XElement("th", "Imie i Nazwisko"),
                                    new XElement("th", "Staz"),
                                    new XElement("th", "Pensja"),
                                    new XElement("th", "Stanowisko"),
                                    new XElement("th", "Ocena"),
                                    new XElement("th", "Premia"),
                                    new XElement("th", "Wyksztalcenie")
                                )
                            ),
                            new XElement("tbody",
                                from employee in FlattenHierarchy(pracownicy)
                                select new XElement("tr",
                                    new XElement("td",
                                        new XAttribute("style", $"padding-left: {employee.Depth * 3}em"),
                                        $"{employee.Employee.Imie} {employee.Employee.Nazwisko}"),
                                    new XElement("td", employee.Employee.Staz),
                                    new XElement("td", $"{employee.Employee.Pensja:C}"),
                                    new XElement("td", employee.Employee.Stanowisko),
                                    new XElement("td", employee.Employee.Info.OcenaPracownika),
                                    new XElement("td", $"{employee.Employee.Info.Premia:C}"),
                                    new XElement("td", employee.Employee.Info.Wyksztalcenie)
                                )
                            )
                        )
                    )
                )
            );

            return htmlDoc;
        }

        private static IEnumerable<(Pracownik Employee, int Depth)> FlattenHierarchy(Pracownik root, int depth)
        {
            yield return (root, depth);
            foreach (var subordinate in root.Podwladni)
            {
                foreach (var item in FlattenHierarchy(subordinate, depth + 1))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<(Pracownik Employee, int Depth)> FlattenHierarchy(IEnumerable<Pracownik> roots)
        {
            return roots.SelectMany(root => FlattenHierarchy(root, 0));
        }
    }
}