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
                    if (currentBook != newBook)
                        equal = false;
                    else
                        equal = true;
                }

                if (!equal)
                    xmlDAL.Serialize<Book>(newBook, "Books");
                else
                    throw new Exception("Libro già presente a sistema!");
            }
        }

        public void BookSearch(string search)
        {
            int counter = 0;
            string response = "Nessuna corrispondenza trovata!";
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

        public string AdminMenu() //
        {
            return "1. Ricerca un libro\r\n2. Modifica un libro\r\n3. Inserisci un nuovo libro\r\n4. Cancella un libro\r\n5. Chiedi un prestito\r\n" +
                "6. Restituisci un libro\r\n7. Visualizza lo storico delle prenotazioni\r\n8. Esci";
        }

        public string UserMenu() //
        {
            return "1. Ricerca un libro\r\n2. Chiedi un prestito\r\n3. Restituisci un libro\r\n4. Visualizza lo storico delle prenotazioni\r\n5. Esci";
        }


    }
}