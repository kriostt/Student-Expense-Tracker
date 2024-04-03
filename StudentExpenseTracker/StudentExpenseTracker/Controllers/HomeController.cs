using Microsoft.AspNetCore.Mvc;
using StudentExpenseTracker.Models;
using System.Diagnostics;

namespace StudentExpenseTracker.Controllers
{
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
            // retrieves list of transactions from database, ordered by date
            var transactions = context.Transactions
                .OrderBy(t => t.Date)
                .ToList();

            // pass list of transactions to Index.cshtml view
            return View(transactions);
        }
    }
}
