using FleetMaintenanceApi.DTOs;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using FleetMaintenanceApi.Services.Interfaces;

namespace FleetMaintenanceApi.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;

        public VehicleService(
            IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<VehicleResponseDto>>
            GetAllVehiclesAsync()
        {
            var vehicles =
                await _repository.GetAllAsync();

            return vehicles.Select(v =>
                new VehicleResponseDto
                {
                    VehicleId = v.VehicleId,
                    VehicleNumber = v.VehicleNumber,
                    VehicleType = v.VehicleType,
                    Brand = v.Brand,
                    Model = v.Model,
                    PurchaseYear = v.PurchaseYear,
                    IsActive = v.IsActive
                }).ToList();
        }

        public async Task<VehicleResponseDto?>
            GetVehicleByIdAsync(int id)
        {
            var vehicle =
                await _repository.GetByIdAsync(id);

            if (vehicle == null)
                return null;

            return new VehicleResponseDto
            {
                VehicleId = vehicle.VehicleId,
                VehicleNumber = vehicle.VehicleNumber,
                VehicleType = vehicle.VehicleType,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                PurchaseYear = vehicle.PurchaseYear,
                IsActive = vehicle.IsActive
            };
        }

        public async Task<VehicleResponseDto>
            AddVehicleAsync(VehicleCreateDto dto)
        {
            Vehicle vehicle = new()
            {
                VehicleNumber = dto.VehicleNumber,
                VehicleType = dto.VehicleType,
                Brand = dto.Brand,
                Model = dto.Model,
                PurchaseYear = dto.PurchaseYear,
                IsActive = dto.IsActive
            };

            await _repository.AddAsync(vehicle);
            await _repository.SaveChangesAsync();

            return new VehicleResponseDto
            {
                VehicleId = vehicle.VehicleId,
                VehicleNumber = vehicle.VehicleNumber,
                VehicleType = vehicle.VehicleType,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                PurchaseYear = vehicle.PurchaseYear,
                IsActive = vehicle.IsActive
            };
        }
    }
}