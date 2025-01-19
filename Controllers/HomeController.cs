using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


            var WhatOffer = _context.Whatoffers.ToList();

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

            return View(viewModel);
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







        public async Task<IActionResult> MemberProfile(decimal? id)
        {
            var userid = HttpContext.Session.GetInt32("MemberId");
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
        public async Task<IActionResult> MemberProfile([Bind("Userid,Username,Password,Roleid,Fname,Lname,Email,Phonenumber,Createdat,Gender,Birthdate,Imagepath")] Profile user)
        {
            var sessionId = HttpContext.Session.GetInt32("MemberId");
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

                return RedirectToAction(nameof(MemberProfile));
            }

            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "RoleName", user.Roleid); // Adjust RoleName
            return View(user);
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
        public IActionResult message()
		{
            TempData["SuccessMessage"] = "We will contact with you as fast as possible ";
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







        public async Task<IActionResult> TrainerProfile(decimal? id)
        {
            var userid = HttpContext.Session.GetInt32("TrainerId");
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
        public async Task<IActionResult> TrainerProfile([Bind("Userid,Username,Password,Roleid,Fname,Lname,Email,Phonenumber,Createdat,Gender,Birthdate,Imagepath")] Profile user)
        {
            var sessionId = HttpContext.Session.GetInt32("TrainerId");
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

                return RedirectToAction(nameof(TrainerProfile));
            }

            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "RoleName", user.Roleid); // Adjust RoleName
            return View(user);
        }


















    }
}
