using API.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace API.DevDataSeed {
    public static class MigrationManager {
        public static IHost MigrateDatabase( this IHost host ) {
            using ( var scope = host.Services.CreateScope() ) {
                using ( var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>() ) {
                    try {
                        productsDbContext.Database.Migrate();
                    }
                    catch ( Exception ex ) {
                        //Log errors or do anything you think it's needed
                        Console.WriteLine( "Issue when loading dev data: " + ex.Message );
                        throw;
                    }
                }
            }
            return host;
        }
    }
}
