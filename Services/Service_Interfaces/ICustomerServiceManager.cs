using HotelApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Services.Service_Interfaces
{
    public interface ICustomerServiceManager
    {
        void CreateCustomer();
        void CustomerInfo();
        void UpdateCustomer();
        void DeleteCustomer();
    }
}