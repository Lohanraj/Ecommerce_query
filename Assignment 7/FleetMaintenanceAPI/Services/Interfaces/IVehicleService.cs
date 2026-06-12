using FleetMaintenanceApi.DTOs;

namespace FleetMaintenanceApi.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<VehicleResponseDto>> GetAllVehiclesAsync();

        Task<VehicleResponseDto?> GetVehicleByIdAsync(int id);

        Task<VehicleResponseDto> AddVehicleAsync(
            VehicleCreateDto dto);
    }
}