using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Library
{
    internal class Program
    {
        static List<Book> books = new();
        static List<Book> lendBooks = new();
        static void Main(string[] args)
        {
            List<Option> mainMenuOptions = new() 
            { 
                new Option("Add"), 
                new Option("Remove"),
                new Option("Edit"),
                new Option("Search")
            };
            Menu mainMenu = new("Main menu", mainMenuOptions);

            PrintMenu(mainMenu);
        }

        static void PrintMenu(Menu menu)
        {
            Console.WriteLine(menu.Name);

            foreach (Option option in menu.Options)
            {
                Console.WriteLine($"> {option.Name}");
            }
        }
    }
}