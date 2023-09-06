using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Model
{
    public class admins
    {
        [Required]
        [Key]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
