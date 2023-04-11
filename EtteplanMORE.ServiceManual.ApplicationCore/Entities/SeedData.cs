using EtteplanMORE.ServiceManual.ApplicationCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FactoryDeviceDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<FactoryDeviceDbContext>>()))
            {
                // Look for any movies.
                if (context.FactoryDevices.Any())
                {
                    return;   // DB has been seeded
                }
                var file = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName, "seeddata.csv");

                if (File.Exists(file))
                {
                    // Read a text file line by line.
                    string[] lines = File.ReadAllLines(file);
                    foreach (string line in lines.Skip(1))
                    {
                        var data = line.Split(new[] { ',' });
                        var device = new FactoryDevice() { Name = data[0], Year = int.Parse(data[1]), Type = data[2] };
                        context.FactoryDevices.Add(device);
                    }
                }
             
                context.SaveChanges();
            }
        }
    }
}
