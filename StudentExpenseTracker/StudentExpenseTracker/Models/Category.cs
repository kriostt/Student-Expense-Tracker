using System.ComponentModel.DataAnnotations;

namespace StudentExpenseTracker.Models
{
    public class Category
    {
        // EF core will configure the database to generate this value
        public int CategoryId { get; set; }

        // category name
        [Required(ErrorMessage = "Please enter a name.")]
        public string Name { get; set; }

        // category type (expense or income)
        [Required(ErrorMessage = "Please select a type.")]
        public string Type { get; set; }
    }
}
