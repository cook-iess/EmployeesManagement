using EmployeesManagement.Data;
using EmployeesManagement.Models;
using EmployeesManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Include(x => x.Role).ToListAsync();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //var roles = await _roleManager.Roles.ToListAsync();
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsersViewModel model)
        {
            ApplicationUser user = new()
            {
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                NationalId = model.NationalId,
                UserName = model.Username,
                NormalizedUserName = model.Username.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                CreatedOn = DateTime.UtcNow,
                CreatedById = "Macro Code",
                RoleId = model.RoleId,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                return View(model);

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", model.RoleId);
        }
    }
}
