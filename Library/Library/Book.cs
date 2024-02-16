using System.Runtime.CompilerServices;

namespace Library
{
    internal class Book
    {
        public Book(string title, string author, Genre genre, string description, int pages, Tags tags, bool isLend) 
        {
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
            Pages = pages;
            Tags = tags;
            IsLend = isLend;
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public Tags Tags { get; set; }
        public bool IsLend {  get; set; }
    }
}
