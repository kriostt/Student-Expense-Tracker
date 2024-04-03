using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentExpenseTracker.Models
{
    // define custom DbContext class that inherits IdentityDbContext
    public class UserContext : IdentityDbContext<User>
    {
        // constructor to initialize the DbContext with provided options
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}
