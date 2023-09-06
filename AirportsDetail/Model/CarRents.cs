using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Model
{
    public class CarRents
    {
        [Key]
        [Required]
        public int RentId { get; set; }
        public int? Carid { get; set; }
        public int? Customerid { get; set; }
        public DateTime? RentOrderDate { get; set; }
        public int? OdoReading { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? ReturnOdoReading { get; set; }
        public string? Licensenumber { get; set; }
        [ForeignKey(nameof(Carid))]
        public virtual Cars? Car { get; set; }
        [ForeignKey(nameof(Customerid))]
        public virtual Customers? Customer { get; set; }
    }
}
