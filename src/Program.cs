using API.DevDataSeed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailerInterviewAPITask {
    public class Program {
        public static void Main( string[] args ) {
            var hostBuilder = CreateHostBuilder( args ).Build();

            if ( Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" ) == Environments.Development ) {
                hostBuilder
                    .MigrateDatabase()
                    .SeedDatabaseIfEmpty();
            };

            hostBuilder.Run();
        }

        public static IHostBuilder CreateHostBuilder( string[] args ) =>
            Host.CreateDefaultBuilder( args )
                .ConfigureWebHostDefaults( webBuilder => {
                    webBuilder.UseStartup<Startup>();
                } );
    }
}
