using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models
{
    public class RegisterWebModel : LoginWebModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
    }
}
