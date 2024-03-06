using Microsoft.VisualBasic.FileIO;
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
        static Book activeBook;

        static Menu mainMenu;
        static Menu addMenu;
        static Menu removeMenu;
        static Menu searchMenu;
        static void Main(string[] args)
        {
            List<Option> addMenuOptions = new()
            {
                new Option("Title", EditProperty),
                new Option("Author", EditProperty),
                new Option("Genre", EditProperty),
                new Option("Description", EditProperty),
                new Option("Pages", EditProperty),
                new Option("Create", () => { books.Add(activeBook); activeBook = DefaultBook(); })
            };
            addMenu = new("Add", addMenuOptions);

            List<Option> removeMenuOptions = new()
            {
                new Option("Title", EditProperty),
                new Option("Author", EditProperty),
                new Option("Remove", RemoveBook)
            };
            removeMenu = new("Remove", removeMenuOptions);

            List<Option> searchMenuOptions = new()
            {
                new Option("Title", EditProperty),
                new Option("Author", EditProperty),
                new Option("Search", Search)
            };
            searchMenu = new("Search", searchMenuOptions);

            List<Option> mainMenuOptions = new() 
            { 
                new Option("Add", () => { activeMenu = addMenu; line = 0; activeBook = DefaultBook(); }),
                new Option("Remove", () => { activeMenu = removeMenu; line = 0; activeBook = DefaultBook(); }),
                //new Option("Edit")
                new Option("Search", () => { activeMenu = searchMenu; line = 0; activeBook = DefaultBook(); })
                //new Option("Exit")
            };
            mainMenu = new("Main menu", mainMenuOptions);
            activeMenu = mainMenu;

            bool isRunning = true;
            while (isRunning)
            {
                foreach (Book book in books)
                {
                    Console.WriteLine(book.Title);
                }
                PrintMenu();
                HandleInput();
                Console.Clear();
            }
        }
        static Book DefaultBook()
        {
            return new Book("None", "None", Genre.None, "None", 0, false);
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
                    line = 0;
                    break;
                }
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine(activeMenu.Name);

            for (int i = 0; i < activeMenu.Options.Count; i++)
            {
                Option option = activeMenu.Options[i];
                string lineToPrint;
                if (i == line)
                {
                    lineToPrint = $"> {option.Name}";
                }
                else
                {
                    lineToPrint = $" {option.Name}";
                }

                if (activeMenu == addMenu || activeMenu == removeMenu || activeMenu == searchMenu)
                {
                    switch (option.Name)
                    {
                        case "Title":
                            lineToPrint += $": {activeBook.Title}"; break;
                        case "Author":
                            lineToPrint += $": {activeBook.Author}"; break;
                        case "Genre":
                            lineToPrint += $": {activeBook.Genre}"; break;
                        case "Description":
                            lineToPrint += $": {activeBook.Description}"; break;
                        case "Pages":
                            lineToPrint += $": {activeBook.Pages}"; break;
                    }
                }

                Console.WriteLine(lineToPrint);
            }
        }

        static void EditProperty()
        {
            Option option = activeMenu.Options[line];

            Console.Write($"Enter new {option.Name}: ");
            switch (option.Name)
            {
                case "Title":
                    activeBook.Title = Console.ReadLine(); break;
                case "Author":
                    activeBook.Author = Console.ReadLine(); break;
                case "Genre":
                    activeBook.Genre = Enum.Parse<Genre>(Console.ReadLine()); break;
                case "Description":
                    activeBook.Description = Console.ReadLine(); break;
                case "Pages":
                    activeBook.Pages = int.Parse(Console.ReadLine()); break;
            }
        }

        static void RemoveBook()
        {
            foreach (Book book in books)
            {
                if (activeBook.Title == book.Title && activeBook.Author == book.Author)
                {
                    books.Remove(book);
                    break;
                }   
            }
        }

        static void Search()
        {
            foreach (Book book in books)
            {
                if (activeBook.Title == book.Title && activeBook.Author == book.Author)
                {
                    activeBook = book;
                    activeMenu = mainMenu;
                    break;
                }
            }
        }
    }
}