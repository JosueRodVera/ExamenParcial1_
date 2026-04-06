using Hoteleria.Data;
using Hoteleria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hoteleria.Controllers
{
    [Authorize(Roles = "Cliente")]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Mostrar reservas del usuario
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var reservas = await _context.Reservas
                .Include(r => r.Hotel)
                .Where(r => r.UsuarioId == userId)
                .ToListAsync();

            return View(reservas);
        }

        // Crear reserva
        public async Task<IActionResult> Create()
        {
            ViewBag.Hoteles = await _context.Hoteles.ToListAsync();
            return View();
        }

        // POST: Crear reserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            var userId = _userManager.GetUserId(User);
            reserva.UsuarioId = userId;

            // VALIDACIONES
            if (reserva.FechaInicio < DateTime.Today)
                ModelState.AddModelError("FechaInicio", "La fecha de inicio no puede ser en el pasado.");

            if (reserva.FechaFin <= reserva.FechaInicio)
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser mayor a la fecha de inicio.");

            if (!ModelState.IsValid)
            {
                ViewBag.Hoteles = await _context.Hoteles.ToListAsync();
                return View(reserva);
            }

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Detalles
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var reserva = await _context.Reservas
                .Include(r => r.Hotel)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id && r.UsuarioId == userId);

            if (reserva == null) return Unauthorized();

            return View(reserva);
        }

        // Eliminar reserva
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var reserva = await _context.Reservas
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id && r.UsuarioId == userId);

            if (reserva == null) return Unauthorized();

            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}