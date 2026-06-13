using GymHub.Data;
using GymHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymHub.Controllers
{
    [Authorize]
    public class MembershipSignupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MembershipSignupController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Join()
        {
            string? userEmail = User.Identity?.Name;

            bool hasActiveMembership = await _context.Memberships
                .AnyAsync(m => m.UserEmail == userEmail && m.Status == "Active");

            if (hasActiveMembership)
            {
                return RedirectToAction("AlreadyMember");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(Membership membership)
        {
            string? userEmail = User.Identity?.Name;

            var currentUser = await _userManager.GetUserAsync(User);
            string displayName = currentUser?.FullName ?? membership.MemberName;

            bool hasActiveMembership = await _context.Memberships
                .AnyAsync(m =>
                    (m.UserEmail == userEmail || m.MemberName == membership.MemberName)
                    && m.Status == "Active");

            if (hasActiveMembership)
            {
                return RedirectToAction("AlreadyMember");
            }

            membership.UserEmail = userEmail;
            membership.MemberName = displayName;
            membership.Duration = 30;
            membership.StartDate = DateTime.Today;
            membership.Status = "Active";

            if (membership.MembershipType == "Basic")
            {
                membership.Price = 19.99m;
            }
            else if (membership.MembershipType == "Premium")
            {
                membership.Price = 34.99m;
            }
            else if (membership.MembershipType == "VIP")
            {
                membership.Price = 49.99m;
            }

            ModelState.Remove("UserEmail");
            ModelState.Remove("MemberName");
            ModelState.Remove("Price");
            ModelState.Remove("Duration");
            ModelState.Remove("StartDate");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                _context.Memberships.Add(membership);

                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = displayName,
                    Action = displayName + " joined GymHub with a " + membership.MembershipType + " membership.",
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmation");
            }

            return View(membership);
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public IActionResult AlreadyMember()
        {
            return View();
        }
    }
}