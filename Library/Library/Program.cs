﻿using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Library
{
    internal class Program
    {
        static Book? pinnedBook = null;
        static List<Book> books = new();
        static List<Book> lendBooks = new();
        static void Main(string[] args)
        {
            string[] commandChunks;
            bool isRunning = true;

            ReadFile();

            while (isRunning)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Gray;
                //Print menu
                Help();
                Console.WriteLine("Pinned book:");
                if (pinnedBook != null) PrintBook(pinnedBook);
                Console.Write("Enter a command: ");

                commandChunks = Console.ReadLine().Split("; ");
                switch (commandChunks[0].ToLower())
                {
                    case "add":
                        AddBook(commandChunks, EnterPassword());
                        WriteFile();
                        break;
                    case "print_all":
                        PrintAll();
                        break;
                    case "remove":
                        RemoveBook(commandChunks, EnterPassword());
                        WriteFile();
                        break;
                    case "edit":
                        EditBook(commandChunks, EnterPassword());
                        WriteFile();
                        break;
                    case "pin":
                        PinBook(commandChunks);
                        break;
                    case "search":
                        SearchBook(commandChunks);
                        break;
                    case "search_title":
                        SearchTitle(commandChunks);
                        break;
                    case "search_author":
                        SearchAuthor(commandChunks);
                        break;
                    default:
                        Console.WriteLine("No command found");
                        break;
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Press any button to continue");
                Console.ReadKey();
            }
        }

        static void Help()
        {
            // Write title
            Console.WriteLine("These are the available genres:                   These are the available tags:");
            Console.ForegroundColor = ConsoleColor.Gray;
            List<string> genres = new();
            List<string> tags = new();
            int genreAmount = 0;
            // Add genres and tags to separate lists
            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                genreAmount++;
                genres.Add(genre.ToString());
            }
            int tagAmount = 0;
            foreach (Tags tag in Enum.GetValues(typeof(Tags)))
            {
                tagAmount++;
                tags.Add(tag.ToString());
            }
            // Check whether there are more tags och genres
            int lineAmount;
            if (genreAmount > tagAmount) lineAmount = genreAmount;
            else lineAmount = tagAmount;

            // Construct a line that first prints the genre, then the tag so that the tag has 50 characters before it.
            for (int i = 0; i < lineAmount; i++)
            {
                string line = "";
                string genre = "";
                string tag = "";
                if (i < genres.Count) genre = genres[i].ToString();
                if (i < tags.Count) tag = tags[i].ToString();

                int spaces = 50 - genre.Length;
                line += genre;
                for (int j = 0; j < spaces; j++)
                {
                    line += " ";
                }
                line += tag;
                Console.WriteLine(line);
            }

            Console.WriteLine("--------------------------------------------------------------------------------");

            // prints a list of commands
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
            Console.WriteLine("search; Title; Author");
            Console.WriteLine("search_author; Author");
            Console.WriteLine("search_title; Title");
            Console.WriteLine("pin; Title; Author");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("------------------------");
        }

        static void PinBook(string[] commandChunks)
        {
            foreach(Book book in books)
            {
                if (book.Title == commandChunks[1] && book.Author == commandChunks[2])
                {
                    pinnedBook = book;
                    Console.WriteLine($"{book.Title} by {book.Author} is now pinned");
                }
            }
        }

        static void WriteFile()
        {
            StreamWriter writeFile = new("LibraryData.txt");

            foreach (Book book in books)
            {
                writeFile.WriteLine($"book;{book.Title};{book.Author};{book.Genre};{book.Description};{book.Pages};{book.Tags};{book.IsLend}");
            }

            writeFile.Close();
        }

        static void ReadFile()
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

        static void AddBook(string[] commandChunks, bool isPasswordCorrect)
        {
            if (!isPasswordCorrect)
            {
                Console.WriteLine("Incorrect password");
                return;
            }

            Book book = StringToBook(commandChunks);

            books.Add(book);
        }

        static void RemoveBook(string[] commandChunks, bool isPasswordCorrect)
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

        static void EditBook(string[] commandChunks, bool isPasswordCorrect) // edit; Title; Author; Property = newValue, etc.
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

        static void PrintBook(Book? book)
        {
            if (book == null) return;

            Console.WriteLine($"'{book.Title}' by {book.Author} info: ");
            Console.WriteLine($"Genre: '{book.Genre}'");
            Console.WriteLine($"Description: '{book.Description}'");
            Console.WriteLine($"Pages: {book.Pages}");
            Console.WriteLine($"Tags: {book.Tags}");
            if (book.IsLend)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Availability: This book is NOT available.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Availability: This book is available.");
                Console.ForegroundColor= ConsoleColor.Gray;
            }
            Console.WriteLine();
        }
        static void PrintAll()
        {
            Console.Clear();
            Console.WriteLine("These are all the books in the library:");

            foreach (Book book in books)
            {
                PrintBook(book);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }

        static void SearchBook(string[] commandChunks)
        {
            foreach(Book book in books)
            {
                if (commandChunks[1] == book.Title && commandChunks[2] == book.Author)
                {
                    Console.Clear();
                    Console.WriteLine("1.");
                    PrintBook(book);

                    if (Console.ReadLine() == ) // TODO Make it posible to pin from search

                    return;
                }
            }

            Console.WriteLine("No book found.");
        }

        static void SearchTitle(string[] commandChunks)
        {
            bool bookFound = false;
            Console.Clear();

            foreach (Book book in books)
            {
                if (commandChunks[1] == book.Title)
                {
                    PrintBook(book);
                    bookFound = true;
                }
            }

            if (!bookFound) Console.WriteLine("No books found.");
        }

        static void SearchAuthor(string[] commandChunks)
        {
            bool bookFound = false;
            Console.Clear();

            foreach (Book book in books)
            {
                if (commandChunks[1] == book.Author)
                {
                    PrintBook(book);
                    bookFound = true;
                }
            }

            if (!bookFound) Console.WriteLine("No books found.");
        }
    }
}