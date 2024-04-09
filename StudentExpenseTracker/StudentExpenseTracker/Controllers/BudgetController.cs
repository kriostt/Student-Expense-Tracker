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
    public class BudgetController : Controller
    {
        private readonly TransactionContext _context;

        public BudgetController(TransactionContext context)
        {
            _context = context;
        }

        // GET: Budget
        // Action method to retrieve and display all budgets
        public async Task<IActionResult> Index()
        {
            // Retrieving budgets including their associated category
            var transactionContext = _context.Budgets.Include(b => b.Category);
            return View(await transactionContext.ToListAsync());
        }

        // GET: Budget/Create
        // Action method to display the form for creating a new budget
        public IActionResult Create()
        {
            // Providing a SelectList for categories to be displayed in a dropdown
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Budget/Create
        // Action method to handle the creation of a new budget
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BudgetId,Name,CategoryId,Amount")] Budget budget)
        {
            // Checking if the model state is valid
            if (ModelState.IsValid)
            {
                // Adding the new budget to the context and saving changes
                _context.Add(budget);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If model state is not valid, re-rendering the create view with errors
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", budget.CategoryId);
            return View(budget);
        }

        // GET: Budget/Edit/5
        // Action method to display the form for editing a budget
        public async Task<IActionResult> Edit(int? id)
        {
            // Checking if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieving the budget with the provided id
            var budget = await _context.Budgets.FindAsync(id);
            // Checking if the budget is not found
            if (budget == null)
            {
                return NotFound();
            }
            // Providing a SelectList for categories to be displayed in a dropdown
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", budget.CategoryId);
            return View(budget);
        }

        // POST: Budget/Edit/5
        // Action method to handle the editing of a budget
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BudgetId,Name,CategoryId,Amount")] Budget budget)
        {
            // Checking if the provided id matches the budget's id
            if (id != budget.BudgetId)
            {
                return NotFound();
            }

            // Checking if the model state is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Updating the budget in the context and saving changes
                    _context.Update(budget);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudgetExists(budget.BudgetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // If model state is not valid, re-rendering the edit view with errors
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", budget.CategoryId);
            return View(budget);
        }

        // GET: Budget/Delete/5
        // Action method to display the confirmation page for deleting a budget
        public async Task<IActionResult> Delete(int? id)
        {
            // Checking if the provided id is null
            if (id == null)
            {
                return NotFound();
            }

            // Retrieving the budget with the provided id including its associated category
            var budget = await _context.Budgets
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.BudgetId == id);
            // Checking if the budget is not found
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // POST: Budget/Delete/5
        // Action method to handle the deletion of a budget
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieving the budget with the provided id
            var budget = await _context.Budgets.FindAsync(id);
            // Checking if the budget exists
            if (budget != null)
            {
                // Removing the budget from the context and saving changes
                _context.Budgets.Remove(budget);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Method to check if a budget with the provided id exists
        private bool BudgetExists(int id)
        {
            return _context.Budgets.Any(e => e.BudgetId == id);
        }
    }
}
