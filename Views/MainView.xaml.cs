using CoreWPF.Windows;
using Test_DragDrop.ViewModels;

namespace Test_DragDrop.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainView : WindowExt
    {
        public MainView()
        {
            InitializeComponent();
            MainViewModel vm = new MainViewModel();
            this.DataContext = vm;
        }
    }
}
