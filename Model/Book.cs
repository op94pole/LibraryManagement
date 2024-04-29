using System.Xml.Serialization;

namespace Model
{
    public class Book
    {    
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
    }
}