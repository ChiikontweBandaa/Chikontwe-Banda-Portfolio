using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }

        public User(string username, string password, string role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

        User adminUser = new User("admin", "password123", "Admin");
    }
}
