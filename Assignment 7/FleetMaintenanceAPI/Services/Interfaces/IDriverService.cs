using FleetMaintenanceApi.DTOs;

namespace FleetMaintenanceApi.Services.Interfaces
{
    public interface IDriverService
    {
        Task<List<DriverResponseDto>> GetAllDriversAsync();

        Task<DriverResponseDto?> GetDriverByIdAsync(int id);

        Task<DriverResponseDto> AddDriverAsync(
            DriverCreateDto dto);
    }
}