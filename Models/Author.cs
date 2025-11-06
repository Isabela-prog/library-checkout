using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Author
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
        public Author(string name, string nationality)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Author name cannot be null or empty.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(nationality))
            {
                throw new ArgumentException("Author nationality cannot be null or empty.", nameof(nationality));
            }

            Name = name;
            Nationality = nationality;
        }
    }
}
