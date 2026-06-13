using GymHub.Data;
using GymHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymHub.Controllers
{
    [Authorize]
    public class MyMembershipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyMembershipController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string? userEmail = User.Identity?.Name;

            var membership = await _context.Memberships
                .Where(m => m.UserEmail == userEmail && m.Status == "Active")
                .OrderByDescending(m => m.StartDate)
                .FirstOrDefaultAsync();

            if (membership != null)
            {
                DateTime expiryDate = membership.StartDate.AddDays(membership.Duration);

                if (expiryDate < DateTime.Today)
                {
                    membership.Status = "Expired";

                    _context.ActivityLogs.Add(new ActivityLog
                    {
                        UserEmail = membership.MemberName,
                        Action = membership.MemberName + "'s membership expired.",
                        DateCreated = DateTime.Now
                    });

                    await _context.SaveChangesAsync();
                }
            }

            return View(membership);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeMembership(string membershipType)
        {
            string? userEmail = User.Identity?.Name;

            var membership = await _context.Memberships
                .Where(m => m.UserEmail == userEmail && m.Status == "Active")
                .OrderByDescending(m => m.StartDate)
                .FirstOrDefaultAsync();

            if (membership == null)
            {
                return RedirectToAction("Join", "MembershipSignup");
            }

            if (membership.MembershipType == membershipType)
            {
                return RedirectToAction(nameof(Index));
            }

            string oldType = membership.MembershipType;

            membership.MembershipType = membershipType;

            if (membershipType == "Basic")
            {
                membership.Price = 19.99m;
            }
            else if (membershipType == "Premium")
            {
                membership.Price = 34.99m;
            }
            else if (membershipType == "VIP")
            {
                membership.Price = 49.99m;
            }

            _context.ActivityLogs.Add(new ActivityLog
            {
                UserEmail = membership.MemberName,
                Action = membership.MemberName + " changed membership from " + oldType + " to " + membershipType + ".",
                DateCreated = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}