using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MyFitnessLife.Models;

namespace MyFitnessLife.Controllers
{
	public class RegisterAndLoginController : Controller 
	{
            private readonly ModelContext _context;
            public RegisterAndLoginController(ModelContext context)
            {
                _context = context;
            }
            public IActionResult Index()
            {
                return View();
            }
		    [HttpGet]
		    public IActionResult Register()
		    {
			    return View();
		    }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Register(UserLogins user)
            {
                if (ModelState.IsValid)
                {
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
                        Status = "Approved"
                    };
                    try
                    {
                        _context.Add(newUser);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Login");
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine(ex.InnerException?.Message);
                        return BadRequest("An error occurred while registering the user.");
                    }
                }
                return View(user);
            }
        
            [HttpGet]
		    public IActionResult Login()
		    {
			    return View(); 
		    }

		    // Login action
		    [HttpPost]
		    [ValidateAntiForgeryToken]
            public IActionResult Login([Bind("Username,Password")] User user)
            {
                var auth = _context.Users.Where(x => x.Username == user.Username && x.Password == user.Password).SingleOrDefault();
                if (auth != null)
                {
                    switch (auth.Roleid)
                    {
                    case 1:
                        HttpContext.Session.SetString("AdminName", auth.Username ?? string.Empty); // Default to empty string if null
                        HttpContext.Session.SetInt32("AdminId", (int)auth.Userid);
                        HttpContext.Session.SetString("AdminEmail", auth.Email ?? string.Empty);
                        HttpContext.Session.SetString("birthdate", auth.Birthdate?.ToString("yyyy-MM-dd") ?? string.Empty);
                        HttpContext.Session.SetString("AdminfName", auth.Fname ?? string.Empty);
                        HttpContext.Session.SetString("AdminlName", auth.Lname ?? string.Empty);
                        HttpContext.Session.SetString("AdminPhoneNumber", auth.Phonenumber ?? string.Empty); // Handle null phone number
                        HttpContext.Session.SetString("AdminPassword", auth.Password ?? string.Empty);       // Handle null password
                        HttpContext.Session.SetString("AdminGender", auth.Gender ?? string.Empty);
                        return RedirectToAction("Index", "Admin");

                        case 2://Trainer 
                        HttpContext.Session.SetString("TrainerName", auth.Username ?? string.Empty);
                        HttpContext.Session.SetInt32("TrainerId", (int)auth.Userid);
                        HttpContext.Session.SetString("TrainerEmail", auth.Email ?? string.Empty);
                        HttpContext.Session.SetString("TrainerBirthdate", auth.Birthdate?.ToString("yyyy-MM-dd") ?? string.Empty);
                        HttpContext.Session.SetString("TrainerfName", auth.Fname ?? string.Empty);
                        HttpContext.Session.SetString("TrainerlName", auth.Lname ?? string.Empty);
                        HttpContext.Session.SetString("TrainerPhoneNumber", auth.Phonenumber ?? string.Empty);
                        HttpContext.Session.SetString("TrainerPassword", auth.Password ?? string.Empty);
                        HttpContext.Session.SetString("TrainerGender", auth.Gender ?? string.Empty); 
                        return RedirectToAction("TrainerIndex", "Home");

					    case 3://Member 
                        HttpContext.Session.SetString("MemberName", auth.Username ?? string.Empty);
                        HttpContext.Session.SetInt32("MemberId", (int)auth.Userid);
                        HttpContext.Session.SetString("MemberEmail", auth.Email ?? string.Empty);
                        HttpContext.Session.SetString("MemberBirthdate", auth.Birthdate?.ToString("yyyy-MM-dd") ?? string.Empty);
                        HttpContext.Session.SetString("MemberfName", auth.Fname ?? string.Empty);
                        HttpContext.Session.SetString("MemberlName", auth.Lname ?? string.Empty);
                        HttpContext.Session.SetString("MemberPhoneNumber", auth.Phonenumber ?? string.Empty);
                        HttpContext.Session.SetString("MemberPassword", auth.Password ?? string.Empty);
                        HttpContext.Session.SetString("MemberGender", auth.Gender ?? string.Empty); 
                        return RedirectToAction("MemberIndex", "Home");

                        default:
                            ModelState.AddModelError(string.Empty, "Invalid role assigned.");
                            break;
                    }
                }

                return View();
            }

    }
}

