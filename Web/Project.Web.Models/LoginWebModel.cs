using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models
{
    public class LoginWebModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool RememberMe { get; set; }
    }
}
