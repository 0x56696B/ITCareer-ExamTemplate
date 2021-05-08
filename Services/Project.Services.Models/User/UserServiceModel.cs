using System;
using System.Collections.Generic;

namespace Project.Services.Models.User
{
    public class UserServiceModel
    {
        public Guid Id { get; set; }

        public IList<string> Roles { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
