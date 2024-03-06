using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Library
{
    internal class Program
    {
        static List<Book> books = new();
        static List<Book> lendBooks = new();
        static int line = 0;
        static Menu activeMenu;
        static void Main(string[] args)
        {
            List<Option> addOptions = new()
            {
                new Option("Title", EditProperty)
            };

            List<Option> mainMenuOptions = new() 
            { 
                new Option("Add", (menu) => activeMenu = ),
                new Option("Remove"),
                new Option("Edit"),
                new Option("Search"),
                new Option("Exit")
            };
            Menu mainMenu = new("Main menu", mainMenuOptions);
            activeMenu = mainMenu;

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();

                switch (activeMenu.Name)
                {
                    case "Main menu":
                        PrintMenu(mainMenu);
                        break;
                }

                HandleInput(activeMenu);
            }
        }

        static void HandleInput(Menu menu)
        {
            while (true)
            {
                ConsoleKey input = Console.ReadKey().Key;

                if (input == ConsoleKey.DownArrow)
                {
                    line = (line + 1) % menu.Options.Count;
                    break;
                }
                else if (input == ConsoleKey.UpArrow)
                {
                    line--;
                    if (line == -1) line = menu.Options.Count - 1;
                    break;
                }
                else if (input == ConsoleKey.Enter)
                {
                    menu.Options[line].OnSelect(menu.Options[line])
                }
            }
        }

        static void PrintMenu(Menu menu)
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

        static void EditProperty()
        {
            Console.WriteLine("Meow");
        }
    }
}