using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPA3_0.Services
{
    public class InMemoryUserService : IUserService
    {
        private List<User> _users = new List<User>();
        string postgresUsername = Environment.GetEnvironmentVariable("postgresUsername");
        string postgresPassword = Environment.GetEnvironmentVariable("postgresPassword");



        public InMemoryUserService()
        {
            _users = UpdateUsers();
        }
        public List<User> UpdateUsers()
        {
            var host = "localhost";
            var port = "54321";
            var db = "WebPA";
            var connString = $"Host={host};Port={port};Username={postgresUsername};Password={postgresPassword};Database={db}";
            var conn = new NpgsqlConnection(connString);
            using (var cmd = new NpgsqlCommand("select * from users", conn))
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var user_id = (int)reader["user_id"];
                    var user_email = (string)reader["user_email"];
                    var pw = (string)reader["pw"];
                    var isadmin = (bool)reader["isadmin"];
                    User user = new User(user_id, user_email, pw, isadmin);
                    _users.Add(user);
                }
                conn.Close();
            }
            return _users;
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetOne(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
        public User GetOne(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);

        }
       


        public User Login(string email, string password)
        {
            var user = GetOne(email);
            if (user==null)
            {
                return null;
            }
            else if(user.Password != password)
            {
                return null;
            }
            return user;
        }

       
        User IUserService.Register(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
