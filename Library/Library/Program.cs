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
        static Menu mainMenu;
        static Menu addMenu;
        static void Main(string[] args)
        {
            List<Option> addOptions = new()
            {
                new Option("Title", EditProperty),
                new Option("Author", EditProperty),
                new Option("Genre", EditProperty),
                new Option("Description", EditProperty),
                new Option("Pages", EditProperty)
            };
            addMenu = new("Add", addOptions);

            List<Option> mainMenuOptions = new() 
            { 
                new Option("Add", () => activeMenu = addMenu)
                //new Option("Remove"),
                //new Option("Edit"),
                //new Option("Search"),
                //new Option("Exit")
            };
            mainMenu = new("Main menu", mainMenuOptions);
            activeMenu = mainMenu;

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();

                switch (activeMenu.Name)
                {
                    case "Main menu":
                        PrintMenu(mainMenu); break;
                    case "Add":
                        PrintMenu(addMenu); break;
                }

                HandleInput();
            }
        }

        static void HandleInput()
        {
            while (true)
            {
                ConsoleKey input = Console.ReadKey().Key;

                if (input == ConsoleKey.DownArrow)
                {
                    line = (line + 1) % activeMenu.Options.Count;
                    break;
                }
                else if (input == ConsoleKey.UpArrow)
                {
                    line--;
                    if (line == -1) line = activeMenu.Options.Count - 1;
                    break;
                }
                else if (input == ConsoleKey.Enter)
                {
                    activeMenu.Options[line].OnSelect();
                    break;
                }
                else if (input == ConsoleKey.Escape)
                {
                    activeMenu = mainMenu;
                    break;
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
            
        }
    }
}