using CarRentalApi.DataModel;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.DataAccess
{
	internal class VehicleRepository : DbContext, IVehicleRepository
	{
		private DbSet<Vehicle> _vehicles;
		private DbSet<VehicleReservation> _vehiclesReservations;

		public VehicleRepository() { }

		public VehicleRepository(DbContextOptions<VehicleRepository> options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseInMemoryDatabase("VehiclesDB");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Vehicle>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.VehicleType).IsRequired();
			});

			modelBuilder.Entity<VehicleReservation>(entity =>
			{
				entity.HasKey(x => x.Id);
			});
		}

		public async Task AddVehicleToInventoryAsync(Vehicle vehicle)
		{
			_vehicles.Add(vehicle);
			await SaveChangesAsync();
		}

		public async Task AddVehicleReservationAsync(VehicleReservation reservation)
		{
			_vehiclesReservations.Add(reservation);
			await SaveChangesAsync();
		}

		public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
		{
			return await _vehicles.ToListAsync();
		}
	}
}
