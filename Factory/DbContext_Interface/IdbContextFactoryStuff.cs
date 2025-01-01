using HotelApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Factory.DbContext_Interface
{
    public interface IdbContextFactoryStuff
    {
        DbContextOptions<ApplicationDbContext> CreateDbContext();
    }
}