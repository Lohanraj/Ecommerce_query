using FleetMaintenanceApi.Data;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Repositories.Implementations
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly FleetMaintenanceDbContext _context;

        public MaintenanceRepository(
            FleetMaintenanceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            MaintenanceRecord maintenance)
        {
            await _context.MaintenanceRecords
                .AddAsync(maintenance);
        }

        public IQueryable<MaintenanceRecord>
            GetMaintenanceQueryable()
        {
            return _context.MaintenanceRecords
                .Include(m => m.Vehicle)
                .Include(m => m.Driver)
                .AsQueryable();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}