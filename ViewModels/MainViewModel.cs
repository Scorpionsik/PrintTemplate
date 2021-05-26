using CoreWPF.MVVM;
using CoreWPF.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Test_DragDrop.Models;

namespace Test_DragDrop.ViewModels
{
    class MainViewModel : ViewModel
    {
        public ListExt<Element> Elements { get; private set; }
        public MainViewModel()
        {
            this.Title = App.AppTitle;
            this.Elements = new ListExt<Element>
            {
                new Element(50, 50) { Position = new Point(30, 30), Text = "t1"},
                new Element(100, 50) { Position = new Point(60, 60), Text = "t2"}
            };
        }
    }
}
