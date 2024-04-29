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