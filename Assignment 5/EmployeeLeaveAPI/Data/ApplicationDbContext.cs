using Microsoft.EntityFrameworkCore;
using EmployeeLeaveAPI.Models;

namespace EmployeeLeaveAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }
    }
}