using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    internal class Checkout
    {
        public int CheckoutId { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public DateTime CheckoutDate { get; private set; }
        public DateTime? ReturnDate { get; private set; } //? -> the variable can be null

        public Checkout(int checkoutId, Book book, User user)
        {
            CheckoutId = checkoutId;
            BookId = book.Id;
            UserId = user.Id;
            CheckoutDate = DateTime.Now; // capture exact date time in moment of checkout
            ReturnDate = null; // in moment of checkout, return date is null
        }

        public void RegisterDevolucion()
        {
            //return date time exactly to devolution moment
            ReturnDate = DateTime.Now;
        }

    }

}
