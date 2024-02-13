namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string password = new StreamReader("Password.txt").ReadLine();
            string[] commandChunks;
            bool isRunning = true;
            List<Book> books = new();
            List<Book> lendBooks = new();

            ReadFile(books);

            while (isRunning)
            {
                commandChunks = Console.ReadLine().Split("; ");
                switch (commandChunks[0].ToLower())
                {
                    case "add":
                        Console.WriteLine(commandChunks[0].ToLower());
                        AddBook(books, commandChunks, EnterPassword(password)); // add;Title;Author;Genre;Description;Pages;Tags
                        WriteFile(books);
                        break;
                    case "print":
                        PrintAll(books);
                        break;
                }
            }
        }

        static void WriteFile(List<Book> books)
        {
            StreamWriter writeFile = new("LibraryData.txt");

            foreach (Book book in books)
            {
                writeFile.WriteLine($"book;{book.Title};{book.Author};{book.Genre};{book.Description};{book.Pages};{book.Tags}");
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

        static bool EnterPassword(string password)
        {
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
            string[] tagsChunk = sChunks[6].Split(",");
            int tags = tagsChunk.Select(Enum.Parse<Tags>).Cast<int>().Sum();
            return new Book(title, author, genre, description, pages, (Tags)tags);
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

        static void PrintAll(List<Book> books)
        {
            foreach (Book book in books)
            {
                Console.WriteLine($"'{book.Title}' info: ");
                Console.WriteLine($"Author: '{book.Author}'");
                Console.WriteLine($"Genre: '{book.Genre}'");
                Console.WriteLine($"Description: '{book.Description}'");
                Console.WriteLine($"Number of pages: {book.Pages}");
                Console.WriteLine($"Tags: {book.Tags}");
                Console.WriteLine();
            }
        }
    }
}