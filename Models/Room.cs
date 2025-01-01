using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int AmmountOfBeds { get; set; }
        public int Size { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int ExtraBedOption { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
