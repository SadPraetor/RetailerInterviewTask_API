using API.DataAccess;
using API.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace API.DevDataSeed {
    public static class DataSeed {
        public static IHost SeedDatabaseIfEmpty( this IHost host, int count = 1 ) {
            using ( var scope = host.Services.CreateScope() ) {
                using ( var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>() ) {

                    productsDbContext.Database.EnsureCreated();

                    if ( productsDbContext.Products.Any() )
                        return host;

                    var products = new ProductFaker().GetFakeProducts( count );
                    
                    productsDbContext.Products.AddRange( products );
                    productsDbContext.SaveChanges();
                }
            }
            return host;
        }

    }
}
