using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentExpenseTracker.Models
{
    public class Budget
    {
        // EF core will configure the database to generate this value
        public int BudgetId { get; set; }

        // Name of the budget (optional)
        [Required(ErrorMessage = "Please enter a name for the budget.")]
        public string Name { get; set; }

        // Category for which the budget is set
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Budget amount
        [Required(ErrorMessage = "Please enter a budget amount.")]
        [Range(0, double.MaxValue, ErrorMessage = "Budget amount must be a non-negative number.")]
        public double Amount { get; set; }

        // Additional properties as needed (e.g., start date, end date)
    }
}
