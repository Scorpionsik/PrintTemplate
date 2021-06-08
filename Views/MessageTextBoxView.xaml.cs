using CoreWPF.Windows;
using PrintTemplate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrintTemplate.Views
{
    /// <summary>
    /// Логика взаимодействия для MessageTextBox.xaml
    /// </summary>
    public partial class MessageTextBoxView : DialogWindowExt
    {
        public MessageTextBoxView(string text)
        {
            InitializeComponent();
            this.DataContext = new MessageTextBoxViewModel(text);
        }
    }
}
