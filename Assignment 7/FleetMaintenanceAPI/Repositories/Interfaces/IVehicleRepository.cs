using FleetMaintenanceApi.Models;

namespace FleetMaintenanceApi.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();

        Task<Vehicle?> GetByIdAsync(int id);

        Task AddAsync(Vehicle vehicle);

        Task SaveChangesAsync();
    }
}