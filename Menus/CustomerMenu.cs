using HotelApp.Menus.Menus_Interfaces;
using HotelApp.Services;
using HotelApp.Services.Service_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Menus
{
    internal class CustomerMenu(ICustomerServiceManager customerServiceManager) : ICustomerMenu
    {
        
        public void CustomerMenuNavigation()
        {
            
            while (true)
            {
                // Gör en metod med dessa val
                Console.Clear();
                Console.WriteLine("Kundmeny");
                Console.WriteLine("1. Skapa kund");
                Console.WriteLine("2. Visa kundinformation");
                Console.WriteLine("3. Uppdatera kundinformation");
                Console.WriteLine("4. Ta bort kund");
                Console.WriteLine("5. Tillbaka");

                int CustomerChoice;
                if (int.TryParse(Console.ReadLine(), out CustomerChoice))
                {
                    switch (CustomerChoice)
                    {
                        case 1:
                            customerServiceManager.CreateCustomer();
                            break;
                        case 2:
                            customerServiceManager.CustomerInfo();
                            break;
                        case 3:
                            customerServiceManager.UpdateCustomer();
                            break;
                        case 4:
                            customerServiceManager.DeleteCustomer();
                            break;
                        case 5:
                            return;
                        default:
                            // Metod för detta
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
}
