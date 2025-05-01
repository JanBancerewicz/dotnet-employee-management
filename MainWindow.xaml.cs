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
            var forest = MockPracownikGenerator.GenerateForest(branchingFactor: 3, depth: 2);

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

        public MainWindow()
        {
            InitializeComponent();
            Generate_Click(this, null); // xddd samo sie klika
        }
    }
}