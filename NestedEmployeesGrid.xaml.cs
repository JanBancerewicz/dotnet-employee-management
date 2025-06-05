using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace dotnet
{
    public partial class NestedEmployeesGrid : UserControl
    {
        public NestedEmployeesGrid()
        {
            InitializeComponent();
            NestedDataGrid.SelectionChanged += NestedDataGrid_SelectionChanged;
        }

        private void NestedDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NestedDataGrid.SelectedItem is Pracownik selected &&
                Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.UpdateEmployeeDetails(selected);

                // Odznacz w głównym DataGrid
                mainWindow.PracownicyDataGrid.SelectedItem = null;

                // Odznacz w rodzicielskich DataGridach
                var parentDataGrid = FindParentDataGrid();
                if (parentDataGrid != null)
                {
                    parentDataGrid.SelectedItem = null;
                }
            }
        }

        private DataGrid FindParentDataGrid()
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is DataGrid))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as DataGrid;
        }

        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.DeleteEmployee_Click(sender, e);
            }
        }

        private void AddSubordinate_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.AddSubordinate_Click(sender, e);
            }
        }
    }
}