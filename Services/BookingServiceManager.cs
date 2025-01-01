using HotelApp.Data;
using HotelApp.Factory.DbContext_Interface;
using HotelApp.Models;
using HotelApp.Services.Service_Interfaces;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Services
{
    internal class BookingServiceManager : IBookingServiceManager
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public BookingServiceManager(IdbContextFactoryHelper dbContextFactory)
        {
            _options = dbContextFactory.CreateDbContext();
        }



        public void StartBooking()
        {
            Console.Clear();
            Console.WriteLine("Välkommen till Robins Hotel!");

            var checkInDate = CheckIn();
            var checkOutDate = CheckOut(checkInDate);       

            var selectedRoom = BookRoom(checkInDate, checkOutDate);
            var customer = GetOrCreateCustomer();


            CreateBooking(customer, checkInDate, checkOutDate, selectedRoom);
        }

        public void CreateBooking(Customer customer, DateTime checkInDate, DateTime checkOutDate, Room selectedRoom)
        {
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var existingCustomer = dbContext.Customer
                    .FirstOrDefault(c => c.CustomerId == customer.CustomerId);

                if (existingCustomer == null)
                {
                    Console.WriteLine("Kunden finns inte i systemet. Bokningen kunde inte genomföras.");
                    return;
                }

                var room = dbContext.Room
                    .Include(r => r.Bookings)
                    .FirstOrDefault(r => r.RoomNumber == selectedRoom.RoomNumber);

                if (room != null)
                {
                    bool isRoomAvailable = !room.Bookings
                        .Any(b => b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate);

                    if (!isRoomAvailable)
                    {
                        Console.WriteLine("Tyvärr, det valda rummet är inte tillgängligt för den valda perioden.");
                        return;
                    }

 
                    int maxExtraBeds = GetMaxExtraBeds(room.Size);

                    if (maxExtraBeds > 0)
                    {
                        Console.WriteLine($"Detta rum har {room.AmmountOfBeds} sängar. Du kan lägga till upp till {maxExtraBeds} extra säng(ar).");

                        int additionalBeds;
                        while (true)
                        {
                            Console.WriteLine($"Hur många extra sängar vill du lägga till? (Max {maxExtraBeds}):");
                            string? input = Console.ReadLine();

                            if (int.TryParse(input, out additionalBeds) && additionalBeds >= 0 && additionalBeds <= maxExtraBeds)
                            {
                                room.ExtraBedOption = additionalBeds;
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Ogiltigt antal sängar. Ange ett värde mellan 0 och {maxExtraBeds}.");
                            }
                        }
                    }

                    var booking = new Booking
                    {
                        Customer = existingCustomer,
                        Room = room,
                        CheckInDate = checkInDate,
                        CheckOutDate = checkOutDate
                    };

                    dbContext.Booking.Add(booking);
                    dbContext.SaveChanges();

                    if (room.ExtraBedOption > 0)
                    {
                        Console.WriteLine($"Bokning genomförd! Du har bokat rumsnummer: {room.RoomNumber} från {checkInDate:yyyy-MM-dd} till {checkOutDate:yyyy-MM-dd} med {room.ExtraBedOption} extra säng(ar).");
                    }
                    else
                    {
                        Console.WriteLine($"Bokning genomförd! Du har bokat rumsnummer: {room.RoomNumber} från {checkInDate:yyyy-MM-dd} till {checkOutDate:yyyy-MM-dd}");
                    }
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Tyvärr, det valda rummet är inte längre tillgängligt.");
                }
            }
        }

        public int GetMaxExtraBeds(int roomSize)
        {
            if (roomSize <= 60)
            {
                return 0;
            }
            else if (roomSize > 61)
            {
                return 2;
            }

            return 0;
        }

        public Room BookRoom(DateTime checkInDate, DateTime checkOutDate)
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var rooms = dbContext.Room
                    .Include(r => r.Bookings)
                    .ToList();

                var availableRooms = rooms
                    .Where(r => !r.Bookings.Any(b => b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate))
                    .ToList();

                if (availableRooms.Any())
                {
                    Console.WriteLine($"Lediga Rum mellan: {checkInDate:yyyy-MM-dd} - {checkOutDate:yyyy-MM-dd}");
                    Console.WriteLine("========================================");

                    foreach (var room in availableRooms)
                    {
                        Console.WriteLine($"Rumsnummer: {room.RoomNumber} | Storlek: {room.Size} | Sängar: {room.AmmountOfBeds}");
                    }

                    string userRoomChoice;
                    Room selectedRoom = null;

                    while (selectedRoom == null)
                    {
                        Console.WriteLine("Vilket Rum vill du boka? Skriv rumsnummer:");
                        userRoomChoice = Console.ReadLine();

                        selectedRoom = availableRooms.FirstOrDefault(r => r.RoomNumber == userRoomChoice);

                        if (selectedRoom == null)
                        {
                            Console.WriteLine("Ogiltigt val eller rummet är inte tillgängligt. Försök igen.");
                        }
                    }

                    Console.Clear();
                    Console.WriteLine($"Du har valt Rumsnummer: {selectedRoom.RoomNumber} mellan {checkInDate:yyyy-MM-dd} - {checkOutDate:yyyy-MM-dd}");
                    return selectedRoom;
                }
                else
                {
                    Console.WriteLine("Det finns inga lediga rum.");
                    return null;
                }
            }
        }

        public Customer GetOrCreateCustomer()
        {
            string? customerName;
            while (true)
            {
                Console.WriteLine("Ange ditt namn:");
                customerName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(customerName) && customerName.All(char.IsLetter))
                {
                    break;
                }

                Console.WriteLine("Namnet får inte vara tomt och måste bestå av bokstäver. Vänligen ange ett giltigt namn.");
            }


            using (var dbContext = new ApplicationDbContext(_options))
            {
                var existingCustomers = dbContext.Customer
                    .Where(c => c.Name.ToLower() == customerName.ToLower())
                    .ToList();

                if (existingCustomers.Any())
                {
                    Console.Clear();
                    Console.WriteLine("Detta namn fanns redan i databasen. Välj ett ID");
                    Console.WriteLine("(Eller välj 0, för att skapa ny användare)");

                    for (int i = 0; i < existingCustomers.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {existingCustomers[i].Name} {existingCustomers[i].LastName}");
                    }

                    int selectedIndex;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out selectedIndex))
                        {
                            if (selectedIndex == 0)
                            {
                                break;
                            }
                            else if (selectedIndex > 0 && selectedIndex <= existingCustomers.Count)
                            {
                                var selectedCustomer = existingCustomers[selectedIndex - 1];
                                Console.WriteLine($"Välkommen tillbaka, {selectedCustomer.Name}!");
                                return selectedCustomer;
                            }
                        }

                        Console.WriteLine("Ogiltigt val. Försök igen.");
                    }
                }

                Console.Clear();
                Console.WriteLine("Du är inte registrerad i vårt system. Vi skapar ett nytt konto åt dig.");

                string? userLastName;
                while (true)
                {
                    Console.WriteLine($"{customerName}, vad har ni för Efternamn?");
                    userLastName = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(userLastName) && userLastName.All(char.IsLetter))
                    {
                        break;
                    }

                    Console.WriteLine("Efternamn får inte vara tomt och måste bestå av bokstäver. Försök igen.");
                }

                Console.Clear();
                int userAge;
                while (true)
                {
                    Console.WriteLine("Hur gammal är du? (Från 15 till 100)");
                    string? ageInput = Console.ReadLine();

                    if (int.TryParse(ageInput, out userAge) && userAge >= 15 && userAge <= 100)
                    {
                        break;
                    }

                    Console.WriteLine("Ogiltig ålder. Vänligen ange en giltig ålder som ett heltal större än 15 år.");
                }

                Console.Clear();
                Console.WriteLine("Vad har du för Telefonnummer? (Enter för att skippa)");
                string? userPhoneInput = Console.ReadLine();
                int? userPhoneNumber = null;

                if (!string.IsNullOrWhiteSpace(userPhoneInput))
                {
                    if (int.TryParse(userPhoneInput, out int parsedPhone))
                    {
                        userPhoneNumber = parsedPhone; 
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt telefonnummer. Försök igen.");
                        Console.WriteLine("(Eller tryck Enter för att lägga till senare)");
                    }
                }

                Console.WriteLine("Vad har du för Email?");
                string? userEmail = Console.ReadLine();

                var newCustomer = new Customer
                {
                    Name = customerName,
                    LastName = userLastName,
                    Age = userAge,
                    PhoneNumber = userPhoneNumber,
                    Email = userEmail
                };

                dbContext.Customer.Add(newCustomer);
                dbContext.SaveChanges();

                Console.WriteLine($"Välkommen, {newCustomer.Name}! Ditt konto har skapats.");

                return newCustomer;
            }
        }

        public DateTime CheckIn()
        {
            using (var dbContext = new ApplicationDbContext(_options))
            {
                Console.WriteLine("Ange datum för incheckning");
                Console.WriteLine("(yyyy-mm-dd)");
            }
            DateTime checkInDate;
            while (true)
            {
                if (DateTime.TryParse(Console.ReadLine(), out checkInDate))
                {
                    if (checkInDate >= DateTime.Today)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Incheckning kan inte vara tidigare än dagens datum. Försök igen (yyyy-mm-dd):");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt datumformat, använd detta format: yyyy-mm-dd");
                }
            }
            return checkInDate;
        }

        public DateTime CheckOut(DateTime checkInDate)
        {
            DateTime checkOutDate;

            while (true)
            {
                Console.WriteLine("Ange datum för utcheckning");
                if (DateTime.TryParse(Console.ReadLine(), out checkOutDate))
                {
                    if (checkOutDate > checkInDate && checkOutDate >= DateTime.Today)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Utcheckning måste vara efter incheckning och inte tidigare än dagens datum. Försök igen (yyyy-mm-dd):");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt datumformat, använd detta format: yyyy-mm-dd");
                }
            }
            return checkOutDate;
        }        

        public void CancelBooking()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var bookings = dbContext.Booking
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .ToList();

                if (bookings.Any())
                {
                    Console.WriteLine("Här är en lista över alla BOKNINGAR:");
                    Console.WriteLine("========================================");

                    foreach (var booking in bookings)
                    {
                        if (booking.Room != null)
                        {
                            Console.WriteLine($"BokningID: {booking.BookingId}, Kund: {booking.Customer.Name}, Rum: {booking.Room.RoomNumber}, Incheckning: {booking.CheckInDate:yyyy-MM-dd}, Utcheckning: {booking.CheckOutDate:yyyy-MM-dd}");
                        }
                        else
                        {
                            Console.WriteLine($"BokningID: {booking.BookingId}, Bokning för {booking.Customer.Name} finns, men rummet är inte tilldelat.");
                        }
                    }

                    int bookingIdChoice;
                    while (true)
                    {
                        Console.WriteLine("Ange bokningsID du vill avboka:");
                        string input = Console.ReadLine();

                        if (int.TryParse(input, out bookingIdChoice))
                        {
                            var bookingToCancel = dbContext.Booking
                                .FirstOrDefault(b => b.BookingId == bookingIdChoice);

                            if (bookingToCancel != null)
                            {
                                var room = bookingToCancel.Room;

                                if (room != null)
                                {
                                    if (room.AmmountOfBeds > 2) 
                                    {
                                        room.AmmountOfBeds = 2;
                                    }

                                    room.IsAvailable = true;
                                }

                                dbContext.Booking.Remove(bookingToCancel);
                                dbContext.SaveChanges();

                                Console.WriteLine("Bokningen har avbokats och rummet har blivit tillgängligt igen.");
                                Console.ReadKey();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Ingen bokning hittades för det angivna BokningsID:t.");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ogiltig inmatning, vänligen skriv in ett giltigt boknings-ID.");
                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Inga bokningar finns.");
                    Console.ReadKey();
                }
            }
        }

        public void ViewBookings()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var bookings = dbContext.Booking
                    .Include(b => b.Room)
                    .Include(b => b.Customer)
                    .ToList();

                if (bookings.Any())
                {
                    Console.WriteLine("Här är en lista över alla BOKNINGAR:");
                    Console.WriteLine("========================================");

                    foreach (var booking in bookings)
                    {
                        if (booking.Room != null)
                        {
                            Console.WriteLine($"ID: {booking.BookingId}, Kund: {booking.Customer.Name}, Bokat Rum: {booking.Room.RoomNumber}, Från: {booking.CheckInDate.ToString("yyyy-MM-dd")} Till {booking.CheckOutDate.ToString("yyyy-MM-dd")}");
                        }
                        else
                        {
                            Console.WriteLine($"Bokning för {booking.Customer.Name} finns, men rummet är inte tilldelat.");                       
                        }                     
                    }
                }
                else
                {
                    Console.WriteLine("Inga bokningar finns.");
                }
                Console.ReadKey();
            }
        }

        public void UpdateBooking()
        {
            Console.WriteLine("Med brisst av främst tid men även kunskap, lämnas denna utan funktion...");
            Console.ReadKey();
        }          
    }
}
