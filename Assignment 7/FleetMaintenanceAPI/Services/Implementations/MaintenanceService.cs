using FleetMaintenanceApi.DTOs;
using FleetMaintenanceApi.Models;
using FleetMaintenanceApi.Repositories.Interfaces;
using FleetMaintenanceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleetMaintenanceApi.Services.Implementations
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _repository;

        public MaintenanceService(
            IMaintenanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<MaintenanceResponseDto>
            AddMaintenanceAsync(
            MaintenanceCreateDto dto)
        {
            MaintenanceRecord record = new()
            {
                VehicleId = dto.VehicleId,
                DriverId = dto.DriverId,
                ServiceDate = dto.ServiceDate,
                ServiceType = dto.ServiceType,
                ServiceCost = dto.ServiceCost,
                ServiceStatus = dto.ServiceStatus,
                Remarks = dto.Remarks,
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(record);
            await _repository.SaveChangesAsync();

            return new MaintenanceResponseDto
            {
                MaintenanceId = record.MaintenanceId,
                VehicleId = record.VehicleId,
                DriverId = record.DriverId,
                ServiceDate = record.ServiceDate,
                ServiceType = record.ServiceType,
                ServiceCost = record.ServiceCost,
                ServiceStatus = record.ServiceStatus,
                Remarks = record.Remarks,
                CreatedDate = record.CreatedDate
            };
        }

        public async Task<
            PagedResponseDto<MaintenanceResponseDto>>
            GetMaintenanceRecordsAsync(
            MaintenanceFilterRequestDto filter)
        {
            var query =
                _repository.GetMaintenanceQueryable();

            // Filtering

            if (filter.VehicleId.HasValue)
            {
                query = query.Where(x =>
                    x.VehicleId ==
                    filter.VehicleId.Value);
            }

            if (filter.DriverId.HasValue)
            {
                query = query.Where(x =>
                    x.DriverId ==
                    filter.DriverId.Value);
            }

            if (!string.IsNullOrEmpty(
                filter.ServiceStatus))
            {
                query = query.Where(x =>
                    x.ServiceStatus ==
                    filter.ServiceStatus);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(x =>
                    x.ServiceDate >=
                    filter.FromDate.Value);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(x =>
                    x.ServiceDate <=
                    filter.ToDate.Value);
            }

            // Sorting

            if ((filter.SortDirection ?? "")
                .ToLower() == "desc")
            {
                query = query
                    .OrderByDescending(x =>
                        x.ServiceDate);
            }
            else
            {
                query = query
                    .OrderBy(x =>
                        x.ServiceDate);
            }

            int totalRecords =
                await query.CountAsync();

            var records =
                await query
                .Skip(
                    (filter.PageNumber - 1)
                    * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var result =
                records.Select(x =>
                new MaintenanceResponseDto
                {
                    MaintenanceId =
                        x.MaintenanceId,

                    VehicleId =
                        x.VehicleId,

                    VehicleNumber =
                        x.Vehicle!.VehicleNumber,

                    VehicleType =
                        x.Vehicle.VehicleType,

                    DriverId =
                        x.DriverId,

                    DriverName =
                        x.Driver!.DriverName,

                    ServiceDate =
                        x.ServiceDate,

                    ServiceType =
                        x.ServiceType,

                    ServiceCost =
                        x.ServiceCost,

                    ServiceStatus =
                        x.ServiceStatus,

                    Remarks =
                        x.Remarks,

                    CreatedDate =
                        x.CreatedDate
                }).ToList();

            return new
                PagedResponseDto
                <MaintenanceResponseDto>
            {
                StatusCode = 200,
                Message = "Success",
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalRecords = totalRecords,
                TotalPages =
                    (int)Math.Ceiling(
                    totalRecords /
                    (double)filter.PageSize),

                HasPreviousPage =
                    filter.PageNumber > 1,

                HasNextPage =
                    filter.PageNumber *
                    filter.PageSize
                    < totalRecords,

                Data = result
            };
        }
    }
}