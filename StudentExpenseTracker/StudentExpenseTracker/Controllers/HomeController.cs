using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentExpenseTracker.Models;
using System.Diagnostics;
using System.Transactions;

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
            // retrieves list of transactions with Category from database, ordered by date
            var transactions = context.Transactions
                .Include(t => t.Category)
                .OrderBy(t => t.Date)
                .ToList();

            // pass distinct category names to the view
            ViewBag.DistinctCategoryNames = context.Categories.Select(c => c.Name).Distinct().ToList();

            // pass list of transactions to Index.cshtml view
            return View(transactions);
        }

        // action method for displaying the filtered transactions
        public async Task<IActionResult> Filter(string categoryName, string type)
        {
            // Set categoryName and type in ViewData
            ViewData["CategoryName"] = categoryName;
            ViewData["Type"] = type;

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

            // pass filtered transactions to Index.cshtml
            return View("Index", filteredTransactions);
        }
    }
}
