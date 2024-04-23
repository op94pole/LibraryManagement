using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model
{
    public class Reservation
    {
        private static int _reservationId = 1;

        public Reservation()
        {
            ReservationId = _reservationId++;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(30);
        }

        [XmlElement]
        public int ReservationId { get; set; }
        //[XmlAttribute]
        //public int UserId { get; set; }
        //[XmlAttribute]
        //public int BookId { get; set; }
        [XmlElement]
        public User UserId { get; set; }
        [XmlElement]
        public Book BookId { get; set; }
        [XmlElement]
        public DateTime StartDate { get; set; }
        [XmlElement]
        public DateTime EndDate { get; set; }
    }
}