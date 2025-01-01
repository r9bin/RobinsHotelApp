using Autofac;
using HotelApp.Data;
using HotelApp.Data.Data_Interfaces;
using HotelApp.Factory;
using HotelApp.Factory.DbContext_Interface;
using HotelApp.Main_Interfaces;
using HotelApp.Menus;
using HotelApp.Menus.Menus_Interfaces;
using HotelApp.Services;
using HotelApp.Services.Service_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.AutoFacBuilder
{
    internal static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<App>().As<IApp>();
            builder.RegisterType<DataInitializer>().As<IDataInitializer>();

            builder.RegisterType<MainMenu>().As<IMainMenu>();
            builder.RegisterType<BookingMenu>().As<IBookingMenu>();
            builder.RegisterType<CustomerMenu>().As<ICustomerMenu>();
            builder.RegisterType<RoomMenu>().As<IRoomMenu>();


            builder.RegisterType<BookingServiceManager>().As<IBookingServiceManager>();
            builder.RegisterType<CustomerServiceManager>().As<ICustomerServiceManager>();
            builder.RegisterType<RoomServiceManager>().As<IRoomServiceManager>();

            builder.RegisterType<DbContextFactory>().As<IdbContextFactoryHelper>();

            return builder.Build();
        }
    }
}
