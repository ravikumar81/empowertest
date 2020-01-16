using System.ComponentModel.DataAnnotations;

namespace Empower.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }    
    }

    public class LoginPostDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Email")]       
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a Password")]
        public string Password { get; set; }
    }
}
