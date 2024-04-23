using BusinessLogic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement
{
    public class MenuHandling
    {
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
            User currentUser;
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
            User currentUser;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) 
            {
                Console.Clear();
                Console.Write("Input non valido! ");

                Retry();

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
                    {
                        Login();
                    }
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
            Console.WriteLine("4. Cancella un libro");
            Console.WriteLine("5. Chiedi un prestito");
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

                        businessLogic.BookSearch(search);

                        Console.WriteLine();
                        Console.WriteLine("Vuoi fare una nuova ricerca? y/n ");

                        input = Console.ReadLine();
                    } while (input == "y");
                    
                    break;

                case "2":
                    break;

                case "3":
                    string title;
                    string authorName;
                    string authorSurname;
                    string publisher;
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
                            string.IsNullOrEmpty(publisher) || string.IsNullOrEmpty(title))
                        {
                            Console.WriteLine();
                            Console.Write("I campi non sono stati valorizzati tutti correttamente! ");

                            Retry();
                        }
                        else
                        {
                            businessLogic.CreateBook(title, authorName, authorSurname, publisher, quantity);
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
                    break;

                case "5":
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
        }
    }
}
