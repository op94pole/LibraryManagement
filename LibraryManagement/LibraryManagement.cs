using Model;
using BusinessLogic;

namespace LibraryManager
{
    public class LibraryManagement
    {
        //InterfaceManager manager = new();
        Logic logic = new();
        XMLDataAccessLayer xmlDAL = new();
        
        List<User> users = new();
        List<Book> books = new();
        List<Reservation> reservations = new();

        User currentUser = new();

        public LibraryManagement()
        {
            users = xmlDAL.Deserialize<List<User>>("Users");
            books = xmlDAL.Deserialize<List<Book>>("Books");
            reservations = xmlDAL.Deserialize<List<Reservation>>("Reservations");
        }

        public void LibraryManagementLoad()
        {
            Console.WriteLine("Ti diamo il benvenuto in LIBRARY MANAGMENT CONSOLE APP");
            Console.WriteLine();
            Console.WriteLine("Premi un tasto qualsiasi per procedere con il login...");
            Console.ReadKey();
            Console.Clear();

            Login();
        }

        public void Login()
        {
            bool success = default;

            do
            {
                Console.Clear();
                Console.WriteLine("Login.................");
                Console.WriteLine();
                Console.Write("Inserisci lo username: ");
                var username = Console.ReadLine();
                Console.Write("Inserisci la password: ");
                var password = Console.ReadLine();

                if (CredentialsCheck(username, password))
                    success = true;
                else
                    success = false;
            } while (!success);
        }

        public bool CredentialsCheck(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.Clear();
                Console.Write("Input non valido! ");

                if (!Retry())
                {
                    LibraryManagementLoad();
                }

                return false;
            }
            else
            {
                if (logic.ValidateLogin(username, password, out currentUser))
                {
                    if (currentUser.Role == User.UserRole.Admin)
                    {
                        Console.Clear();
                        Console.WriteLine($"Benvenuto {currentUser.Username}! ({currentUser.Role})");
                        Console.WriteLine();
                        Console.WriteLine("Premi un tasto qualsiasi per continuare.");
                        Console.ReadKey();

                        GetAdminMenu();
                    }
                    if (currentUser.Role == User.UserRole.User)
                    {
                        Console.Clear();
                        Console.WriteLine($"Benvenuto {currentUser.Username}! ({currentUser.Role})");
                        Console.WriteLine();
                        Console.WriteLine("Premi un tasto qualsiasi per continuare.");
                        Console.ReadKey();

                        GetUserMenu();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.Write("Username o password errati! ");

                    if (Retry())
                        Login();
                    else
                        LibraryManagementLoad();
                }

                return true;
            }
        }

        public bool Retry() //
        {
            Console.WriteLine("Vuoi riprovare? y/n");
            var input = Console.ReadLine();

            switch (input)
            {
                case "y":
                    Console.Clear();
                    return true;

                default:
                    Console.Clear();
                    return false;
            }
        }

        public void GetAdminMenu() //
        {
            Console.Clear();
            Console.WriteLine("1. Ricerca un libro");
            Console.WriteLine("2. Modifica un libro");
            Console.WriteLine("3. Inserisci un nuovo libro");
            Console.WriteLine("4. Rimuovi un libro");
            Console.WriteLine("5. Richiedi un prestito");
            Console.WriteLine("6. Restituisci un libro");
            Console.WriteLine("7. Visualizza lo storico delle prenotazioni");
            Console.WriteLine("8. Esci");
            Console.WriteLine();

            Console.WriteLine("Effettua una scelta e premi Invio.");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SearchBook();
                    break;

                case "2":
                    ModifyBook();
                    break;

                case "3":
                    InsertBook();
                    break;

                case "4":
                    DeleteBook();
                    break; ;

                case "5":
                    CreateReservation();
                    break;

                case "6":
                    break;
                    ReturnBook();

                case "7":
                    break;
                    ShowReservations();

                case "8":
                    logic.Exit();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Scelta non valida!.................");
                    Console.WriteLine();
                    Console.WriteLine("Premi un tasto qualsiasi e riprova.");
                    Console.ReadKey();
                    break;
            }

            GetAdminMenu();
        }

