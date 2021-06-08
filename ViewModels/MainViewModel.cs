using CoreWPF.MVVM;
using CoreWPF.Utilites;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PrintTemplate.Models;
using PrintTemplate.Views;
using System.Windows.Media;
using System.Windows.Documents;
using TextElement = PrintTemplate.Models.TextElement;
using PrintTemplate.Utilites;

namespace PrintTemplate.ViewModels
{
    class MainViewModel : ViewModel
    {
        private event Action eventPreparePrint;
        public event Action EventPreparePrint
        {
            add
            {
                this.eventPreparePrint -= value;
                this.eventPreparePrint += value;
            }
            remove => this.eventPreparePrint -= value;
        }

        private event Action eventAfterPrint;
        public event Action EventAfterPrint
        {
            add
            {
                this.eventAfterPrint -= value;
                this.eventAfterPrint += value;
            }
            remove => this.eventAfterPrint -= value;
        }

        private event Func<Size> eventGetPageSize;
        public event Func<Size> EventGetPageSize
        {
            add
            {
                this.eventGetPageSize -= value;
                this.eventGetPageSize += value;
            }
            remove => this.eventGetPageSize -= value;
        }

        private event Func<DocumentPage> eventMakeDocumentPage;
        public event Func<DocumentPage> EventMakeDocumentPage
        {
            add
            {
                this.eventMakeDocumentPage -= value;
                this.eventMakeDocumentPage += value;
            }
            remove => this.eventMakeDocumentPage -= value;
        }

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

        private event Func<bool> eventPrintDialog;
        public event Func<bool> EventPrintDialog
        {
            add
            {
                this.eventPrintDialog -= value;
                this.eventPrintDialog += value;
            }
            remove => this.eventPrintDialog -= value;
        }

        private event Action<string> eventStartPrint;
        public event Action<string> EventStartPrint
        {
            add
            {
                this.eventStartPrint -= value;
                this.eventStartPrint += value;
            }
            remove => this.eventStartPrint -= value;
        }
        private event Action<DocumentPaginator, string> eventStartPrintPaginator;
        public event Action<DocumentPaginator, string> EventStartPrintPaginator
        {
            add
            {
                this.eventStartPrintPaginator -= value;
                this.eventStartPrintPaginator += value;
            }
            remove => this.eventStartPrintPaginator -= value;
        }

        private event Action<string, DrawingVisual> eventStartPrintDrawingVisual;
        public event Action<string, DrawingVisual> EventStartPrintDrawingVisual
        {
            add
            {
                this.eventStartPrintDrawingVisual -= value;
                this.eventStartPrintDrawingVisual += value;
            }
            remove => this.eventStartPrintDrawingVisual -= value;
        }

        private event Action<int> eventSetScale;
        public event Action<int> EventSetScale
        {
            add
            {
                this.eventSetScale -= value;
                this.eventSetScale += value;
                this.eventSetScale?.Invoke(this.PercentScale);
            }
            remove => this.eventSetScale -= value;
        }

        private int percentScale;
        public int PercentScale
        {
            get => this.percentScale;
            set
            {
                this.percentScale = value;
                this.OnPropertyChanged("PercentScale");
                this.eventSetScale?.Invoke(value);
            }
        }

        private bool printButtonEnable;
        public bool PrintButtonEnable
        {
            get => this.printButtonEnable;
            private set
            {
                this.printButtonEnable = value;
                this.OnPropertyChanged("PrintButtonEnable");
            }
        }

        private bool editorVisibility;
        public bool EditorVisibility
        {
            get => this.editorVisibility;
            set
            {
                this.editorVisibility = value;
                this.OnPropertyChanged("EditorVisibility");
                this.OnPropertyChanged("CurrentColor");
                this.eventHideEditorsChanged?.Invoke(this.CurrentVisibility);
            }
        }

        private string titlePrint;
        public string TitlePrint
        {
            get => this.titlePrint;
            set
            {
                this.titlePrint = value;
                this.OnPropertyChanged("TitlePrint");
            }
        }

