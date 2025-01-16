using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyFitnessLife.Models;

namespace MyFitnessLife.Controllers
{
    public class WebsiteinfoesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public WebsiteinfoesController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Websiteinfoes
        public async Task<IActionResult> Index()
        {
              return _context.Websiteinfos != null ? 
                          View(await _context.Websiteinfos.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Websiteinfos'  is null.");
        }

        // GET: Websiteinfoes/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Websiteinfos == null)
            {
                return NotFound();
            }

            var websiteinfo = await _context.Websiteinfos
                .FirstOrDefaultAsync(m => m.Websitetid == id);
            if (websiteinfo == null)
            {
                return NotFound();
            }

            return View(websiteinfo);
        }

        // GET: Websiteinfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Websiteinfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Websitetid,Title1,Title2,Openhour,Address,ImageFile")] Websiteinfo websiteinfo)
        {
            if (ModelState.IsValid)
            {

                if (websiteinfo.ImageFile != null)
                {
                    string wwwRootPath = _environment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString()
                                      + "_" + websiteinfo.ImageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await websiteinfo.ImageFile.CopyToAsync(fileStream);
                    }
                    websiteinfo.Image = fileName;
                }
                _context.Add(websiteinfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(websiteinfo);
        }

        // GET: Websiteinfoes/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Websiteinfos == null)
            {
                return NotFound();
            }

            var websiteinfo = await _context.Websiteinfos.FindAsync(id);
            if (websiteinfo == null)
            {
                return NotFound();
            }
            return View(websiteinfo);
        }

        // POST: Websiteinfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Websitetid,Title1,Title2,Openhour,Address,ImageFile")] Websiteinfo websiteinfo)
        {
            if (id != websiteinfo.Websitetid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (websiteinfo.ImageFile != null)
                    {
                        string wwwRootPath = _environment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString()
                                          + "_" + websiteinfo.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Images/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await websiteinfo.ImageFile.CopyToAsync(fileStream);
                        }
                        websiteinfo.Image = fileName;
                    }


                    _context.Update(websiteinfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WebsiteinfoExists(websiteinfo.Websitetid))
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
            return View(websiteinfo);
        }

        // GET: Websiteinfoes/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Websiteinfos == null)
            {
                return NotFound();
            }

            var websiteinfo = await _context.Websiteinfos
                .FirstOrDefaultAsync(m => m.Websitetid == id);
            if (websiteinfo == null)
            {
                return NotFound();
            }

            return View(websiteinfo);
        }

        // POST: Websiteinfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Websiteinfos == null)
            {
                return Problem("Entity set 'ModelContext.Websiteinfos'  is null.");
            }
            var websiteinfo = await _context.Websiteinfos.FindAsync(id);
            if (websiteinfo != null)
            {
                _context.Websiteinfos.Remove(websiteinfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WebsiteinfoExists(decimal id)
        {
          return (_context.Websiteinfos?.Any(e => e.Websitetid == id)).GetValueOrDefault();
        }
    }
}
