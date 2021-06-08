using CoreWPF.MVVM;
using CoreWPF.Utilites;
using System.Windows;

namespace PrintTemplate.Models
{
    public class Element : NotifyPropertyChanged
    {
        protected const double DefaultMinWidth = 450;
        protected const double DefaultMinHeight = 120;

        private double minWidth;
        public double MinWidth
        {
            get => this.minWidth;
            protected set
            {
                this.minWidth = value;
                this.OnPropertyChanged("MinWidth");
            }
        }

        private double minHeight;
        public double MinHeight
        {
            get => this.minHeight;
            protected set
            {
                this.minHeight = value;
                this.OnPropertyChanged("MinHeight");
            }
        }
        public static Element Default
        {
            get
            {
                return new Element() { MinWidth = DefaultMinWidth, MinHeight = DefaultMinHeight, Width = DefaultMinWidth, Height = DefaultMinHeight, Position = new Point(10, 10), ZIndex = 1};
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

        public Element()
        {
            this.Visible = Visibility.Visible;
        }

        public Element(Element element) : this()
        {
            this.Position = element.Position;
            this.Width = element.Width;
            this.Height = element.Height;
        }

        public override string ToString()
        {
            string result = "=-= " + this.GetType().Name + " =-=";
            result += "\nPosition: x" + this.Position.X + " y" + this.Position.Y;
            result += "\nWidth: " + this.Width;
            result += "\nHeight: " + this.Height;
            return result;
        }

        void MoveTo(Point newPosition)
        {
            Position = newPosition;
        }
        public RelayCommand<Point> RequestMove { get => new RelayCommand<Point>(point =>
            {
                if(this.Visible == Visibility.Visible) this.MoveTo(point);
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
