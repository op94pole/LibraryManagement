using BusinessLogic;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement
{
    public class MenuHandling
    {
        XMLDataAccessLayer xmlDAL = new();
        User currentUser = new();
        bool success1 = default;

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
            var businessLogic = new Logic();
            //User currentUser = new();
            bool success = default;
            string input;

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
            string input;
            var businessLogic = new Logic();
            //User currentUser;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Console.Clear();
                Console.Write("Input non valido! ");

                Retry(); //
                return false;
            }
            else
            {
                if (businessLogic.DoLogin(username, password, out currentUser))
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
                }
                return true;
            }
        }

        public bool Retry()
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
                    LibraryManagementLoad();
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
            var businessLogic = new Logic();
            string input;

            Console.WriteLine("Effettua una scelta e premi Invio.");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Ricerca un libro");
                        Console.WriteLine();
                        Console.Write("Cerca: ");
                        var search = Console.ReadLine();

                        businessLogic.SearchBook(search);

                        Console.WriteLine();
                        Console.WriteLine("Vuoi fare una nuova ricerca? y/n ");
                        input = Console.ReadLine();
                    } while (input == "y");

                    break;

                case "2":
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("2. Modifica un libro");
                        Console.WriteLine();

                        int counter = 0;
                        List<Book> list = xmlDAL.Deserialize<List<Book>>("Books");

                        foreach (Book currentBook in list)
                        {
                            counter++;
                            Console.WriteLine($"{counter}. Titolo: {currentBook.Title}, Autore: {currentBook.AuthorName} " +
                                    $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Seleziona un libro da modificare e premi Invio.");
                        Int32.TryParse(Console.ReadLine(), out int bookChoice);

                        Console.Clear();
                        Console.Write("Inserisci il titolo: ");
                        var title = Console.ReadLine();
                        Console.Write("Inserisci il nome dell'autore: ");
                        var authorName = Console.ReadLine();
                        Console.Write("Inserisci il cognome dell'autore: ");
                        var authorSurname = Console.ReadLine();
                        Console.Write("Inserisci la casa editrice: ");
                        var publisher = Console.ReadLine();

                        businessLogic.ModifyBook(bookChoice, title, authorName, authorSurname, publisher);

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
                    } while (!success1);                    
                    break;

                case "3":
                    string _title;
                    string _authorName;
                    string _authorSurname;
                    string _publisher;
                    int quantity = 0;
                    bool success = default;
                    bool parsed = default;

                    do
                    {
                        Console.Clear();
                        Console.WriteLine("2. Inserisci un nuovo libro");
                        Console.WriteLine();
                        Console.Write("Inserisci il titolo: ");
                        _title = Console.ReadLine();
                        Console.Write("Inserisci il nome dell'autore: ");
                        _authorName = Console.ReadLine();
                        Console.Write("Inserisci il cognome dell'autore: ");
                        _authorSurname = Console.ReadLine();
                        Console.Write("Inserisci la casa editrice: ");
                        _publisher = Console.ReadLine();

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

                        if (string.IsNullOrEmpty(_title) || string.IsNullOrEmpty(_authorName) || string.IsNullOrEmpty(_authorSurname) ||
                            string.IsNullOrEmpty(_publisher) || string.IsNullOrEmpty(_title))
                        {
                            Console.WriteLine();
                            Console.Write("I campi non sono stati valorizzati tutti correttamente! ");

                            Retry();
                        }
                        else
                        {
                            businessLogic.AddBook(_title, _authorName, _authorSurname, _publisher, quantity);
                            success = false;

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
                    break;

                case "4":
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("2. Rimuovi un libro");
                        Console.WriteLine();

                        int counter1 = 0;
                        List<Book> list1 = xmlDAL.Deserialize<List<Book>>("Books");

                        foreach (Book currentBook in list1)
                        {
                            counter1++;
                            Console.WriteLine($"{counter1}. Titolo: {currentBook.Title}, Autore: {currentBook.AuthorName} " +
                                    $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Seleziona un libro da eliminare e premi Invio.");
                        Int32.TryParse(Console.ReadLine(), out int choice1);

                        businessLogic.DeleteBook(choice1);

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
                    break; ;


                //Console.WriteLine("Premi un tasto qualsiasi per uscire.....");
                //Console.ReadKey();

                case "5":
                    Console.Clear();
                    Console.WriteLine("5. Richiedi un prestito");
                    Console.WriteLine();

                    int counter2 = 0;
                    List<Book> list2 = xmlDAL.Deserialize<List<Book>>("Books");

                    foreach (Book currentBook in list2)
                    {
                        counter2++;
                        Console.WriteLine($"{counter2}. Titolo: {currentBook.Title}, Autore: {currentBook.AuthorName} " +
                                $"{currentBook.AuthorSurname}, Casa editrice: {currentBook.Publisher}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Seleziona il libro da prenotare e premi Invio.");
                    Int32.TryParse(Console.ReadLine(), out int choice2);

                    businessLogic.CreateReservation(list2, choice2, currentUser);
                    break;

                case "6":
                    break;

                case "7":
                    break;

                case "8":
                    Environment.Exit(0);
                    break;
            }

            GetAdminMenu();
        }

        public void GetUserMenu()
        {
            Console.WriteLine("1. Ricerca un libro");
            Console.WriteLine("2. Chiedi un prestito");
            Console.WriteLine("3. Restituisci un libro");
            Console.WriteLine("4. Visualizza lo storico delle prenotazioni");
            Console.WriteLine("5. Esci");

            UserMenuChoice();
        }

        public void UserMenuChoice()
        {
            var businessLogic = new Logic();
            string input;

            Console.WriteLine("Effettua una scelta e premi Invio.");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("1. Ricerca un libro");
                        Console.WriteLine();
                        Console.Write("Cerca: ");
                        var search = Console.ReadLine();

                        businessLogic.SearchBook(search);

                        Console.WriteLine();
                        Console.WriteLine("Vuoi fare una nuova ricerca? y/n ");

                        input = Console.ReadLine();
                    } while (input == "y");

                    break;
            }
        }
    }
}
