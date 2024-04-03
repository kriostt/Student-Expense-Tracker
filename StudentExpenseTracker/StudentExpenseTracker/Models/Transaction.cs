using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentExpenseTracker.Models
{
    public class Transaction
    {
        // EF core will configure the database to generate this value
        public int TransactionId { get; set; }

        // transaction category
        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // transaction amount
        [Required(ErrorMessage = "Please enter an amount.")]
        public int Amount { get; set; }

        // transaction description
        public string? Description { get; set; }

        // transaction date
        [Required(ErrorMessage = "Please enter a date.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
