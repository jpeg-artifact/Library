using Microsoft.VisualBasic.FileIO;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Library
{
    internal class Program
    {
        static List<Book> books = new();
        static int line = 0;
        static Menu activeMenu;
        static Book activeBook = DefaultBook();

        static Menu mainMenu;
        static Menu addMenu;
        static Menu removeMenu;
        static Menu editMenu;
        static Menu searchTitleAuthorMenu;
        static Menu searchAuthorMenu;
        static Menu searchTitleMenu;
        static Menu searchMenu;
        static Menu censorMenu;
        static Menu bookListMenu;
        static Menu bookMenu;
        static Menu browseMenu;

        static bool goToEditFromSearch = false;
        static bool goToRemoveFromSearch = false;

        static void Main(string[] args)
        {


            // Load saved books
            ReadFile();

            /*
             * Menu  system:
             * When a manu is created it has a list of options assigned to it.
             * While option is highlighten and enter is pressed, the action attached to the option-object is fired.
             */

            // Create add-menu
            List<Option> addMenuOptions = new()
            {
                new Option("Title", EditProperty),
                new Option("Author", EditProperty),
                new Option("Genre", EditProperty),
                new Option("Description", EditProperty),
                new Option("Pages", EditProperty),
                new Option("Create", () => { books.Add(activeBook); activeBook = DefaultBook(); SaveFile(); })
            };
            addMenu = new("Add", addMenuOptions);

            // Create remove-menu
            List<Option> removeMenuOptions = new()
            {
                new Option("Remove", () => { books.Remove(activeBook); activeMenu = mainMenu; SaveFile(); })
            };
            removeMenu = new("Remove", removeMenuOptions);

            // Create search-title-author-menu
            List<Option> searchTitleAuthorMenuOptions = new()
            {
                new Option("Title", EditProperty),
                new Option("Author", EditProperty),
                new Option("Search", () => { line = 0; Search(); })
            };
            searchTitleAuthorMenu = new("Search by title and author", searchTitleAuthorMenuOptions);

            // Create search-title-menu
            List<Option> searchTitleMenuOptions = new()
            {
                new Option("Title", EditProperty),
                new Option("Search", () => { line = 0; Search(); })
            };
            searchTitleMenu = new("Search by title", searchTitleMenuOptions);

            // Create search-author-menu
            List<Option> searchAuthorMenuOptions = new()
            {
                new Option("Author", EditProperty),
                new Option("Search", () => { line = 0; Search(); })
            };
            searchAuthorMenu = new("Search by author", searchAuthorMenuOptions);

            // Create searchMenu
            List<Option> searchMenuOptions = new()
            {
                new Option("Search by title and author", () => { activeMenu = searchTitleAuthorMenu; line = 0; }),
                new Option("Search by title", () => { activeMenu = searchTitleMenu; line = 0; }),
                new Option("Search by author", () => { activeMenu = searchAuthorMenu; line = 0; })
            };
            searchMenu = new("Search", searchMenuOptions);

            // Create edit-menu
            List<Option> editMenuOptions = new()
            {
                new Option("Title", () => { EditProperty(); SaveFile(); }),
                new Option("Author", () => { EditProperty(); SaveFile(); }),
                new Option("Genre", () => { EditProperty(); SaveFile(); }),
                new Option("Description", () => { EditProperty(); SaveFile(); }),
                new Option("Pages", () => { EditProperty(); SaveFile(); })

            };
            editMenu = new("Edit", editMenuOptions);

            // Create censor-menu
            List<Option> censorMenuOptions = new()
            {
                new Option("Author", EditProperty),
                new Option("Censor", () => { books.RemoveAll(book => book.Author == activeBook.Author); SaveFile(); activeBook = DefaultBook(); })
            };
            censorMenu = new("Censor", censorMenuOptions);

            // Create book-list-menu
            List<Option> bookListMenuOptions = new()
            {
                
            };
            bookListMenu = new("Book", bookListMenuOptions);

            // Create book-menu
            List<Option> bookMenuOptions = new()
            {
                new Option("Borrow", () => { activeBook.IsLend = true; SaveFile(); }),
                new Option("Return", () => { activeBook.IsLend = false; SaveFile(); })
            };
            bookMenu = new("Book", bookMenuOptions);

            // Create browse-menu
            List<Option> browseMenuOptions = new()
            {
                new Option("Refresh", PrintMenu)
            };
            browseMenu = new("Browse", browseMenuOptions);

            // Create main-menu
            List<Option> mainMenuOptions = new() 
            { 
                new Option("Add", () => { activeMenu = addMenu; line = 0; activeBook = DefaultBook(); }),
                new Option("Remove", () => { activeMenu = searchMenu; goToRemoveFromSearch = true; line = 0; activeBook = DefaultBook(); }),
                new Option("Edit", () => { activeMenu = searchMenu; line = 0; goToEditFromSearch = true; }),
                new Option("Censor", () => { activeMenu = censorMenu; line = 0; }),
                new Option("Search", () => { activeMenu = searchMenu; line = 0; activeBook = DefaultBook(); }),
                new Option("Browse", () => { activeMenu = browseMenu; line = 0; activeBook = DefaultBook(); }),
                new Option("Exit", () => Environment.Exit(0))
            };
            mainMenu = new("Main", mainMenuOptions);
            activeMenu = mainMenu;

            // Loops the program. Prints the menu and then reads for inputs. When a input is detected the menu refreshes.
            while (true)
            {
                PrintMenu();
                HandleInput();
            }
        }

        // Creates and empty book-object
        static Book DefaultBook()
        {
            return new Book("None", "None", Genre.None, "None", 0, false);
        }

        // Converts books-list to a JSON string and then writes it to the "LibraryData.txt" file
        static void SaveFile()
        {
            string jsonString = JsonSerializer.Serialize(books);

            StreamWriter writeFile = new("LibraryData.json");
            writeFile.Write(jsonString);
            writeFile.Close();
        }

        // Converts the JSON string read from the "LibraryData.txt" file to a List<Option>
        static void ReadFile()
        {
            StreamReader readFile = new("LibraryData.json");
            string jsonString = readFile.ReadToEnd();
            books = JsonSerializer.Deserialize<List<Book>>(jsonString);
            readFile.Close();
        }

        /* Waits for the user to press a key. If the key is one of the 4 inputs it updates
         * the corresponding variable and then breakes the loop. Allowing the program to continue
         * in the main loop.
         */
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
                else if (input == ConsoleKey.Escape && activeMenu != mainMenu)
                {
                    activeMenu = mainMenu;
                    line = 0;
                    break;
                }
            }
        }

        static void PrintMenu()
        {
            NukeConsole();
            Console.WriteLine("\u001b[94mNavigation\n\u001b[96mUpp: up arrow\nDown: down arrow\nSelect: enter\nBack: esc\n\u001b[0m");
            Console.WriteLine($"\u001b[94m{activeMenu.Name} menu:\u001b[0m");

            // Checks whether the menu requires any special outputs.
            if (activeMenu == bookMenu || activeMenu == removeMenu)
            {
                activeBook.Print();
            }

            if (activeMenu == browseMenu)
            {
                foreach (Book book in books)
                {
                    book.Print();
                }
            }

            // Loops through every option in menu
            for (int i = 0; i < activeMenu.Options.Count; i++)
            {
                Option option = activeMenu.Options[i];
                string lineToPrint;

                // Check whether option should be highlighted or not.
                if (i == line)
                {
                    lineToPrint = $"> \u001b[37m{option.Name}\u001b[0m";
                }
                else
                {
                    lineToPrint = $" \u001b[90m{option.Name}\u001b[0m";
                }

                // Checks whether the menu should have any of the following printed.
                if (activeMenu == addMenu || activeMenu == removeMenu || activeMenu == editMenu || activeMenu == censorMenu || activeMenu == searchAuthorMenu || activeMenu == searchTitleMenu || activeMenu == searchTitleAuthorMenu)
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

                // Prints the line.
                Console.WriteLine(lineToPrint);
            }
        }

        static void EditProperty()
        {
            Option option = activeMenu.Options[line];

            // Checks which property of the book is highlighted. Then changed the property to what ever the user writes.
            Console.Write($"Enter new {option.Name}: ");
            switch (option.Name)
            {
                case "Title":
                    activeBook.Title = Console.ReadLine(); break;
                case "Author":
                    activeBook.Author = Console.ReadLine(); break;
                case "Genre":
                    try
                    {
                        activeBook.Genre = Enum.Parse<Genre>(Console.ReadLine());
                    }
                    catch { Console.WriteLine("Invalid genre."); Console.ReadKey(); }
                    break;
                case "Description":
                    activeBook.Description = Console.ReadLine(); break;
                case "Pages":
                    try
                    {
                        activeBook.Pages = Math.Abs(int.Parse(Console.ReadLine()));
                    }
                    catch { Console.WriteLine("Invalid pages."); Console.ReadKey(); }
                    break;
            }
        }

        // Check if Title and Author matches, goes to the book's menu if true and then breaks out of loop early.
        static void Search()
        {
            bookListMenu.Options.Clear();

            static void SwitchToBookMenu(Book book)
            {
                Action action;
                if (goToEditFromSearch) action = () => { activeBook = book; line = 0; activeMenu = editMenu; goToEditFromSearch = false; };
                else if (goToRemoveFromSearch) action = () => { activeBook = book; line = 0; activeMenu = removeMenu; goToRemoveFromSearch = false; };
                else action = () => { activeBook = book; line = 0; activeMenu = bookMenu; };
                string availability;
                if (book.IsLend) availability = "\u001b[91mNot available\u001b[0m";
                else availability = "\u001b[92mAvailable\u001b[0m";
                bookListMenu.Options.Add(new Option($"{book.Title} by {book.Author}: {availability}", action));
                Debug.Print(book.Title);
            }

            foreach (Book book in books)
            {
                switch (activeMenu.Name)
                {
                    case "Search by title":
                        if (activeBook.Title == book.Title)
                        {
                            SwitchToBookMenu(book);
                        }
                        break;
                    case "Search by author":
                        if (activeBook.Author == book.Author)
                        {
                            SwitchToBookMenu(book);
                        }
                        break;
                    case "Search by title and author":
                        if (activeBook.Title == book.Title && activeBook.Author == book.Author)
                        {
                            SwitchToBookMenu(book);
                        }
                        break;
                }
            }

            bookListMenu.Options.Add(new Option("Refresh", PrintMenu));
            activeMenu = bookListMenu;
        }

        // Clear console completely. Including off screen text.
        static void NukeConsole()
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
        }
    }
}