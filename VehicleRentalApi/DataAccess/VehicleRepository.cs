using VehicleRentalApi.DataModel;
using Microsoft.EntityFrameworkCore;

namespace VehicleRentalApi.DataAccess
{
	internal class VehicleRepository : DbContext, IVehicleRepository
	{
		private DbSet<Vehicle> _vehicles;
		private DbSet<VehicleReservation> _vehiclesReservations;

		public VehicleRepository() { }

		public VehicleRepository(DbContextOptions<VehicleRepository> options) : base(options)
		{
			_vehicles = Set<Vehicle>();
			_vehiclesReservations = Set<VehicleReservation>();
			Database.EnsureCreated();
		}

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

			modelBuilder.Entity<Vehicle>().HasData(SeedData.Generate());
		}

		public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
		{
			return await _vehicles.ToListAsync();
		}

		public async Task<IEnumerable<VehicleReservation>> GetVehiclesReservationsAsync()
		{
			return await _vehiclesReservations.ToListAsync();
		}

		public async Task AddVehicleReservationAsync(VehicleReservation reservation)
		{
			_vehiclesReservations.Add(reservation);
			await SaveChangesAsync();
		}

		public async Task ExecuteInTransactionAsync(Func<Task> operation)
		{
			using var transaction = await Database.BeginTransactionAsync();
			try
			{
				await operation();
				await transaction.CommitAsync();
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}
	}
}
