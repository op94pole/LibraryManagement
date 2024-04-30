using System.Xml;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class XMLDataAccessLayer
    {
        private static readonly string databasePath = @"C:\Users\luca9\Downloads\Database.xml"; // @"C:\Users\luca9\Documents\Visual Studio 2022\Coding\LibraryManagment\Model\samples\Database.xml"

        public void Serialize<T>(T obj, string rootElementName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(databasePath);

            XmlNode? currentNode = xmlDocument.SelectSingleNode($"/Library/{rootElementName}"); 
            currentNode.RemoveAll();

            if (currentNode != null)
            {
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
                        currentNode.AppendChild(importedNode);
                    }
                }

                xmlDocument.Save(databasePath);
            }
            else
                throw new Exception($"Root element '{rootElementName}' not found.");
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

            throw new InvalidOperationException($"Root element '{rootElementName}' not found.");
        }
    }
}