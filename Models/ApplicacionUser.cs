using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hoteleria.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(120)]
        public string NombreCompleto { get; set; }  // obligatorio

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }        // obligatorio
    }
}