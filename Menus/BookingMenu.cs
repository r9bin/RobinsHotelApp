using HotelApp.Menus.Menus_Interfaces;
using HotelApp.Services.Service_Interfaces;
using Spectre.Console;
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
            while (true)
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
                            continue;
                    }
                }
            }
        }
    }
}
