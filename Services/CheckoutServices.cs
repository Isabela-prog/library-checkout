using Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// creat a new checkout process for valid book and user
// update the returnDate of checkout, when book is returned
// Return all registered checkouts
// Locate  a checkout by its ID

namespace Library.Services
{
    public class CheckoutServices
    {
        private readonly CheckoutRepository _checkoutRepository;
        private readonly BookRepository _bookRepository;
        private readonly UserRepository _userRepository;

        public CheckoutServices(CheckoutRepository checkoutRepository, BookRepository bookRepository, UserRepository userRepository)
        {
            _checkoutRepository = checkoutRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public void CreateCheckout(int checkoutId, int bookId, int userId)
        {
            var book = _bookRepository.GetById(bookId);
            var user = _userRepository.GetById(userId);
            if (book == null)
            {
                throw new ArgumentException("Book not found.", nameof(bookId));
            }
            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(userId));
            }

            var checkout = new Models.Checkout(checkoutId, book, user);
            _checkoutRepository.Add(checkout);

            book.Available = false;
            _bookRepository.Update(book);
        }

        public void RegisterReturn(int checkoutId)
        {
            var checkout = _checkoutRepository.GetById(checkoutId);

            if (checkout == null)
                throw new ArgumentException("Checkout not found.", nameof(checkoutId));
            if (checkout.ReturnDate != null)
                throw new Exception("Livro já foi devolvido.");

            checkout.RegisterDevolucion();
            _checkoutRepository.Update(checkout);
            var book = _bookRepository.GetById(checkout.BookId);

            if (book != null)
            {
                book.Available = true;
                _bookRepository.Update(book);
            }
        }

        public List<Models.Checkout> GetAllCheckouts()
        {
            return _checkoutRepository.GetAll();
        }

        public Models.Checkout GetCheckoutById(int id) { 
            return _checkoutRepository.GetById(id);
        }
    }
}
