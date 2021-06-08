using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintTemplate.Models
{
    public class BorderElement : Element
    {
        public static new Element Default
        {
            get => new BorderElement() { Height = 100, Width = 100, BorderThickness = 1 };
        }

        private double borderThickness;
        public double BorderThickness
        {
            get => this.borderThickness;
            set
            {
                this.borderThickness = value;
                this.OnPropertyChanged("BorderThickness");
            }
        }

        public BorderElement() : base()
        {
            this.MinWidth = 100;
            this.MinHeight = 100;
        }

        public BorderElement(Element element) : this()
        {
            this.ZIndex = 0;
            this.Position = element.Position;
            this.Width = element.Width;
            this.Height = element.Height;
        }

        public override string ToString()
        {
            string result =  base.ToString();
            result += "\nBorderThickness: " + this.BorderThickness;
            return result;
        }
    }
}
