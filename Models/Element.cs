using CoreWPF.MVVM;
using CoreWPF.Utilites;
using System.Windows;

namespace Test_DragDrop.Models
{
    public class Element : NotifyPropertyChanged
    {
        public static double MinWidth{ get => 450; }
        public static double MinHeight { get => 120; }
        public static Element Default
        {
            get
            {
                return new Element() { Width = MinWidth, Height = MinHeight, Position = new Point(10, 10), ZIndex = 1};
            }
        }

        private int zIndex;
        public int ZIndex
        {
            get => this.zIndex;
            set
            {
                this.zIndex = value;
                this.OnPropertyChanged("ZIndex");
            }
        }

        private Visibility visible;
        public Visibility Visible
        {
            get => this.visible;
            private set
            {
                this.visible = value;
                this.OnPropertyChanged("Visible");
            }
        }


        private double width;
        public double Width
        {
            get => this.width;
            set
            {
                this.width = value;
                this.OnPropertyChanged("Width");
            }
        }

        private double height;
        public double Height
        {
            get => this.height;
            set
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

        void MoveTo(Point newPosition)
        {
            Position = newPosition;
        }
        public RelayCommand<Point> RequestMove { get => new RelayCommand<Point>(point =>
            {
                this.MoveTo(point);
            });
        }

        public virtual void SetVisible(Visibility visibility)
        {
            this.Visible = visibility;
        }

        private RelayCommand<Element> commandRemoveElement;
        public RelayCommand<Element> CommandRemoveElement
        {
            get => this.commandRemoveElement;
            set
            {
                this.commandRemoveElement = value;
                this.OnPropertyChanged("CommandRemoveElement");
            }
        }

        private RelayCommand<Element> commandSelectItem;
        public RelayCommand<Element> CommandSelectItem
        {
            get => this.commandSelectItem;
            set
            {
                this.commandSelectItem = value;
                this.OnPropertyChanged("CommandSelectItem");
            }
        }

        public RelayCommand CommandResetSize
        {
            get => new RelayCommand(obj =>
            {
                this.Width = MinWidth;
                this.Height = MinHeight;
            });
        }
    }
}
