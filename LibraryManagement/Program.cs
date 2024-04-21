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

            User currentUser;
            

            Console.WriteLine("Ti diamo il benvenuto in LIBRARY MANAGMENT APP CONSOLE");
            Console.WriteLine();
            Console.WriteLine("Premi un tasto qualsiasi per procedere con il login...");
            Console.ReadKey();
            Console.Clear();

            Console.Write("Inserisci lo username: ");
            string username = Console.ReadLine();
            Console.Write("Inserisci la password: ");
            string password = Console.ReadLine();

            businessLogic.DoLogin(username, password);

            //if (businessLogic.DoLogin(username, password))
            //{
            //    if (xmlDAL.CurrentUser.Role == "Administrator")
            //    {
            //        Console.WriteLine("Welcom admin");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Welcome user");
            //    }
            //}       


            //var newBook = new Book { Title = "Il piccolo principe", AuthorName = "Antoine", AuthorSurname = "de Saint-Exupéry",
            //    Publisher = "Feltrinelli", Quantity = 2 };

            //xmlDAL.Serialize(newBook, "Books");
            //The process cannot access the file 'C:\Users\luca9\Downloads\Database.xml' because it is being used by another process.'


            Console.ReadKey();  
        }
    }
}