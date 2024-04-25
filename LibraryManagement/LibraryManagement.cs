using BusinessLogic;
using Model;

namespace LibraryManagement
{
    public class LibraryManagement
    {
        XMLDataAccessLayer xmlDAL = new();
        Logic BL = new();
        User currentUser = new();
        List<Book> books = new();
        List<Reservation> reservations = new();
        string input;
        string choice;
        bool success;
        bool success1 = default;
        int counter = 0;

        public LibraryManagement()
        {
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
                if (BL.DoLogin(username, password, out currentUser))
                {
                    if (currentUser.Role == User.UserRole.Admin)
                    {
                        Console.Clear();
                        Console.WriteLine($"Benvenuto {currentUser.Username}! ({currentUser.Role})");
                        Console.WriteLine();

                        GetAdminMenu();
                    }
                    if (currentUser.Role == User.UserRole.User)
                    {
                        Console.Clear();
                        Console.WriteLine($"Benvenuto {currentUser.Username}! ({currentUser.Role})");
                        Console.WriteLine();

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

        public bool Retry()
        {
            Console.WriteLine("Vuoi riprovare? y/n");
            input = Console.ReadLine();

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

        public void GetAdminMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Ricerca un libro");
            Console.WriteLine("2. Modifica un libro");
            Console.WriteLine("3. Inserisci un nuovo libro");
            Console.WriteLine("4. Elimina un libro");
            Console.WriteLine("5. Richiedi un prestito");
            Console.WriteLine("6. Restituisci un libro");
            Console.WriteLine("7. Visualizza lo storico delle prenotazioni");
            Console.WriteLine("8. Esci");
            Console.WriteLine();

            AdminMenuChoice();
        }

        public void AdminMenuChoice()
        {
            Console.WriteLine("Effettua una scelta e premi Invio.");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SearchBook();
                    break;

                case "2":
                    ModifyBook();
                    break;

                case "3":
                    CreatBook();
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
                    GetReservations();

                case "8":
                    BL.Exit();
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

        public void GetUserMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Ricerca un libro");
            Console.WriteLine("2. Chiedi un prestito");
            Console.WriteLine("3. Restituisci un libro");
            Console.WriteLine("4. Visualizza lo storico delle prenotazioni");
            Console.WriteLine("5. Esci");

            UserMenuChoice();
        }

        public void UserMenuChoice()
        {
            Console.WriteLine("Effettua una scelta e premi Invio.");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SearchBook();
                    break;

                case "2":
                    GetReservations();
                    break;

                case "3":
                    ReturnBook();
                    break;

                case "4":
                    GetReservations();
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
            do
            {
                Console.Clear();
                Console.WriteLine("1. Ricerca un libro");
                Console.WriteLine();
                Console.Write("Cerca: ");
                var search = Console.ReadLine();

                BL.SearchBook(search);

                Console.WriteLine();
                Console.WriteLine("Vuoi fare una nuova ricerca? y/n ");
                choice = Console.ReadLine();
            } while (choice == "y");
        }

        public void ModifyBook()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("2. Modifica un libro");
                Console.WriteLine();

                counter = 0;

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
                    Console.WriteLine("Scelta errata!..........................");
                    Console.WriteLine();

                    if (!Retry())
                        GetAdminMenu();

                    continue;
                }
                else
                {
                    Console.Clear();
                    Console.Write("Inserisci il titolo: ");
                    var _title = Console.ReadLine();
                    Console.Write("Inserisci il nome dell'autore: ");
                    var _authorName = Console.ReadLine();
                    Console.Write("Inserisci il cognome dell'autore: ");
                    var _authorSurname = Console.ReadLine();
                    Console.Write("Inserisci la casa editrice: ");
                    var _publisher = Console.ReadLine();

                    try
                    {
                        BL.ModifyBook(bookChoice, _title, _authorName, _authorSurname, _publisher);

                        Console.WriteLine();
                        Console.WriteLine("Modifica apportata con successo!");
                        Console.WriteLine();
                        Console.WriteLine("Vuoi modificare un altro libro? y/n");
                        choice = Console.ReadLine();

                        if (choice == "y")
                            success1 = false;
                        else
                            success1 = true;

                        continue;
                    }
                    catch (Exception e) 
                    {
                        Console.WriteLine();
                        Console.WriteLine("Impossibile apportare la modifica! Libro già presente a sistema.");

                        if (Retry())
                            success1 = false;
                        else
                            success1 = true;
                    }
                    
                }
            } while (!success1);
        }

        public void CreatBook()
        {
            string title;
            string authorName;
            string authorSurname;
            string publisher;
            int quantity;

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

                do
                {
                    Console.Write("Inserisci la quantità: ");
                    Int32.TryParse(Console.ReadLine(), out quantity);

                    if (quantity == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Input non valido! ");
                    }
                } while (quantity == 0);

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(authorName) || string.IsNullOrEmpty(authorSurname) ||
                    string.IsNullOrEmpty(publisher) || quantity == 0)
                {
                    Console.WriteLine();
                    Console.Write("I campi non sono stati valorizzati tutti correttamente! ");

                    Retry();
                }
                else
                {
                    BL.AddBook(title, authorName, authorSurname, publisher, quantity);

                    Console.WriteLine();
                    Console.WriteLine("Vuoi inserire un altro libro? y/n");
                    choice = Console.ReadLine();

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
            do
            {
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
                Int32.TryParse(Console.ReadLine(), out int choice1);

                BL.DeleteBook(choice1);

                Console.Clear();
                Console.WriteLine("Libro rimosso correttamente dal sistema.");
                Console.WriteLine();
                Console.WriteLine("Vuoi rimuovere un altro libro? y/n");
                choice = Console.ReadLine();

                if (choice == "y")
                    success = false;
                else
                    success = true;
            } while (!success);
        }

        public void CreateReservation()
        {
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

            BL.CreateReservation(books, input, currentUser);
        }

        public void ReturnBook()
        {

        }

        public void GetReservations()
        {

        }
    }
}
