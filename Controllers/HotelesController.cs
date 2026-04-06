using Hoteleria.Data;
using Hoteleria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hoteleria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class HotelesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hoteles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hoteles.ToListAsync());
        }

        // GET: Hoteles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var hotel = await _context.Hoteles
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null) return NotFound();

            return View(hotel);
        }

        // GET: Hoteles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hoteles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hoteles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var hotel = await _context.Hoteles.FindAsync(id);
            if (hotel == null) return NotFound();

            return View(hotel);
        }

        // POST: Hoteles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hotel hotel)
        {
            if (id != hotel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Hoteles.Any(h => h.Id == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hoteles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hotel = await _context.Hoteles
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null) return NotFound();

            return View(hotel);
        }

        // POST: Hoteles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Hoteles.FindAsync(id);
            _context.Hoteles.Remove(hotel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}