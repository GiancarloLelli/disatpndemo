using DISATPN.Web.Context;
using DISATPN.Web.Models.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DISATPN.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("DISATPN_Customer_Visits"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
                SeedCustomerFakeData(context);
            }

            app.UseCors("AllowAll");
            app.UseMvc();
        }

        private void SeedCustomerFakeData(DatabaseContext ctx)
        {
            var start = new DateTime(2019, 1, 20, 9, 30, 0);

            var devices = new string[]
            {
                "BED8218D-8AE9-4EDE-BBD9-32942F1C6348",
                "77D09A9E-48E7-4B53-9285-A214651903DB",
                "2DE6F734-D134-45DD-827B-F5E06A4F09E1",
                "EA6CA285-9B85-4BDD-A608-A09087D04BA7"
            };

            var storageVisit = new Visit
            {
                Timestamp = start,
                Paths = new List<Path>(),
                SessionId = Guid.Parse("078034A4-1F7E-4EB1-97D1-9E90AE649023"),
                UserId = Guid.Parse("A8778CEC-9319-E911-A82B-000D3A31270C")
            };

            for (int i = 0; i < 4; i++)
            {
                storageVisit.Paths.Add(new Path
                {
                    DeviceId = Guid.Parse(devices[i]),
                    Timestamp = start.AddMinutes(i * 10)
                });
            }

            ctx.Visits.Add(storageVisit);
            ctx.SaveChanges();
        }
    }
}
