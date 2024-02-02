using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class Book
    {
        public Book(string title, string author, Genre genre, string description, int pages, Tags tags) 
        {
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
            Pages = pages;
            Tags = tags;
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public Tags Tags { get; set; }
    }
    }
}
