using CoreWPF.MVVM;
using CoreWPF.Utilites;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Test_DragDrop.Models;

namespace Test_DragDrop.ViewModels
{
    class MainViewModel : ViewModel
    {
        private event Action<Visibility> eventHideEditorsChanged;
        public event Action<Visibility> EventHideEditorsChanged
        {
            add
            {
                this.eventHideEditorsChanged -= value;
                this.eventHideEditorsChanged += value;
            }
            remove => this.eventHideEditorsChanged -= value;
        }

        private bool hideEditors;
        public bool HideEditors
        {
            get => this.hideEditors;
            set
            {
                this.hideEditors = value;
                this.OnPropertyChanged("HideEditors");
                this.OnPropertyChanged("CurrentColor");
                this.eventHideEditorsChanged?.Invoke(this.CurrentVisibility);
            }
        }

        private Visibility CurrentVisibility
        {
            get
            {
                return this.HideEditors ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public string CurrentColor
        {
            get => this.HideEditors ? "White" : "Beige";
        }

        private Element selectedElement;
        public Element SelectedElement
        {
            get => this.selectedElement;
            set
            {
                if(this.selectedElement != null) this.selectedElement.ZIndex = 0;
                this.selectedElement = value;
                this.selectedElement.ZIndex = 1;
            }
        }
        public ListExt<Element> Elements { get; private set; }
        public MainViewModel()
        {
            this.Title = App.AppTitle;
            this.Elements = new ListExt<Element>();
            this.AddElement(TextElement.Default);
        }

        private void AddElement(Element element)
        {
            element.CommandRemoveElement = this.CommandRemoveElement;
            element.CommandSelectItem = this.CommandSelectItem;
            this.EventHideEditorsChanged += element.SetVisible;
            this.Elements.Add(element);
        }

        public RelayCommand CommandAddElement
        {
            get => new RelayCommand(obj =>
            {
                this.AddElement(TextElement.Default);
            });
        }
        public RelayCommand<Element> CommandRemoveElement
        {
            get => new RelayCommand<Element>(element =>
            {
                this.Elements.Remove(element);
            });
        }

        public RelayCommand<Element> CommandSelectItem
        {
            get => new RelayCommand<Element>(element =>
            {
                if(this.SelectedElement != element) this.SelectedElement = element;
            });
        }
    }
}
