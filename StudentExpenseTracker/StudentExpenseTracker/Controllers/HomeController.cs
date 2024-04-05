using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentExpenseTracker.Models;
using System.Globalization;

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
        public async Task<IActionResult> Filter(string search, string categoryName, string type, DateTime? startDate, DateTime? endDate, string sortBy)
        {
            // set variables in ViewData
            ViewData["Search"] = search;
            ViewData["CategoryName"] = categoryName;
            ViewData["Type"] = type;
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;
            ViewData["SortBy"] = sortBy;

            // query the database to get transactions, including Category
            var transactions = context.Transactions
                .Include(t => t.Category)
                .AsQueryable();

            // pass distinct category names to the view
            ViewBag.DistinctCategoryNames = context.Categories.Select(c => c.Name).Distinct().ToList();

            // check if a search string was provided
            if (!string.IsNullOrEmpty(search))
            {
                // filter by description
                transactions = transactions.Where(t => t.Description.ToLower().Contains(search.ToLower()));
            }
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

            // check if start date was provided
            if (startDate != null)
            {
                // if start date provided, find transactions with date greater than or equal to provided date
                transactions = transactions.Where(t => t.Date.Date >= startDate.Value.Date);
            }

            // check if end date was provided
            if (endDate != null)
            {
                // if end date provided, find transactions with date less than or equal to provided date
                transactions = transactions.Where(t => t.Date.Date <= endDate.Value.Date);
            }

            // execute the query and retrieve the list of transactions
            var filteredTransactions = await transactions.ToListAsync();

            // Calculate Total Income
            double TotalIncome = filteredTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C2");

            // Calculate Total Expense
            double TotalExpense = filteredTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C2");

            // Calculate Balance
            double Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C2}", Balance);

            // pass filtered transactions to the sort action method
            return Sort(filteredTransactions, search, startDate, endDate, sortBy);
        }

        public IActionResult Sort(List<Transaction> transactions, string search, DateTime? startDate, DateTime? endDate, string sortBy)
        {
            // set variables in ViewData
            ViewData["Search"] = search;
            ViewData["StartDate"] = startDate;
            ViewData["EndDate"] = endDate;

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

            // Calculate Total Income
            double TotalIncome = transactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C2");

            // Calculate Total Expense
            double TotalExpense = transactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C2");

            // Calculate Balance
            double Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C2}", Balance);

            // pass sorted transactions to Index.cshtml
            return View("Index", transactions);
        }
    }
}
