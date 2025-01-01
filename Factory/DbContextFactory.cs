using HotelApp.Data;
using HotelApp.Factory.DbContext_Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Factory
{
    public class DbContextFactory : IdbContextFactoryStuff
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public DbContextFactory()
        {
            // 4: Create json builder (boiler plate code)
            // Makes it possible to connect to appsettings.json
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            // 6: Create DBContext(boiler plate code).
            // Create options & connectionstring variables(boiler plate code).
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
            //DbContextOptions<ApplicationDbContext> options

            _options = options.Options;

        }

        public DbContextOptions<ApplicationDbContext> CreateDbContext()
        {
            return _options;
        }
    }
}
