using HotelApp.Data;
using HotelApp.Factory.DbContext_Interface;
using HotelApp.Models;
using HotelApp.Services.Service_Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HotelApp.Services
{
    internal class RoomServiceManager : IRoomServiceManager
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public RoomServiceManager(IdbContextFactoryHelper dbContextFactory)
        {
            _options = dbContextFactory.CreateDbContext();
        }



        public void CreateRoom()
        {
            using (var dbContext = new ApplicationDbContext(_options))
            {
                Console.WriteLine("Skapa ett nytt Rum");
                Console.WriteLine("=====================");

                string? numberInput;
                while (true)
                {
                    Console.WriteLine("Ange numret på Rummet: ");
                    numberInput = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(numberInput))
                    {
                        Console.WriteLine("Rumsnumret får inte vara tomt.");
                        continue;
                    }

                    if (!int.TryParse(numberInput, out _))
                    {
                        Console.WriteLine("Rumsnumret måste vara ett giltigt nummer.");
                        continue;
                    }

                    var existingRoom = dbContext.Room.FirstOrDefault(r => r.RoomNumber == numberInput);
                    if (existingRoom != null)
                    {
                        Console.WriteLine("Det finns redan ett rum med detta rumsnummer.");
                    }
                    else
                    {
                        break;
                    }
                }

                Console.Clear();
                int bedsInput;
                while (true)
                {
                    Console.WriteLine("Ange hur många sängar Rummet har, 1 till 2:");

                    if (int.TryParse(Console.ReadLine(), out bedsInput) && (bedsInput == 1 || bedsInput == 2))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltig inmatning. Ange 1 eller 2.");
                    }
                }

                Console.Clear();
                Console.WriteLine("Ange Storleken på Rummet: ");
                Console.WriteLine("(Rum över 60 i storlek får möjlighet för extra sängval vid bokning)");
                int sizeInput;
                while (!int.TryParse(Console.ReadLine(), out sizeInput) || sizeInput <= 0)
                {
                    Console.WriteLine("Ogiltig inmatning. Ange ett positivt heltal för storleken.");
                }

                dbContext.Room.Add(new Room
                {
                    RoomNumber = numberInput,
                    Size = sizeInput,
                    AmmountOfBeds = bedsInput
                });
                dbContext.SaveChanges();
                Console.WriteLine($"Rummet '{numberInput}' skapades!");
                Console.ReadKey();
            }
        }

        public void ViewRooms()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var rooms = dbContext.Room
                    .Include(r => r.Bookings)
                    .ThenInclude(b => b.Customer)
                    .ToList();

                if (rooms.Any())
                {
                    Console.WriteLine("Här är en lista över alla helt lediga Rum:");                
                    Console.WriteLine("========================================");

                    foreach (var room in rooms)
                    {
                        bool isAvailable = !room.Bookings.Any();
                        if (isAvailable)
                        {
                            Console.WriteLine($"Rumsnummer: {room.RoomNumber}, Storlek: {room.Size}, Sängplatser: {room.AmmountOfBeds}");
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Här är en lista över rum med vissa bokningar:");
                    Console.WriteLine("========================================");

                    foreach (var room in rooms)
                    {
                        if (room.Bookings.Any())
                        {
                            foreach (var booking in room.Bookings)
                            {
                                if (booking.Customer != null)
                                {
                                    Console.WriteLine($"Rum: {booking.Room.RoomNumber}, Bokad av: {booking.Customer.Name}, Sängar: {booking.Room.AmmountOfBeds + booking.Room.ExtraBedOption}st, från {booking.CheckInDate:yyyy-MM-dd} till {booking.CheckOutDate:yyyy-MM-dd}");
                                    Console.WriteLine();
                                }
                                else
                                {
                                    Console.WriteLine("  Bokad utan kundinformation");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Det finns inga rum registrerade.");
                }

                Console.ReadKey();
            }
        }

        public void UpdateRoom()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var rooms = dbContext.Room
                            .Where(room => !dbContext.Booking
                            .Any(b => b.RoomId == room.RoomId))
                            .ToList();

                if (rooms.Any())
                {
                    Console.WriteLine("Lista över rum som inte har en Bokning:");
                    Console.WriteLine("========================================");

                    foreach (var room in rooms)
                    {
                        Console.WriteLine($"ID: {room.RoomId}, Rumsnummer: {room.RoomNumber}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Välj ID på Rummet du vill Uppdatera");

                int userInputUpdate;

                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out userInputUpdate))
                    {
                        var roomToUpdate = rooms.FirstOrDefault(c => c.RoomId == userInputUpdate);

                        if (roomToUpdate != null)
                        {
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine($"Du uppdaterar Rummet med Rumsnummer: {roomToUpdate.RoomNumber} och RumID: {roomToUpdate.RoomId}");

                                Console.WriteLine("Välj vad du vill ändra hos Kunden:");
                                Console.WriteLine("1. Rumsnummer");
                                Console.WriteLine("2. Antal Sängar");
                                Console.WriteLine("3. Storlek");

                                if (int.TryParse(Console.ReadLine(), out int choice))
                                {
                                    switch (choice)
                                    {
                                        case 1:
                                            string? numberInput;
                                            while (true)
                                            {
                                                Console.WriteLine("Ange nytt Rumsnummer:");

                                                numberInput = Console.ReadLine();
                                              
                                                if (string.IsNullOrWhiteSpace(numberInput))
                                                {
                                                    Console.WriteLine("Rumsnummer får inte vara tomt. Försök igen.");
                                                    continue;
                                                }

                                                if (!int.TryParse(numberInput, out int roomNumber))
                                                {
                                                    Console.WriteLine("Rumsnummer måste vara ett nummer. Försök igen.");
                                                    continue;
                                                }

                                                var existingRoom = dbContext.Room.FirstOrDefault(r => r.RoomNumber == numberInput);
                                                if (existingRoom != null)
                                                {
                                                    Console.WriteLine("Det finns redan ett rum med detta rumsnummer.");
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            roomToUpdate.RoomNumber = numberInput;
                                            break;


                                        case 2:
                                            while (true)
                                            {
                                                Console.WriteLine("Ange nytt antal Sängar, 1 eller 2:");

                                                if (int.TryParse(Console.ReadLine(), out int bedsInput))
                                                {
                                                    if (bedsInput == 1 || bedsInput == 2)
                                                    {
                                                        roomToUpdate.AmmountOfBeds = bedsInput;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Antalet sängar måste vara antingen 1 eller 2. Försök igen.");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Ogiltig inmatning. Antalet sängar måste vara ett heltal. Försök igen.");
                                                }
                                            }
                                            break;


                                        case 3:
                                            while (true)
                                            {
                                                int newSize;
                                                Console.WriteLine("Ange ny Storlek (mellan 40 och 90):");

                                                if (int.TryParse(Console.ReadLine(), out newSize))
                                                {
                                                    if (newSize >= 40 && newSize <= 90)
                                                    {
                                                        roomToUpdate.Size = newSize;
                                                        break;  
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Storleken måste vara mellan 40 och 90. Försök igen.");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Ogiltigt värde. Storleken måste vara ett heltal. Försök igen.");
                                                }
                                            }
                                            break;


                                        default:
                                            continue;
                                    }

                                    dbContext.SaveChanges();
                                    Console.WriteLine("Ändringarna har sparats!");
                                    Console.ReadLine();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Inget Rum hittades med det angivna ID:t.");
                            Console.WriteLine("Försök igen.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt ID. Försök igen.");
                    }
                }
            }
        }

        public void DeleteRoom()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var rooms = dbContext.Room
                    .Include(r => r.Bookings).ToList();

                if (rooms.Any())
                {
                    Console.WriteLine("Här är en lista över alla Lediga Rum:");
                    Console.WriteLine("========================================");

                    foreach (var room in rooms.Where(c => c.IsAvailable))
                    {
                        Console.WriteLine($"ID: {room.RoomId}, Rumsnummer: {room.RoomNumber}");
                    }
                }
                else
                {
                    Console.WriteLine("Det finns inga Rum");
                }

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Välj vilket ID på det rum du vill ta bort");
                    Console.WriteLine("(Eller skriv 'exit' för att avbryta)");
                    var input = Console.ReadLine();

                    if (input?.ToLower() == "exit")
                    {
                        Console.WriteLine("Åtgärden avbröts.");
                        break;
                    }

                    if (int.TryParse(input, out int roomId))
                    {
                        var roomToDelete = rooms.FirstOrDefault(c => c.RoomId == roomId);
                        if (roomToDelete != null)
                        {
                            if (roomToDelete.Bookings.Any())
                            {
                                Console.WriteLine($"Det finns bokningar på rummet med Rumsnummer: '{roomToDelete.RoomNumber}' och ID: {roomToDelete.RoomId}. " +
                                                  "Du kan inte ta bort rummet medan det är bokat.");
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"Rummet med Rumsnummer: '{roomToDelete.RoomNumber}' och ID: {roomToDelete.RoomId} är nu borttaget!");

                      
                                dbContext.Room.Remove(roomToDelete);                            
                                dbContext.SaveChanges();
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Inget Rum hittades med det angivna ID:t. Försök igen.");
                        }
                    }
                }
            }
        }
    }
}
