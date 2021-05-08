namespace Project.Services.Models.User
{
    public class LoginServiceModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
