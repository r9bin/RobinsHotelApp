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
    public class DbContextFactory : IdbContextFactoryHelper
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public DbContextFactory()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            _options = options.Options;

        }

        public DbContextOptions<ApplicationDbContext> CreateDbContext()
        {
            return _options;
        }
    }
}
