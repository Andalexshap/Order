using Microsoft.AspNetCore.Mvc;
using Order.Interfaces;
using Order.Models.Account;

namespace Order.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly User _user;
        public AccountController(IAccountService accountService, User user)
        {
            _accountService = accountService;
            _accountService.SetFileName("accounts.json");
            _user = user;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return View("Error");
            }

            var user = new User
            {
                Login = login,
                Password = password
            };

            var response = _accountService.LoginUser(user);

            if (response.Sucsess)
            {
                _user.SetUser(response.User);
            }

            return View(response);
        }

        [HttpGet]
        [Route("registration")]
        public IActionResult Register(string login, string password, string email,
            string firstName, string lastName, string meddleName, string phoneNumber,
            string country, string city, string region, string street, string numberOfHome,
            string flat, string postalCode)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNumber))
            {
                return View("Error");
            }

            var user = new User
            {
                Login = login,
                Password = password,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                MeddleName = meddleName,
                PhoneNumber = phoneNumber,
                Address = new Address
                {
                    Country = country,
                    City = city,
                    Region = region,
                    Street = street,
                    NumberOfHome = numberOfHome,
                    Flat = flat,
                    PostalCode = postalCode
                }
            };

            var response = _accountService.RegisterUser(user);

            return View(response);
        }

        [HttpGet]
        [Route("update")]
        public ActionResult UpdateUser(string login, string password, string email,
            string firstName, string lastName, string meddleName, string phoneNumber,
            string country, string city, string region, string street, string numberOfHome,
            string flat, string postalCode)
        {
            if (_user.Login != login
                || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNumber))
            {
                return View("Error");
            }

            var user = new User
            {
                Login = login,
                Password = password,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                MeddleName = meddleName,
                PhoneNumber = phoneNumber,
                Address = new Address
                {
                    Country = country,
                    City = city,
                    Region = region,
                    Street = street,
                    NumberOfHome = numberOfHome,
                    Flat = flat,
                    PostalCode = postalCode
                }
            };

            var response = _accountService.UpdateUser(user);

            return View(response);
        }

        [HttpGet]
        [Route("delete-by-login")]
        public ActionResult DeleteAccoutByUserLogin(string userLogin)
        {
            if (_user.MemberType != MemberType.Administrator)
            {
                return View("Access denied!");
            }

            var response = _accountService.DeleteUserbyLogin(userLogin);

            return View(response);
        }

        [HttpGet]
        [Route("delete-by-id")]
        public ActionResult DeleteAccoutById(string userId)
        {
            if (_user.MemberType != MemberType.Administrator)
            {
                return View("Access denied!");
            }

            var response = _accountService.DeleteUserbyId(userId);

            return View(response);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            _user.Logout();
            return View("Login");
        }
    }
}
