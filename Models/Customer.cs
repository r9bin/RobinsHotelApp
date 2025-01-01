using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        [Range(15, 100)]
        public int Age { get; set; }
        public string? Email { get; set; }

        public int? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }


}
