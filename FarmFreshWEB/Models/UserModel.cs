using System.ComponentModel.DataAnnotations;

namespace FarmFreshWEB.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum length is 2")]
        [Display(Name = "Username")]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minimun length is 4")]
        public string Password { get; set; }
    }
}
