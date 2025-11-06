using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Cpf { get; set; }
        public User(int id, string name, string email, string passowrd, string userName, DateTime createdAt, int cpf)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = passowrd;
            UserName = userName;
            CreatedAt = createdAt;
            Cpf = cpf;
        }
    }
}
