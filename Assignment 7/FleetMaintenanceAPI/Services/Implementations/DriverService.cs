using FleetMaintenanceApi.DTOs;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using FleetMaintenanceApi.Services.Interfaces;

namespace FleetMaintenanceApi.Services.Implementations
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _repository;

        public DriverService(
            IDriverRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DriverResponseDto>>
            GetAllDriversAsync()
        {
            var drivers =
                await _repository.GetAllAsync();

            return drivers.Select(d =>
                new DriverResponseDto
                {
                    DriverId = d.DriverId,
                    DriverName = d.DriverName,
                    LicenseNumber = d.LicenseNumber,
                    PhoneNumber = d.PhoneNumber,
                    City = d.City,
                    IsAvailable = d.IsAvailable
                }).ToList();
        }

        public async Task<DriverResponseDto?>
            GetDriverByIdAsync(int id)
        {
            var driver =
                await _repository.GetByIdAsync(id);

            if (driver == null)
                return null;

            return new DriverResponseDto
            {
                DriverId = driver.DriverId,
                DriverName = driver.DriverName,
                LicenseNumber = driver.LicenseNumber,
                PhoneNumber = driver.PhoneNumber,
                City = driver.City,
                IsAvailable = driver.IsAvailable
            };
        }

        public async Task<DriverResponseDto>
            AddDriverAsync(DriverCreateDto dto)
        {
            Driver driver = new()
            {
                DriverName = dto.DriverName,
                LicenseNumber = dto.LicenseNumber,
                PhoneNumber = dto.PhoneNumber,
                City = dto.City,
                IsAvailable = dto.IsAvailable
            };

            await _repository.AddAsync(driver);
            await _repository.SaveChangesAsync();

            return new DriverResponseDto
            {
                DriverId = driver.DriverId,
                DriverName = driver.DriverName,
                LicenseNumber = driver.LicenseNumber,
                PhoneNumber = driver.PhoneNumber,
                City = driver.City,
                IsAvailable = driver.IsAvailable
            };
        }
    }
}