using DataAccessLayer;
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
            {
                return true;
            }
            else
            {
                return false;
            }
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
                    response = $"\n{counter}.\nTitolo: {currentBook.Title}\nAutore: {currentBook.AuthorName} " +
                        $"{currentBook.AuthorSurname}\nCasa editrice: {currentBook.Publisher}\n";

                    var reservedCopies = reservations.Where(r => r.BookId == currentBook.BookId && r.EndDate > DateTime.Now).Count();
                    var firstAvailability = reservations.OrderBy(r => r.EndDate).Where(r => r.BookId == currentBook.BookId).FirstOrDefault();

                    if (reservedCopies == currentBook.Quantity)
                    {
                        response += $"Info: Disponibile a partire da: {firstAvailability.EndDate.AddDays(1).ToShortDateString()}";
                    }
                    else
                    {
                        response += "Info: Attualmente disponibile";
                    }

                    Console.WriteLine(response);
                }
            }

            if (response == "")
            {
                throw new Exception("Nessuna corrispondenza trovata!");
            }

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
                {
                    alreadyExist = true;
                }
            }

            if (alreadyExist)
            {
                throw new Exception("Impossibile apportare la modifica! Libro già presente a sistema.");
            }
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
                newBook.BookId = (books.Count == 0 ? 1 : (books[books.Count - 1].BookId + 1));
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
                foreach (Reservation currentReservation in reservations.Where(r => r.BookId == books[choice - 1].BookId).ToList())
                {
                    reservations.Remove(currentReservation);
                }

                xmlDAL.Serialize<List<Reservation>>(reservations, "Reservations");

                books.Remove(books[choice - 1]);
                xmlDAL.Serialize<List<Book>>(books, "Books");
            }
            else
            {
                throw new Exception("Impossibile procedere con la rimozione! Il libro risulta associato ad una prenotazione attiva.");
            }
        }

        public void AddReservation(int choice, User currentUser)
        {
            Reservation newReservation = new Reservation
            {
                ReservationId = (reservations.Count == 0 ? 1 : (reservations[reservations.Count - 1].ReservationId + 1)),
                UserId = currentUser.UserId,
                BookId = books[choice - 1].BookId
            };

            List<Reservation> currentBookReservations = reservations.Where(r => r.BookId == newReservation.BookId &&
            r.EndDate > DateTime.Now).ToList();
            Reservation? currentUserBookReservation = currentBookReservations.Where(r => r.UserId == currentUser.UserId).
                OrderByDescending(r => r.EndDate).SingleOrDefault();

            if (currentBookReservations.Count >= books[choice - 1].Quantity)
            {
                throw new Exception("Impossibile proseguire con la prenotazione! Tutte le copie di questo libro risultano in prestito.");
            }
            if (currentUserBookReservation != null)
            {
                throw new Exception("Impossibile proseguire con la prenotazione! Possiedi già questo libro in prestito.");
            }
            else
            {
                reservations.Add(newReservation);
                xmlDAL.Serialize(reservations, "Reservations");
            }
        }

        public bool RemoveReservation(User currentUser, Book bookToReturn)
        {
            Reservation? currentReservation = reservations.Where(r => r.BookId == bookToReturn.BookId && r.UserId == currentUser.UserId 
            && r.EndDate > DateTime.Now).SingleOrDefault();           

            if (currentReservation == null)
            { 
                return false; 
            }
            else
            {
                currentReservation.EndDate = DateTime.Now.Date;
                xmlDAL.Serialize(reservations, "Reservations");
                return true;
            }
        }

        public void GetReservations(User currentUser, string search) 
        {
            User? user = new User();
            Book? book = new Book();
            string state = "";

            if (currentUser.Role == User.UserRole.Admin)
            {
                foreach (Reservation currentReservation in reservations)
                {
                    user = users.Where(u => u.UserId == currentReservation.UserId).FirstOrDefault();
                    book = books.Where(b => b.BookId == currentReservation.BookId).FirstOrDefault();

                    if (user.Username.ToLower().Contains(search) || book.Title.ToLower().Contains(search) || book.AuthorName.ToLower().Contains(search)
                        || book.AuthorSurname.ToLower().Contains(search) || book.Publisher.ToLower().Contains(search))
                    {
                        if (currentReservation.EndDate > DateTime.Now)
                        {
                            state = "attiva";
                        }
                        if (currentReservation.EndDate < DateTime.Now)
                        {
                            state = "non attiva";
                        }

                        Console.WriteLine($"Titolo: {book.Title}\nUtente: {user.Username}\nData inizio: {currentReservation.StartDate.ToShortDateString()}\n" +
                            $"Data fine: {currentReservation.EndDate.ToShortDateString()}\nInfo sulla prenotazione: {state}");
                        Console.WriteLine();
                    }
                    else
                    {
                        throw new Exception("Nessuna corrispondenza trovata!");
                    }
                }
            }
            if (currentUser.Role == User.UserRole.User)
            {
                foreach (Reservation currentReservation in reservations)
                {
                    user = users.Where(u => u.UserId == currentReservation.UserId && u.UserId == currentUser.UserId).FirstOrDefault();
                    book = books.Where(b => b.BookId == currentReservation.BookId).FirstOrDefault();

                    if (book.Title.ToLower().Contains(search) || book.AuthorName.ToLower().Contains(search)
                        || book.AuthorSurname.ToLower().Contains(search) || book.Publisher.ToLower().Contains(search))
                    {
                        if (user == null)
                        {
                            continue;
                        }
                        if (currentReservation.EndDate > DateTime.Now)
                        {
                            state = "attiva";
                        }
                        if (currentReservation.EndDate < DateTime.Now)
                        {
                            state = "non attiva";
                        }

                        Console.WriteLine($"Titolo: {book.Title}\nUtente: {user.Username}\nData inizio: {currentReservation.StartDate.ToShortDateString()}\n" +
                            $"Data fine: {currentReservation.EndDate.ToShortDateString()}\nInfo sulla prenotazione: {state}");
                        Console.WriteLine();
                    }
                    else
                    {
                        throw new Exception("Nessuna corrispondenza trovata!");
                    }
                }
            }
        }

        public void Exit()
        {
            Environment.Exit(0);
        }
    }
}