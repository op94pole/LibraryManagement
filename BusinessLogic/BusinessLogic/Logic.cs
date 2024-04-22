using Model;
using System.Reflection;

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

        public void CreateBook(string title, string authorName, string authorSurname, string publisher, int quantity)
        {
            Book newBook = new()
            {
                Title = title,
                AuthorName = authorName,
                AuthorSurname = authorSurname,
                Publisher = publisher,
                Quantity = quantity
            };

            xmlDAL.AddBook(newBook);
        }        
    }
}