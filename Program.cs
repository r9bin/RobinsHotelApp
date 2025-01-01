using Autofac;
using HotelApp.AutoFacBuilder;
using HotelApp.Main_Interfaces;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace HotelApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // TO-DO-LIST

            //MÅSTE GÖRAS
            // När man får valet stämmer detta? Y/N, så går det välja ett val som inte finns, sen fortsätter programmet, gör en while-loop där
            //RoomServiceManager har väldigt metoder jämförelse med dom andra
            //Kika i dokumentet du skickade till Tord om "update", kanske går att fixa booking update med dbContext.BookingID?

            //Dubbelkolla på ALLA val att det inte går att köra enter/blankt eller siffra etc!!!!!!
            //Rensa upp massa kommentarer
            //lägg till Console.ReadKey på vissa ställen


            // ERD diagram
            // linq-koder mot min databas


            // EXTRA OM DU HAR TID

            // refactoring, service och controller classes
            // Service-klass har using dbContext



            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApp>();
                app.Run();
            }
        }
    }
}
