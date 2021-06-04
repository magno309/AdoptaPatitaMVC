using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdoptaPatitaMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace AdoptaPatitaMVC
{
    public class Program
    {
        /*public static void Main(string[] args)
            => CreateHostBuilder(args).Build().Run();  */
        public static void Main(string[] args){
            var host = CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider;

                try{
                    var context = services.GetRequiredService<AdoptaPatitaContext>();
                    context.Database.Migrate();

                    // requires using Microsoft.Extensions.Configuration;
                    var config = host.Services.GetRequiredService<IConfiguration>();

                    SeedData.Initialize(services).Wait();

                } catch(Exception ex){
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex.Message, "An error occurred seeding the DB.");
                }
            }
            host.Run();
        }



        // EF Core uses this method at design time to access the DbContext
        public static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => 
                {
                    webBuilder.ConfigureKestrel(serverOptions => {
                        serverOptions.ConfigureHttpsDefaults(co => {
                            co.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        });
                    }).UseStartup<Startup>();
                }                
            );
    }
}
