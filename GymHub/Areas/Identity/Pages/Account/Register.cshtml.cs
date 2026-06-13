using GymHub.Data;
using GymHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateTime? DateOfBirth { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [StringLength(100, ErrorMessage = "The password must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!Input.DateOfBirth.HasValue)
            {
                ModelState.AddModelError("Input.DateOfBirth",
                    "Date of Birth is required.");
            }
            else if (Input.DateOfBirth.Value > DateTime.Today.AddYears(-18))
            {
                ModelState.AddModelError("Input.DateOfBirth",
                    "You must be at least 18 years old to register.");
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FullName = Input.FullName,
                    DateOfBirth = Input.DateOfBirth.Value,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");

                    _context.ActivityLogs.Add(new ActivityLog
                    {
                        UserEmail = Input.Email,
                        Action = Input.FullName + " registered a new GymHub account.",
                        DateCreated = DateTime.Now
                    });

                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}