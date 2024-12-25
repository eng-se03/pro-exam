using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pro_exam.Authorization;
using pro_exam.DataBaseContext;
using pro_exam.email_service;
using pro_exam.ViewModel;

namespace pro_exam.Controllers
{
    public class AuthController : Controller
    {
        public IAuthentication<UserViewModel> Authentication { get; }

        private readonly AppDBcontext _context;

        private readonly EmailService mail;
        public AuthController(IAuthentication<UserViewModel> authentication, AppDBcontext context, EmailService mail)
        {
            Authentication = authentication;
            _context = context;
            this.mail = mail;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login( string Email ,string OTP)
        {
            try
            {
                // Retrieve the user by EmployeeNumber
                //var hashedpassword = BCrypt.Net.BCrypt.HashPassword(loginModel.Password);
                var user = await _context.User.Where(x => x.Email == Email && x.OTP == OTP).SingleOrDefaultAsync();
                if (user == null )
                {
                    ViewBag.ErrorMessage = "Email or Password not Correct";
                    return View();
                }

                var datamapping = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name
                };

                if (datamapping is not null)
                {
                    datamapping.Tokcen = Authentication.GetJsonWebToken(datamapping);
                }

                HttpContext.Session.SetString("token", datamapping.Tokcen);

                return RedirectToAction("DoctorsWithSchedules", "Doctor");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Email or Password not Correct";
                return View();
            }

        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");

            //add log
            return RedirectToAction("Login", "Auth");
        }



        [HttpPost]
        public async Task<IActionResult> UserCode()
        {
            var User = await _context.User.FindAsync(1);
            User.OTP = GenerateOTP(6);
            User.OTPExpirationTime= DateTime.Now.AddMinutes(2);
            _context.User.Update(User);
            _context.SaveChanges();
            await mail.SendVerificationEmailAsync(User.Email, User.OTP);
            return RedirectToAction("Login", "Auth");

        }

        // Generate OTP
        private string GenerateOTP(int length = 6)
        {
            var random = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => (char)('0' + random.Next(0, 10))).ToArray());
        }

    }
}
