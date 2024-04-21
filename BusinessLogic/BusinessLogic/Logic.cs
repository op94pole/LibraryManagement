using Model;

namespace BusinessLogic
{
    public class Logic
    {
        private XMLDataAccessLayer xmlDAL = new();

        public bool DoLogin(string username, string password, out User currentUser)
        {
            if (xmlDAL.CheckCredentials(username, password, out currentUser))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string AdminMenu()
        {
            return "1. Ricerca un libro\r\n2. Modifica un libro\r\n3. Inserisci un nuovo libro\r\n4. Cancella un libro\r\n5. Chiedi un prestito\r\n" +
                "6. Restituisci un libro\r\n7. Visualizza lo storico delle prenotazioni\r\n8. Esci";
        }

        public string UserMenu()
        {
            return "1. Ricerca un libro\r\n2. Chiedi un prestito\r\n3. Restituisci un libro\r\n4. Visualizza lo storico delle prenotazioni\r\n5. Esci";
        }

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