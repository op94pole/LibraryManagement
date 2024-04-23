using Model;
using System.ComponentModel.Design;
using System.Reflection;

namespace BusinessLogic
{
    public class Logic
    {
        private XMLDataAccessLayer xmlDAL = new();

        public bool DoLogin(string username, string password, out User? currentUser)
        {
            currentUser = xmlDAL.Deserialize<List<User>>("Users").Where(u => u.Username == username && u.Password == password).
                SingleOrDefault();

            if (currentUser != null)
                return true;
            else
                return false;
        }

        public void CreateBook(string title, string authorName, string authorSurname, string publisher, int quantity)
        {
            Book newBook = new Book()
            {
                Title = title,
                AuthorName = authorName,
                AuthorSurname = authorSurname,
                Publisher = publisher,
                Quantity = quantity
            };

            List<Book> books = xmlDAL.Deserialize<List<Book>>("Books");

            bool equal = default;

            if (books.Count > 0)
            {
                foreach (Book currentBook in books)
                {
                    if (currentBook.Title.ToLowerInvariant() == newBook.Title.ToLowerInvariant() &&
                            currentBook.AuthorName.ToLowerInvariant() == newBook.AuthorName.ToLowerInvariant() &&
                        currentBook.AuthorSurname.ToLowerInvariant() == newBook.AuthorSurname.ToLowerInvariant() &&
                            currentBook.Publisher.ToLowerInvariant() == newBook.Publisher.ToLowerInvariant())
                    {
                        newBook.Quantity += currentBook.Quantity;

                        List<Book> booksList = xmlDAL.Deserialize<List<Book>>("Books");

                        booksList.Add(newBook);

                        xmlDAL.Serialize(booksList, "Books");
                    }

                    //if (currentBook != newBook)
                    //    equal = false;
                    //else
                    //    equal = true;
                }

                xmlDAL.Serialize<Book>(newBook, "Books");
            }
        }

        public void BookSearch(string search)
        {
            int counter = 0;
            string response = "Nessuna corrispondenza trovata!"; //
            List<Book> books = xmlDAL.Deserialize<List<Book>>("Books");

            foreach (Book currentBook in books)
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
        }

        public List<Reservation> GetReservation()
        {
            return xmlDAL.Deserialize<List<Reservation>>("Reservations");
        }
    }
}