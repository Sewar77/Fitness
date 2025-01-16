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
    public class WhatoffersController : Controller
    {
        private readonly ModelContext _context;

        public WhatoffersController(ModelContext context)
        {
            _context = context;
        }

        // GET: Whatoffers
        public async Task<IActionResult> Index()
        {
              return _context.Whatoffers != null ? 
                          View(await _context.Whatoffers.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Whatoffers'  is null.");
        }

        // GET: Whatoffers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Whatoffers == null)
            {
                return NotFound();
            }

            var whatoffer = await _context.Whatoffers
                .FirstOrDefaultAsync(m => m.Offerid == id);
            if (whatoffer == null)
            {
                return NotFound();
            }

            return View(whatoffer);
        }

        // GET: Whatoffers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Whatoffers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Offerid,Title,Text")] Whatoffer whatoffer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(whatoffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(whatoffer);
        }

        // GET: Whatoffers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Whatoffers == null)
            {
                return NotFound();
            }

            var whatoffer = await _context.Whatoffers.FindAsync(id);
            if (whatoffer == null)
            {
                return NotFound();
            }
            return View(whatoffer);
        }

        // POST: Whatoffers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Offerid,Title,Text")] Whatoffer whatoffer)
        {
            if (id != whatoffer.Offerid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(whatoffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WhatofferExists(whatoffer.Offerid))
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
            return View(whatoffer);
        }

        // GET: Whatoffers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Whatoffers == null)
            {
                return NotFound();
            }

            var whatoffer = await _context.Whatoffers
                .FirstOrDefaultAsync(m => m.Offerid == id);
            if (whatoffer == null)
            {
                return NotFound();
            }

            return View(whatoffer);
        }

        // POST: Whatoffers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Whatoffers == null)
            {
                return Problem("Entity set 'ModelContext.Whatoffers'  is null.");
            }
            var whatoffer = await _context.Whatoffers.FindAsync(id);
            if (whatoffer != null)
            {
                _context.Whatoffers.Remove(whatoffer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WhatofferExists(decimal id)
        {
          return (_context.Whatoffers?.Any(e => e.Offerid == id)).GetValueOrDefault();
        }
    }
}
