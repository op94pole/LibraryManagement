using BusinessLogic;
using System.Xml.Serialization;
using Model;
using System.Net.NetworkInformation;

namespace LibraryManagement
{
    internal class Program
    {
        static void Main()
        {
            var app = new LibraryManagement();
            app.LibraryManagementLoad();

            Console.ReadKey();
        }
    }
}