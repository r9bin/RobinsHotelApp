using HotelApp.Menus.Menus_Interfaces;
using HotelApp.Services.Service_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Menus
{
    internal class BookingMenu(IBookingServiceManager bookingServiceManager) : IBookingMenu
    {
        public void BookingMenuNavigation()
        {
            Console.Clear();
            Console.WriteLine("Bokningsmeny");
            Console.WriteLine("1. Skapa bokning");
            Console.WriteLine("2. Visa bokningar");
            Console.WriteLine("3. Uppdatera bokning");
            Console.WriteLine("4. Ta bort bokning");
            Console.WriteLine("5. Tillbaka");

            int BookingChoice;
            if (int.TryParse(Console.ReadLine(), out BookingChoice))
            {
                switch (BookingChoice)
                {
                    case 1:
                        bookingServiceManager.StartBooking();
                        break;

                    case 2:
                        bookingServiceManager.ViewBookings();
                        break;

                    case 3:
                        bookingServiceManager.UpdateBooking();
                        break;

                    case 4:
                        bookingServiceManager.CancelBooking();
                        break;

                    case 5:
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltigt val. Vänligen välj ett alternativ mellan 1 till 5.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
