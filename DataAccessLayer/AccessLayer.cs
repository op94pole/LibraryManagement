//using Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks; 
//using System.Xml.Serialization;
//using System.Xml;

//namespace DataAccessLayer
//{
//    internal class AccessLayer
//    {
//        //private readonly string usersFilePath = "C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\users.xml";
//        private readonly string booksFilePath = "C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\books.xml";
//        private readonly string reservationsFilePath = "C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\reservations.xml";

//        // Funzione per verificare i dati di login
//        public User VerifyLogin(string username, string password)
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
//            using FileStream xmlStream = new FileStream(databasePath, FileMode.Open);


//            List<User> users = (List<User>)serializer.Deserialize(xmlStream);

//            User currentUser = users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();

//            return currentUser;
//        }

//        // Funzione per recuperare tutti i libri
//        public List<Book> GetBooks()
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(Book));
//            using FileStream xmlStream = new FileStream(databasePath, FileMode.Open);

//            List<Book> books = (List<Book>)serializer.Deserialize(xmlStream);

//            return books;
//        }

//        // Funzione per recuperare un libro per ID-Titolo-Autore-Casa editrice
//        public List<Book> SearchBook(string search)
//        {
//            List<Book> result = (List<Book>)GetBooks().Where(b => b.BookId.ToString().Contains(search) || b.AuthorName.Contains(search) ||
//            b.AuthorSurname.Contains(search) || b.Publisher.Contains(search));

//            return result;
//        }

//        // Funzione per inserire un nuovo libro
//        public void InsertBook(Book newBook)
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(Book));
//            using TextWriter writer = new StreamWriter(databasePath);

//            serializer.Serialize(writer, newBook);
//        }

//        // Funzione per aggiornare un libro
//        public void UpdateBook(string search, string title, string authorName, string authorSurname, string publishingHouse)
//        {
//            Book existingBook = SearchBook(search).SingleOrDefault();

//            existingBook.Title = title;
//            existingBook.AuthorName = authorName;
//            existingBook.AuthorSurname = authorSurname;
//            existingBook.Publisher = publishingHouse;
//        }

//        // Funzione per eliminare un libro
//        public void DeleteBook(string search)
//        {
//            List<Book> books = GetBooks();
//            Book existingBook = SearchBook(search).SingleOrDefault();

//            books.Remove(existingBook);
//        }

//        // Funzione per recuperare tutte le prenotazioni
//        public List<Reservation> GetReservations()
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(Reservation));
//            using FileStream xmlStream = new FileStream(reservationsFilePath, FileMode.Open);

//            List<Reservation> reservations = (List<Reservation>)serializer.Deserialize(xmlStream);

//            return reservations;
//        }

//        // Funzione per inserire una nuova prenotazione
//        public void InsertReservation(Reservation newReservation)
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(Reservation));
//            using FileStream xmlStream = new FileStream("C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\reservations.xml",
//                FileMode.Open);

//            List<Reservation> reservations = (List<Reservation>)serializer.Deserialize(xmlStream);

//            reservations.Add(newReservation);
//        }

//        public void DeleteReservation(int reservationId)
//        {
//            List<Reservation> reservations = GetReservations();
//            Reservation existingReservation = reservations.Where(r => r.ReservationId == reservationId).SingleOrDefault();

//            reservations.Remove(existingReservation);
//        }

//        public void XmlDeserialize(User user = null, Book book = null, Reservation reservation = null) // return?
//        {
//            var users = new List<User>();
//            var books = new List<Book>();
//            var reservations = new List<Reservation>();

//            using (XmlReader reader = XmlReader.Create(databasePath))
//            {
//                while (reader.Read())
//                {
//                    if (reader.NodeType == XmlNodeType.Element) // ?
//                    {
//                        switch (reader.Name)
//                        {
//                            case "Users":
//                                //reader.ReadToDescendant(reader.Name.Substring(0, reader.Name.Length - 1)); 

//                                //do
//                                //{
//                                //    var currentUser = new User();   

//                                //    if (reader.HasAttributes)
//                                //    {
//                                //        while (reader.MoveToNextAttribute())
//                                //        {
//                                //            var attributeName = reader.Name; // 
//                                //            var attributeValue = reader.Value; //

//                                //            switch (attributeName)
//                                //            {
//                                //                case "UserId":
//                                //                    currentUser.UserId = Convert.ToInt32(attributeValue); // 
//                                //                    break;

//                                //                case "Username":
//                                //                    currentUser.Username = attributeValue; 
//                                //                    break;

//                                //                case "Password":
//                                //                    currentUser.Password = attributeValue;
//                                //                    break;

//                                //                case "Role":
//                                //                    currentUser.Role = attributeValue; // to enum
//                                //                    break;

//                                //                default:
//                                //                    break;
//                                //            }
//                                //        }

//                                //        reader.MoveToElement();
//                                //    }
//                                //} while (reader.ReadToNextSibling(reader.Name));

//                                Deserialize<User>(reader, users);
//                                break;

//                            case "Books":
//                                reader.ReadToDescendant(reader.Name.Substring(0, reader.Name.Length - 1));

