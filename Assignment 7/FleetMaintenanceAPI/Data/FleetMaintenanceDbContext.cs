using FleetMaintenanceApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FleetMaintenanceApi.Data
{
    public class FleetMaintenanceDbContext : DbContext
    {
        public FleetMaintenanceDbContext(
            DbContextOptions<FleetMaintenanceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Driver> Drivers { get; set; }

        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceRecord>()
                .HasKey(m => m.MaintenanceId);

            modelBuilder.Entity<MaintenanceRecord>()
                .HasOne(m => m.Vehicle)
                .WithMany(v => v.MaintenanceRecords)
                .HasForeignKey(m => m.VehicleId);

            modelBuilder.Entity<MaintenanceRecord>()
                .HasOne(m => m.Driver)
                .WithMany(d => d.MaintenanceRecords)
                .HasForeignKey(m => m.DriverId);

            base.OnModelCreating(modelBuilder);
        }
    }
}