using System.ComponentModel.DataAnnotations;

namespace StudentExpenseTracker.Models
{
    public class Register
    {
        // name property
        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; }

        // email property
        [Required(ErrorMessage = "Please enter your email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // password property
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // confirm password property to confirm the password entered
        [Compare("Password", ErrorMessage  = "Passwords don't match.")] 
        [Display(Name = "Confirm Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
    }
}
