using HotelApp.Data;
using HotelApp.Factory.DbContext_Interface;
using HotelApp.Main_Interfaces;
using HotelApp.Models;
using HotelApp.Services.Service_Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Services
{
    internal class CustomerServiceManager : ICustomerServiceManager
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public CustomerServiceManager(IdbContextFactoryHelper dbContextFactory)
        {
            _options = dbContextFactory.CreateDbContext();
        }



        public void CreateCustomer()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                Console.WriteLine("Skapa en ny Kund");
                Console.WriteLine("=====================");

                string? customerName;
                while (true)
                {
                    Console.WriteLine("Ange ditt namn:");
                    customerName = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(customerName) && !customerName.Any(char.IsDigit))
                    {
                        break;
                    }

                    Console.WriteLine("Namnet får inte vara tomt och får inte innehålla siffror. Vänligen ange ett giltigt namn.");
                }

                string? lastNameInput;
                while (true)
                {
                    Console.WriteLine("Ange ditt Efternamn:");
                    lastNameInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(lastNameInput) && !lastNameInput.Any(char.IsDigit))
                    {
                        break;
                    }

                    Console.WriteLine("Efternamnet får inte vara tomt och får inte innehålla siffror. Vänligen ange ett giltigt efternamn.");
                }

                int ageInput;
                while (true)
                {
                    Console.WriteLine("Ange Ålder:");
                    Console.WriteLine("(Från 15 till 100år)");
                    var ageInputRaw = Console.ReadLine();

                    if (int.TryParse(ageInputRaw, out ageInput) && ageInput >= 15 && ageInput <= 100)
                    {
                        break;
                    }

                    Console.WriteLine("Ogiltig ålder. Vänligen ange ett giltigt heltal mellan 15 och 100.");
                }

                dbContext.Customer.Add(new Customer
                {
                    Age = ageInput,
                    Name = customerName,
                    LastName = lastNameInput
                });
                dbContext.SaveChanges();
                Console.WriteLine("Användaren Skapades!");
                Console.ReadKey();
            }
        }

        public void CustomerInfo()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var customers = dbContext.Customer.ToList();

                if (customers.Any())
                {
                    Console.WriteLine("Här är en lista över alla AKTIVA kunder:");
                    Console.WriteLine("========================================");

                    foreach (var customer in customers.Where(c => c.IsActive))
                    {
                        Console.WriteLine($"ID: {customer.CustomerId}, Namn: {customer.Name} {customer.LastName}");
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Här är en lista över alla EJ AKTIVA kunder:");
                    Console.WriteLine("========================================");
                    foreach (var customer in customers.Where(c => !c.IsActive))
                    {
                        Console.WriteLine($"ID: {customer.CustomerId}, Namn: {customer.Name} {customer.LastName}");
                    }

                    while (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Välj den Kund du vill ha information ifrån (ange ID):");

                        if (int.TryParse(Console.ReadLine(), out var userIDchoice))
                        {
                            var selectedCustomer = dbContext.Customer.FirstOrDefault(c => c.CustomerId == userIDchoice);

                            if (selectedCustomer != null)
                            {
                                Console.Clear();
                                Console.WriteLine("Kundens information:");
                                Console.WriteLine($"ID: {selectedCustomer.CustomerId}");
                                Console.WriteLine($"Name: {selectedCustomer.Name}");
                                Console.WriteLine($"Last Name: {selectedCustomer.LastName}");
                                Console.WriteLine($"Age: {selectedCustomer.Age}");
                                Console.WriteLine($"E-post: {selectedCustomer.Email}");
                                Console.WriteLine($"Telefon: {selectedCustomer.PhoneNumber}");
                                Console.WriteLine($"Aktiv: {selectedCustomer.IsActive}");
                                Console.ReadLine();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Ingen kund hittades med det angivna ID:t. Försök igen.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ogiltigt ID. Försök igen.");
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Det finns inga kunder.");
                    Console.ReadKey();
                }
            }
        }

        public void UpdateCustomer()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var customers = dbContext.Customer.ToList();

                if (!customers.Any())
                {
                    Console.WriteLine("Det finns inga kunder.");
                    return;
                }

                DisplayCustomerLists(customers);

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Välj ID på den kund du vill uppdatera");
                Console.ForegroundColor = ConsoleColor.Gray;

                int userInputUpdate = 0;


                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out userInputUpdate))
                    {
                        Console.WriteLine("Ogiltigt ID. Försök igen.");
                        continue;
                    }

                    var customerToUpdate = customers.FirstOrDefault(c => c.CustomerId == userInputUpdate);

                    if (customerToUpdate == null)
                    {
                        Console.WriteLine("Ingen kund hittades med det angivna ID:t.");
                        continue;
                    }

                    UpdateCustomerDetails(customerToUpdate, dbContext);
                    return;
                }
            }
        }

        private void UpdateCustomerDetails(Customer customerToUpdate, ApplicationDbContext dbContext)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Du uppdaterar kunden: {customerToUpdate.Name} {customerToUpdate.LastName}");
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine("Välj vad du vill ändra hos Kunden:");
                Console.WriteLine("1. Förnamn");
                Console.WriteLine("2. Efternamn");
                Console.WriteLine("3. Ålder");
                Console.WriteLine("4. E-post");
                Console.WriteLine("5. Telefonnummer");
                Console.WriteLine("6. Aktivera/Inaktivera Kund");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        UpdateCustomerFirstName(customerToUpdate);
                        break;

                    case 2:
                        UpdateCustomerLastName(customerToUpdate);
                        break;

                    case 3:
                        UpdateCustomerAge(customerToUpdate);
                        break;

                    case 4:
                        UpdateCustomerEmail(customerToUpdate);
                        break;

                    case 5:
                        UpdateCustomerPhoneNumber(customerToUpdate);
                        break;

                    case 6:
                        UpdateCustomerStatus(customerToUpdate);
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        continue;
                }

                dbContext.SaveChanges();
                Console.WriteLine("Ändringarna har sparats!");
                Console.ReadKey();
                break; 
            }
        }

        private void DisplayCustomerLists(List<Customer> customers)
        {
            Console.WriteLine("Här är en lista över alla AKTIVA kunder:");
            Console.WriteLine("========================================");
            foreach (var customer in customers.Where(c => c.IsActive))
            {
                Console.WriteLine($"ID: {customer.CustomerId}, Namn: {customer.Name} {customer.LastName}");
            }

            Console.WriteLine();
            Console.WriteLine("Här är en lista över alla EJ AKTIVA kunder:");
            Console.WriteLine("========================================");
            foreach (var customer in customers.Where(c => !c.IsActive))
            {
                Console.WriteLine($"ID: {customer.CustomerId}, Namn: {customer.Name} {customer.LastName}");
            }
        }

        private void UpdateCustomerStatus(Customer customerToUpdate)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Du ändrar aktiv status för {customerToUpdate.Name} {customerToUpdate.LastName}");
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine("Vill du aktivera eller inaktivera kunden?");
                Console.WriteLine("1. Aktivera");
                Console.WriteLine("2. Inaktivera");
                Console.WriteLine("3. Avbryt");

                if (int.TryParse(Console.ReadLine(), out int statusChoice))
                {
                    switch (statusChoice)
                    {
                        case 1:
                            customerToUpdate.IsActive = true;
                            Console.WriteLine("Kunden har blivit aktiverad.");
                            return;

                        case 2:
                            customerToUpdate.IsActive = false;
                            Console.WriteLine("Kunden har blivit inaktiverad.");
                            return;

                        case 3:
                            Console.WriteLine("Ändring av aktiveringsstatus avbröts.");
                            return;

                        default:
                            continue;
                    }
                }
            }
        }

        public void UpdateCustomerFirstName(Customer customer)
        {
            customer.Name = "";
            while (true)
            {
                Console.WriteLine("Ange ditt namn:");
                customer.Name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(customer.Name) && !customer.Name.Any(char.IsDigit))
                {
                    break;
                }

                Console.WriteLine("Namnet får inte vara tomt och får inte innehålla siffror. Vänligen ange ett giltigt namn.");
            }
        }

        public void UpdateCustomerLastName(Customer customer)
        {
            customer.LastName = "";
            while (true)
            {
                Console.WriteLine("Ange ditt efternamn:");
                customer.LastName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(customer.LastName) && !customer.LastName.Any(char.IsDigit))
                {
                    break;
                }

                Console.WriteLine("Efternamnet får inte vara tomt och får inte innehålla siffror. Vänligen ange ett giltigt efternamn.");
            }
        }

        public void UpdateCustomerAge(Customer customer)
        {
            while (true)
            {
                Console.WriteLine("Ange ny Ålder (mellan 15 och 100):");

                if (int.TryParse(Console.ReadLine(), out int newAge))
                {
                    if (newAge >= 15 && newAge <= 100)
                    {
                        customer.Age = newAge;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Åldern måste vara mellan 15 och 100. Försök igen.");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt värde för ålder. Ange ett numeriskt värde mellan 15 och 100.");
                }
            }
        }

        public void UpdateCustomerEmail(Customer customer)
        {
            Console.WriteLine("Ange ny e-postadress:");
            customer.Email = Console.ReadLine();
        }

        public void UpdateCustomerPhoneNumber(Customer customer)
        {
            Console.WriteLine("Ange nytt telefonnummer (lämna tomt för att inte ändra):");

            while (true)
            {
                string? userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Telefonnumret ändrades inte.");
                    break;
                }
                else if (int.TryParse(userInput, out int newPhoneNumber))
                {
                    customer.PhoneNumber = newPhoneNumber;
                    Console.WriteLine("Telefonnumret har uppdaterats.");
                    break;
                }
                else
                {
                    Console.WriteLine("Ogiltigt telefonnummer. Försök igen.");
                }
            }
        }

        public void DeleteCustomer()
        {
            Console.Clear();
            using (var dbContext = new ApplicationDbContext(_options))
            {
                var customers = dbContext.Customer.Where(c => c.IsActive).ToList();

                if (customers.Any())
                {
                    Console.WriteLine("Här är en lista över alla Aktiva kunder:");
                    Console.WriteLine("========================================");

                    foreach (var customer in customers)
                    {
                        Console.WriteLine($"ID: {customer.CustomerId}, Namn: {customer.Name} {customer.LastName}");
                    }       
                    
                    while (true)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Välj vilket ID på den kund du vill ta bort");
                        Console.WriteLine("(Eller skriv 'exit' för att avbryta)");
                        var input = Console.ReadLine();

                        if (input?.ToLower() == "exit")
                        {
                            break;
                        }

                        if (int.TryParse(input, out int customerId))
                        {
                            var customerToDelete = customers.FirstOrDefault(c => c.CustomerId == customerId);
                            if (customerToDelete != null)
                            {
                                Console.Clear();
                                Console.WriteLine($"Kunden '{customerToDelete.Name} {customerToDelete.LastName}' är nu borttagen!");
                                customerToDelete.IsActive = false;
                                dbContext.SaveChanges();
                                Console.ReadKey();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Ingen kund hittades med det angivna ID:t. Försök igen.");
                                Console.ReadKey();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Inga aktiva kunder finns i systemet.");
                    Console.ReadKey();
                }
            }
        }
    }
}
