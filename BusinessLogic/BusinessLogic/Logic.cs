using Model;
using System.ComponentModel.Design;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace BusinessLogic
{
    public class Logic
    {
        private XMLDataAccessLayer xmlDAL = new();
        private Book modelBook = new();
        private Reservation modelReservation = new();

        public bool DoLogin(string username, string password, out User? currentUser)
        {
            currentUser = xmlDAL.Deserialize<List<User>>("Users").Where(u => u.Username == username && u.Password == password).
                SingleOrDefault();

            if (currentUser != null)
                return true;
            else
                return false;
        }

        public void AddBook(string title, string authorName, string authorSurname, string publisher, int quantity)
        {
            var newBook = new Book();
            newBook.Title = title;
            newBook.AuthorName = authorName;
            newBook.AuthorSurname = authorSurname;
            newBook.Publisher = publisher;
            newBook.Quantity = quantity;

            modelBook.BooksList = xmlDAL.Deserialize<List<Book>>("Books");

            if (modelBook.BooksList.Count > 0)
            {
                foreach (Book currentBook in modelBook.BooksList)
                {
                    if (currentBook.Title.ToLowerInvariant() == newBook.Title.ToLowerInvariant() &&
                            currentBook.AuthorName.ToLowerInvariant() == newBook.AuthorName.ToLowerInvariant() &&
                            currentBook.AuthorSurname.ToLowerInvariant() == newBook.AuthorSurname.ToLowerInvariant() &&
                            currentBook.Publisher.ToLowerInvariant() == newBook.Publisher.ToLowerInvariant())
                        newBook.Quantity += currentBook.Quantity;
                }

                modelBook.BooksList.Add(newBook);

                xmlDAL.Serialize<List<Book>>(modelBook.BooksList, "Books");
            }

            modelBook.BooksList.Add(newBook);

            xmlDAL.Serialize<List<Book>>(modelBook.BooksList, "Books");
        }

        public void BookSearch(string search)
        {
            int counter = 0;
            string response = ""; 

            modelBook.BooksList = xmlDAL.Deserialize<List<Book>>("Books");

            foreach (Book currentBook in modelBook.BooksList)
            {
                if (currentBook.Title.ToLower().Contains(search) || currentBook.AuthorName.ToLower().Contains(search) ||
                    currentBook.AuthorSurname.ToLower().Contains(search) || currentBook.Publisher.ToLower().Contains(search))
                {
                    counter++;
                    response = $"{counter}. Titolo: {currentBook.Title}, Autore: {currentBook.AuthorName} " +
                        $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}";

                    Console.WriteLine(response);
                }
            }

            if (response == "")
                Console.WriteLine("Nessuna corrispondenza trovata");
        }

        public List<Reservation> GetReservation()
        {
            modelReservation.ReservationsList = xmlDAL.Deserialize<List<Reservation>>("Reservations");
            return modelReservation.ReservationsList;
        }
    }
}