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
                writeFile.WriteLine($"{book.Title};{book.Author};{book.Genre};{book.Description};{book.Pages};{book.Tags}");
            }

            writeFile.Close();
        }

        static void ReadFile()
        {
            StreamReader readFile = new("LibraryData.txt");
            string s;

            while ((s = readFile.ReadLine()) != null)
            {

            }
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

        static Book StringToBook(string[] s)
        {
            string title = s[1];
            string author = s[2];
            Genre genre = Enum.Parse<Genre>(s[3]);
            string description = s[4];
            int pages = int.Parse(s[5]);
            string[] tagsChunk = s[6].Split(",");
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

            string title = commandChunks[1];
            string author = commandChunks[2];
            Genre genre = Enum.Parse<Genre>(commandChunks[3]);
            string description = commandChunks[4];
            int pages = int.Parse(commandChunks[5]);
            string[] tagsChunk = commandChunks[6].Split(",");
            int tags = tagsChunk.Select(Enum.Parse<Tags>).Cast<int>().Sum();

            books.Add(new Book(title, author, genre, description, pages, (Tags)tags));
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