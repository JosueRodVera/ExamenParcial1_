using System.ComponentModel.DataAnnotations;

namespace Hoteleria.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        // FK User
        public string UsuarioId { get; set; }
        public ApplicationUser Usuario { get; set; }

        // FK Hotel
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}