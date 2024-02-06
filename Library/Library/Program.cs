namespace Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] commandChunks;
            bool isRunning = true;
            List<Book> books = new();

            while (isRunning)
            {
                commandChunks = Console.ReadLine().Split("; ");
                switch (commandChunks[0].ToLower())
                {
                    case "add":
                        Console.WriteLine(commandChunks[0].ToLower());
                        AddBook(books, commandChunks); // add;Title;Author;Genre;Description;Pages;Tags
                        break;
                    case "print":
                        PrintAll(books);
                        break;
                }
            }
        }

        static void AddBook(List<Book> books, string[] commandChunks)
        {
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