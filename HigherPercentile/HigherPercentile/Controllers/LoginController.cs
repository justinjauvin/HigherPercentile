using HigherPercentile.Data;
using HigherPercentile.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace HigherPercentile.Controllers
{
    public class LoginController : Controller
    {
        ApplicationDbContext db;
        public User? loggedInUser = null;

        public LoginController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User loginAttempt)
        {

            //implement password hashing************************
            //getHashPassword(loginAttempt.Password);
            IQueryable<User>? users = db.Users.Where(T => T.Username == loginAttempt.Username && T.Password == loginAttempt.Password);
              
            if (users.Count() == 0)
            {
                ModelState.AddModelError("CustomeError", "There is no user by those credetials");
                ViewData["error"] = "Incorrect Credentials";
                return View(loginAttempt);
            }

            User? user = users.First();
            
            if (user.Password != loginAttempt.Password) 
            {
                    ModelState.AddModelError("CustomeError", "There is no user by those credetials");
            }

            if (ModelState.IsValid && user.Password == loginAttempt.Password);
            {
                //user.Password = "";
                //loginAttempt.Password = "";
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("id", user.Id.ToString());
                TempData["success"] = "Logged in successfully";
                   
               return RedirectToAction("Index", "Home"); //might need to specify the controller as the second parameter, not here since where in the same controller
             }
           
            
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User newUser)
        {
            IQueryable<User> usernameCheck = db.Users.Where(t => t.Username==newUser.Username);

            if (usernameCheck.Count()>0)
            {
                ModelState.AddModelError("CustomeError", "This username is taken...");
            }

            //implement password hashing************************
            if (newUser.Username == newUser.Password)
            {
                ModelState.AddModelError("CustomeError", "The username and password cannot be the same.");
            }

            if (newUser.Password != newUser.ConfirmPassword)
            {
                ModelState.AddModelError("CustomeError", "The passwords do no mathc!.");
            }

            //if statement checking newCategory to see if it follows the models required input
            if (ModelState.IsValid)
            {
                db.Users.Add(newUser);
                db.SaveChanges();
                TempData["success"] = "Account Created Successfully";
                //check this line!
                return RedirectToAction("Index"); //might need to specify the controller as the second parameter, not here since where in the same controller
            }

            return View(newUser);
        }

        /*
        String getHashPassword(String password)
        {
            // generate a 128-bit salt using a cryptographically strong random sequence of nonzero values
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"********************************Hashed: {hashed}");
            return hashed;
        }
        */
    }
}
