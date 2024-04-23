﻿using System;
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

        [XmlAttribute]
        public int ReservationId { get; set; }
        //[XmlAttribute]
        //public int UserId { get; set; }
        //[XmlAttribute]
        //public int BookId { get; set; }
        [XmlAttribute]
        public User UserId { get; set; }
        [XmlAttribute]
        public Book BookId { get; set; }
        [XmlAttribute]
        public DateTime StartDate { get; set; }
        [XmlAttribute]
        public DateTime EndDate { get; set; }
        [XmlElement]
        public List<Reservation> ReservationsList { get; set;}
    }
}