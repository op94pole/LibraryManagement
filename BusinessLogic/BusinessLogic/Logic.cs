using Model;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
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
            modelBook.BooksList = xmlDAL.Deserialize<List<Book>>("Books");

            bool bookFound = false;

            foreach (Book currentBook in modelBook.BooksList)
            {
                if (currentBook.Title.ToLowerInvariant() == title.ToLowerInvariant() &&
                    currentBook.AuthorName.ToLowerInvariant() == authorName.ToLowerInvariant() &&
                    currentBook.AuthorSurname.ToLowerInvariant() == authorSurname.ToLowerInvariant() &&
                    currentBook.Publisher.ToLowerInvariant() == publisher.ToLowerInvariant())
                {
                    currentBook.Quantity += quantity;
                    bookFound = true;
                    break;
                }
            }

            if (!bookFound)
            {
                var newBook = new Book();
                newBook.Title = title;
                newBook.AuthorName = authorName;
                newBook.AuthorSurname = authorSurname;
                newBook.Publisher = publisher;
                newBook.Quantity = quantity;

                modelBook.BooksList.Add(newBook);
            }

            xmlDAL.Serialize<List<Book>>(modelBook.BooksList, "Books");
        }

        public void ModifyBook(int choiceNumber, string title, string authorName, string authorSurname, string publisher)
        {
            modelBook.BooksList = xmlDAL.Deserialize<List<Book>>("Books");

            modelBook.BooksList[choiceNumber - 1].Title = title;
            modelBook.BooksList[choiceNumber - 1].AuthorName = authorName;
            modelBook.BooksList[choiceNumber - 1].AuthorSurname = authorSurname;
            modelBook.BooksList[choiceNumber - 1].Publisher = publisher;

            xmlDAL.Serialize<List<Book>>(modelBook.BooksList, "Books");
        }

        public void DeleteBook(int choiceNumber)
        {

        }

        public List<Book> SearchBook(string search)
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

            return modelBook.BooksList;
        }
        
        public void CreateReservation() //
        {

        }

        public void ReturnBook() //
        {

        }

        public List<Reservation> GetReservation() //
        {
            modelReservation.ReservationsList = xmlDAL.Deserialize<List<Reservation>>("Reservations");
            return modelReservation.ReservationsList;
        }
    }
}