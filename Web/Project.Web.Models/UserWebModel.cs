using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models
{
    public class UserWebModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public IList<string> Roles { get; set; }
    }
}
