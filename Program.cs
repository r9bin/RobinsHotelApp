using Autofac;
using HotelApp.AutoFacBuilder;
using HotelApp.Main_Interfaces;
using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace HotelApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApp>();
                app.Run();
            }
        }
    }
}
