using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAuthenticationAuthorization.Models;
using TaskAuthenticationAuthorization.ViewModels;

namespace TaskAuthenticationAuthorization.Controllers
{
    public class AccountController : Controller
    {
        private ShoppingContext context;

        public AccountController(ShoppingContext context)
        {
            this.context = context;
        }

        private async Task Authenticate(Customer customer)
        {
            var adminId = context.Roles.First(r => r.Name == "admin").Id;

            if (customer.RoleId == adminId)
                customer.Role.Name = "admin";
            else
                customer.Role.Name = "buyer";
            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, customer.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, customer.Role.Name)
            };
            claims.Add(CustomerClaims.GetProperClaim(customer.Discount));
            ClaimsIdentity id = new ClaimsIdentity(claims, "AppCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await context.Customers.Include(c => c.Role).FirstOrDefaultAsync(c => c.Email == model.Email &&
                    c.Password == model.Password);
                if (customer != null)
                {
                    await Authenticate(customer);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("","Incorrect login or password");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = await context.Customers.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (customer == null)
                {
                    Customer newOne = new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Address = model.Address,
                        Discount = Discount.Regular,
                        Email = model.Email,
                        Password = model.Password,
                        RoleId = context.Roles.First(role => role.Name == "buyer").Id
                    };
                    // adding user to DB
                    context.Customers.Add(newOne);
                    await context.SaveChangesAsync();

                    await Authenticate(newOne); // authentication
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(model);
        }

        [Authorize(Policy = "VIP Only")]
        public async Task<IActionResult> MyDiscount()
        {
            Customer currentCustomer = await context.Customers.FirstOrDefaultAsync
                            (c => c.Email == User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType));

            ViewBag.Discount = currentCustomer.Discount switch
            {
                Discount.Golden => 15,
                Discount.Wholesale => 25,
            };
            return View(currentCustomer);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }
    }
}
