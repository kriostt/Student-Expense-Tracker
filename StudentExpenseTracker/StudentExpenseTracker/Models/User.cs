using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudentExpenseTracker.Models
{
    // define custom user class that inherits IdentityUser
    public class User : IdentityUser
    {
        // name property
        [Required(ErrorMessage = "Please enter your name.")]
        public string Name { get; set; }
    }
}
