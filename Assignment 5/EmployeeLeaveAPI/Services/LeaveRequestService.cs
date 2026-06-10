using EmployeeLeaveAPI.Data;
using EmployeeLeaveAPI.DTOs;
using EmployeeLeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveRequestResponseDto> CreateAsync(
            LeaveRequestCreateDto dto)
        {
            if (dto.StartDate <= DateTime.Today)
                throw new Exception("StartDate must be a future date");

            if (dto.EndDate <= DateTime.Today)
                throw new Exception("EndDate must be a future date");

            if (dto.EndDate < dto.StartDate)
                throw new Exception("EndDate cannot be before StartDate");

            var leaveRequest = new LeaveRequest
            {
                EmployeeName = dto.EmployeeName,
                EmployeeEmail = dto.EmployeeEmail,
                MobileNumber = dto.MobileNumber,
                LeaveType = dto.LeaveType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                TotalDays = (dto.EndDate - dto.StartDate).Days + 1,
                Status = "Pending",
                CreatedOn = DateTime.Now
            };

            _context.LeaveRequests.Add(leaveRequest);

            await _context.SaveChangesAsync();

            return MapToResponse(leaveRequest);
        }

        public async Task<List<LeaveRequestResponseDto>> GetAllAsync()
        {
            var leaveRequests = await _context.LeaveRequests.ToListAsync();

            return leaveRequests
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<LeaveRequestResponseDto?> GetByIdAsync(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(x => x.LeaveRequestId == id);

            if (leaveRequest == null)
                return null;

            return MapToResponse(leaveRequest);
        }

        private LeaveRequestResponseDto MapToResponse(
            LeaveRequest leaveRequest)
        {
            return new LeaveRequestResponseDto
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                EmployeeName = leaveRequest.EmployeeName,
                EmployeeEmail = leaveRequest.EmployeeEmail,
                LeaveType = leaveRequest.LeaveType,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                Reason = leaveRequest.Reason,
                TotalDays = leaveRequest.TotalDays,
                Status = leaveRequest.Status,
                CreatedOn = leaveRequest.CreatedOn
            };
        }
    }
}