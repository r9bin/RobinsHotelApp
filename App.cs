using HotelApp.Data;
using HotelApp.Data.Data_Interfaces;
using HotelApp.Factory.DbContext_Interface;
using HotelApp.Main_Interfaces;
using HotelApp.Menus.Menus_Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp
{
    public class App(IMainMenu mainMenu, IdbContextFactoryHelper contextFactory, IDataInitializer dataInitializer) : IApp
    {

        public void Run()
        {

            var options = contextFactory.CreateDbContext();

            using (var dbContext = new ApplicationDbContext(options))
            {
                dataInitializer.MigrateAndSeed(dbContext);
            }



            mainMenu.MainMenuNavigation();
        }
    }
}
