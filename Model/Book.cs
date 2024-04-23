using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model
{
    public class Book
    {
        private static int _bookId = 1;

        public Book()
        {
            BookId = _bookId++;
        }

        [XmlAttribute]
        public int BookId { get; set; }
        [XmlAttribute]
        public string Title { get; set; }
        [XmlAttribute]
        public string AuthorName { get; set; }
        [XmlAttribute]
        public string AuthorSurname { get; set; }
        [XmlAttribute]
        public string Publisher { get; set; }
        [XmlAttribute]
        public int Quantity { get; set; }
        [XmlElement("Book")]
        public List<Book> BooksList { get; set; }
    }
}