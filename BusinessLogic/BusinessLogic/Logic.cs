using DataAccessLayer;
using Model;
using System.Reflection.Metadata.Ecma335;

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
            try
            {
                currentUser = users.Where(u => u.Username == username && u.Password == password).SingleOrDefault();

                if (currentUser != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw new Exception(@"User duplicate found at 'C:\Users\luca9\Documents\Visual Studio 2022\Coding\LibraryManagment\Model\samples\Database.xml'");
            }
        }

        public List<Book> GetBooks(string search, out string response)
        {
            int counter = 0;
            response = "";

            foreach (Book currentBook in books)
            {
                string currentBookResponse = "";

                if (currentBook.Title.ToLower().Contains(search) || currentBook.AuthorName.ToLower().Contains(search) ||
                    currentBook.AuthorSurname.ToLower().Contains(search) || currentBook.Publisher.ToLower().Contains(search))
                {
                    counter++;
                    currentBookResponse = $"{counter}. {currentBook.Title}, {currentBook.AuthorName} " +
                        $"{currentBook.AuthorSurname}, {currentBook.Publisher}, ";

                    int reservedCopies = reservations.Where(r => r.BookId == currentBook.BookId && r.EndDate > DateTime.Now).Count();
                    var firstAvailability = reservations.OrderByDescending(r => r.EndDate).Where(r => r.BookId == currentBook.BookId).FirstOrDefault();

                    if (reservedCopies == currentBook.Quantity)
                        currentBookResponse += $"disponibile a partire dal {firstAvailability.EndDate.AddDays(1).ToShortDateString()}\n";
                    else
                        currentBookResponse += "attualmente disponibile\n";

                    response += currentBookResponse;
                }
            }

            if (response == "")
                response = "Nessuna corrispondenza trovata!\n";

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

            foreach (var currentBook in books)
            {
                if (currentBook.Title.ToLowerInvariant() == modifiedBook.Title.ToLowerInvariant() &&
                    currentBook.AuthorName.ToLowerInvariant() == modifiedBook.AuthorName.ToLowerInvariant() &&
                    currentBook.AuthorSurname.ToLowerInvariant() == modifiedBook.AuthorSurname.ToLowerInvariant() &&
                    currentBook.Publisher.ToLowerInvariant() == modifiedBook.Publisher.ToLowerInvariant())
                    alreadyExist = true;
            }

            if (alreadyExist)
                throw new Exception(@"Book duplicate found at 'C:\Users\luca9\Documents\Visual Studio 2022\Coding\LibraryManagment\Model\samples\Database.xml'");

            books[choice - 1].Title = title;
            books[choice - 1].AuthorName = authorName;
            books[choice - 1].AuthorSurname = authorSurname;
            books[choice - 1].Publisher = publisher;

            xmlDAL.Serialize<List<Book>>(books, "Books");
        }

        public void AddBook(string title, string authorName, string authorSurname, string publisher, int quantity)
        {
            bool alreadyExist = default;

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
                Book newBook = new()
                {
                    BookId = (books.Count == 0 ? 1 : (books[books.Count - 1].BookId + 1)),
                    Title = title,
                    AuthorName = authorName,
                    AuthorSurname = authorSurname,
                    Publisher = publisher,
                    Quantity = quantity
                };

                books.Add(newBook);
            }

            xmlDAL.Serialize<List<Book>>(books, "Books");
        }

        public bool RemoveBook(int choice, out string error)
        {
            error = "";

            Reservation? existingReservation = reservations.Where(r => r.BookId == books[choice - 1].BookId && r.EndDate > DateTime.Now)
                .FirstOrDefault();

            if (existingReservation == null)
            {
                foreach (Reservation currentReservation in reservations.Where(r => r.BookId == books[choice - 1].BookId).ToList())
                    reservations.Remove(currentReservation);

                xmlDAL.Serialize<List<Reservation>>(reservations, "Reservations");

                books.Remove(books[choice - 1]);
                xmlDAL.Serialize<List<Book>>(books, "Books");

                return true;
            }
            else
            {
                error = "Impossibile procedere con la rimozione! Il libro risulta associato ad una o più prenotazioni attive.";

                return false;
            }
        }

        public void AddReservation(int choice, User currentUser, out string response)
        {
            response = "";

            Reservation newReservation = new Reservation()
            {
                ReservationId = (reservations.Count == 0 ? 1 : (reservations[reservations.Count - 1].ReservationId + 1)),
                UserId = currentUser.UserId,
                BookId = books[choice - 1].BookId
            };

            List<Reservation> currentBookReservations = reservations.Where(r => r.BookId == newReservation.BookId &&
            r.EndDate > DateTime.Now).ToList();
            Reservation? currentUserBookReservation = currentBookReservations.Where(r => r.UserId == currentUser.UserId).
                OrderByDescending(r => r.EndDate).SingleOrDefault();

            if (currentUserBookReservation != null)
                response = "Impossibile proseguire con la prenotazione! Possiedi già questo libro in prestito.";
            if (currentBookReservations.Count >= books[choice - 1].Quantity)
                response = "Impossibile proseguire con la prenotazione! Tutte le copie di questo libro risultano in prestito.";
            else
            {
                response = "Prenotazione effettuata con successo.";

                reservations.Add(newReservation);
                xmlDAL.Serialize(reservations, "Reservations");
            }
        }

        public bool RemoveReservation(User currentUser, Book bookToReturn)
        {
            Reservation? currentReservation = reservations.Where(r => r.BookId == bookToReturn.BookId && r.UserId == currentUser.UserId
            && r.EndDate > DateTime.Now).SingleOrDefault();

            if (currentReservation != null)
            {
                currentReservation.EndDate = DateTime.Now.Date;
                xmlDAL.Serialize(reservations, "Reservations");
                return true;
            }
            else
                return false;
        }

        public void GetReservations(User currentUser, string search, out string response)
        {
            User? user = new User();
            Book? book = new Book();

            int counter = 0;
            string state = "";
            string currentReservationResponse = "";
            response = "";

            if (currentUser.Role == User.UserRole.Admin)
            {
                foreach (Reservation currentReservation in reservations)
                {
                    user = users.Where(u => u.UserId == currentReservation.UserId).SingleOrDefault();
                    book = books.Where(b => b.BookId == currentReservation.BookId).SingleOrDefault();

                    if (user.Username.ToLower().Contains(search) || book.Title.ToLower().Contains(search) || book.AuthorName.ToLower().Contains(search)
                        || book.AuthorSurname.ToLower().Contains(search) || book.Publisher.ToLower().Contains(search))
                    {
                        if (currentReservation.EndDate > DateTime.Now)
                            state = "attiva";
                        if (currentReservation.EndDate < DateTime.Now)
                            state = "non attiva";

                        counter++;
                        currentReservationResponse = $"{counter}. {book.Title}, {user.Username}, {currentReservation.StartDate.ToShortDateString()} " +
                            $"- {currentReservation.EndDate.ToShortDateString()}, {state}\n";

                        response += currentReservationResponse;
                    }
                    else
                        response = "Nessuna corrispondenza trovata!\n";
                }
            }
            if (currentUser.Role == User.UserRole.User)
            {
                foreach (Reservation currentReservation in reservations)
                {
                    user = users.Where(u => u.UserId == currentReservation.UserId && u.UserId == currentUser.UserId).FirstOrDefault();
                    book = books.Where(b => b.BookId == currentReservation.BookId).FirstOrDefault();

                    if (user != null)
                    {
                        if (book.Title.ToLower().Contains(search) || book.AuthorName.ToLower().Contains(search)
                        || book.AuthorSurname.ToLower().Contains(search) || book.Publisher.ToLower().Contains(search))
                        {

                            if (currentReservation.EndDate > DateTime.Now)
                                state = "attiva";
                            if (currentReservation.EndDate < DateTime.Now)
                                state = "non attiva";

                            counter++;
                            currentReservationResponse = $"{counter}. {book.Title}, {user.Username}, {currentReservation.StartDate.ToShortDateString()} " +
                                $"- {currentReservation.EndDate.ToShortDateString()}, {state}\n";

                            response += currentReservationResponse;
                        }
                        else
                            response = "Nessuna corrispondenza trovata!\n";
                    }
                    else
                        continue;                    
                }
            }
        }

        public void Exit() => Environment.Exit(0);
    }
}