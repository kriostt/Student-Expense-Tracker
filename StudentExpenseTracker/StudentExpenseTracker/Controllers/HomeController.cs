using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentExpenseTracker.Models;

namespace StudentExpenseTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // instance of TransactionContext to interact with database
        private TransactionContext context { get; set; }

        // initialize HomeController with TransactionContext
        public HomeController(TransactionContext context)
        {
            this.context = context;
        }

        // action method handle requests for the home page
        public IActionResult Index()
        {
            // query the database to get transactions ordered by date, including Category
            var transactions = context.Transactions
                .Include(t => t.Category)
                .OrderBy(t => t.Date)
                .ToList();

            // pass distinct category names to the view
            ViewBag.DistinctCategoryNames = context.Categories.Select(c => c.Name).Distinct().ToList();

            // pass list of transactions to Index.cshtml
            return View(transactions);
        }

        // action method for displaying the filtered transactions
        public async Task<IActionResult> Filter(string categoryName, string type, string sortBy)
        {
            // set categoryName, type, and sorting option in ViewData
            ViewData["CategoryName"] = categoryName;
            ViewData["Type"] = type;
            ViewData["SortBy"] = sortBy;

            // query the database to get transactions, including Category
            var transactions = context.Transactions
                .Include(t => t.Category)
                .AsQueryable();

            // pass distinct category names to the view
            ViewBag.DistinctCategoryNames = context.Categories.Select(c => c.Name).Distinct().ToList();

            // check if a category was selected
            if (!string.IsNullOrEmpty(categoryName))
            {
                // filter by category name
                transactions = transactions.Where(t => t.Category.Name == categoryName);
            }

            // check if a category type was selected
            if (!string.IsNullOrEmpty(type))
            {
                // filter by type
                transactions = transactions.Where(t => t.Category.Type == type);
            }

            // execute the query and retrieve the list of transactions
            var filteredTransactions = await transactions.ToListAsync();

            // pass filtered transactions to the sort action method
            return Sort(filteredTransactions, sortBy);
        }

        public IActionResult Sort(List<Transaction> transactions, string sortBy)
        {
            // pass distinct category names to the view
            ViewBag.DistinctCategoryNames = context.Categories.Select(c => c.Name).Distinct().ToList();

            // check which sorting option was chosen
            switch (sortBy)
            {
                // order the transaction list according to chosen sorting option
                case "dateDescending":
                    transactions = transactions.OrderByDescending(t => t.Date).ToList();
                    break;
                case "dateAscending":
                    transactions = transactions.OrderBy(t => t.Date).ToList();
                    break;
                case "amountDescending":
                    transactions = transactions.OrderByDescending(t => t.Amount).ToList();
                    break;
                case "amountAscending":
                    transactions = transactions.OrderBy(t => t.Amount).ToList();
                    break;
                // set a default case when no sorting option is chosen
                default:
                    transactions = transactions.OrderBy(t => t.Date).ToList();
                    break;
            }

            // pass sorted transactions to Index.cshtml
            return View("Index", transactions);
        }
    }
}
