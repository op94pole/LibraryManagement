using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class XMLDataAccessLayer
{
    private static readonly string databasePath = "C:\\Users\\luca9\\Downloads\\Database.xml";   

    public void Serialize<T>(T obj, string rootElementName)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(databasePath);

        XmlNode parentNode = xmlDocument.DocumentElement;
        XmlNode targetNode = null;

        foreach (XmlNode childNode in parentNode.ChildNodes)
        {
            if (childNode.Name == rootElementName)
            {
                targetNode = childNode;
                break;
            }
        }

        if (targetNode == null)
        {
            targetNode = xmlDocument.CreateElement(rootElementName);
            parentNode.AppendChild(targetNode);
        }

        XmlSerializer serializer = new XmlSerializer(typeof(T));
        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
        namespaces.Add("", ""); 

        using (MemoryStream stream = new MemoryStream())
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                serializer.Serialize(xmlWriter, obj, namespaces);
            }

            stream.Position = 0;

            XmlDocument serializedXml = new XmlDocument();
            serializedXml.Load(stream);

            XmlNode serializedRoot = serializedXml.DocumentElement;
            XmlNode importedNode = xmlDocument.ImportNode(serializedRoot, true);
            targetNode.AppendChild(importedNode);
        }

        xmlDocument.Save(databasePath);
    }

    public T Deserialize<T>(string rootElementName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));

        using (XmlReader reader = XmlReader.Create(databasePath))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == rootElementName)
                {
                    return (T)serializer.Deserialize(reader.ReadSubtree()); 
                }
            }
        }

        throw new InvalidOperationException($"Root element '{rootElementName}' not found."); //
    }

    public List<Book> GetAvaiableBooks() //
    {
        var books = (List<Book>)Deserialize<List<Book>>("Books"); //.Where(b => b.Quantity > numero reservations...);
        return books;
    }

    //public List<Book> SearchBook() { } //    

    //public void UpdateBook() { } //

    //public void DeleteBook() { } //

    public List<Reservation> GetReservations() //
    {
        var reservations = Deserialize<List<Reservation>>("Reservations");
        return reservations;
    }

    //public void CreateReservation() { }

    //public void DeleteReservation() { }

}