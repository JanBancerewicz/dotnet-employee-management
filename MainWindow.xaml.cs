using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Quic;
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

namespace dotnet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TreeViewItem BuildTreeItem(Pracownik pracownik)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = $"{pracownik.Stanowisko}: {pracownik.Imie} {pracownik.Nazwisko}"
            };

            foreach (var podwladny in pracownik.Podwladni)
            {
                item.Items.Add(BuildTreeItem(podwladny));
            }

            return item;
        }


        private void Version_Click(object sender, RoutedEventArgs e)
        {
            string version = "7_0b00000001";
            MessageBox.Show($"Wersja aplikacji: {version}", "Wersja", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            // Generowanie danych
            //var forest = MockPracownikGenerator.GenerateForest(branchingFactor: 3, depth: 2);

            var forest = MockPracownikGenerator.GenerateFlatStructure(branchingFactor: 3, depth: 2, count: 50); 

            PracownicyTreeView.ItemsSource = forest;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PracownicyTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue;

            if (selectedItem is Pracownik pracownik)
            {
                detailsTextBlock.Text = pracownik.GetDetailsString(0);
            }
        }

        private void ItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PracownicyTreeView.SelectedItem is Pracownik pracownik)
            {
                if (pracownik.Przelozony == null)
                {
                    ((ObservableCollection<Pracownik>) PracownicyTreeView.ItemsSource).Remove(pracownik);
                    PracownicyTreeView.UpdateLayout();
                }
                else
                {
                    pracownik.Przelozony.Podwladni.Remove(pracownik);
                }
            }
        }

        private void ItemAdd_Click(object sender, RoutedEventArgs e)
        {
            if (PracownicyTreeView.SelectedItem is Pracownik pracownik)
            {
                var formWindow = new AddPodwladnyWindow(pracownik);
                formWindow.ShowDialog();
            }
        }

        private void Zapytanie1_Click(object sender, RoutedEventArgs e)
        {
            if (PracownicyTreeView.ItemsSource is ObservableCollection<Pracownik> pracownicy)
            {
                var wynik = PracownikQueries.Zapytanie1(pracownicy);
                foreach (var item in wynik)
                {
                    Console.WriteLine($"UPPERCASE: {item.UPPERCASE}, SUM_OF: {item.SUM_OF}");
                }
            }
        }

        private void Zapytanie2_Click(object sender, RoutedEventArgs e)
        {
            if (PracownicyTreeView.ItemsSource is ObservableCollection<Pracownik> pracownicy)
            {
                PracownikQueries.Zapytanie2(pracownicy);
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            Generate_Click(this, null); // xddd samo sie klika
        }
    }
}