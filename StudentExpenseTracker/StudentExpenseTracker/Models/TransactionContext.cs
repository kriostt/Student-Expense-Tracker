using Microsoft.EntityFrameworkCore;

namespace StudentExpenseTracker.Models
{
    public class TransactionContext : DbContext
    {
        // initialize TransactionContext with options
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }

        // represents collection of transactions in the database
        public DbSet<Transaction> Transactions { get; set; }

        // represents collection of categories in the database
        public DbSet<Category> Categories { get; set; }
    }
}
