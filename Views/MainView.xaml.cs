using CoreWPF.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PrintTemplate.Models;
using PrintTemplate.ViewModels;
using System.Windows.Documents;
using System.Printing;
using System.Windows.Markup;

namespace PrintTemplate.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainView : WindowExt
    {
        private PrintDialog Dialog;
        private double PercentScale;
        

        public MainView(string title, List<List<Element>> pages)
        {
            InitializeComponent();
            this.Dialog = new PrintDialog();
            
            MainViewModel vm = new MainViewModel(title, pages);
            vm.EventStartPrint += PrintCanvas;
            vm.EventSetScale += this.SetScale;
            vm.EventPrintDialog += this.PrintDialog;
            vm.EventStartPrintDrawingVisual += this.PrintDrawingVisual;
            vm.EventGetPageSize += this.GetPageSize;
            vm.EventMakeDocumentPage += this.MakeDocumentPage;
            vm.EventStartPrintPaginator += this.PrintCanvasPaginator;
            vm.EventPreparePrint += this.PreparePrint;
            vm.EventAfterPrint += this.DoAfterPrint;
            this.CurrentCanvas.Width = GetWidth();
            this.CurrentCanvas.Height = GetHeight();
            
            this.DataContext = vm;
        }

        private void SetScale(int percent = 0)
        {
            if(percent > 0) this.PercentScale = (double)percent / 100;
            this.CurrentCanvas.LayoutTransform = new ScaleTransform(this.PercentScale, this.PercentScale);
        }

        private double GetWidth()
        {
            return 8 * 96;
        }

        private Size GetPageSize()
        {
            return new Size(this.CurrentCanvas.ActualWidth, this.CurrentCanvas.ActualHeight);
        }

        private double GetHeight()
        {
            return 11.5 * 96;
        }

        private bool PrintDialog() => (bool)this.Dialog.ShowDialog();

        private void PreparePrint()
        {
            ParentForCanvas.Visibility = Visibility.Hidden;
            this.CurrentCanvas.LayoutTransform = new ScaleTransform(1, 1);
        }

        private void DoAfterPrint()
        {
            this.SetScale();
            
            ParentForCanvas.Visibility = Visibility.Visible;
        }
        private void PrintCanvas(string titlePrint)
        {
            //this.PreparePrint();
                
            int pageMargin = 0;
            Size pageSize = new Size(this.CurrentCanvas.ActualWidth, this.CurrentCanvas.ActualHeight);
            this.CurrentCanvas.Arrange(new Rect(pageMargin, pageMargin, pageSize.Width, pageSize.Height));

            //печать
            this.Dialog.PrintVisual(CurrentCanvas, titlePrint);

            //вернуть в изначальную позицию

            //this.DoAfterPrint();
            this.CurrentCanvas.Measure(pageSize);
        }

        private DocumentPage MakeDocumentPage()
        {
            this.PreparePrint();

            int pageMargin = 0;
            Size pageSize = new Size(this.CurrentCanvas.ActualWidth, this.CurrentCanvas.ActualHeight);
            this.CurrentCanvas.Arrange(new Rect(pageMargin, pageMargin, pageSize.Width, pageSize.Height));
            return new DocumentPage(this.CurrentCanvas);
        }

        private void PrintCanvasPaginator(DocumentPaginator paginator, string title)
        {
            Size pageSize = new Size(this.CurrentCanvas.ActualWidth, this.CurrentCanvas.ActualHeight);
            this.Dialog.PrintDocument(paginator, title);
            this.DoAfterPrint();
            this.CurrentCanvas.Measure(pageSize);
        }

        private void PrintDrawingVisual(string title, DrawingVisual visual)
        {
            this.PreparePrint();

            this.Dialog.PrintVisual(visual, title);

            this.DoAfterPrint();
        }

        public static FixedDocument GetFixedDocument(FrameworkElement toPrint, PrintDialog printDialog)
        {
            PrintCapabilities capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
            Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
            Size visibleSize = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);
            FixedDocument fixedDoc = new FixedDocument();

            // If the toPrint visual is not displayed on screen we neeed to measure and arrange it.
            toPrint.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            toPrint.Arrange(new Rect(new Point(0, 0), toPrint.DesiredSize));

            Size size = toPrint.DesiredSize;

            // Will assume for simplicity the control fits horizontally on the page.
            double yOffset = 0;
            while (yOffset < size.Height)
            {
                VisualBrush vb = new VisualBrush(toPrint);
                vb.Stretch = Stretch.None;
                vb.AlignmentX = AlignmentX.Left;
                vb.AlignmentY = AlignmentY.Top;
                vb.ViewboxUnits = BrushMappingMode.Absolute;
                vb.TileMode = TileMode.None;
                vb.Viewbox = new Rect(0, yOffset, visibleSize.Width, visibleSize.Height);

                PageContent pageContent = new PageContent();
                FixedPage page = new FixedPage();
                ((IAddChild)pageContent).AddChild(page);
                fixedDoc.Pages.Add(pageContent);
                page.Width = pageSize.Width;
                page.Height = pageSize.Height;

                Canvas canvas = new Canvas();
                FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
                FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
                canvas.Width = visibleSize.Width;
                canvas.Height = visibleSize.Height;
                canvas.Background = vb;
                page.Children.Add(canvas);

                yOffset += visibleSize.Height;
            }
            return fixedDoc;
        }
    }
}
