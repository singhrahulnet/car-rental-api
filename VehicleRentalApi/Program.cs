using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VehicleRentalApi.DataAccess;
using VehicleRentalApi.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddSingleton<IValidator, Validator>();
builder.Services.AddDbContext<VehicleRepository>(options =>
	options.UseInMemoryDatabase("VehiclesDB")
	//Ignoring transactions for In-memory
	.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning)));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
