using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataAccessLayer
{
    public class EntitiesWrapper<T>
    {
        [XmlElement("Book")]
        public List<T> Books { get; set; }
        public List<T> Reservations { get; set; }
    }
}
