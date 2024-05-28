using biliard.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ReservationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReservationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Reservations
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Reservation.Include(r => r.Table).Include(r => r.User);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Reservations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Reservation == null)
        {
            return NotFound();
        }

        var reservation = await _context.Reservation
            .Include(r => r.Table)
            .Include(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    // GET: Reservations/Create
    public IActionResult Create()
    {
        ViewData["TableId"] = new SelectList(_context.Table, "TableId", "TableId");
        ViewData["UserId"] = new SelectList(_context.User, "UserId", "FullName");
        return View();
    }

    // POST: Reservations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,UserId,TableId,DateAndHour,Description")] Reservation reservation)
    {
        if (ModelState.IsValid)
        {
            var isOverlapping = _context.Reservation
                .Any(r => r.TableId == reservation.TableId &&
                          r.DateAndHour <= reservation.DateAndHour.AddHours(1) &&
                          r.DateAndHour.AddHours(1) >= reservation.DateAndHour);

            if (isOverlapping)
            {
                ModelState.AddModelError("", "The reservation time overlaps with an existing reservation.");
                ViewData["TableId"] = new SelectList(_context.Table, "TableId", "TableId", reservation.TableId);
                ViewData["UserId"] = new SelectList(_context.User, "UserId", "FullName", reservation.UserId);
                return View(reservation);
            }

            _context.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["TableId"] = new SelectList(_context.Table, "TableId", "TableId", reservation.TableId);
        ViewData["UserId"] = new SelectList(_context.User, "UserId", "FullName", reservation.UserId);
        return View(reservation);
    }

    // GET: Reservations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Reservation == null)
        {
            return NotFound();
        }

        var reservation = await _context.Reservation.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }
        ViewData["TableId"] = new SelectList(_context.Table, "TableId", "TableId", reservation.TableId);
        ViewData["UserId"] = new SelectList(_context.User, "UserId", "FullName", reservation.UserId);
        return View(reservation);
    }

    // POST: Reservations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TableId,DateAndHour,Description")] Reservation reservation)
    {
        if (id != reservation.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var isOverlapping = _context.Reservation
                    .Any(r => r.TableId == reservation.TableId &&
                              r.Id != reservation.Id &&
                              r.DateAndHour <= reservation.DateAndHour.AddHours(1) &&
                              r.DateAndHour.AddHours(1) >= reservation.DateAndHour);

                if (isOverlapping)
                {
                    ModelState.AddModelError("", "The reservation time overlaps with an existing reservation.");
                    ViewData["TableId"] = new SelectList(_context.Table, "TableId", "TableId", reservation.TableId);
                    ViewData["UserId"] = new SelectList(_context.User, "UserId", "FullName", reservation.UserId);
                    return View(reservation);
                }

                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["TableId"] = new SelectList(_context.Table, "TableId", "TableId", reservation.TableId);
        ViewData["UserId"] = new SelectList(_context.User, "UserId", "FullName", reservation.UserId);
        return View(reservation);
    }

    // GET: Reservations/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Reservation == null)
        {
            return NotFound();
        }

        var reservation = await _context.Reservation
            .Include(r => r.Table)
            .Include(r => r.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (reservation == null)
        {
            return NotFound();
        }

        return View(reservation);
    }

    // POST: Reservations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Reservation == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
        }
        var reservation = await _context.Reservation.FindAsync(id);
        if (reservation != null)
        {
            _context.Reservation.Remove(reservation);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReservationExists(int id)
    {
        return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
