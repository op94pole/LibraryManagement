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
            List<Book> attuale = modelBook.BooksList;

            bool existing = default;

            Book modifiedBook = new()
            {
                Title = title,
                AuthorName = authorName,
                AuthorSurname = authorSurname,
                Publisher = publisher
            };

            foreach (var book in attuale)
            {
                if (book.Title == modifiedBook.Title && book.AuthorName == modifiedBook.AuthorName && book.AuthorSurname ==
                    modifiedBook.AuthorSurname && book.Publisher == modifiedBook.Publisher)
                    existing = true;
            }

            if (existing) // throw new exception
            {
                Console.WriteLine();
                Console.WriteLine("Impossibile apportare la modifica! Libro già presente a sistema.");
                Console.WriteLine();
                Console.WriteLine("Premi un tasto qualsiasi per proseguire.........................");
                Console.ReadKey();
            }
            else
            {
                modelBook.BooksList[choiceNumber - 1].Title = title;
                modelBook.BooksList[choiceNumber - 1].AuthorName = authorName;
                modelBook.BooksList[choiceNumber - 1].AuthorSurname = authorSurname;
                modelBook.BooksList[choiceNumber - 1].Publisher = publisher;

                xmlDAL.Serialize<List<Book>>(modelBook.BooksList, "Books");
            }
        }

        public void DeleteBook(int choiceNumber)
        {
            modelBook.BooksList = xmlDAL.Deserialize<List<Book>>("Books");
            List<Book> currentList = modelBook.BooksList;
            currentList.Remove(currentList[choiceNumber - 1]);

            xmlDAL.Serialize<List<Book>>(currentList, "Books");

            // TODO: gestire la rimozione del libro in base alle prenotazioni attive => BusinessLogic/DeleteBook
            // è possibile rimuovere un libro dal sistema solo se non ci sono prenotazioni attive => messaggio di errore
            // per ogni prenotazione con indicate le informazioni dell'utente che ha attiva la prenotazione e fino a quando
            // con l'eliminazione del libro da sistema va cancellato pure lo storico delle sue prenotazioni
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

            // TODO: gestire la stampa della disponibilità => BusinessLogic/SearchBook
            // va mostrata anche la data in cui sarà possibile la prenotazione
        }

        public void CreateReservation(List<Book> filtered, int choice, User currentUser) //
        {
            Book reservated = filtered[choice - 1]; // libro da prenotare
            Reservation newReservation = new Reservation // prenotazione da inserire
            {
                UserId = currentUser.UserId, // 
                BookId = filtered[choice - 1].BookId // 
            };

            newReservation.ReservationsList = xmlDAL.Deserialize<List<Reservation>>("Reservations");
            newReservation.ReservationsList.Add(newReservation);

            xmlDAL.Serialize(newReservation.ReservationsList, "Reservations");

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
            modelReservation.ReservationsList = xmlDAL.Deserialize<List<Reservation>>("Reservations");
            return modelReservation.ReservationsList;
        }
    }
}