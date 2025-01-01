using HotelApp.Data.Data_Interfaces;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Data
{
    public class DataInitializer : IDataInitializer
    {
        public void MigrateAndSeed(ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate(); 

            if (!dbContext.Customer.Any())
            {
                SeedCustomers(dbContext);
            }

            if (!dbContext.Room.Any())
            {
                SeedRooms(dbContext);
            }

            dbContext.SaveChanges();
        }

        public void SeedCustomers(ApplicationDbContext dbContext)
        {
            var customers = new List<Customer>
            {
                new Customer { Name = "Anna", LastName = "Andersson", Age = 25, Email = "anna.andersson@example.com", PhoneNumber = 123456789, IsActive = true },
                new Customer { Name = "Erik", LastName = "Eriksson", Age = 35, Email = "erik.eriksson@example.com", PhoneNumber = 987654321, IsActive = true },
                new Customer { Name = "Lisa", LastName = "Larsson", Age = 45, Email = "lisa.larsson@example.com", PhoneNumber = 456123789, IsActive = true },
                new Customer { Name = "Olof", LastName = "Olsson", Age = 55, Email = "olof.olsson@example.com", PhoneNumber = 321654987, IsActive = true },
                new Customer { Name = "Bernt", LastName = "Karlsson", Age = 73, Email = "bernt.karlsson@example.com", PhoneNumber = 392935055, IsActive = true}
            };

            dbContext.Customer.AddRange(customers);
        }

        public void SeedRooms(ApplicationDbContext dbContext)
        {
            var rooms = new List<Room>
            {
                new Room { RoomNumber = "100", AmmountOfBeds = 1, Size = 55, IsAvailable = true},
                new Room { RoomNumber = "101", AmmountOfBeds = 1, Size = 53, IsAvailable = true},
                new Room { RoomNumber = "102", AmmountOfBeds = 1, Size = 58, IsAvailable = true},
                new Room { RoomNumber = "103", AmmountOfBeds = 2, Size = 75, IsAvailable = true},
                new Room { RoomNumber = "104", AmmountOfBeds = 2, Size = 71, IsAvailable = true},
                new Room { RoomNumber = "105", AmmountOfBeds = 2, Size = 82, IsAvailable = true},
                new Room { RoomNumber = "106", AmmountOfBeds = 2, Size = 87, IsAvailable = true}
            };
            dbContext.Room.AddRange(rooms);
        }
    }
}
