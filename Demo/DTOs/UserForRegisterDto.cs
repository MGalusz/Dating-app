using System.ComponentModel.DataAnnotations;

namespace Demo.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
         [Required]
         [StringLength(8, MinimumLength = 4, ErrorMessage =  "You must specify password bettwen 4 and 8 characters")]
        public string Password { get; set; }
    }
}