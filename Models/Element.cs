using CoreWPF.MVVM;
using CoreWPF.Utilites;
using System.Windows;

namespace Test_DragDrop.Models
{
    class Element : NotifyPropertyChanged
    {
        private string text;
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;
                this.OnPropertyChanged("Text");
            }
        }

        private double width;
        public double Width
        {
            get => this.width;
            private set
            {
                this.width = value;
                this.OnPropertyChanged("Width");
            }
        }

        private double height;
        public double Height
        {
            get => this.height;
            private set
            {
                this.height = value;
                this.OnPropertyChanged("Height");
            }
        }

        // стандартное свойство
        Point position;
        public Point Position
        {
            get { return position; }
            set 
            { 
                if (position != value) 
                { 
                    position = value; 
                    this.OnPropertyChanged(); 
                } 
            }
        }

        public Element(double width, double height)
        {
            this.SetWidth(width);
            this.SetHeight(height);
        }

        void MoveTo(Point newPosition)
        {
            Position = newPosition;
        }

        public void SetWidth(double value)
        {
            this.Width = value;
        }

        public void SetHeight(double value)
        {
            this.Height = value;
        }
        public RelayCommand<Point> RequestMove { get => new RelayCommand<Point>(point =>
            {
                this.MoveTo(point);
            });
        }
    }
}
