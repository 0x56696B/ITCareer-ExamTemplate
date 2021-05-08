namespace Project.Services.Models.User
{
    public class RegisterServiceModel : LoginServiceModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
