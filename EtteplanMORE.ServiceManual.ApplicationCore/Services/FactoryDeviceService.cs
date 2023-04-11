using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class FactoryDeviceDbContext : DbContext
    {
        public FactoryDeviceDbContext(DbContextOptions<FactoryDeviceDbContext> options)
            : base(options)
        {
        }

        public DbSet<FactoryDevice> FactoryDevices { get; set; }
    }


    public class FactoryDeviceService : IFactoryDeviceService
    {
        private readonly FactoryDeviceDbContext _dbContext;

        public FactoryDeviceService()
        {
        }

        public FactoryDeviceService(FactoryDeviceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FactoryDevice>> GetAll()
        {
            return await _dbContext.FactoryDevices.ToListAsync();
        }

        public async Task<FactoryDevice> Get(int id)
        {
            return await _dbContext.FactoryDevices.FindAsync(id);
        }

        public async Task<FactoryDevice> Create(FactoryDevice device)
        {
            _dbContext.FactoryDevices.Add(device);
            await _dbContext.SaveChangesAsync();
            return device;
        }

        public async Task<FactoryDevice> Update(FactoryDevice device)
        {
            _dbContext.Entry(device).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return device;
        }

        public async Task<bool> Delete(int id)
        {
            var device = await _dbContext.FactoryDevices.FindAsync(id);
            if (device == null)
                return false;

            _dbContext.FactoryDevices.Remove(device);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<FactoryDevice>> FilterDevicesByType(string type)
        {
            var query = _dbContext.FactoryDevices.AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(d => EF.Functions.Like(d.Type, $"%{type}%"));
            }

            return await query.ToListAsync();
        }

    }
}
