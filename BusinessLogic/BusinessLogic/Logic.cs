using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Model;

namespace BusinessLogic
{
    public class Logic
    {
        private XMLDataAccessLayer layer = new XMLDataAccessLayer();
        public void Login()
        {
            bool success = default;

            do
            {
                Console.Clear();
                Console.Write("Inserisci lo username: ");
                string? username = Console.ReadLine();
                Console.Write("Inserisci la password: ");
                string? password = Console.ReadLine();

                if (layer.VerifyLogin(username, password))
                {
                    success = true;
                    
                    Menu();
                }
                else
                {
                    bool validInput = default;

                    do
                    {
                        Console.WriteLine("Username o password errati! Premi y per riprovare o n per uscire dall'applicazione");
                        string? input = Console.ReadLine();

                        switch (input)
                        {
                            case "y":
                                success = false;
                                validInput = true;
                                break;
                            case "n":
                                success = true;
                                validInput = true;
                                break;
                            default:
                                Console.WriteLine("Input non valido!");
                                Console.Clear();
                                success = true;
                                validInput = false;
                                break;
                        }                    
                    } while (!validInput);
                }
            } while (!success);
        }

        public void Menu()
        {
            Console.WriteLine("Menu");
        }
        
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