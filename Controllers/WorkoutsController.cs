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
    public class WorkoutsController : Controller
    {
        private readonly ModelContext _context;

        public WorkoutsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Workouts
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Workouts.Include(w => w.Member).Include(w => w.Trainer);
            return View(await modelContext.ToListAsync());
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.Member)
                .Include(w => w.Trainer)
                .FirstOrDefaultAsync(m => m.Workoutid == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid");
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid");
            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Workoutid,Trainerid,Memberid,Goals,Createdat")] Workout workout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", workout.Memberid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", workout.Trainerid);
            return View(workout);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", workout.Memberid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", workout.Trainerid);
            return View(workout);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Workoutid,Trainerid,Memberid,Goals,Createdat")] Workout workout)
        {
            if (id != workout.Workoutid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.Workoutid))
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
            ViewData["Memberid"] = new SelectList(_context.Users, "Userid", "Userid", workout.Memberid);
            ViewData["Trainerid"] = new SelectList(_context.Users, "Userid", "Userid", workout.Trainerid);
            return View(workout);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Workouts == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.Member)
                .Include(w => w.Trainer)
                .FirstOrDefaultAsync(m => m.Workoutid == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Workouts == null)
            {
                return Problem("Entity set 'ModelContext.Workouts'  is null.");
            }
            var workout = await _context.Workouts.FindAsync(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(decimal id)
        {
          return (_context.Workouts?.Any(e => e.Workoutid == id)).GetValueOrDefault();
        }
    }
}
