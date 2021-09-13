using API.DataAccess;
using API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace API.DevDataSeed {
    public static class DataSeed {
        public static IHost SeedDatabaseIfEmpty( this IHost host ) {
            using ( var scope = host.Services.CreateScope() ) {
                using ( var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>() ) {

                    productsDbContext.Database.EnsureCreated();

                    if ( productsDbContext.Products.Any() )
                        return host;

                    var products = new List<Product>()
                        {
                            new Product () { Name = "bike", ImgUri=@"domain/1.png",Price= 15M,Description="test"},
                            new Product () { Name = "computer", ImgUri=@"domain/2.png",Price= 20M,Description="test"}
                        };
                    
                    productsDbContext.Products.AddRange( products );
                    productsDbContext.SaveChanges();
                }
            }
            return host;
        }

    }
}
