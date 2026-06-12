using FleetMaintenanceApi.Models;

namespace FleetMaintenanceApi.Repositories.Interfaces
{
    public interface IMaintenanceRepository
    {
        Task AddAsync(MaintenanceRecord maintenance);

        IQueryable<MaintenanceRecord> GetMaintenanceQueryable();

        Task SaveChangesAsync();
    }
}