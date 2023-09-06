using System.ComponentModel.DataAnnotations;

namespace CarRental.Model
{
    public class Cars
    {
        [Required]
        [Key]
        public int Carid { get; set; }
        public string? Carname { get; set; }
        public string? Cartype { get; set; }
        public int? PerDayCharge { get; set; }
        public int? ChargePerKm { get; set; }
        public string? Available { get; set; }

        public string? Photo { get; set; }
        public virtual ICollection<CarRents>? CarRents { get; set; }
    }
}
