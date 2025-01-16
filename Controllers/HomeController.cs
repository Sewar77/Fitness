using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

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
        public async Task<IActionResult> Index(Whatoffer whatoffer)
        {

            ViewBag.AboutTitle = _context.Aboutus.Select(p => p.Title).FirstOrDefault();
            ViewBag.AboutText1 = _context.Aboutus.Select(p => p.Text1).FirstOrDefault();
            ViewBag.AboutText2 = _context.Aboutus.Select(p => p.Text2).FirstOrDefault();
            ViewBag.AboutImage = _context.Aboutus.Select(p => p.Image).FirstOrDefault();


            ViewBag.WebsiteinfosTitle1 = _context.Websiteinfos.Select(p => p.Title1).FirstOrDefault();
            ViewBag.WebsiteinfosTitle2 = _context.Websiteinfos.Select(p => p.Title2).FirstOrDefault();
            ViewBag.Websiteinfosopenhour = _context.Websiteinfos.Select(p => p.Openhour).FirstOrDefault();
            ViewBag.WebsiteinfosAddress = _context.Websiteinfos.Select(p => p.Address).FirstOrDefault();


            var WhatOffer =_context.Whatoffers.ToList();

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
           // var users = await _context.Users.ToListAsync();

            // Create a Tuple of the feedbacks and membership plans
            var viewModel = new Tuple<IEnumerable<Feedback>, IEnumerable<Membershipplan>, IEnumerable<Whatoffer>>(
                feedbacks ?? new List<Feedback>(),
                membershipPlans ?? new List<Membershipplan>(),
                WhatOffer
            );

            return View( viewModel);
        }


        public IActionResult Aboutus()
		{
            ViewBag.AboutTitle = _context.Aboutus.Select(p => p.Title).FirstOrDefault();
            ViewBag.AboutText1 = _context.Aboutus.Select(p => p.Text1).FirstOrDefault();
            ViewBag.AboutText2 = _context.Aboutus.Select(p => p.Text2).FirstOrDefault();
            ViewBag.AboutImage = _context.Aboutus.Select(p => p.Image).FirstOrDefault();


            return View();
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
        public async Task<IActionResult> TrainerIndex(decimal trainerId)
        {
            // Fetch all users, workouts, membership plans, and trainer assignments
            var users = await _context.Users.ToListAsync();
            var workouts = await _context.Workouts.ToListAsync();
            var membershipPlans = await _context.Membershipplans.ToListAsync();
            var trainerAssignments = await _context.Trainerassignments.ToListAsync();

            // Fetch the plans assigned to the specific trainer
            var trainerPlans = await _context.Trainerassignments
                .Where(ta => ta.Trainerid == trainerId)
                .Join(_context.Users,
                    ta => ta.Trainerid,
                    mp => mp.Userid,
                    (ta, mp) => new { mp.Userid, mp.Username })
                .ToListAsync();

            // Get the members who are training on those plans
            var members = await _context.Subscriptions
                .Where(s => trainerPlans.Select(tp => tp.Userid).Contains(s.Userid))
                .Join(_context.Users,
                    s => s.Userid,
                    u => u.Userid,
                    (s, u) => new { u.Username, u.Fname, u.Lname, u.Email })
                .ToListAsync();

            // Create a ViewModel to pass data to the view
            var viewModel = new TrainerIndexViewModel
            {
                TrainerId = trainerId,
                Users = users,
                Workouts = workouts,
                MembershipPlans = membershipPlans,
                TrainerAssignments = trainerAssignments,
                TrainerPlans = trainerPlans,
                Members = members
            };

            return View(viewModel);
        }

        public IActionResult UserPlans()
        {
            var subscriptions = _context.Subscriptions
                                        .Include(s => s.Plan) // Ensure the plan details are included
                                        .ToList();
            return View(subscriptions);
        }


        public async Task<IActionResult> MemberIndex()
		{
            var feedbacks = await _context.Feedbacks.ToListAsync();
            var users = await _context.Users.ToListAsync();

            // Create a Tuple of the feedbacks and membership plans
            var viewModel = new Tuple<IEnumerable<Feedback>, IEnumerable<User>>(
                feedbacks ?? new List<Feedback>(),
                users ?? new List<User>()
            );

            return View(viewModel);
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

            if (!id.HasValue || !memberIdFromSession.HasValue || memberIdFromSession.Value != id)
            {
                return RedirectToAction("Login", "Account");
            }

            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Userid == id);

            if (userEntity == null)
            {
                return NotFound();
            }

            // Populate ViewData for the view
            ViewData["MemberDetails"] = new
            {
                userEntity.Userid,
                userEntity.Username,
                userEntity.Email,
                userEntity.Fname,
                userEntity.Lname,
                userEntity.Phonenumber,
                Birthdate = userEntity.Birthdate?.ToString("yyyy-MM-dd"),
                userEntity.Gender,
                userEntity.Imagepath
            };

            // Return a strongly-typed profile model to the view
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
            if (!ModelState.IsValid)
            {
                return View(profileModel);
            }

            var userEntity = await _context.Users.FindAsync(profileModel.Userid);

            if (userEntity == null)
            {
                return NotFound();
            }

            // Handle image upload if a file is provided
            if (profileModel.ImageFile != null && profileModel.ImageFile.Length > 0)
            {
                string allowedExtensions = ".jpg,.jpeg,.png,.gif";
                string fileExtension = Path.GetExtension(profileModel.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("", "Invalid file type. Only .jpg, .jpeg, .png, and .gif are allowed.");
                    return View(profileModel);
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + fileExtension;
                string path = Path.Combine(wwwRootPath, "Images", fileName);

                try
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await profileModel.ImageFile.CopyToAsync(fileStream);
                    }

                    userEntity.Imagepath = fileName; // Update image path in the database
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"File upload failed: {ex.Message}");
                    return View(profileModel);
                }
            }

            // Update user fields
            userEntity.Fname = profileModel.Fname;
            userEntity.Lname = profileModel.Lname;
            userEntity.Email = profileModel.Email;
            userEntity.Username = profileModel.Username;
            userEntity.Phonenumber = profileModel.Phonenumber;
            userEntity.Birthdate = profileModel.Birthdate;
            userEntity.Gender = profileModel.Gender;

            try
            {
                _context.Users.Update(userEntity);
                await _context.SaveChangesAsync();

                // Update session data
                HttpContext.Session.SetString("MemberName", userEntity.Username ?? string.Empty);
                HttpContext.Session.SetString("MemberEmail", userEntity.Email ?? string.Empty);
                HttpContext.Session.SetString("MemberFirstName", userEntity.Fname ?? string.Empty);
                HttpContext.Session.SetString("MemberLastName", userEntity.Lname ?? string.Empty);

                return RedirectToAction(nameof(MemberProfile), new { id = userEntity.Userid });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while saving data: {ex.Message}");
                return View(profileModel);
            }
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
