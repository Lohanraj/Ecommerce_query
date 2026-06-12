using FleetMaintenanceApi.Models;

namespace FleetMaintenanceApi.Repositories.Interfaces
{
    public interface IDriverRepository
    {
        Task<List<Driver>> GetAllAsync();

        Task<Driver?> GetByIdAsync(int id);

        Task AddAsync(Driver driver);

        Task SaveChangesAsync();
    }
}