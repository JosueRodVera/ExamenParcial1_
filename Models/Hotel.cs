using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hoteleria.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }

        [Required]
        [Range(20, 2000)]
        public decimal PrecioPorNoche { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        public List<Reserva>? Reservas { get; set; }
    }
}