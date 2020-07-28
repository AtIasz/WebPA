using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPA3_0
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public User(int id, string email, string password, bool isadmin)
        {
            this.Id = id;
            this.Email = email;
            this.Password = password;
            this.IsAdmin = isadmin;
        }
    }
}
