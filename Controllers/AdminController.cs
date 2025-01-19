using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyFitnessLife.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewData["AdminName"] = HttpContext.Session.GetString("AdminName");
            ViewData["AdminEmail"] = HttpContext.Session.GetString("AdminEmail");
            ViewData["AdminBirthdate"] = HttpContext.Session.GetString("birthdate"); // Birthdate stored in session
            ViewData["AdminfName"] = HttpContext.Session.GetString("AdminfName");
            ViewData["AdminlName"] = HttpContext.Session.GetString("AdminlName");
            ViewData["AdminPhoneNumber"] = HttpContext.Session.GetString("AdminPhoneNumber");
            ViewData["AdminGender"] = HttpContext.Session.GetString("AdminGender");
            ViewData["AdminPassword"] = HttpContext.Session.GetString("AdminPassword");
            ViewData["AdminId"] = HttpContext.Session.GetInt32("AdminId");


            ViewBag.totalUsers = _context.Users.Count();
            ViewBag.totalRevenue = _context.Subscriptions.Sum(x => x.Amount);

            var activeSubscriptions = _context.Subscriptions
                .Where(s => DateTime.Now >= s.Startdate
                            && DateTime.Now <= s.Enddate
                            && s.Paymentstatus == "Paid")
                .Count();
            ViewBag.ActiveSubscriptions = activeSubscriptions;

            var activeMembers = _context.Subscriptions
                .Where(s => s.Enddate > DateTime.Now)
                .Select(s => s.Userid)
                .Distinct()
                .Count();
            ViewBag.ActiveMembers = activeMembers;

            var inactiveMembers = _context.Users
                .Where(u => !_context.Subscriptions
                    .Any(s => s.Userid == u.Userid && s.Enddate > DateTime.Now))
                .Count();
            ViewBag.InactiveMembers = inactiveMembers;

            // For new members this year
            var startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            var newMembersThisYear = _context.Users
                .Where(u => u.Createdat.HasValue && u.Createdat >= startOfYear) // Handle NULL Createdat
                .Count();
            ViewBag.NewMembersThisYear = newMembersThisYear;

            // For new members this month
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var newMembersThisMonth = _context.Users
                .Where(u => u.Createdat.HasValue && u.Createdat >= startOfMonth) // Handle NULL Createdat
                .Count();
            ViewBag.NewMembersThisMonth = newMembersThisMonth;

            // Get all users as a list
            var data = _context.Users.ToList(); // Fetch actual User objects

            if (data == null)
            {
                return View(new List<User>());
            }           

            return View(data); // Pass the list of User objects to the view
        }

        public async Task<IActionResult> PendingFeedbacks()
        {
            var pendingFeedback = await _context.Feedbacks
                .Include(f => f.User) // Include related user data if needed
                .Where(f => f.Approved == false)
                .ToListAsync();

            return View(pendingFeedback);
        }

        [HttpPost]
     
        public async Task<IActionResult> ApproveFeedback(decimal id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Approved = true; // Mark feedback as approved
            _context.Update(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction("PendingFeedbacks"); // Redirect to the admin feedback list page
        }

        public async Task<IActionResult> RejectFeedback(decimal id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback); // Delete the feedback
            await _context.SaveChangesAsync();

            return RedirectToAction("PendingFeedbacks");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "RegisterAndLogin");
        }
        public IActionResult MonthlyReport()
        {

            var MonthlyRevenue = _context.Subscriptions
                .GroupBy(s => new { year = s.Startdate.Year, month = s.Startdate.Month })
                .Select(g => new
                {
                    Year = g.Key.year,
                    Month = g.Key.month,
                    TotalRevenue = g.Sum(s => s.Amount)
                })
                .OrderBy(r => r.Year).ThenBy(r => r.Month)
                .ToList();

            var chartData = new
            {
                Lables = MonthlyRevenue.Select(m => $"{m.Year} {m.Month}").ToArray(),
                Revenue = MonthlyRevenue.Select(m => m.TotalRevenue).ToArray()
            };

            return View(new { MonthlyRevenue, chartData });
        }


        public ActionResult AnnualReport()
        {

            var AnnualRevenue = _context.Subscriptions
               .GroupBy(s =>   s.Startdate.Year)
               .Select(g => new
               {
                   Year = g.Key,
                  
                   TotalRevenue = g.Sum(s => s.Amount)
               })
               .OrderBy(r => r.Year)
               .ToList();

            var chartData = new
            {
                Lables = AnnualRevenue.Select(m => $"{m.Year} ").ToArray(),
                Revenue =AnnualRevenue.Select(m => m.TotalRevenue).ToArray()
            };

            return View(new { AnnualRevenue, chartData });


           
        }

        public ActionResult Charts()
        {
            return View();
        }







        [HttpGet]
        public async Task<IActionResult> AdminProfile(decimal? id)
        {
            var userid = HttpContext.Session.GetInt32("AdminId");
            if (userid == null || _context.Users == null)
            {
                return NotFound();
            }
            id = Convert.ToDecimal(userid);
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> AdminProfile([Bind("Userid,Username,Password,Roleid,Fname,Lname,Email,Phonenumber,Gender,Birthdate,ImageFile")] Profile user)
        {
            var sessionId = HttpContext.Session.GetInt32("AdminId");
            if (sessionId == null)
            {
                return NotFound();
            }

            var userId = Convert.ToDecimal(sessionId);

            if (userId != user.Userid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(userId);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Validate Roleid exists in the Roles table
                    if (user.Roleid.HasValue)
                    {
                        var roleExists = await _context.Roles.AnyAsync(r => r.Roleid == user.Roleid.Value);
                        if (!roleExists)
                        {
                            ModelState.AddModelError("Roleid", "Invalid Roleid. The specified role does not exist.");
                            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "RoleName", user.Roleid);
                            return View(user);
                        }
                    }

                    // Update user properties
                    existingUser.Username = user.Username;
                    existingUser.Password = string.IsNullOrEmpty(user.Password) ? existingUser.Password : user.Password;
                    existingUser.Roleid = user.Roleid ?? existingUser.Roleid;
                    existingUser.Email = user.Email;
                    existingUser.Birthdate = user.Birthdate;
                    existingUser.Phonenumber = user.Phonenumber;
                    existingUser.Fname = user.Fname;
                    existingUser.Lname = user.Lname;
                    existingUser.Imagepath = user.Imagepath ?? existingUser.Imagepath;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                catch (DbUpdateException)
                {
                    // Log or inspect dbEx to diagnose the specific issue
                    ModelState.AddModelError("", "An error occurred while updating the database. Please check your inputs.");
                    ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "RoleName", user.Roleid);
                    return View(user);
                }

                return RedirectToAction(nameof(AdminProfile));
            }

            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "RoleName", user.Roleid); // Adjust RoleName
            return View(user);
        }



















    }
}
