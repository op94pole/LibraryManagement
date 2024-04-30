using System.Xml.Serialization;

namespace Model
{
    public class Reservation
    {
        public Reservation()
        {
            StartDate = DateTime.Now.Date;
            EndDate = StartDate.AddDays(30).Date;
        }

        [XmlAttribute]
        public int ReservationId { get; set; }
        [XmlAttribute]
        public int UserId { get; set; }
        [XmlAttribute]
        public int BookId { get; set; }
        //[XmlAttribute]
        //public User UserId { get; set; }
        //[XmlAttribute]
        //public Book BookId { get; set; }
        [XmlAttribute]
        public DateTime StartDate { get; set; }
        [XmlAttribute]
        public DateTime EndDate { get; set; }        
    }
}