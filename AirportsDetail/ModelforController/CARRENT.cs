using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace CarRental.ModelforController
{
    public class CARRENT
    {
        public int RentId { get; set; }
        public Nullable<int> CarId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime RentOrderDate { get; set; }
        public Nullable<int> OdoReading { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]

        public DateTime ReturnDate { get; set; }

        public Nullable<int> ReturnOdoReading { get; set; }
        public string LicenseNumber
        {
            get;
            set;
        }


    }
    public class searchdates
    {

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RentDate
        {
            get;
            set;
        }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReturnDate
        {
            get;
            set;
        }
    }
}