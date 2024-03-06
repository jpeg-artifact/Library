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
            int line = 0;

            List<Option> mainMenuOptions = new() 
            { 
                new Option("Add"),
                new Option("Remove"),
                new Option("Edit"),
                new Option("Search"),
                new Option("i love huge c#cks")
            };
            Menu mainMenu = new("Main menu", mainMenuOptions);

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                PrintMenu(mainMenu, line);
                ConsoleKey input = Console.ReadKey().Key;

                if (input == ConsoleKey.DownArrow)
                {
                    line = (line + 1) % mainMenuOptions.Count;
                }
                else if (input == ConsoleKey.UpArrow)
                {
                    line--;
                    if (line == -1) line = mainMenuOptions.Count;
                }
            }
        }

        static void PrintMenu(Menu menu, int line)
        {
            Console.WriteLine(menu.Name);

            for (int i = 0; i < menu.Options.Count; i++)
            {
                Option option = menu.Options[i];
                if (i == line)
                {
                    Console.WriteLine($"> {option.Name}");
                }
                else
                {
                    Console.WriteLine($" {option.Name}");
                }
            }
        }
    }
}