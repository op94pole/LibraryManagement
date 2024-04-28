using Model;

namespace BusinessLogic
{
    public class Logic
    {
        private XMLDataAccessLayer xmlDAL = new();
        private List<User> users = new();
        private List<Book> books = new();
        private List<Reservation> reservations = new();

        public Logic()
        {
            users = xmlDAL.Deserialize<List<User>>("Users");
            books = xmlDAL.Deserialize<List<Book>>("Books");
            reservations = xmlDAL.Deserialize<List<Reservation>>("Reservations");
        }

        public bool ValidateLogin(string username, string password, out User? currentUser)
        {
            currentUser = xmlDAL.Deserialize<List<User>>("Users").Where(u => u.Username == username && u.Password == password).
                SingleOrDefault();

            if (currentUser != null)
                return true;
            else
                return false;
        }

        public List<Book> GetBooks(string search)
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
                        $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}. ";

                    var reservedCopies = reservations.Where(r => r.BookId == currentBook.BookId && r.EndDate >
                    DateTime.Now).Count();
                    var firstAvailability = reservations.OrderBy(r => r.EndDate).Where(r => r.BookId ==
                    currentBook.BookId).FirstOrDefault();

                    if (reservedCopies == currentBook.Quantity)
                        response += $"Disponibile a partire da: {firstAvailability.EndDate.AddDays(1).ToShortDateString()}";
                    else
                        response += "Attualmente disponibile";

                    Console.WriteLine(response);
                }
            }

            if (response == "")
                throw new Exception("Nessuna corrispondenza trovata!");

            return books;
        }

        public void OverrideBook(int choice, string title, string authorName, string authorSurname, string publisher)
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

        public void RemoveBook(int choice)
        {
            Reservation? existingReservation = reservations.Where(r => r.BookId == books[choice - 1].BookId && r.EndDate > DateTime.Now)
                .FirstOrDefault();

            if (existingReservation == null)
            {
                books.Remove(books[choice - 1]);
                xmlDAL.Serialize<List<Book>>(books, "Books");

                reservations.Remove(existingReservation);
                xmlDAL.Serialize<List<Reservation>>(reservations, "Reservations");
            }
            else
                throw new Exception("Impossibile procedere con la rimozione! Il libro risulta associato ad una prenotazione attiva.");
        }

        public void AddReservation(List<Book> filtered, int choice, User currentUser) //
        {
            Reservation newReservation = new Reservation
            {
                ReservationId = 1 + reservations[reservations.Count - 1].ReservationId,
                UserId = currentUser.UserId,
                BookId = filtered[choice - 1].BookId
            };

            Reservation? alreadyExist = reservations.Where(r => r.BookId == newReservation.BookId)
                .SingleOrDefault();

            if (alreadyExist == null || alreadyExist.EndDate < DateTime.Now)
            {
                reservations.Add(newReservation);
                xmlDAL.Serialize(reservations, "Reservations");
            }
            else
                throw new Exception("Impossibile proseguire con la prenotazione! Possiedi già questo libro in prestito.");

            // TODO: gestire la sovrapposizione di richieste
            // Un libro può essere prenotato solo se non sono attive prenotazioni (reservations < quantity)
        }

        public void RemoveReservation() //
        {
            // messaggio di errore se il libro non risulta prenotato
        }

        public List<Reservation> GetReservations() //
        {
            return reservations;
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}