        private Visibility CurrentVisibility
        {
            get
            {
                return this.EditorVisibility ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public string CurrentColor
        {
            get => this.EditorVisibility ? "White" : "Beige";
        }

        private Element selectedElement;
        public Element SelectedElement
        {
            get => this.selectedElement;
            set
            {
                if (this.selectedElement != null) this.selectedElement.ZIndex--;
                this.selectedElement = value;
                this.selectedElement.ZIndex++;
            }
        }
        private ListExt<Element> elements;
        public ListExt<Element> Elements
        {
            get
            {
                //if(this.SelectedPage <= this.Pages.Count && this.SelectedPage > 0)
                //{
                this.elements = this.Pages[this.SelectedPage - 1];
                //}
                return this.elements;
            }
        }

        public ListExt<ListExt<Element>> Pages { get; private set; }
        private int selectedPage;
        public int SelectedPage
        {
            get => this.selectedPage;
            set
            {
                if (value <= this.Pages.Count && value > 0)
                {
                    this.selectedPage = value;
                    this.OnPropertyChanged("SelectedPage");
                    this.OnPropertyChanged("Elements");
                }
            }
        }
        public MainViewModel(string title, IEnumerable<IEnumerable<Element>> pages)
        {
            this.Title = title;
            this.TitlePrint = title;
            this.PrintButtonEnable = true;
            this.PercentScale = 85;
            //this.Elements = new ListExt<Element>(elements);
            this.Pages = new ListExt<ListExt<Element>>();
            this.SelectedPage = 0;
            foreach (IEnumerable<Element> page in pages)
            {
                this.Pages.Add(new ListExt<Element>());
                this.SelectedPage++;
                foreach (Element element in page)
                {
                    this.AddElement(element);
                }
            }
            this.SelectedPage = 1;

            //this.AddElement(TextElement.Default);
        }

        private void AddElement(Element element)
        {
            element.CommandRemoveElement = this.CommandRemoveElement;
            element.CommandSelectItem = this.CommandSelectItem;
            this.EventHideEditorsChanged += element.SetVisible;
            this.Elements.Add(element);
        }

        private DrawingVisual MakeDrawingVisual(IEnumerable<Element> elements)
        {
            DrawingVisual result = new DrawingVisual();

            using (DrawingContext dc = result.RenderOpen())
            {
                /*
                Size pageSize = (Size)this.eventGetPageSize?.Invoke();
                if (pageSize != Size.Empty) dc.DrawRectangle(Brushes.Transparent, null, new Rect(0, 0, pageSize.Width, pageSize.Height));
                */
                foreach (Element element in elements)
                {
                    if (element is TextElement textElement)
                    {
                        FormattedText text = new FormattedText(
                            textElement.Text,
                            System.Globalization.CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight,
                            new Typeface(textElement.FontFamily),
                            textElement.FontSize,
                            Brushes.Black
                            );
                        text.SetFontWeight(textElement.IsBold ? FontWeights.Bold : FontWeights.Normal);
                        text.SetFontStyle(textElement.IsItalic ? FontStyles.Italic : FontStyles.Normal);
                        text.TextAlignment = TextAlignment.Left;
                        switch (textElement.TextAlignment)
                        {
                            case "Center":
                                text.TextAlignment = TextAlignment.Center;
                                break;
                            case "Right":
                                text.TextAlignment = TextAlignment.Right;
                                break;
                        }
                        
                        dc.DrawText(text, new Point(textElement.Position.X, textElement.Position.Y + 45));
                    }
                    else if (element is BorderElement borderElement)
                    {
                        dc.DrawRectangle(null,
                            new Pen(Brushes.Black, borderElement.BorderThickness),
                            new Rect(borderElement.Position.X, borderElement.Position.Y + 45, borderElement.Width, borderElement.Height - 45));
                    }
                }
            }

            return result;
        }

        public RelayCommand CommandAddTextElement
        {
            get => new RelayCommand(obj =>
            {
                this.AddElement(TextElement.Default);
            });
        }

        public RelayCommand CommandAddBorderElement
        {
            get => new RelayCommand(obj =>
            {
                this.AddElement(BorderElement.Default);
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
                if (this.SelectedElement != element) this.SelectedElement = element;
            });
        }

        public RelayCommand<string> CommandStartPrint
        {
            get => new RelayCommand<string>(title =>
            {
                this.EditorVisibility = true;
                this.PrintButtonEnable = false;
                if ((bool)eventPrintDialog?.Invoke())
                {
                    /*
                    int currentPage = this.SelectedPage;
                    this.SelectedPage = 1;
                    List<DocumentPage> pages = new List<DocumentPage>();
                    foreach (ListExt<Element> page in this.Pages)
                    {
                        pages.Add(this.eventMakeDocumentPage?.Invoke());

                    }
                    CollectionDocumentPaginator paginator = new CollectionDocumentPaginator(pages);
                    //this.eventStartPrint?.Invoke(title);
                    // this.eventStartPrintDrawingVisual?.Invoke(title, this.MakeDrawingVisual(this.Elements));
                    this.eventStartPrintPaginator?.Invoke(paginator, this.TitlePrint);
                    */

                    this.eventPreparePrint?.Invoke();
                    int currentPage = this.SelectedPage;
                    this.SelectedPage = 1;

                    for(int i = 0; i < this.Pages.Count; i++)
                    {

                        this.eventStartPrint?.Invoke(title);
                        this.SelectedPage++;
                    }
                    this.eventAfterPrint?.Invoke();
                    this.SelectedPage = currentPage;
                }


                

                this.EditorVisibility = false;
                this.PrintButtonEnable = true;
            });
        }

        public RelayCommand<string> CommandTurnThePage
        {
            get => new RelayCommand<string>(param =>
            {
                int newPos = 0;
                switch (param)
                {
                    case "left":
                        newPos--;
                        break;
                    case "right":
                        newPos++;
                        break;
                }
                this.SelectedPage += newPos;
            });
        }

        public RelayCommand CommandAddPage
        {
            get => new RelayCommand(obj =>
            {
                this.Pages.Add(new ListExt<Element>());
            });
        }

        public RelayCommand CommandRemovePage
        {
            get => new RelayCommand(obj =>
            {
                if(App.SetMessageBox("Вы уверены?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.Pages.RemoveAt(this.SelectedPage - 1);
                    if (this.Pages.Count < this.SelectedPage) this.SelectedPage = this.Pages.Count;
                    else this.OnPropertyChanged("Elements");
                }
            },
                (obj) => this.Pages.Count > 1);
        }

        public RelayCommand CommandToStringElements
        {
            get => new RelayCommand(obj =>
            {
                string text = "";
                foreach(Element element in this.Elements)
                {
                    text += element.ToString() + "\n\n";
                }
                MessageTextBoxView window = new MessageTextBoxView(text);
                window.ShowDialog();
            });
        }

        
    }
}
