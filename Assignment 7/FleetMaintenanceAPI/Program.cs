using FleetMaintenanceApi.Data;
using FleetMaintenanceApi.Repositories.Implementations;
using FleetMaintenanceApi.Repositories.Interfaces;
using FleetMaintenanceApi.Services.Implementations;
using FleetMaintenanceApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<
    FleetMaintenanceDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration
            .GetConnectionString("DefaultConnection"));
    });

builder.Services.AddScoped<
    IVehicleRepository,
    VehicleRepository>();

builder.Services.AddScoped<
    IDriverRepository,
    DriverRepository>();

builder.Services.AddScoped<
    IMaintenanceRepository,
    MaintenanceRepository>();

builder.Services.AddScoped<
    IVehicleService,
    VehicleService>();

builder.Services.AddScoped<
    IDriverService,
    DriverService>();

builder.Services.AddScoped<
    IMaintenanceService,
    MaintenanceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();