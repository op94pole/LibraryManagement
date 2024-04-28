//namespace LibraryManagement
//{
//    public class InterfaceManager
//    {
//        LibraryManagement management = new();

//        public void LibraryManagementLoad()
//        {
//            Console.WriteLine("Ti diamo il benvenuto in LIBRARY MANAGMENT CONSOLE APP");
//            Console.WriteLine();
//            Console.WriteLine("Premi un tasto qualsiasi per procedere con il login...");
//            Console.ReadKey();
//            Console.Clear();

//            Login();
//        }

//        public void Login()
//        {
//            bool success = default;

//            do
//            {
//                Console.Clear();
//                Console.WriteLine("Login.................");
//                Console.WriteLine();
//                Console.Write("Inserisci lo username: ");
//                var username = Console.ReadLine();
//                Console.Write("Inserisci la password: ");
//                var password = Console.ReadLine();

//                if (management.CredentialsCheck(username, password))
//                    success = true;
//                else
//                    success = false;
//            } while (!success);
//        }

//        public bool Retry()
//        {
//            Console.WriteLine("Vuoi riprovare? y/n");
//            var input = Console.ReadLine();

//            switch (input)
//            {
//                case "y":
//                    Console.Clear();
//                    return true;

//                default:
//                    Console.Clear();
//                    return false;
//            }
//        }

//        public void GetAdminMenu()
//        {
//            Console.Clear();
//            Console.WriteLine("1. Ricerca un libro");
//            Console.WriteLine("2. Modifica un libro");
//            Console.WriteLine("3. Inserisci un nuovo libro");
//            Console.WriteLine("4. Rimuovi un libro");
//            Console.WriteLine("5. Richiedi un prestito");
//            Console.WriteLine("6. Restituisci un libro");
//            Console.WriteLine("7. Visualizza lo storico delle prenotazioni");
//            Console.WriteLine("8. Esci");
//            Console.WriteLine();

//            Console.WriteLine("Effettua una scelta e premi Invio.");
//            var input = Console.ReadLine();

//            switch (input)
//            {
//                case "1":
//                    management.SearchBook();
//                    break;

//                case "2":
//                    management.ModifyBook();
//                    break;

//                case "3":
//                    management.InsertBook();
//                    break;

//                case "4":
//                    management.DeleteBook();
//                    break; ;

//                case "5":
//                    management.CreateReservation();
//                    break;

//                case "6":
//                    break;
//                    management.ReturnBook();

//                case "7":
//                    break;
//                    management.ShowReservations();

//                case "8":
//                    management.CloseApplication();
//                    break;

//                default:
//                    Console.Clear();
//                    Console.WriteLine("Scelta non valida!.................");
//                    Console.WriteLine();
//                    Console.WriteLine("Premi un tasto qualsiasi e riprova.");
//                    Console.ReadKey();
//                    break;
//            }

//            GetAdminMenu();
//        }

//        public void GetUserMenu()
//        {
//            Console.Clear();
//            Console.WriteLine("1. Ricerca un libro");
//            Console.WriteLine("2. Chiedi un prestito");
//            Console.WriteLine("3. Restituisci un libro");
//            Console.WriteLine("4. Visualizza lo storico delle prenotazioni");
//            Console.WriteLine("5. Esci");
            
//            Console.WriteLine("Effettua una scelta e premi Invio.");
//            var input = Console.ReadLine();

//            switch (input)
//            {
//                case "1":
//                    management.SearchBook();
//                    break;

//                case "2":
//                    management.CreateReservation();
//                    break;

//                case "3":
//                    management.ReturnBook();
//                    break;

//                case "4":
//                    management.ShowReservations();
//                    break;

//                case "5":
//                    management.CloseApplication();
//                    break;

//                default:
//                    Console.Clear();
//                    Console.WriteLine("Scelta non valida!.................");
//                    Console.WriteLine();
//                    Console.WriteLine("Premi un tasto qualsiasi e riprova.");
//                    Console.ReadKey();
//                    break;
//            }

//            GetUserMenu();
//        }
//    }
//}