using GymHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActivityLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _context.ActivityLogs
                .OrderByDescending(a => a.DateCreated)
                .ToListAsync();

            return View(logs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var logs = await _context.ActivityLogs.ToListAsync();

            _context.ActivityLogs.RemoveRange(logs);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}