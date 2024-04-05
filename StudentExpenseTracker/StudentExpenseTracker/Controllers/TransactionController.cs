using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentExpenseTracker.Models;

namespace StudentExpenseTracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TransactionContext _context;

        // Constructor to initialize the controller with the TransactionContext
        public TransactionController(TransactionContext context)
        {
            _context = context;
        }

        // GET: Transaction - Displays a list of all transactions
        public async Task<IActionResult> Index()
        {
            var transactionContext = _context.Transactions.Include(t => t.Category);
            return View(await transactionContext.ToListAsync());
        }


        // GET: Transaction/Add - Displays a form to add a new transaction
        public IActionResult Add()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Transaction/Add - Adds a new transaction to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("TransactionId,CategoryId,Amount,Description,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transaction/Edit/5 - Displays a form to edit a transaction
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transaction/Edit/5 - Updates a transaction in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,CategoryId,Amount,Description,Date")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transaction/Delete/5 - Displays a confirmation page before deleting a transaction
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transaction/Delete/5 - Deletes a transaction from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        // Helper method to check if a transaction exists based on its ID
        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
