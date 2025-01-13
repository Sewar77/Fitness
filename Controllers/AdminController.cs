using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;

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

        [HttpGet]
        public IActionResult AdminProfile()
        {
            return View();
        }



        
    


    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminProfile(Profile user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Profiles == null)
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(PendingFeedbacks));
            } 
            return View(user);
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

        public class AccountController : Controller
        {
            // Other action methods (login, etc.)

            // Logout action
            public IActionResult Logout()
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                // Redirect to the home page or login page after logging out
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
