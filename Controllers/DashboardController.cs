using Hoteleria.Data;
using Hoteleria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hoteleria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var totalHoteles = await _context.Hoteles.CountAsync();
            var totalUsuarios = await _userManager.Users.CountAsync();
            var totalReservas = await _context.Reservas.CountAsync();

            // RESERVAS POR MES
            var reservasPorMes = await _context.Reservas
                .GroupBy(r => r.FechaInicio.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .OrderBy(x => x.Mes)
                .ToListAsync();

            ViewBag.ReservasMesLabels = reservasPorMes.Select(r => r.Mes).ToList();
            ViewBag.ReservasMesValues = reservasPorMes.Select(r => r.Total).ToList();

            ViewBag.TotalHoteles = totalHoteles;
            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.TotalReservas = totalReservas;

            return View();
        }
    }
}