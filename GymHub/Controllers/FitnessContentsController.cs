using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymHub.Data;
using GymHub.Models;

namespace GymHub.Controllers
{
    [Authorize]
    public class FitnessContentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FitnessContentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.FitnessContents.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fitnessContent = await _context.FitnessContents
                .FirstOrDefaultAsync(m => m.FitnessContentId == id);

            if (fitnessContent == null)
            {
                return NotFound();
            }

            return View(fitnessContent);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FitnessContent fitnessContent, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                string[] allowedExtensions =
                {
                    ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"
                };

                string extension = Path.GetExtension(imageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImagePath", "Only image files are allowed.");
                    return View(fitnessContent);
                }

                string fileName = Guid.NewGuid().ToString() + extension;

                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "fitness");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                fitnessContent.ImagePath = "/images/fitness/" + fileName;
            }

            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                _context.FitnessContents.Add(fitnessContent);

                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = User.Identity?.Name ?? "Admin",
                    Action = "Fitness content added: " + fitnessContent.Title,
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(fitnessContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fitnessContent = await _context.FitnessContents.FindAsync(id);

            if (fitnessContent == null)
            {
                return NotFound();
            }

            return View(fitnessContent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FitnessContent fitnessContent, IFormFile? imageFile)
        {
            if (id != fitnessContent.FitnessContentId)
            {
                return NotFound();
            }

            var existingContent = await _context.FitnessContents
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FitnessContentId == id);

            if (existingContent == null)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                string[] allowedExtensions =
                {
                    ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp"
                };

                string extension = Path.GetExtension(imageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImagePath", "Only image files are allowed.");
                    return View(fitnessContent);
                }

                string fileName = Guid.NewGuid().ToString() + extension;

                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "fitness");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                fitnessContent.ImagePath = "/images/fitness/" + fileName;
            }
            else
            {
                fitnessContent.ImagePath = existingContent.ImagePath;
            }

            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fitnessContent);

                    _context.ActivityLogs.Add(new ActivityLog
                    {
                        UserEmail = User.Identity?.Name ?? "Admin",
                        Action = "Fitness content updated: " + fitnessContent.Title,
                        DateCreated = DateTime.Now
                    });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FitnessContentExists(fitnessContent.FitnessContentId))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(fitnessContent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fitnessContent = await _context.FitnessContents
                .FirstOrDefaultAsync(m => m.FitnessContentId == id);

            if (fitnessContent == null)
            {
                return NotFound();
            }

            return View(fitnessContent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fitnessContent = await _context.FitnessContents.FindAsync(id);

            if (fitnessContent != null)
            {
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = User.Identity?.Name ?? "Admin",
                    Action = "Fitness content deleted: " + fitnessContent.Title,
                    DateCreated = DateTime.Now
                });

                _context.FitnessContents.Remove(fitnessContent);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FitnessContentExists(int id)
        {
            return _context.FitnessContents.Any(e => e.FitnessContentId == id);
        }
    }
}