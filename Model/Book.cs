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
                
        public int BookId { get; set; }       
        public string Title { get; set; }      
        public string AuthorName { get; set; }        
        public string AuthorSurname { get; set; }        
        public string Publisher { get; set; }       
        public int Quantity { get; set; }
    }
}