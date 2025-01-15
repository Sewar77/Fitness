using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyFitnessLife.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
			_logger = logger;
			_context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["MemberId"] = HttpContext.Session.GetInt32("MemberId");
            ViewData["MemberEmail"] = HttpContext.Session.GetString("MemberEmail");
            ViewData["MemberFirstName"] = HttpContext.Session.GetString("firstname");
            ViewData["MemberLastName"] = HttpContext.Session.GetString("lastname");
            ViewData["MemberPhone"] = HttpContext.Session.GetString("phone");
            ViewData["MemberUsername"] = HttpContext.Session.GetString("username");
            ViewData["MemberBirthdate"] = HttpContext.Session.GetString("birthdate");
            ViewData["MemberGender"] = HttpContext.Session.GetString("Gender");

            var feedbacks = await _context.Feedbacks.ToListAsync();
            var membershipPlans = await _context.Membershipplans.ToListAsync();

            // Create a Tuple of the feedbacks and membership plans
            var viewModel = new Tuple<IEnumerable<Feedback>, IEnumerable<Membershipplan>>(
                feedbacks ?? new List<Feedback>(),
                membershipPlans ?? new List<Membershipplan>()
            );

            return View(viewModel);
        }


        public IActionResult Aboutus()
		{
			return View();
        }
        public IActionResult Subsicription(decimal? id)
		{
            var plan = _context.Membershipplans.FirstOrDefault(p => p.Planid == id);

            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        [HttpPost]
        public IActionResult ProcessSubscription(decimal PlanId, string Name, string Email, decimal card)
        {
            try
            {
                // Step 1: Retrieve the plan details
                var plan = _context.Membershipplans.FirstOrDefault(p => p.Planid == PlanId);
                if (plan == null)
                {
                    throw new Exception("Invalid Plan.");
                }

                // Step 2: Validate the card details
                var bankAccount = _context.Banks.FirstOrDefault(b => b.Card == card);
                if (bankAccount == null)
                {
                    throw new Exception("Invalid Card Details.");
                }

                // Step 3: Check if sufficient balance exists
                if (bankAccount.Balance < plan.Price)
                {
                    throw new Exception("Insufficient balance.");
                }

                // Step 4: Deduct the amount
                bankAccount.Balance -= plan.Price;
                _context.Banks.Update(bankAccount);
                _context.SaveChanges();

                // Step 5: Create a new subscription record for the user
                var subscription = new Subscription
                {
                    Planid = plan.Planid,
                    Startdate = DateTime.Now
                };

                _context.Subscriptions.Add(subscription);
                _context.SaveChanges();

                // Step 6: Return success message and redirect to UserPlans
                ViewBag.Message = "Subscription successful!";
                return RedirectToAction("UserPlans");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"sewar shorman: {ex.Message}");
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }




        public IActionResult HomeMember()
        {
            var plans = _context.Membershipplans.ToList();
            return View(plans);
        }


        public IActionResult Services()
		{
			return View();
		}

        public IActionResult UserPlans()
        {
            var subscriptions = _context.Subscriptions
                                        .Include(s => s.Plan) // Ensure the plan details are included
                                        .ToList();
            return View(subscriptions);
        }


        public IActionResult MemberIndex()
		{
            var feedbacks = _context.Feedbacks.ToList(); // Retrieve feedbacks from the database
            return View(feedbacks);
		}

		public IActionResult Schedule()
		{
			return View();
		}


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MemberProfile(decimal? id)
        {
            var memberIdFromSession = HttpContext.Session.GetInt32("MemberId");
            if (id == null || !memberIdFromSession.HasValue || memberIdFromSession.Value != id)
            {
                return RedirectToAction("Login", "Account");
            }

            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Userid == id);
            if (userEntity == null)
            {
                return NotFound();
            }

            ViewData["MemberID"] = userEntity.Userid;
            ViewData["MemberName"] = userEntity.Username;
            ViewData["MemberEmail"] = userEntity.Email;
            ViewData["MemberFirstName"] = userEntity.Fname;
            ViewData["MemberLastName"] = userEntity.Lname;
            ViewData["MemberPhone"] = userEntity.Phonenumber;
            ViewData["MemberBirthdate"] = userEntity.Birthdate?.ToString("yyyy-MM-dd");
            ViewData["MemberGender"] = userEntity.Gender;
            ViewData["MemberImagePath"] = userEntity.Imagepath;

            return View(new Profile
            {
                Userid = userEntity.Userid,
                Fname = userEntity.Fname,
                Lname = userEntity.Lname,
                Email = userEntity.Email,
                Username = userEntity.Username,
                Phonenumber = userEntity.Phonenumber,
                Birthdate = userEntity.Birthdate,
                Gender = userEntity.Gender,
                Imagepath = userEntity.Imagepath
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberProfile(Profile profileModel)
        {
            if (ModelState.IsValid)
            {
                var userEntity = await _context.Users.FindAsync(profileModel.Userid);
                if (userEntity == null)
                {
                    return NotFound();
                }

                if (profileModel.ImageFile != null && profileModel.ImageFile.Length > 0)
                {
                    string wwwwrootPath = _webHostEnvironment.WebRootPath;
                    string FileName = Guid.NewGuid().ToString() + "_" + profileModel.ImageFile.FileName;
                    string path = Path.Combine(wwwwrootPath + "/Images/", FileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await profileModel.ImageFile.CopyToAsync(fileStream);
                    }

                    profileModel.Imagepath = FileName;
                    userEntity.Imagepath = profileModel.Imagepath; // Update Imagepath
                }

                userEntity.Fname = profileModel.Fname;
                userEntity.Lname = profileModel.Lname;
                userEntity.Email = profileModel.Email;
                userEntity.Username = profileModel.Username;
                userEntity.Phonenumber = profileModel.Phonenumber;
                userEntity.Birthdate = profileModel.Birthdate;
                userEntity.Gender = profileModel.Gender;
                _context.Users.Update(userEntity);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("MemberName", userEntity.Username ?? string.Empty);
                HttpContext.Session.SetString("MemberEmail", userEntity.Email ?? string.Empty);
                HttpContext.Session.SetString("MemberFirstName", userEntity.Fname ?? string.Empty);
                HttpContext.Session.SetString("MemberLastName", userEntity.Lname ?? string.Empty);

                return RedirectToAction(nameof(MemberProfile), new { id = userEntity.Userid });
            }

            return View(profileModel);
        }

        public IActionResult Gallery()
		{
			return View();
		}

		public IActionResult Blog()
		{
			return View();
		}

		public IActionResult BlogDetails()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
