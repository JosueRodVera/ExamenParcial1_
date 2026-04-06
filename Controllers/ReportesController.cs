using Hoteleria.Data;
using Hoteleria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace Hoteleria.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener fuentes
        private PdfFont BoldFont => PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        private PdfFont NormalFont => PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        // ========== REPORTE 1: TODAS LAS RESERVAS ==========
        public async Task<IActionResult> ReservasPDF()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Hotel)
                .ToListAsync();

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph("REPORTE DE RESERVAS")
                .SetFont(BoldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph("\n"));

            Table table = new Table(5).UseAllAvailableWidth();
            table.AddHeaderCell(new Cell().Add(new Paragraph("Usuario").SetFont(BoldFont)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Hotel").SetFont(BoldFont)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Inicio").SetFont(BoldFont)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Fin").SetFont(BoldFont)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Total Días").SetFont(BoldFont)));

            foreach (var r in reservas)
            {
                table.AddCell(r.Usuario.NombreCompleto);
                table.AddCell(r.Hotel.Nombre);
                table.AddCell(r.FechaInicio.ToShortDateString());
                table.AddCell(r.FechaFin.ToShortDateString());
                table.AddCell((r.FechaFin - r.FechaInicio).Days.ToString());
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", "Reservas.pdf");
        }

        // ========== REPORTE 2: RESERVAS POR USUARIO ==========
        public async Task<IActionResult> ReservasPorUsuarioPDF()
        {
            var grupos = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Hotel)
                .GroupBy(r => r.Usuario.NombreCompleto)
                .ToListAsync();

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph("REPORTE - RESERVAS POR USUARIO")
                .SetFont(BoldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph("\n"));

            foreach (var grupo in grupos)
            {
                doc.Add(new Paragraph("Usuario: " + grupo.Key)
                    .SetFont(BoldFont)
                    .SetFontSize(14));

                Table tabla = new Table(3).UseAllAvailableWidth();
                tabla.AddHeaderCell(new Cell().Add(new Paragraph("Hotel").SetFont(BoldFont)));
                tabla.AddHeaderCell(new Cell().Add(new Paragraph("Inicio").SetFont(BoldFont)));
                tabla.AddHeaderCell(new Cell().Add(new Paragraph("Fin").SetFont(BoldFont)));

                foreach (var r in grupo)
                {
                    tabla.AddCell(r.Hotel.Nombre);
                    tabla.AddCell(r.FechaInicio.ToShortDateString());
                    tabla.AddCell(r.FechaFin.ToShortDateString());
                }

                doc.Add(tabla);
                doc.Add(new Paragraph("\n"));
            }

            doc.Close();

            return File(stream.ToArray(), "application/pdf", "ReservasPorUsuario.pdf");
        }

        // ========== REPORTE 3: HOTELES MÁS RESERVADOS ==========
        public async Task<IActionResult> HotelesMasReservadosPDF()
        {
            var datos = await _context.Reservas
                .Include(r => r.Hotel)
                .GroupBy(r => r.Hotel.Nombre)
                .Select(g => new
                {
                    Hotel = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph("REPORTE - HOTELES MÁS RESERVADOS")
                .SetFont(BoldFont)
                .SetFontSize(18)
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph("\n"));

            Table table = new Table(2).UseAllAvailableWidth();
            table.AddHeaderCell(new Cell().Add(new Paragraph("Hotel").SetFont(BoldFont)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Reservas").SetFont(BoldFont)));

            foreach (var item in datos)
            {
                table.AddCell(item.Hotel);
                table.AddCell(item.Total.ToString());
            }

            doc.Add(table);
            doc.Close();

            return File(stream.ToArray(), "application/pdf", "HotelesMasReservados.pdf");
        }
    }
}