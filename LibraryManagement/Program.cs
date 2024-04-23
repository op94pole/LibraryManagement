using BusinessLogic;
using System.Xml.Serialization;
using Model;
using System.Net.NetworkInformation;

namespace LibraryManagement
{
    internal class Program
    {
        static void Main()
        {
            var xmlDAL = new XMLDataAccessLayer();
            var businessLogic = new Logic();

            var currentUser = new User();
            bool success = default;
            string input;

            Console.WriteLine("Ti diamo il benvenuto in LIBRARY MANAGMENT APP CONSOLE");
            Console.WriteLine();
            Console.WriteLine("Premi un tasto qualsiasi per procedere con il login...");
            Console.ReadKey();
            Console.Clear();

            do
            {
                Console.Clear();
                Console.Write("Inserisci lo username: ");
                var username = Console.ReadLine();
                Console.Write("Inserisci la password: ");
                var password = Console.ReadLine();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Input non valido. Vuoi riprovare? y/n");
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "y":
                            continue;

                        default:
                            Environment.Exit(0);
                            break;
                    }

                    break;
                }
                else
                {
                    if (businessLogic.DoLogin(username, password, out currentUser))
                    {
                        if (currentUser.Role == User.UserRole.Admin)
                        {
                        //AdminMenu:
                            Console.Clear();
                            Console.WriteLine($"Benvenuto {currentUser.Username}! ({currentUser.Role})");
                            Console.WriteLine();
                            Console.WriteLine(businessLogic.AdminMenu());

                            //
                        }
                        if (currentUser.Role == User.UserRole.User)
                        {
                            Console.Clear();
                            Console.WriteLine($"Benvenuto {currentUser.Username}! ({currentUser.Role})");
                            Console.WriteLine();
                            Console.WriteLine(businessLogic.UserMenu());

                            //
                        }
                        success = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Username o password errati! Vuoi riprovare? y/n");
                        input = Console.ReadLine();

                        switch (input)
                        {
                            case "y":
                                continue;

                            default:
                                Environment.Exit(0);
                                break;
                        }

                        break;
                    }
                }
            } while (!success);

            Console.WriteLine();
            Console.WriteLine("Effettua una scelta e premi un tasto qualsiasi:");
            var choice = Console.ReadLine();

            if (currentUser.Role == User.UserRole.Admin)
            {
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Cerca: ");
                        var search = Console.ReadLine();

                        businessLogic.BookSearch(search);
                        break;

                    case "2":
                        break;

                    case "3":
                        string title;
                        string authorName;
                        string authorSurname;
                        string publisher;
                        int quantity;

                        do
                        {
                            Console.Clear(); //
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
                                Console.WriteLine("I campi non sono stati valorizzati tutti correttamente! Vuoi riprovare? y/n");
                                input = Console.ReadLine();

                                switch (input)
                                {
                                    case "y":
                                        success = false;
                                        break;

                                    default: // not working
                                        success = true;
                                        break;
                                }
                            }
                            else
                            {
                                try
                                {
                                    businessLogic.CreateBook(title, authorName, authorSurname, publisher, quantity);
                                    success = false;
                                    continue;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
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
            }
            if (currentUser.Role == User.UserRole.User)
            {
                switch (choice)
                {
                    case "1":
                        break;

                    case "2":
                        break;

                    case "3":
                        break;

                    case "4":
                        break;

                    case "5":
                        Environment.Exit(0);
                        break;
                }
            }


            //var newBook = new Book { Title = "Il piccolo principe", AuthorName = "Antoine", AuthorSurname = "de Saint-Exupéry",
            //    Publisher = "Feltrinelli", Quantity = 2 };

            //xmlDAL.Serialize(newBook, "Books");
            //The process cannot access the file 'C:\Users\luca9\Downloads\Database.xml' because it is being used by another process.'


            Console.ReadKey();
        }
    }
}