using System;
using Microsoft.AspNetCore.Identity;

namespace Project.Data.Models
{
    public class AppRole : IdentityRole
    {
        public const string Admin = "Administrator";
        public const string User = "User";
    }
}
