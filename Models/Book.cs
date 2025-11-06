using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book : Author
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublicationYear { get; set; }
        public string Genre { get; set; }

        public Book(int id, string title, DateTime publicationYear, string genre, string name, string authorName, string authorNationality) : base(authorName, authorNationality)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Book title cannot be null or empty.", nameof(title));
            }
            if (string.IsNullOrWhiteSpace(genre))
            {
                throw new ArgumentException("Literary genre cannot be null or empty.", nameof(genre));
            }

            Id = id;
            Title = title;
            PublicationYear = publicationYear;
            Genre = genre;
        }
    }
}
