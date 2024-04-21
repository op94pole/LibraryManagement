using Model;

namespace BusinessLogic
{
    public class Logic
    {
        private XMLDataAccessLayer xmlDAL = new();

        public bool DoLogin(string username, string password)
        {
            if (xmlDAL.CheckCredentials(username, password))
            {
                Console.WriteLine("Benvenuto");
                return true;
            }
            else
            {
                Console.WriteLine("Errore");
                return false;
            }
        }
       
        //public void Login()
        //{
        //    bool success = default;
        //    bool validInput = default;
        //    User currentUser;

        //    do
        //    {
        //        Console.Clear();
        //        Console.Write("Inserisci lo username: ");
        //        var username = Console.ReadLine();
        //        Console.Write("Inserisci la password: ");
        //        var password = Console.ReadLine();

        //        if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
        //        {
        //            Console.WriteLine("Input non valido. Vuoi riprovare? y/n");
        //            var input = Console.ReadLine();

        //            switch (input)
        //            {
        //                case "y":
        //                    continue;

        //                default:
        //                    success = false;
        //                    break;
        //            }

        //            continue;
        //        }

        //        //currentUser = layer.GetUser(username, password);

        //        //if (currentUser != null)
        //        //{
        //        //    success = true;
        //        //}
        //        else
        //        {
        //            do
        //            {
        //                Console.WriteLine("Username o password errati! Premi y per riprovare o n per uscire dall'applicazione");
        //                string? input = Console.ReadLine();

        //                switch (input)
        //                {
        //                    case "y":
        //                        success = false;
        //                        validInput = true;
        //                        break;
        //                    case "n":
        //                        success = true;
        //                        validInput = true;
        //                        break;
        //                    default:
        //                        Console.WriteLine("Input non valido!");
        //                        Console.Clear();
        //                        success = true;
        //                        validInput = false;
        //                        break;
        //                }
        //            } while (!validInput);
        //        }
        //    } while (!success);

        //    //if (currentUser.Role == User.UserRole.Administrator) // currentUser was null
        //    //    AdminMenu();
        //    //if (currentUser.Role == User.UserRole.User)
        //    //    UserMenu();
        //}

        //public void AdminMenu()
        //{
        //    Console.WriteLine("Admin menu");
        //}

        //public void UserMenu()
        //{
        //    Console.WriteLine("User menu");
        //}

        //public void AdmninMenu()
        //{
        //    Console.WriteLine("1. Ricerca un libro");
        //    Console.WriteLine("2. Modifica un libro");
        //    Console.WriteLine("3. Inserisci un nuovo libro");
        //    Console.WriteLine("4. Cancella un libro");
        //    Console.WriteLine("5. Chiedi un prestito");
        //    Console.WriteLine("6. Restituisci un libro");
        //    Console.WriteLine("7. Visualizza lo storico delle prenotazioni");
        //    Console.WriteLine("6. Esci");

        //    string? input = Console.ReadLine();

        //    switch (input)
        //    {
        //        case "1":
        //            //
        //            break;
        //        case "2":
        //            //
        //            break;
        //        case "3":
        //            CreateBook();
        //            break;
        //        case "4":
        //            //
        //            break;
        //        case "5":
        //            //
        //            break;
        //        case "6":
        //            //
        //            break;
        //        case "7":
        //            //
        //            break;
        //        default:
        //            break;

        //    }
        //}

        //public void UserMenu()
        //{
        //    Console.WriteLine("1. Ricerca un libro");
        //    Console.WriteLine("2. Chiedi un prestito");
        //    Console.WriteLine("3. Restituisci un libro");
        //    Console.WriteLine("4. Visualizza lo storico delle prenotazioni");
        //    Console.WriteLine("5. Esci");

        //    string? input = Console.ReadLine();

        //    switch (input)
        //    {
        //        case "1":
        //            //
        //            break;
        //        case "2":
        //            //
        //            break;
        //        case "3":
        //            //
        //            break;
        //        case "4":
        //            //
        //            break;
        //        case "5":
        //            //
        //            break;
        //        default:
        //            //
        //            break;
        //    }
        //}        
    }
}