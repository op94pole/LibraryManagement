using Model;

namespace BusinessLogic
{
    public class Logic
    {
        private XMLDataAccessLayer xmlDAL = new();
        private List<Book> books = new();
        private List<Reservation> reservations = new();

        public Logic()
        {
            books = xmlDAL.Deserialize<List<Book>>("Books");
            reservations = xmlDAL.Deserialize<List<Reservation>>("Reservations");
        }

        public bool DoLogin(string username, string password, out User? currentUser)
        {
            currentUser = xmlDAL.Deserialize<List<User>>("Users").Where(u => u.Username == username && u.Password == password).
                SingleOrDefault();

            if (currentUser != null)
                return true;
            else
                return false;
        }

        public List<Book> SearchBook(string search)
        {
            int counter = 0;
            string response = "";

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

            if (response == "")
                Console.WriteLine("Nessuna corrispondenza trovata");

            return books;

            // TODO: gestire la stampa della disponibilità => BusinessLogic/SearchBook
            // va mostrata anche la data in cui sarà possibile la prenotazione
        }

        public void ModifyBook(int choice, string title, string authorName, string authorSurname, string publisher)
        {
            bool alreadyExist = default;

            Book modifiedBook = new()
            {
                Title = title,
                AuthorName = authorName,
                AuthorSurname = authorSurname,
                Publisher = publisher
            };

            foreach (var book in books)
            {
                if (book.Title == modifiedBook.Title && book.AuthorName == modifiedBook.AuthorName && book.AuthorSurname ==
                    modifiedBook.AuthorSurname && book.Publisher == modifiedBook.Publisher)
                    alreadyExist = true;
            }

            if (alreadyExist) 
                throw new Exception("Impossibile apportare la modifica! Libro già presente a sistema.");
            else
            {
                books[choice - 1].Title = title;
                books[choice - 1].AuthorName = authorName;
                books[choice - 1].AuthorSurname = authorSurname;
                books[choice - 1].Publisher = publisher;

                xmlDAL.Serialize<List<Book>>(books, "Books");
            }
        }

        public void AddBook(string title, string authorName, string authorSurname, string publisher, int quantity)
        {
            bool alreadyExist = false;

            foreach (Book currentBook in books)
            {
                if (currentBook.Title.ToLowerInvariant() == title.ToLowerInvariant() &&
                    currentBook.AuthorName.ToLowerInvariant() == authorName.ToLowerInvariant() &&
                    currentBook.AuthorSurname.ToLowerInvariant() == authorSurname.ToLowerInvariant() &&
                    currentBook.Publisher.ToLowerInvariant() == publisher.ToLowerInvariant())
                {
                    currentBook.Quantity += quantity;
                    alreadyExist = true;

                    break;
                }
            }

            if (!alreadyExist)
            {
                var newBook = new Book();
                newBook.BookId = 1 + books[books.Count - 1].BookId;
                newBook.Title = title;
                newBook.AuthorName = authorName;
                newBook.AuthorSurname = authorSurname;
                newBook.Publisher = publisher;
                newBook.Quantity = quantity;

                books.Add(newBook);
            }

            xmlDAL.Serialize<List<Book>>(books, "Books");
        }

        public void DeleteBook(int choice)
        {
            books.Remove(books[choice - 1]);

            xmlDAL.Serialize<List<Book>>(books, "Books");

            // TODO: gestire la rimozione del libro in base alle prenotazioni attive => BusinessLogic/DeleteBook
            // è possibile rimuovere un libro dal sistema solo se non ci sono prenotazioni attive => messaggio di errore
            // per ogni prenotazione con indicate le informazioni dell'utente che ha attiva la prenotazione e fino a quando
            // con l'eliminazione del libro da sistema va cancellato pure lo storico delle sue prenotazioni
        }

        public void CreateReservation(List<Book> filtered, int choice, User currentUser) //
        {
            Book reservated = filtered[choice - 1]; 
            Reservation newReservation = new Reservation 
            {
                UserId = currentUser.UserId,  
                BookId = filtered[choice - 1].BookId 
            };

            reservations.Add(newReservation);

            xmlDAL.Serialize(reservations, "Reservations");

            // TODO: gestire la sovrapposizione di richieste
            // Un libro può essere prenotato solo se non sono attive prenotazioni e se la prenotazione corrente non va a
            // sovrapporsi ad una prenotazione successiva esistente => messaggio di errore
        }

        public void ReturnBook() //
        {
            // messaggio di errore se il libro non risulta prenotato
        }

        public List<Reservation> GetReservation() //
        {
            return reservations;
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}