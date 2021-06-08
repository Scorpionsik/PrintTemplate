using CoreWPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintTemplate.ViewModels
{
    internal class MessageTextBoxViewModel : ViewModel
    {
        private string text;
        public string Text
        {
            get => this.text;
            private set
            {
                this.text = value;
                this.OnPropertyChanged("Text");
            }
        }
        public MessageTextBoxViewModel(string text)
        {
            this.Text = text;
        }
    }
}
