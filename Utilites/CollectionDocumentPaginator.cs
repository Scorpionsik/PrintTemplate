using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PrintTemplate.Utilites
{
    internal class CollectionDocumentPaginator : DocumentPaginator
    {
        // Реальный класс разбиения на страницы (выполняющий всю работу по разбиению)
        
        
        private List<DocumentPage> PageCollection;

        // Сохранить класс разбиения на страницы FlowDocument из заданного документа
        public CollectionDocumentPaginator(IEnumerable<DocumentPage> pageCollection)
        {
            //flowDocumentPaginator = ((IDocumentPaginatorSource)document).DocumentPaginator;
            FixedDocument doc = new FixedDocument();
            
            this.PageCollection = new List<DocumentPage>(pageCollection);
            this.pageCount = 0;
        }

        public override bool IsPageCountValid
        {
            get { return this.PageCount < this.PageCollection.Count; }
        }

        private int pageCount;
        public override int PageCount
        {
            get { return this.pageCount; }
        }

        private Size pageSize;
        public override Size PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            return this.PageCollection[pageNumber];
        }
    }
}
