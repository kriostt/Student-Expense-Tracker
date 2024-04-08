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
        public object Budgets { get; internal set; }

        // Seed initial data for categories using modelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Salary",
                    Type = "Income",
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Grocery",
                    Type = "Expense",

                },
                new Category
                {
                    CategoryId = 3,
                    Name = "Entertainment",
                    Type = "Expense",
                },
                new Category
                {
                    CategoryId = 4,
                    Name = "Eating outside",
                    Type = "Expense",
                },
                new Category
                {
                    CategoryId = 5,
                    Name = "Health and Wellness",
                    Type = "Expense",
                },
                new Category
                {
                    CategoryId = 6,
                    Name = "Shopping",
                    Type = "Expense",
                }
             );
        }
    }
}
