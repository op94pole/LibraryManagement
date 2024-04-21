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
    private static readonly string databasePath = "C:\\Users\\luca9\\source\\repos\\op94pole\\LibraryManagement\\Model\\samples\\Database.xml";
    private static readonly string databasePath2 = "C:\\Users\\luca9\\source\\repos\\op94pole\\LibraryManagement\\Model\\samples\\Database.xml";


    static T Deserialize<T>(string rootElementName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));

        using (XmlReader reader = XmlReader.Create(databasePath))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == rootElementName)
                {
                    return (T)serializer.Deserialize(reader.ReadSubtree()); // possible null value
                }
            }
        }

        throw new InvalidOperationException($"Root element '{rootElementName}' not found."); //
    }

    public List<User> GetUsers()
    {
        var users = Deserialize<List<User>>("Users");
        return users;
    }

    public List<Book> GetBooks()
    {
        var books = Deserialize<List<Book>>("Books");
        return books;
    }

    public List<Reservation> GetReservations()
    {
        var reservations = Deserialize<List<Reservation>>("Reservations");
        return reservations;
    }

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
                        // Posizioniamoci subito dopo l'ultimo child node dell'elemento radice
                        reader.MoveToContent();
                        reader.Read();
                        
                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            reader.Read();
                        }

                        // Serializziamo l'oggetto utilizzando un XmlWriter posizionato subito dopo l'ultimo child node
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(writer, obj);
                        break;
                    }
                }
            }
        }
    }    
}