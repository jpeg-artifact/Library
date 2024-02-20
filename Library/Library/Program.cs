using System;
using System.Runtime.CompilerServices;

namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] commandChunks;
            bool isRunning = true;
            List<Book> books = new();
            List<Book> lendBooks = new();

            Help();

            ReadFile(books);

            while (isRunning)
            {
                Console.Clear();
                Help();
                Console.Write("Enter a command: ");
                commandChunks = Console.ReadLine().Split("; ");
                switch (commandChunks[0].ToLower())
                {
                    case "add":
                        AddBook(books, commandChunks, EnterPassword()); // add; Title; Author; Genre; Description; Pages; Tags
                        WriteFile(books);
                        break;
                    case "print_all":
                        PrintAll(books);
                        break;
                    case "remove":
                        RemoveBook(books, commandChunks, EnterPassword()); // remove; Title; Author
                        WriteFile(books);
                        break;
                    case "edit":
                        EditBook(books, commandChunks, EnterPassword());
                        WriteFile(books);
                        break;
                }

                Console.ReadKey();
            }
        }

        static void Help()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            //Genres
            Console.WriteLine("These are the available genres: ");
            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                Console.WriteLine(genre);
            }
            Console.WriteLine("------------------------");

            //Tags
            Console.WriteLine("These are the available tags: ");
            foreach (Tags tag in Enum.GetValues(typeof(Tags)))
            {
                Console.WriteLine(tag);
            }
            Console.WriteLine("------------------------");

            //Commands
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Blue commands require password.");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Dark yellow commands DO NOT require password.\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("These are all the available commands: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("add; Title; Author; Genre; Description; Pages; Tags");
            Console.WriteLine("remove; Title; Author");
            Console.WriteLine("edit; Title; Author");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("print_all");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("------------------------");
        }

        static void WriteFile(List<Book> books)
        {
            StreamWriter writeFile = new("LibraryData.txt");

            foreach (Book book in books)
            {
                writeFile.WriteLine($"book;{book.Title};{book.Author};{book.Genre};{book.Description};{book.Pages};{book.Tags};{book.IsLend}");
            }

            writeFile.Close();
        }

        static void ReadFile(List<Book> books)
        {
            StreamReader readFile = new("LibraryData.txt");
            string s;

            books.Clear();

            while ((s = readFile.ReadLine()) != null)
            {
                string[] sChunks = s.Split(";");
                books.Add(StringToBook(sChunks));
            }

            readFile.Close();
        }

        static bool EnterPassword()
        {
            String password = new StreamReader("Password.txt").ReadLine();
            Console.Write("Enter password: ");
            string enteredPassword = Console.ReadLine();
            if (enteredPassword == password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static Book StringToBook(string[] sChunks)
        {
            string title = sChunks[1];
            string author = sChunks[2];
            Genre genre = Enum.Parse<Genre>(sChunks[3]);
            string description = sChunks[4];
            int pages = int.Parse(sChunks[5]);
            string[] tagsChunk = sChunks[6].Split(" | ");
            int tags = tagsChunk.Select(Enum.Parse<Tags>).Cast<int>().Sum();
            bool isLend = false;
            return new Book(title, author, genre, description, pages, (Tags)tags, isLend);
        }

        static void AddBook(List<Book> books, string[] commandChunks, bool isPasswordCorrect)
        {
            if (!isPasswordCorrect)
            {
                Console.WriteLine("Incorrect password");
                return;
            }

            Book book = StringToBook(commandChunks);

            books.Add(book);
        }

        static void RemoveBook(List<Book> books, string[] commandChunks, bool isPasswordCorrect)
        {
            if (!isPasswordCorrect)
            {
                Console.WriteLine("Incorrect password");
                return;
            }

            foreach(Book book in books)
            {
                if (book.Title == commandChunks[1] && book.Author == book.Author)
                {
                    books.Remove(book);
                    break;
                }
            }
        }

        static void EditBook(List<Book> books, string[] commandChunks, bool isPasswordCorrect) // edit; Title; Author; Property = newValue, etc.
        {
            if (!isPasswordCorrect)
            {
                Console.WriteLine("Incorrect password");
                return;
            }

            string title = commandChunks[1];
            string author = commandChunks[2];
            string[] properties = commandChunks[3].Split(", ");
            Book? currentBook = null;

            // Searches for matching book
            foreach (Book book in books)
            {
                if (book.Title == title && book.Author == author)
                {
                    currentBook = book;
                    break;
                }
                currentBook = null;
            }

            // If no book is found return out of method
            if (currentBook == null) return;

            foreach (string propertyChunk in properties)
            {
                string property = propertyChunk.Split(" = ")[0];
                string newValue = propertyChunk.Split(" = ")[1];
                
                switch (property.ToLower())
                {
                    case "title":
                        currentBook.Title = newValue;
                        break;
                    case "author":
                        currentBook.Author = newValue;
                        break;
                    case "genre":
                        currentBook.Genre = Enum.Parse<Genre>(newValue);
                        break;
                    case "description":
                        currentBook.Description = newValue;
                        break;
                    case "pages":
                        currentBook.Pages = int.Parse(newValue);
                        break;
                    case "tags":
                        string[] tagsChunk = newValue.Split(" | ");
                        int tags = tagsChunk.Select(Enum.Parse<Tags>).Cast<int>().Sum();
                        currentBook.Tags = (Tags)tags;
                        break;
                    default:
                        Console.WriteLine("No property found");
                        break;
                }
            }
        }

        static void PrintAll(List<Book> books)
        {
            Console.Clear();
            Console.WriteLine("These are all the books in the library:");

            foreach (Book book in books)
            {
                Console.WriteLine($"'{book.Title}' info: ");
                Console.WriteLine($"Author: '{book.Author}'");
                Console.WriteLine($"Genre: '{book.Genre}'");
                Console.WriteLine($"Description: '{book.Description}'");
                Console.WriteLine($"Pages: {book.Pages}");
                Console.WriteLine($"Tags: {book.Tags}");
                if (book.IsLend)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Availability: This book is NOT available.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Availability: This book is available.");
                }
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Press any button to continue");
        }
    }
}