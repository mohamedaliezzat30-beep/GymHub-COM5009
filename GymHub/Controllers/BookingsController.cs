using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymHub.Data;
using GymHub.Models;

namespace GymHub.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string? userEmail = User.Identity?.Name;

            var bookings = _context.Bookings
                .Include(b => b.GymClass)
                .AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m =>
                        m.UserEmail == userEmail &&
                        m.Status == "Active");

                if (membership != null)
                {
                    bookings = bookings.Where(b => b.MemberName == membership.MemberName);
                }
                else
                {
                    bookings = bookings.Where(b => false);
                }
            }

            return View(await bookings.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            string? userEmail = User.Identity?.Name;

            var booking = await _context.Bookings
                .Include(b => b.GymClass)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m =>
                        m.UserEmail == userEmail &&
                        m.Status == "Active");

                if (membership == null || booking.MemberName != membership.MemberName)
                {
                    return Forbid();
                }
            }

            return View(booking);
        }

        public async Task<IActionResult> Create()
        {
            string? userEmail = User.Identity?.Name;

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m =>
                    m.UserEmail == userEmail &&
                    m.Status == "Active");

            if (membership == null && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Join", "MembershipSignup");
            }

            if (membership != null &&
                membership.MembershipType == "Basic" &&
                !User.IsInRole("Admin"))
            {
                TempData["Error"] =
                    "Class booking is available for Premium and VIP members only. Please upgrade your membership.";

                return RedirectToAction("Index", "MyMembership");
            }

            ViewBag.MemberName = membership?.MemberName ?? "Admin";

            await LoadGymClassesDropDownList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GymClassId")] Booking booking)
        {
            string? userEmail = User.Identity?.Name;

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m =>
                    m.UserEmail == userEmail &&
                    m.Status == "Active");

            if (membership == null && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Join", "MembershipSignup");
            }

            if (membership != null &&
                membership.MembershipType == "Basic" &&
                !User.IsInRole("Admin"))
            {
                TempData["Error"] =
                    "Class booking is available for Premium and VIP members only. Please upgrade your membership.";

                return RedirectToAction("Index", "MyMembership");
            }

            booking.MemberName = membership?.MemberName ?? "Admin";
            booking.BookingDate = DateTime.Now;
            booking.BookingStatus = "Confirmed";

            bool alreadyBooked = await _context.Bookings.AnyAsync(b =>
                b.MemberName == booking.MemberName &&
                b.GymClassId == booking.GymClassId &&
                b.BookingStatus == "Confirmed");

            if (alreadyBooked)
            {
                ModelState.AddModelError("", "You have already booked this class.");
            }

            var selectedClass = await _context.GymClasses
                .FirstOrDefaultAsync(g => g.GymClassId == booking.GymClassId);

            if (selectedClass == null)
            {
                ModelState.AddModelError("", "Selected class does not exist.");
            }
            else
            {
                int currentBookings = await _context.Bookings.CountAsync(b =>
                    b.GymClassId == booking.GymClassId &&
                    b.BookingStatus == "Confirmed");

                if (currentBookings >= selectedClass.Capacity)
                {
                    ModelState.AddModelError(
                        "",
                        $"Sorry, {selectedClass.ClassName} on {selectedClass.ClassDate.ToShortDateString()} at {selectedClass.StartTime} is fully booked."
                    );
                }
            }

            ModelState.Remove("MemberName");
            ModelState.Remove("BookingDate");
            ModelState.Remove("BookingStatus");
            ModelState.Remove("GymClass");

            if (ModelState.IsValid)
            {
                _context.Bookings.Add(booking);

                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = booking.MemberName,
                    Action = booking.MemberName + " booked a class.",
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.MemberName = booking.MemberName;
            await LoadGymClassesDropDownList(booking.GymClassId);

            return View(booking);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            string? userEmail = User.Identity?.Name;

            var booking = await _context.Bookings
                .Include(b => b.GymClass)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m =>
                        m.UserEmail == userEmail &&
                        m.Status == "Active");

                if (membership == null || booking.MemberName != membership.MemberName)
                {
                    return Forbid();
                }
            }

            await LoadGymClassesDropDownList(booking.GymClassId);

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,GymClassId")] Booking updatedBooking)
        {
            if (id != updatedBooking.BookingId)
            {
                return NotFound();
            }

            string? userEmail = User.Identity?.Name;

            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (existingBooking == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m =>
                        m.UserEmail == userEmail &&
                        m.Status == "Active");

                if (membership == null || existingBooking.MemberName != membership.MemberName)
                {
                    return Forbid();
                }

                if (membership.MembershipType == "Basic")
                {
                    TempData["Error"] =
                        "Class booking is available for Premium and VIP members only. Please upgrade your membership.";

                    return RedirectToAction("Index", "MyMembership");
                }
            }

            bool alreadyBooked = await _context.Bookings.AnyAsync(b =>
                b.BookingId != existingBooking.BookingId &&
                b.MemberName == existingBooking.MemberName &&
                b.GymClassId == updatedBooking.GymClassId &&
                b.BookingStatus == "Confirmed");

            if (alreadyBooked)
            {
                ModelState.AddModelError("", "You have already booked this class.");
            }

            var selectedClass = await _context.GymClasses
                .FirstOrDefaultAsync(g => g.GymClassId == updatedBooking.GymClassId);

            if (selectedClass == null)
            {
                ModelState.AddModelError("", "Selected class does not exist.");
            }
            else
            {
                int currentBookings = await _context.Bookings.CountAsync(b =>
                    b.GymClassId == updatedBooking.GymClassId &&
                    b.BookingStatus == "Confirmed" &&
                    b.BookingId != existingBooking.BookingId);

                if (currentBookings >= selectedClass.Capacity)
                {
                    ModelState.AddModelError("", "This class is fully booked.");
                }
            }

            ModelState.Remove("MemberName");
            ModelState.Remove("BookingDate");
            ModelState.Remove("BookingStatus");
            ModelState.Remove("GymClass");

            if (ModelState.IsValid)
            {
                existingBooking.GymClassId = updatedBooking.GymClassId;
                existingBooking.BookingDate = DateTime.Now;
                existingBooking.BookingStatus = "Confirmed";

                _context.Bookings.Update(existingBooking);

                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = existingBooking.MemberName,
                    Action = existingBooking.MemberName + " updated a class booking.",
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();

                TempData["Success"] = "Booking updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            existingBooking.GymClassId = updatedBooking.GymClassId;

            await LoadGymClassesDropDownList(updatedBooking.GymClassId);

            return View(existingBooking);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            string? userEmail = User.Identity?.Name;

            var booking = await _context.Bookings
                .Include(b => b.GymClass)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m =>
                        m.UserEmail == userEmail &&
                        m.Status == "Active");

                if (membership == null || booking.MemberName != membership.MemberName)
                {
                    return Forbid();
                }
            }

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? userEmail = User.Identity?.Name;

            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!User.IsInRole("Admin"))
            {
                var membership = await _context.Memberships
                    .FirstOrDefaultAsync(m =>
                        m.UserEmail == userEmail &&
                        m.Status == "Active");

                if (membership == null || booking.MemberName != membership.MemberName)
                {
                    return Forbid();
                }
            }

            _context.ActivityLogs.Add(new ActivityLog
            {
                UserEmail = booking.MemberName,
                Action = booking.MemberName + " cancelled a class booking.",
                DateCreated = DateTime.Now
            });

            booking.BookingStatus = "Cancelled";

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCancelledBookings()
        {
            var cancelledBookings = await _context.Bookings
                .Where(b => b.BookingStatus == "Cancelled")
                .ToListAsync();

            if (cancelledBookings.Any())
            {
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = "Mohamed Abdelrahman",
                    Action = "Admin permanently deleted all cancelled bookings.",
                    DateCreated = DateTime.Now
                });

                _context.Bookings.RemoveRange(cancelledBookings);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadGymClassesDropDownList(int? selectedGymClassId = null)
        {
            ViewData["GymClassId"] = new SelectList(
                await _context.GymClasses
                    .OrderBy(g => g.ClassDate)
                    .ThenBy(g => g.StartTime)
                    .Select(g => new
                    {
                        g.GymClassId,
                        ClassDisplay = g.ClassName + " - " +
                                       g.ClassDate.ToString("dd/MM/yyyy") + " - " +
                                       g.StartTime
                    })
                    .ToListAsync(),
                "GymClassId",
                "ClassDisplay",
                selectedGymClassId);
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
