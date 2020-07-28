using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPA3_0.Services
{
    public interface IUserService
    {
        public List<User> GetAll();
        public User GetOne(int id);
        public User GetOne(string email);
        public User Login(string email, string password);
        public User Register(string email, string password);
        public List<User> UpdateUsers();
    }
}
