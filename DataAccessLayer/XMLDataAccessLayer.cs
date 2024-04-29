using System.Xml;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class XMLDataAccessLayer
    {
        private static readonly string databasePath = "C:\\Users\\luca9\\Downloads\\Database.xml";

        public void Serialize<T>(T obj, string rootElementName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(databasePath);

            XmlNode booksNode = xmlDocument.SelectSingleNode($"/Library/{rootElementName}"); //
            booksNode.RemoveAll();

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

                XmlNodeList interestedNode = serializedXml.DocumentElement.ChildNodes; //

                foreach (XmlNode bookNode in interestedNode)
                {
                    XmlNode importedNode = xmlDocument.ImportNode(bookNode, true);
                    booksNode.AppendChild(importedNode);
                }
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

            throw new InvalidOperationException($"Root element '{rootElementName}' not found."); // unhandled 
        }
    }
}