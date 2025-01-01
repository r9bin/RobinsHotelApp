using HotelApp.Menus.Menus_Interfaces;
using HotelApp.Services.Service_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Menus
{
    public class RoomMenu(IRoomServiceManager roomServiceManager) : IRoomMenu
    {
        public void RoomMenuNavigation()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Kundmeny");
                Console.WriteLine("1. Lägg till Rum");
                Console.WriteLine("2. Visa Rum");
                Console.WriteLine("3. Uppdatera Rum");
                Console.WriteLine("4. Ta bort Rum");
                Console.WriteLine("5. Tillbaka");

                int RoomChoice;
                if (int.TryParse(Console.ReadLine(), out RoomChoice))
                {
                    switch (RoomChoice)
                    {
                        case 1:
                            roomServiceManager.CreateRoom();
                            break;

                        case 2:
                            roomServiceManager.ViewRooms();
                            break;

                        case 3:
                            roomServiceManager.UpdateRoom();
                            break;

                        case 4:
                            roomServiceManager.DeleteRoom();
                            break;

                        case 5:
                            return;
                        default:

                            break;
                    }
                }
            }
        }
    }
}
