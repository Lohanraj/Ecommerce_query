using FleetMaintenanceApi.Data;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Repositories.Implementations
{
    public class DriverRepository : IDriverRepository
    {
        private readonly FleetMaintenanceDbContext _context;

        public DriverRepository(
            FleetMaintenanceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Driver>> GetAllAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<Driver?> GetByIdAsync(int id)
        {
            return await _context.Drivers
                .FirstOrDefaultAsync(d => d.DriverId == id);
        }

        public async Task AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}