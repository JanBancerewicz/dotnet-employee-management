using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


        public void SubordinatesGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (sender is DataGrid grid && grid.DataContext is Pracownik przelozony)
            {
                var nowy = new Pracownik("NoweImie", "NoweNazwisko", 0, 0.0, "NoweStanowisko", przelozony);
                e.NewItem = nowy;
            }
        }

        public void DeleteEmployee_Click(object sender, RoutedEventArgs e)
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

        public void AddSubordinate_Click(object sender, RoutedEventArgs e)
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

        /*private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var forest = MockPracownikGenerator.GenerateFlatStructure(branchingFactor: 3, depth: 2, count: 50);
            Pracownicy.Clear();
            foreach (var pracownik in forest)
            {
                Pracownicy.Add(pracownik);
            }
        }*/

        private void ExpandAllSubordinates(DataGrid grid)
        {
            if (grid == null) return;

            foreach (var item in grid.Items)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.DetailsVisibility = Visibility.Visible;

                    // Rekurencyjne rozwijanie podrzędnych DataGrid
                    var detailsPresenter = FindVisualChild<DataGridDetailsPresenter>(row);
                    if (detailsPresenter != null)
                    {
                        var nestedGrid = FindVisualChild<DataGrid>(detailsPresenter);
                        ExpandAllSubordinates(nestedGrid);
                    }
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T result)
                    return result;

                var childResult = FindVisualChild<T>(child);
                if (childResult != null)
                    return childResult;
            }
            return null;
        }

        // Modyfikacja metody Generate_Click
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var forest = MockPracownikGenerator.GenerateFlatStructure(branchingFactor: 3, depth: 2, count: 10);
            Pracownicy.Clear();
            foreach (var pracownik in forest)
            {
                Pracownicy.Add(pracownik);
            }

            // Rozwiń wszystkie poziomy po wygenerowaniu danych
            Dispatcher.BeginInvoke(new Action(() => ExpandAllSubordinates(PracownicyDataGrid)),
                System.Windows.Threading.DispatcherPriority.Background);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PracownicyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PracownicyDataGrid.SelectedItem is Pracownik selected)
            {
                UpdateEmployeeDetails(selected);
            }
            else
            {
                ClearInspector();
            }
        }

        public void UpdateEmployeeDetails(Pracownik pracownik)
        {
            if (pracownik == null)
            {
                ClearInspector();
                return;
            }

            ImieBox.Text = pracownik.Imie;
            NazwiskoBox.Text = pracownik.Nazwisko;
            StanowiskoBox.Text = pracownik.Stanowisko;
            StazBox.Text = pracownik.Staz.ToString();
            PensjaBox.Text = pracownik.Pensja.ToString("F2");
            PremiaBox.Text = pracownik.Info?.Premia.ToString("F2") ?? "0";
            OcenaBox.Text = pracownik.Info?.OcenaPracownika.ToString("F1") ?? "0";
            WyksztalcenieBox.Text = pracownik.Info?.Wyksztalcenie.ToString() ?? "Brak";
            PodwladniBox.Text = pracownik.GetLiczbaPodwladnych().ToString();
        }

        private void ClearInspector()
        {
            ImieBox.Text = string.Empty;
            NazwiskoBox.Text = string.Empty;
            StanowiskoBox.Text = string.Empty;
            StazBox.Text = string.Empty;
            PensjaBox.Text = string.Empty;
            PremiaBox.Text = string.Empty;
            OcenaBox.Text = string.Empty;
            WyksztalcenieBox.Text = string.Empty;
            PodwladniBox.Text = string.Empty;
        }

        private void DataGridRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.Item is Pracownik pracownik)
            {
                UpdateEmployeeDetails(pracownik);

                // Upewnij się, że wiersz jest zaznaczony
                if (row.IsSelected)
                {
                    if (FindVisualParent<DataGrid>(row) is DataGrid parentGrid)
                    {
                        parentGrid.SelectedItem = pracownik;
                    }
                }
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


        // Rozwiń wszystkie wiersze
        private void ExpandAll_Click(object sender, RoutedEventArgs e)
        {
            SetAllDetailsVisibility(PracownicyDataGrid, true);
        }

        // Zwiń wszystkie wiersze
        private void CollapseAll_Click(object sender, RoutedEventArgs e)
        {
            SetAllDetailsVisibility(PracownicyDataGrid, false);
        }

        // Metoda pomocnicza
        private void SetAllDetailsVisibility(DataGrid dataGrid, bool isExpanded)
        {
            if (dataGrid == null) return;

            foreach (var item in dataGrid.Items)
            {
                var row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (row != null)
                {
                    row.DetailsVisibility = isExpanded ? Visibility.Visible : Visibility.Collapsed;

                    // Rekurencyjnie dla zagnieżdżonych DataGridów
                    if (isExpanded && row.DetailsVisibility == Visibility.Visible)
                    {
                        var detailsPresenter = GetVisualChild<DataGridDetailsPresenter>(row);
                        if (detailsPresenter != null)
                        {
                            foreach (var child in GetVisualChildren(detailsPresenter))
                            {
                                if (child is DataGrid nestedGrid)
                                {
                                    SetAllDetailsVisibility(nestedGrid, isExpanded);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Pomocnicza metoda do znajdowania kontrolek
        private static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;
            }
            return null;
        }

        // Pomocnicza metoda dla wszystkich dzieci
        private static IEnumerable<DependencyObject> GetVisualChildren(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                yield return VisualTreeHelper.GetChild(parent, i);
            }
        }

    }
}