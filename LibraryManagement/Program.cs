using BusinessLogic;
using System.Xml.Serialization;
using Model;
using System.Net.NetworkInformation;

namespace LibraryManager
{
    internal class Program
    {
        static void Main()
        {
            var menu = new LibraryManagement();
            menu.LibraryManagementLoad();

            Console.ReadKey();
        }
    }
}