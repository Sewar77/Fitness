using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;

namespace MyFitnessLife.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ModelContext _context;

        public InvoicesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Invoices.Include(i => i.Subscription).Include(i => i.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Subscription)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Invoiceid == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Subscriptionid", "Subscriptionid");
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Invoiceid,Subscriptionid,Userid,Amount,Invoicedate,Pdfpath")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Subscriptionid", "Subscriptionid", invoice.Subscriptionid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", invoice.Userid);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Subscriptionid", "Subscriptionid", invoice.Subscriptionid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", invoice.Userid);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Invoiceid,Subscriptionid,Userid,Amount,Invoicedate,Pdfpath")] Invoice invoice)
        {
            if (id != invoice.Invoiceid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Invoiceid))
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
            ViewData["Subscriptionid"] = new SelectList(_context.Subscriptions, "Subscriptionid", "Subscriptionid", invoice.Subscriptionid);
            ViewData["Userid"] = new SelectList(_context.Users, "Userid", "Userid", invoice.Userid);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Subscription)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Invoiceid == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'ModelContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(decimal id)
        {
          return (_context.Invoices?.Any(e => e.Invoiceid == id)).GetValueOrDefault();
        }
    }
}
