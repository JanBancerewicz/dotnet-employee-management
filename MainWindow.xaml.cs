using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.XPath;

namespace dotnet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Pracownik> Pracownicy { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Pracownicy = new ObservableCollection<Pracownik>();
            DataContext = this; //stary jest na dole
            Generate_Click(this, null); // xddd samo sie klika
        }

        private static T? FindVisualParent<T>(DependencyObject? child) where T : DependencyObject
        {
            while (child != null && child is not T)
            {
                child = VisualTreeHelper.GetParent(child);
            }
            return child as T;
        }


        private void SubordinatesGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (sender is DataGrid grid && grid.DataContext is Pracownik przelozony)
            {
                var nowy = new Pracownik("NoweImie", "NoweNazwisko", 0, 0.0, "NoweStanowisko", przelozony);
                e.NewItem = nowy;
            }
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is Pracownik pracownik)
            {
                if (pracownik.Przelozony != null)
                {
                    pracownik.Przelozony.Podwladni.Remove(pracownik);
                }
                else
                {
                    Pracownicy.Remove(pracownik);
                }
            }
        }

        private void AddSubordinate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is Pracownik przelozony)
            {
                var nowy = new Pracownik("NoweImie", "NoweNazwisko", 0, 0.0, "NoweStanowisko", przelozony);

                przelozony.Podwladni.Add(nowy);
            }
        }


        private void Version_Click(object sender, RoutedEventArgs e)
        {
            string version = "9_0b00000001";
            MessageBox.Show($"Wersja aplikacji: {version}", "Wersja", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var forest = MockPracownikGenerator.GenerateFlatStructure(branchingFactor: 3, depth: 2, count: 50);
            Pracownicy.Clear();
            foreach (var pracownik in forest)
            {
                Pracownicy.Add(pracownik);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PracownicyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PracownicyDataGrid.SelectedItem is Pracownik selected)
            {
                ImieBox.Text = selected.Imie;
                NazwiskoBox.Text = selected.Nazwisko;
                StanowiskoBox.Text = selected.Stanowisko;
                StazBox.Text = selected.Staz.ToString();
                PensjaBox.Text = selected.Pensja.ToString("F2");
                PremiaBox.Text = selected.Info?.Premia.ToString("F2") ?? "";
                OcenaBox.Text = selected.Info?.OcenaPracownika.ToString("F1") ?? "";
                WyksztalcenieBox.Text = selected.Info?.Wyksztalcenie.ToString() ?? "";
                PodwladniBox.Text = selected.GetLiczbaPodwladnych().ToString();
            }
            else
            {
                ImieBox.Text = "";
                NazwiskoBox.Text = "";
                StanowiskoBox.Text = "";
                StazBox.Text = "";
                PensjaBox.Text = "";
                PremiaBox.Text = "";
                OcenaBox.Text = "";
                WyksztalcenieBox.Text = "";
                PodwladniBox.Text = "";
            }
        }

        private void UpdateInspector(Pracownik p)
        {
            if (p == null) return;

            ImieBox.Text = p.Imie;
            NazwiskoBox.Text = p.Nazwisko;
            StanowiskoBox.Text = p.Stanowisko;
            StazBox.Text = p.Staz.ToString();
            PensjaBox.Text = p.Pensja.ToString("F2");
            PremiaBox.Text = p.Info?.Premia.ToString("F2") ?? "";
            OcenaBox.Text = p.Info?.OcenaPracownika.ToString() ?? "";
            WyksztalcenieBox.Text = p.Info?.Wyksztalcenie.ToString() ?? "";
            PodwladniBox.Text = p.GetLiczbaPodwladnych().ToString();
        }

        private void SubordinateDataGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var row = FindVisualParent<DataGridRow>(e.OriginalSource as DependencyObject);
            if (row != null && row.Item is Pracownik clickedSubordinate)
            {
                // Odznacz głównego pracownika, jeśli zaznaczony
                PracownicyDataGrid.SelectedItem = null;

                // Zaktualizuj inspektor
                UpdateInspector(clickedSubordinate);

                // Zapobiegaj domyślnemu zaznaczeniu (opcjonalnie)
                e.Handled = true;
            }

            
        }

        private void ShowEmployeeDetails(Pracownik emp)
        {
            if (emp == null)
            {
                ImieBox.Text = "";
                NazwiskoBox.Text = "";
                StanowiskoBox.Text = "";
                StazBox.Text = "";
                PensjaBox.Text = "";
                PremiaBox.Text = "";
                OcenaBox.Text = "";
                WyksztalcenieBox.Text = "";
                PodwladniBox.Text = "";
                return;
            }

            ImieBox.Text = emp.Imie;
            NazwiskoBox.Text = emp.Nazwisko;
            StanowiskoBox.Text = emp.Stanowisko;
            StazBox.Text = emp.Staz.ToString();
            PensjaBox.Text = emp.Pensja.ToString("C"); // lub inny format
            PremiaBox.Text = emp.Info?.Premia.ToString("C") ?? "";
            OcenaBox.Text = emp.Info?.OcenaPracownika.ToString() ?? "";
            WyksztalcenieBox.Text = emp.Info?.Wyksztalcenie.ToString() ?? "";
            PodwladniBox.Text = emp.Podwladni?.Count.ToString() ?? "0";
        }

        private void PodwladniDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid?.SelectedItem is Pracownik selectedSubordinate)
            {
                ShowEmployeeDetails(selectedSubordinate);
            }
        }





        private void Zapytanie1_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PracownikQueries.Zapytanie1(Pracownicy))
            {
                Console.WriteLine($"SUM_OF: {item.SUM_OF}, UPPERCASE: {item.UPPERCASE}");
            }
        }

        private void Zapytanie2_Click(object sender, RoutedEventArgs e)
        {
            PracownikQueries.Zapytanie2(Pracownicy);
        }

        private void Eksportuj_Click(object sender, RoutedEventArgs e)
        {
            XmlSerde.SavePracownicyToXml(Pracownicy, "../../../save.xml");
        }

        private void Importuj_Click(object sender, RoutedEventArgs e)
        {
            var imported = XmlSerde.LoadPracownicyFromXml("../../../save.xml");
            Pracownicy.Clear();
            foreach (var p in imported)
                Pracownicy.Add(p);
        }

        private void XPath_Click(object sender, RoutedEventArgs e)
        {
            var xmlFile = @"../../../save.xml";
            XPathDocument doc = new XPathDocument(xmlFile);
            XPathNavigator nav = doc.CreateNavigator();

            string xpath = "//PracownikXml[Podwladni[not(PracownikXml)]][not(Info/OcenaPracownika = preceding::PracownikXml/Info/OcenaPracownika)]";

            var nodes = nav.Select(xpath);

            int i = 1;
            while (nodes.MoveNext())
            {
                var current = nodes.Current;
                Console.WriteLine("\n" + (i++));
                Console.WriteLine(current.OuterXml);
            }
        }

        private void HTML_Click(object sender, RoutedEventArgs e)
        {
            var html = HtmlPrinter.GenerateHtmlTable(Pracownicy);
            html.Save("../../../pracownicy.html");
        }

    }
}