//                                do
//                                {
//                                    var currentBook = new Book();

//                                    if (reader.HasAttributes)
//                                    {
//                                        while (reader.MoveToNextAttribute())
//                                        {
//                                            var attributeName = reader.Name; //
//                                            var attributeValue = reader.Value; //
//                                                                               //    
//                                            switch (attributeName)
//                                            {
//                                                case "BookId":
//                                                    currentBook.BookId = Convert.ToInt32(attributeValue); // 
//                                                    break;

//                                                case "Title":
//                                                    currentBook.Title = attributeValue;
//                                                    break;

//                                                case "AuthorName":
//                                                    currentBook.AuthorName = attributeValue;
//                                                    break;

//                                                case "AuthorSurname":
//                                                    currentBook.AuthorSurname = attributeValue;
//                                                    break;

//                                                case "Publisher":
//                                                    currentBook.Publisher = attributeValue;
//                                                    break;

//                                                case "Quantity":
//                                                    currentBook.Quantity = Convert.ToInt32(attributeValue); //
//                                                    break;

//                                                default:
//                                                    break;
//                                            }
//                                        }

//                                        reader.MoveToElement();
//                                    }
//                                } while (reader.ReadToNextSibling(reader.Name));
//                                break;

//                            case "Reservations":
//                                reader.ReadToDescendant(reader.Name.Substring(0, reader.Name.Length - 1));

//                                do
//                                {
//                                    var currentReservation = new Reservation();

//                                    if (reader.HasAttributes)
//                                    {
//                                        while (reader.MoveToNextAttribute())
//                                        {
//                                            var attributeName = reader.Name; //
//                                            var attributeValue = reader.Value; //

//                                            switch (attributeName)
//                                            {
//                                                case "ReservationId":
//                                                    currentReservation.ReservationId = Convert.ToInt32(attributeValue); // 
//                                                    break;

//                                                case "UserId":
//                                                    currentReservation.UserId = attributeValue; // to User
//                                                    break;

//                                                case "BookId":
//                                                    currentReservation.BookId = attributeValue; // to book
//                                                    break;

//                                                case "StartData":
//                                                    currentReservation.StartDate = attributeValue; // to DateTime
//                                                    break;

//                                                case "EndDate":
//                                                    currentReservation.EndDate = attributeValue; // to DateTime
//                                                    break;

//                                                default:
//                                                    break;
//                                            }
//                                        }

//                                        reader.MoveToElement();
//                                    }
//                                } while (reader.ReadToNextSibling(reader.Name));
//                                break;

//                            default:
//                                break;
//                        }



//                        //// Check if the current element is Users, Books, or Reservations
//                        //if (reader.Name == "Users" || reader.Name == "Books" || reader.Name == "Reservations")
//                        //{
//                        //    // Move to the first child node of Users, Books, or Reservations
//                        //    reader.ReadToDescendant(reader.Name.Substring(0, reader.Name.Length - 1));

//                        //    // Loop through child nodes
//                        //    do
//                        //    {
//                        //        // Print out attributes of the current node
//                        //        if (reader.HasAttributes)
//                        //        {

//                        //            Console.WriteLine("Element: " + reader.Name);
//                        //            while (reader.MoveToNextAttribute())
//                        //            {
//                        //                Console.WriteLine("Attribute: " + reader.Name + " = " + reader.Value);
//                        //            }
//                        //            // Move the reader back to the element node
//                        //            reader.MoveToElement();
//                        //        }
//                        //    } while (reader.ReadToNextSibling(reader.Name));
//                        //}
//                    }

//                    //if (reader.IsStartElement())
//                    //{
//                    //    switch (reader.Name)
//                    //    {
//                    //        case "Users": // 

//                    //            if (reader.HasAttributes)
//                    //            {
//                    //                // Read attributes of the current element
//                    //                while (reader.MoveToNextAttribute())
//                    //                {
//                    //                    Console.WriteLine("Attribute: " + reader.Name + " = " + reader.Value);
//                    //                }
//                    //                // Move the reader back to the element node
//                    //                reader.MoveToElement();
//                    //            }
//                    //            break;


//                    //        case "Book":
//                    //            XmlSerializer booksSerializer = new XmlSerializer(typeof(Book));
//                    //            List<Book> books = (List<Book>)booksSerializer.Deserialize(reader);
//                    //            break;

//                    //        case "":
//                    //            XmlSerializer reservationsSerializer = new XmlSerializer(typeof(Reservation));
//                    //            List<Reservation> reservations = (List<Reservation>)reservationsSerializer.Deserialize(reader);
//                    //            break;

//                    //        default:
//                    //            break;
//                    //    }
//                    //}
//                }
//            }
//        }
//        private void Deserialize<T>(XmlReader reader, List<T> list)
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(T));
//            T deserializedObject = (T)serializer.Deserialize(reader);

//            list.Add(deserializedObject);
//        }

//        public User GetUser(string username, string password)
//        {
//            var currentUser = new User() { Username = username, Password = password };
//            XmlDeserialize(user: currentUser);

//            return currentUser;
//        }
//    }
//}