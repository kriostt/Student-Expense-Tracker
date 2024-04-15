using System.ComponentModel.DataAnnotations;

namespace StudentExpenseTracker.Models
{
    public class Login
    {
        // username property
        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; }

        // password property
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
