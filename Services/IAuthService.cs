using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    // Interface para autenticação de usuários
    public interface IAuthService
    {
        User Register(
             string username,
             string password,
             string fullName,
             string email,
             string city,
             string state,
             string address
         );

        User? Authenticate(string username, string password);

    }
}
