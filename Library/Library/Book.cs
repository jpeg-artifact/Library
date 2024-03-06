using System.Runtime.CompilerServices;

namespace Library
{
    internal class Book
    {
        public Book(string title, string author, Genre genre, string description, int pages, bool isLend) 
        {
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
            Pages = pages;
            IsLend = isLend;
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public bool IsLend { get; set; }

        public void Print()
        {
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Author: {Author}");
            Console.WriteLine($"Genre: {Genre}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Pages: {Pages}");
            if (IsLend)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Availability: Not availabe");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Availability: Availabe");
            }
            Console.ResetColor();
        }
    }
}
