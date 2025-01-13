using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;
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
        public IActionResult Index()
        {
            ViewData["MemberId"] = HttpContext.Session.GetInt32("MemberId");
            ViewData["MemberEmail"] = HttpContext.Session.GetString("MemberEmail");
            ViewData["MemberFirstName"] = HttpContext.Session.GetString("firstname");
            ViewData["MemberLastName"] = HttpContext.Session.GetString("lastname");
            ViewData["MemberPhone"] = HttpContext.Session.GetString("phone");
            ViewData["MemberUsername"] = HttpContext.Session.GetString("username");
            ViewData["MemberBirthdate"] = HttpContext.Session.GetString("birthdate"); // Birthdate stored in session
            ViewData["MemberGender"] = HttpContext.Session.GetString("Gender");

            return View();
        }

        public IActionResult Aboutus()
		{
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
