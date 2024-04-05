using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentExpenseTracker.Models
{
    public class Transaction
    {
        // EF core will configure the database to generate this value
        public int TransactionId { get; set; }

        // transaction category
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // transaction amount
        [Required(ErrorMessage = "Please enter an amount.")]
        public double Amount { get; set; }

        // transaction description
        public string? Description { get; set; }

        // transaction date
        [Required(ErrorMessage = "Please enter a date.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                // Check if the associated Category is null or if its Type is "Expense"
                // If true, prepend "-" to the formatted amount, indicating it's an expense
                // Otherwise, prepend "+" to the formatted amount, indicating it's income
                // Format the Amount property to currency with two decimal places
                return ((Category == null || Category.Type =="Expense") ? "-" : "+") + Amount.ToString("C2");
            }
        }
    }
}
