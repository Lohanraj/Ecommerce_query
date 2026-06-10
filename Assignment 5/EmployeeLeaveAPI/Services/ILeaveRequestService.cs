using EmployeeLeaveAPI.DTOs;

namespace EmployeeLeaveAPI.Services
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequestResponseDto> CreateAsync(
            LeaveRequestCreateDto dto);

        Task<List<LeaveRequestResponseDto>> GetAllAsync();

        Task<LeaveRequestResponseDto> GetByIdAsync(int id);
    }
}