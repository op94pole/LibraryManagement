using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model
{
    public class Reservation
    {
        public Reservation(User currentUser, Book currentBook)
        {
            ReservationId = _reservationId++;            
            StartDate = DateTime.Now;
        }

        private static int _reservationId = 1;
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}