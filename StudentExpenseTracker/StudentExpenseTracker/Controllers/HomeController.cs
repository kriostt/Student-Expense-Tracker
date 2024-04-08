using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentExpenseTracker.Models;
using System.Globalization;

namespace StudentExpenseTracker.Controllers
{
    [Authorize]
    [Route("Home")]
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
        public async Task<ActionResult> Index()
        {
            List<Transaction> SelectedTransactions = await context.Transactions
                .Include(x => x.Category)
                .ToListAsync();

            // Total Income 
            double TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C2");

            // Total Expense 
            double TotalExpense = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C2");

            // Balance 
            double Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C2}", Balance);

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
        [HttpGet]
        [Route("[action]/{search?}/{categoryName?}/{type?}/{startDate?}/{endDate?}/{sortBy?}")]
        public IActionResult Filter(string search, string categoryName, string type, DateTime? startDate, DateTime? endDate, string sortBy)
        {
            // set variables in ViewData
            ViewData["Search"] = search;
            ViewData["CategoryName"] = categoryName;
            ViewData["Type"] = type;
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;
            ViewData["SortBy"] = sortBy;

            // pass distinct category names to the view
            ViewBag.DistinctCategoryNames = context.Categories.Select(c => c.Name).Distinct().ToList();

            // Retrieve the filtered transactions based on provided parameters
            var filteredTransactions = GetFilteredTransactions(search, categoryName, type, startDate, endDate);

            // Apply sorting logic
            var sortedTransactions = ApplySorting(filteredTransactions, sortBy);

            // Calculate Total Income
            double TotalIncome = sortedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C2");

            // Calculate Total Expense
            double TotalExpense = sortedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C2");

            // Calculate Balance
            double Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C2}", Balance);

            // Pass sorted transactions to the Index view
            return View("Index", sortedTransactions);
        }

        // helper method to retrieve filtered transactions
        private IQueryable<Transaction> GetFilteredTransactions(string search, string categoryName, string type, DateTime? startDate, DateTime? endDate)
        {
            var transactions = context.Transactions.Include(t => t.Category).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                transactions = transactions.Where(t => t.Description.ToLower().Contains(search.ToLower()));
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                transactions = transactions.Where(t => t.Category.Name == categoryName);
            }

            if (!string.IsNullOrEmpty(type))
            {
                transactions = transactions.Where(t => t.Category.Type == type);
            }

            if (startDate != null)
            {
                transactions = transactions.Where(t => t.Date.Date >= startDate.Value.Date);
            }

            if (endDate != null)
            {
                transactions = transactions.Where(t => t.Date.Date <= endDate.Value.Date);
            }

            return transactions;
        }

        // helper method to apply sorting
        private List<Transaction> ApplySorting(IQueryable<Transaction> transactions, string sortBy)
        {
            switch (sortBy)
            {
                case "dateDescending":
                    return transactions.OrderByDescending(t => t.Date).ToList();
                case "dateAscending":
                    return transactions.OrderBy(t => t.Date).ToList();
                case "amountDescending":
                    return transactions.OrderByDescending(t => t.Amount).ToList();
                case "amountAscending":
                    return transactions.OrderBy(t => t.Amount).ToList();
                default:
                    return transactions.OrderBy(t => t.Date).ToList();
            }
        }
    }
}
