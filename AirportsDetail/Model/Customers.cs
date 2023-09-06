using System.ComponentModel.DataAnnotations;

namespace CarRental.Model
{
    public class Customers
    {
        [Required]
        [Key]

        public int Customerid { get; set; }
        public string? CustomerName { get; set; }
        public string? mail { get; set; }
        public string? Password { get; set; }
        public int? LoyaltyPoints { get; set; }


        public virtual ICollection<CarRents>? CarRents { get; set; }
    }
}
