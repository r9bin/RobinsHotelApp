namespace HotelApp.Data.Data_Interfaces
{
    public interface IDataInitializer
    {
        void MigrateAndSeed(ApplicationDbContext dbContext);
        void SeedCustomers(ApplicationDbContext dbContext);
        void SeedRooms(ApplicationDbContext dbContext);
    }
}