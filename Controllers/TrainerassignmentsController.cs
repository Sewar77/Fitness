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
    public class TrainerassignmentsController : Controller
    {
        private readonly ModelContext _context;

        public TrainerassignmentsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Trainerassignments
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Trainerassignments.Include(t => t.Member).Include(t => t.Trainer);
            return View(await modelContext.ToListAsync());
        }

        // GET: Trainerassignments/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Trainerassignments == null)
            {
                return NotFound();
            }

            var trainerassignment = await _context.Trainerassignments
                .Include(t => t.Member)
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.Assignmentid == id);
            if (trainerassignment == null)
            {
                return NotFound();
            }

            return View(trainerassignment);
        }

        // GET: Trainerassignments/Create
        public IActionResult Create()
        {
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid");
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Trainerassignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Assignmentid,Trainerid,Memberid,Assignedat")] Trainerassignment trainerassignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainerassignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", trainerassignment.Memberid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", trainerassignment.Trainerid);
            return View(trainerassignment);
        }

        // GET: Trainerassignments/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Trainerassignments == null)
            {
                return NotFound();
            }

            var trainerassignment = await _context.Trainerassignments.FindAsync(id);
            if (trainerassignment == null)
            {
                return NotFound();
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", trainerassignment.Memberid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", trainerassignment.Trainerid);
            return View(trainerassignment);
        }

        // POST: Trainerassignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Assignmentid,Trainerid,Memberid,Assignedat")] Trainerassignment trainerassignment)
        {
            if (id != trainerassignment.Assignmentid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainerassignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerassignmentExists(trainerassignment.Assignmentid))
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
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", trainerassignment.Memberid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", trainerassignment.Trainerid);
            return View(trainerassignment);
        }

        // GET: Trainerassignments/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Trainerassignments == null)
            {
                return NotFound();
            }

            var trainerassignment = await _context.Trainerassignments
                .Include(t => t.Member)
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.Assignmentid == id);
            if (trainerassignment == null)
            {
                return NotFound();
            }

            return View(trainerassignment);
        }

        // POST: Trainerassignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Trainerassignments == null)
            {
                return Problem("Entity set 'ModelContext.Trainerassignments'  is null.");
            }
            var trainerassignment = await _context.Trainerassignments.FindAsync(id);
            if (trainerassignment != null)
            {
                _context.Trainerassignments.Remove(trainerassignment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerassignmentExists(decimal id)
        {
          return (_context.Trainerassignments?.Any(e => e.Assignmentid == id)).GetValueOrDefault();
        }
    }
}
