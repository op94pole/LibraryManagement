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
    private static readonly string databasePath = "C:\\Users\\luca9\\Downloads\\Database.xml";
    //private static readonly string databaseReadPath = "C:\\Users\\luca9\\source\\repos\\op94pole\\LibraryManagement\\Model\\samples\\Database.xml";
    //private static readonly string databaseWritePath = "C:\\Users\\luca9\\Downloads\\Database.xml";

    public /*static*/ void Serialize<T>(T obj, string rootElementName)
    {
        using (XmlWriter writer = XmlWriter.Create(databasePath, new XmlWriterSettings { Indent = true }))
        {
            using (XmlReader reader = XmlReader.Create(databasePath))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == rootElementName)
                    {
                        //Posizioniamoci subito dopo l'ultimo child node dell'elemento radice
                        reader.MoveToContent();
                        reader.Read();

                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            reader.Read();
                        }

                        //Serializziamo l'oggetto utilizzando un XmlWriter posizionato subito dopo l'ultimo child node
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(writer, obj);
                        break;
                    }
                }
            }
        }
    }

    public static T Deserialize<T>(string rootElementName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));

        using (XmlReader reader = XmlReader.Create(databasePath))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == rootElementName)
                {
                    return (T)serializer.Deserialize(reader.ReadSubtree()); //possible null value
                }
            }
        }

        throw new InvalidOperationException($"Root element '{rootElementName}' not found."); //
    }

    public bool CheckCredentials(string username, string password, out User currentUser)
    {
        currentUser = Deserialize<List<User>>("Users").Where(u => u.Username == username && u.Password == password).SingleOrDefault();

        if (currentUser != null)
            return true;
        else
            return false;
    }

    public List<Book> GetAvaiableBooks()
    {
        var books = (List<Book>)Deserialize<List<Book>>("Books").Where(b => b.Quantity > 0); //
        return books;
    }

    //public List<Book> SearchBook() { }

    //public void AddBook() { }

    //public void UpdateBook() { }

    //public void DeleteBook() { }

    public List<Reservation> GetReservations()
    {
        var reservations = Deserialize<List<Reservation>>("Reservations");
        return reservations;
    }

    //public void CreateReservation() { }

    //public void DeleteReservation() { }

}