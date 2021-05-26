using System;
//using AdoptaPatitaMVC.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AdoptaPatitaMVC.Data;

[assembly: HostingStartup(typeof(AdoptaPatitaMVC.Areas.Identity.IdentityHostingStartup))]
namespace AdoptaPatitaMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AdoptaPatitaContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AdoptaPatitaMVCIdentityDbContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<AdoptaPatitaContext>();
            });
            
        }
    }
}