        public void GetUserMenu() //
        {
            Console.Clear();
            Console.WriteLine("1. Ricerca un libro");
            Console.WriteLine("2. Chiedi un prestito");
            Console.WriteLine("3. Restituisci un libro");
            Console.WriteLine("4. Visualizza lo storico delle prenotazioni");
            Console.WriteLine("5. Esci");

            Console.WriteLine("Effettua una scelta e premi Invio.");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SearchBook();
                    break;

                case "2":
                    CreateReservation();
                    break;

                case "3":
                    ReturnBook();
                    break;

                case "4":
                    ShowReservations();
                    break;

                case "5":
                    logic.Exit();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Scelta non valida!.................");
                    Console.WriteLine();
                    Console.WriteLine("Premi un tasto qualsiasi e riprova.");
                    Console.ReadKey();
                    break;
            }

            GetUserMenu();
        }

        public void SearchBook()
        {
            var choice = "";

            do
            {
                Console.Clear();
                Console.WriteLine("1. Ricerca un libro");
                Console.WriteLine();
                Console.Write("Cerca: ");
                var search = Console.ReadLine();

                try
                {
                    logic.GetBooks(search);
                }
                catch
                {
                    Console.WriteLine("Nessuna corrispondenza trovata!");
                }

                Console.WriteLine();
                Console.WriteLine("Vuoi fare una nuova ricerca? y/n ");
                choice = Console.ReadLine();
            } while (choice == "y");
        }

        public void ModifyBook()
        {
            var title = "";
            var authorName = "";
            var authorSurname = "";
            var publisher = "";
            bool success = default;

            do
            {
                int counter = 0;

                Console.Clear();
                Console.WriteLine("2. Modifica un libro");
                Console.WriteLine();

                foreach (Book currentBook in books)
                {
                    counter++;
                    Console.WriteLine($"{counter}. Titolo: {currentBook.Title}, Autore: {currentBook.AuthorName} " +
                            $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}");
                }

                Console.WriteLine();
                Console.WriteLine("Seleziona un libro da modificare e premi Invio.");
                Int32.TryParse(Console.ReadLine(), out int bookChoice);

                if (bookChoice > counter || bookChoice == 0)
                {
                    Console.WriteLine("Input non valido!..............................");
                    Console.WriteLine();

                    if (!Retry())
                        GetAdminMenu();

                    continue;
                }
                else
                {
                    Console.Clear();
                    Console.Write("Inserisci il titolo: ");
                    title = Console.ReadLine();
                    Console.Write("Inserisci il nome dell'autore: ");
                    authorName = Console.ReadLine();
                    Console.Write("Inserisci il cognome dell'autore: ");
                    authorSurname = Console.ReadLine();
                    Console.Write("Inserisci la casa editrice: ");
                    publisher = Console.ReadLine();

                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(authorName) || string.IsNullOrEmpty(authorSurname) ||
                        string.IsNullOrEmpty(publisher)) 
                    {
                        Console.WriteLine();
                        Console.Write("Input non valido!");

                        if (Retry())
                            success = false;
                        else
                            success = true;                        
                    }
                    else
                    {
                        try
                        {
                            logic.OverrideBook(bookChoice, title, authorName, authorSurname, publisher);

                            Console.WriteLine();
                            Console.WriteLine("Modifica apportata con successo!");
                            Console.WriteLine();
                            Console.WriteLine("Vuoi modificare un altro libro? y/n");
                            var choice = Console.ReadLine();

                            if (choice == "y")
                            {
                                success = false;
                                break;
                            }
                            else
                                success = true;

                            continue;
                        }
                        catch //
                        {
                            Console.WriteLine();
                            Console.WriteLine("Impossibile apportare la modifica! Libro già presente a sistema.");

                            if (Retry())
                                success = false;
                            else
                                success = true;
                        }
                    }
                }
            } while (!success);
        }

        public void InsertBook()
        {
            var title = "";
            var authorName = "";
            var authorSurname = "";
            var publisher = "";
            int quantity;
            bool success = default;

            do
            {
                Console.Clear();
                Console.WriteLine("2. Inserisci un nuovo libro");
                Console.WriteLine();
                Console.Write("Inserisci il titolo: ");
                title = Console.ReadLine();
                Console.Write("Inserisci il nome dell'autore: ");
                authorName = Console.ReadLine();
                Console.Write("Inserisci il cognome dell'autore: ");
                authorSurname = Console.ReadLine();
                Console.Write("Inserisci la casa editrice: ");
                publisher = Console.ReadLine();
                Console.Write("Inserisci la quantità: ");
                Int32.TryParse(Console.ReadLine(), out quantity);

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(authorName) || string.IsNullOrEmpty(authorSurname) ||
                    string.IsNullOrEmpty(publisher) || quantity == 0)
                {
                    Console.WriteLine();
                    Console.Write("I campi non sono stati valorizzati correttamente! ");

                    if (Retry())
                        success = false;
                    else
                    {
                        success = true;
                        break;
                    }
                }
                else
                {
                    logic.AddBook(title, authorName, authorSurname, publisher, quantity);

                    Console.WriteLine();
                    Console.WriteLine("Vuoi inserire un altro libro? y/n");
                    var choice = Console.ReadLine();

                    if (choice == "y")
                        success = false;
                    else
                        success = true;

                    continue;
                }
            } while (!success);
        }

        public void DeleteBook()
        {
            bool success = default;

            do
            {
                int counter = 0;

                Console.Clear();
                Console.WriteLine("2. Rimuovi un libro");
                Console.WriteLine();

                foreach (Book currentBook in books)
                {
                    counter++;
                    Console.WriteLine($"{counter}. Titolo: {currentBook.Title}, Autore: {currentBook.AuthorName} " +
                            $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}");
                }

                Console.WriteLine();
                Console.WriteLine("Seleziona un libro da eliminare e premi Invio.");
                Int32.TryParse(Console.ReadLine(), out int choice);

                if (choice == 0 || choice > books.Count)
                {
                    Console.Clear();
                    Console.WriteLine("Scelta non valida! ");

                    if (Retry())
                        success = false;
                    else
                        success = true;
                }
                else
                    try
                    {
                        logic.RemoveBook(choice);

                        Console.Clear();
                        Console.WriteLine("Libro rimosso correttamente dal sistema.");
                        Console.WriteLine();
                    }
                    catch 
                    {
                        counter = 0;

                        List<Reservation> bookReservations = reservations.Where(r => r.BookId == books[choice - 1].BookId && r.EndDate > DateTime.Now)
                            .ToList();

                        Console.WriteLine();
                        Console.WriteLine("Impossibile procedere con la rimozione! Il libro risulta associato ad una o più prenotazioni " +
                            "attive.");

                        foreach (Reservation currentReservation in bookReservations)
                        {
                            counter++;
                            User? associatedUser = users.Where(u => u.UserId == currentReservation.UserId).SingleOrDefault();

                            Console.WriteLine($"{counter}. [{associatedUser.UserId}] {associatedUser.Username}, {currentReservation.StartDate:dd/MM/yyyy}" +
                                $" - {currentReservation.EndDate:dd/MM/yyyy}.");
                        }

                        Console.WriteLine();
                    }
                    finally
                    {
                        Console.WriteLine("Vuoi rimuovere un altro libro? y/n");
                        var choice1 = Console.ReadLine();

                        if (choice1 == "y")
                            success = false;
                        else
                            success = true;
                    }
            } while (!success);
        }

        public void CreateReservation()
        {
            bool success = default;

            do
            {
                int counter = 0;

                Console.Clear();
                Console.WriteLine("5. Richiedi un prestito");
                Console.WriteLine();

                foreach (Book currentBook in books)
                {
                    counter++;
                    Console.WriteLine($"{counter}. Titolo: {currentBook.Title}, Autore: {currentBook.AuthorName} " +
                            $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}");
                }

                Console.WriteLine();
                Console.WriteLine("Seleziona il libro da prenotare e premi Invio.");
                Int32.TryParse(Console.ReadLine(), out int input);

                try
                {
                    logic.AddReservation(books, input, currentUser);

                    Console.Clear();
                    Console.WriteLine("Prenotazione effettuata con successo.");
                    Console.WriteLine();
                }
                catch
                {
                    Console.WriteLine("Impossibile proseguire con la prenotazione! Possiedi già questo libro in prestito.");
                    Console.WriteLine();
                }
                finally
                {
                    Console.WriteLine("Vuoi prenotare un altro libro? y/n");
                    var choice = Console.ReadLine();

                    if (choice == "y")
                        success = false;
                    else
                        success = true;
                }
            } while (!success);
        }

        public void ReturnBook()
        {

        }

        public void ShowReservations()
        {

        }

        public void CloseApplication()
        {
            logic.Exit();
        }
    }
}
