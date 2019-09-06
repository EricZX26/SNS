﻿using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope()){
                
                var service= scope.ServiceProvider;
                try{
                    var dbContext=service.GetRequiredService<DataContext>();
                    dbContext.Database.Migrate();
                    Seed.SeedData(dbContext);
                }catch(Exception ex){
                    var logger=service.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex," An error occured during migration");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
