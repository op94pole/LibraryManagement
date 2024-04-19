using BusinessLogic;
using System.Xml.Serialization;
using Model;

namespace LibraryManagement
{
    internal class Program
    {
        static void Main()
        {
            
            Logic logic = new Logic();
            //XMLDataAccessLayer xml = new XMLDataAccessLayer();

            Console.WriteLine("Ti diamo il benvenuto in LIBRARY MANAGMENT APP CONSOLE");
            Console.WriteLine();
            Console.WriteLine("Premi un tasto qualsiasi per procedere con il login...");
            Console.ReadKey();
            Console.Clear();

            logic.Login();


            Console.ReadKey();  
        }
    }
}