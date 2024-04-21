using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model
{
    public class User
    {
        [XmlAttribute]
        public int UserId { get; set; }
        [XmlAttribute]
        public string Username { get; set; }
        [XmlAttribute]
        public string Password { get; set; }        
        [XmlAttribute]
        public string Role { get; set; }

        //[XmlAttribute]
        //public UserRole Role { get; set; }

        //public enum UserRole
        //{
        //    User,
        //    Admin
        //};
    }
}