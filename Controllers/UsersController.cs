using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;

namespace MyFitnessLife.Controllers
{
    public class UsersController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Users.Include(u => u.Role);
            return View(await modelContext.ToListAsync());
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }




        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Userid,Username,Password,Roleid,Fname,Lname,Email,Phonenumber,Createdat,Gender,Birthdate,Imagepath,Status")] Profile user, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Ensure directory exists
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    user.Imagepath = "/Images/" + fileName;
                }
                var newUser = new User
                {
                    Username = user.Username,
                    Password = user.Password,
                    Gender = user.Gender,
                    Roleid = user.Roleid ?? default(decimal),
                    Email = user.Email,
                    Birthdate = user.Birthdate,
                    Phonenumber = user.Phonenumber,
                    Fname = user.Fname,
                    Lname = user.Lname,
                };
                try
                {
                    _context.Add(newUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Create");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.InnerException?.Message);
                    return BadRequest("An error occurred while registering the user.");
                }
            }

            // Repopulate the Role dropdown if ModelState is invalid
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.Roleid);
            return View(user);
        }












        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.Roleid);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Userid,Username,Password,Roleid,Fname,Lname,Email,Phonenumber,Createdat,Gender,Birthdate,Imagepath,Status")] Profile user)
        {
            if (id != user.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing user from the database
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }
                    string wwwwrootPath = _webHostEnvironment.WebRootPath;

                    if (user.ImageFile != null) // Check if ImageFile is not null
                    {
                        string FileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;

                        string path = Path.Combine(wwwwrootPath + "~/Images/", FileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await user.ImageFile.CopyToAsync(fileStream);
                        }

                        user.Imagepath = FileName; // Update the Imagepath only if a new file is uploaded
                    }

                    var updateUser = new User
                    {
                        Userid = existingUser.Userid, // Keep the original User ID
                        Username = user.Username,
                        Password = user.Password,
                        Gender = user.Gender,
                        Roleid = user.Roleid ?? default(decimal),
                        Email = user.Email,
                        Birthdate = user.Birthdate,
                        Phonenumber = user.Phonenumber,
                        Fname = user.Fname,
                        Lname = user.Lname,
                        Status = "Approved",
                        Imagepath = user.Imagepath, 
                        Createdat = existingUser.Createdat, // Preserve the original creation date
                        Isactive = existingUser.Isactive // Ensure `IsActive` is preserved
                    };

                    // Update the entity
                    _context.Entry(existingUser).CurrentValues.SetValues(updateUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Userid))
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

            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Roleid", user.Roleid);
            return View(user);
        }



        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Userid == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ModelContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool UserExists(decimal id)
        {
          return (_context.Users?.Any(e => e.Userid == id)).GetValueOrDefault();
        }
        public IActionResult TrainerPlans() 
        {
            var result = from plan in _context.Membershipplans

                         join sub in _context.Subscriptions on plan.Planid equals sub.Planid
                         join us in _context.Users on sub.Userid equals us.Userid
                         group new { sub, us } by new { plan.Planid, plan.Planname, plan.Price, plan.Description, plan.Durationinmonths } into grouped
                         select new GroupedMembershipPlanViewModes
                         {
                             Planid = (int)grouped.Key.Planid,
                             Planname = grouped.Key.Planname,
                             Price = grouped.Key.Price,
                             Description = grouped.Key.Description,
                             Durationinmonths = grouped.Key.Durationinmonths,
                             Subscriptions = grouped.Select(g => new TrainerWithUserandSubsecribtions
                             {
                                 Userid = (int)g.us.Userid,
                                 USERNAME = g.us.Username,
                                 Fname = g.us.Fname,
                                 Lname = g.us.Lname,
                                 SubscriptionId = (int)g.sub.Subscriptionid,
                                 StartDate = g.sub.Startdate,
                                 EndDate = g.sub.Enddate,
                                 MembershipPlanId = (int)grouped.Key.Planid,
                             }).ToList()
                         };
            return View(result.ToList());
        }
    }
}
