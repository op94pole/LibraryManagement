using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Model;

public class XMLDataAccessLayer
{
    private readonly string usersFilePath = "C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\users.xml";
    private readonly string booksFilePath = "C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\books.xml";
    private readonly string reservationsFilePath = "C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\reservations.xml";

    // Funzione per verificare i dati di login
    public User VerifyLogin(string username, string password)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
        using FileStream xmlStream = new FileStream(usersFilePath, FileMode.Open);


        List<User> users = (List<User>)serializer.Deserialize(xmlStream);

        User currentUser = users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();

        return currentUser;
    }

    // Funzione per recuperare tutti i libri
    public List<Book> GetBooks()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Book));
        using FileStream xmlStream = new FileStream(usersFilePath, FileMode.Open);

        List<Book> books = (List<Book>)serializer.Deserialize(xmlStream);

        return books;
    }

    // Funzione per recuperare un libro per ID-Titolo-Autore-Casa editrice
    public List<Book> SearchBook(string search)
    {
        List<Book> result = (List<Book>)GetBooks().Where(b => b.BookId.ToString().Contains(search) || b.AuthorName.Contains(search) ||
        b.AuthorSurname.Contains(search) || b.PublishingHouse.Contains(search));

        return result;
    }

    // Funzione per inserire un nuovo libro
    public void InsertBook(Book newBook)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Book));
        using TextWriter writer = new StreamWriter(usersFilePath);

        serializer.Serialize(writer, newBook);
    }

    // Funzione per aggiornare un libro
    public void UpdateBook(string search, string title, string authorName, string authorSurname, string publishingHouse)
    {
        Book existingBook = SearchBook(search).SingleOrDefault();

        existingBook.Title = title;
        existingBook.AuthorName = authorName;
        existingBook.AuthorSurname = authorSurname;
        existingBook.PublishingHouse = publishingHouse;
    }

    // Funzione per eliminare un libro
    public void DeleteBook(string search)
    {
        List<Book> books = GetBooks();
        Book existingBook = SearchBook(search).SingleOrDefault();

        books.Remove(existingBook);
    }

    // Funzione per recuperare tutte le prenotazioni
    public List<Reservation> GetReservations()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Reservation));
        using FileStream xmlStream = new FileStream(reservationsFilePath, FileMode.Open);

        List<Reservation> reservations = (List<Reservation>)serializer.Deserialize(xmlStream);

        return reservations;
    }

    // Funzione per inserire una nuova prenotazione
    public void InsertReservation(Reservation newReservation)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Reservation));
        using FileStream xmlStream = new FileStream("C:\\Users\\luca9\\Documents\\Visual Studio 2022\\Coding\\LibraryManagment\\DataAccessLayer\\bin\\Debug\\net8.0\\reservations.xml",
            FileMode.Open);

        List<Reservation> reservations = (List<Reservation>)serializer.Deserialize(xmlStream);

        reservations.Add(newReservation);
    }

    public void DeleteReservation(int reservationId)
    {
        List<Reservation> reservations = GetReservations();
        Reservation existingReservation = reservations.Where(r => r.ReservationId == reservationId).SingleOrDefault();

        reservations.Remove(existingReservation);
    }

    public List<User> DeserializedUsers()
    {
        List<User> users = new List<User>();
        User currentUser = null;

        using (XmlReader reader = XmlReader.Create(usersFilePath))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "User":
                            currentUser = new User();
                            users.Add(currentUser);
                            break;

                        case "":
                            currentUser.Username = reader.Value;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        return users;
    }

    public User GetUser(string username, string password)
    {
        var user = new User();

        return user;
    }
}