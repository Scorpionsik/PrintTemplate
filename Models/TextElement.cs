using CoreWPF.Utilites;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrintTemplate.Models
{
    public class TextElement : Element
    {
        
        public static ListExt<string> FontFamilyCollection { get; private set; }
        public static ListExt<string> TextAlignmentCollection { get; private set; }
        public static new TextElement Default
        {
            get
            {
                TextElement result = new TextElement(Element.Default);
                result.Text = "Simple Пример";
                result.FontSize = 16;
                result.FontFamily = "Times New Roman";
                return result;
            }
        }

        public string CurrentTextBoxColor
        {
            get => this.Visible == Visibility.Visible ? "White" : "Transparent";
        }

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

        private long fontSize;
        public long FontSize
        {
            get => this.fontSize;
            set
            {
                this.fontSize = value;
                this.OnPropertyChanged("FontSize");
            }
        }

        private string fontFamily;
        public string FontFamily
        {
            get => this.fontFamily;
            set
            {
                this.fontFamily = value;
                this.OnPropertyChanged("FontFamily");
            }
        }

        private string textAlignment;
        public string TextAlignment
        {
            get => this.textAlignment;
            set
            {
                this.textAlignment = value;
                this.OnPropertyChanged("");
            }
        }

        private bool isBold;
        public bool IsBold
        {
            get => this.isBold;
            set
            {
                this.isBold = value;
                this.OnPropertyChanged("IsBold");
                this.OnPropertyChanged("Fontweight");
            }
        }

        private bool isItalic;
        public bool IsItalic
        {
            get => this.isItalic;
            set
            {
                this.isItalic = value;
                this.OnPropertyChanged("IsItalic");
                this.OnPropertyChanged("FontStyle");
            }
        }

        public string FontWeight
        {
            get
            {
                return this.IsBold ? "Bold" : "Normal";
            }
        }

        public string FontStyle
        {
            get
            {
                return this.IsItalic ? "Italic" : "Normal";
            }
        }

        public TextElement() : base()
        {
            this.ZIndex = 2;
            this.IsBold = false;
            this.IsItalic = false;
            this.MinWidth = DefaultMinWidth;
            this.MinHeight = DefaultMinHeight;
            this.TextAlignment = TextAlignmentCollection.First;
        }

        static TextElement()
        {
            FontFamilyCollection = new ListExt<string>();
            InstalledFontCollection InstalledFonts = new InstalledFontCollection();
            foreach(FontFamily fontFamily in InstalledFonts.Families)
            {
                FontFamilyCollection.Add(fontFamily.Name);
            }

            TextAlignmentCollection = new ListExt<string> { "Left", "Center", "Right" };
    }

        public TextElement(Element element) : this()
        {
            this.Position = element.Position;
            this.Width = element.Width;
            this.Height = element.Height;
        }

        public override void SetVisible(Visibility visibility)
        {
            base.SetVisible(visibility);
            this.OnPropertyChanged("CurrentTextBoxColor");
        }

        public override string ToString()
        {
            string result = base.ToString();
            result += "\nText: " + this.Text;
            result += "\nFontSize: " + this.FontSize;
            result += "\nFontFamily: " + this.FontFamily;
            result += "\nTextAlignment: " + this.TextAlignment;
            result += "\nIsBold: " + this.IsBold;
            result += "\nIsItalic: " + this.IsItalic;
            return result;
        }
    }
}
