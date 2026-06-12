using FleetMaintenanceApi.DTOs;

namespace FleetMaintenanceApi.Services.Interfaces
{
    public interface IMaintenanceService
    {
        Task<MaintenanceResponseDto> AddMaintenanceAsync(
            MaintenanceCreateDto dto);

        Task<PagedResponseDto<MaintenanceResponseDto>>
            GetMaintenanceRecordsAsync(
            MaintenanceFilterRequestDto filter);
    }
}