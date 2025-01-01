using HotelApp.Menus.Menus_Interfaces;
using HotelApp.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Menus
{
    public class MainMenu(IBookingMenu bookingMenu, ICustomerMenu customerMenu, IRoomMenu roomMenu) : IMainMenu
    {
        public void MainMenuNavigation()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Välkommen till Hotel Appen");
                Console.WriteLine("1. Hantera Bokningar");
                Console.WriteLine("2. Hantera Rum");
                Console.WriteLine("3. Hantera Kunder");
                Console.WriteLine("4. Avsluta");

                int MainChoice;
                if (int.TryParse(Console.ReadLine(), out MainChoice))
                {
                    switch (MainChoice)
                    {
                        case 1:
                            bookingMenu.BookingMenuNavigation();
                            break;

                        case 2:
                            roomMenu.RoomMenuNavigation();
                            break;

                        case 3:
                            customerMenu.CustomerMenuNavigation();
                            break;

                        case 4:
                            return;

                        default:
                            continue;
                    }
                }
            }
        }
    }
}
