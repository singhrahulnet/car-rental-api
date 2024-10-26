using VehicleRentalApi.Contracts;
using VehicleRentalApi.DataAccess;
using VehicleRentalApi.DataModel;
using VehicleRentalApi.Domain;

namespace VehicleRentalApi.Tests.Unit
{
	public class RentalServiceTests
	{
		private readonly Mock<IVehicleRepository> _mockVehicleRepository;
		private readonly RentalService sut;

		public RentalServiceTests()
		{
			_mockVehicleRepository = new Mock<IVehicleRepository>();
			sut = new RentalService(_mockVehicleRepository.Object);
		}

		[Fact]
		public async void Given_There_Are_No_Vehicles_At_All_Then_Empty_Reponse_Is_Returned()
		{
			var result = await sut.GetAvailableVehiclesAsync(It.IsAny<VehicleAvailabilityQuery>());
			Assert.Empty(result);
		}

		[Fact]
		public async void Given_There_Are_No_Matching_Vehicle_Types_Then_Empty_Reponse_Is_Returned()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN }
			});

			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SUV] });
			Assert.Empty(result);
		}

		[Fact]
		public async void Given_There_Are_Matching_Vehicle_Types_In_Inventory_Then_Matching_Reponse_Is_Returned()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV }
			});

			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SUV] });
			Assert.Single(result);
			Assert.Equal(VehicleType.SUV, result.First().VehicleType);
			Assert.Equal(1, result.First().NumbersAvailable);
		}

		[Fact]
		public async void Given_There_Are_Vehicles_In_Inventory_And_VehicleType_Is_Not_Provided_Then_All_Vehicles_Are_Returned()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV }
			});

			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery());
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async void Given_Multiple_VehicleType_Provided_Then_All__Matching_Vehicles_Are_Returned_With_Correct_Count()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV },
				new Vehicle { VehicleType = VehicleType.SUV }
			});

			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SUV, VehicleType.SEDAN] });
			Assert.Equal(2, result.Count());
			Assert.Equal(1, result.First(v => v.VehicleType == VehicleType.SEDAN).NumbersAvailable);
			Assert.Equal(2, result.First(v => v.VehicleType == VehicleType.SUV).NumbersAvailable);
		}

		[Fact]
		public async void Given_StartDate_Falls_Within_A_Reservation_Date_Then_Result_Subtracts_Reserved_Vehicles_From_Available_Vehicles()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV }
			});

			_mockVehicleRepository.Setup(repo => repo.GetVehiclesReservationsAsync()).ReturnsAsync(new List<VehicleReservation>
			{
				//AddDays(3) to AddDays(7)
				new VehicleReservation { VehicleType = VehicleType.SEDAN, PickupOn = DateTime.Now.AddDays(3), ReturnOn = DateTime.Now.AddDays(7) }
			});

			//AddDays(5) to AddDays(9)
			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SEDAN], PickupOn = DateTime.Now.AddDays(5), ReturnOn = DateTime.Now.AddDays(9) });
			Assert.Empty(result);
		}

		[Fact]
		public async void Given_EndDate_Falls_Within_A_Reservation_Date_Then_Result_Subtracts_Reserved_Vehicles_From_Available_Vehicles()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV }
			});

			_mockVehicleRepository.Setup(repo => repo.GetVehiclesReservationsAsync()).ReturnsAsync(new List<VehicleReservation>
			{
				//AddDays(3) to AddDays(7)
				new VehicleReservation { VehicleType = VehicleType.SEDAN, PickupOn = DateTime.Now.AddDays(3), ReturnOn = DateTime.Now.AddDays(7) }
			});

			//AddDays(1) to AddDays(5)
			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SEDAN], PickupOn = DateTime.Now.AddDays(1), ReturnOn = DateTime.Now.AddDays(5) });
			Assert.Empty(result);
		}

		[Fact]
		public async void Given_Dates_Fully_Span_Reservation_Date_Then_Result_Subtracts_Reserved_Vehicles_From_Available_Vehicles()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV }
			});

			_mockVehicleRepository.Setup(repo => repo.GetVehiclesReservationsAsync()).ReturnsAsync(new List<VehicleReservation>
			{
				//AddDays(3) to AddDays(7)
				new VehicleReservation { VehicleType = VehicleType.SEDAN, PickupOn = DateTime.Now.AddDays(3), ReturnOn = DateTime.Now.AddDays(7) }
			});

			//AddDays(1) to AddDays(9)
			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SEDAN], PickupOn = DateTime.Now.AddDays(1), ReturnOn = DateTime.Now.AddDays(9) });
			Assert.Empty(result);
		}

		[Fact]
		public async void Given_Both_Dates_Falls_Within_Reservations_Then_Result_Subtracts_Reserved_Vehicles_From_Available_Vehicles()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV },
				new Vehicle { VehicleType = VehicleType.SUV },
				new Vehicle { VehicleType = VehicleType.VAN }
			});

			_mockVehicleRepository.Setup(repo => repo.GetVehiclesReservationsAsync()).ReturnsAsync(new List<VehicleReservation>
			{
				//AddDays(3) to AddDays(7)
				new VehicleReservation { VehicleType = VehicleType.SEDAN, PickupOn = DateTime.Now.AddDays(3), ReturnOn = DateTime.Now.AddDays(7) },
				//AddDays(4) to AddDays(8)
				new VehicleReservation { VehicleType = VehicleType.SUV, PickupOn = DateTime.Now.AddDays(4), ReturnOn = DateTime.Now.AddDays(8) }
			});

			//AddDays(5) to AddDays(6)
			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SEDAN, VehicleType.SUV], PickupOn = DateTime.Now.AddDays(5), ReturnOn = DateTime.Now.AddDays(6) });
			Assert.Equal(2, result.Count());
			Assert.Equal(1, result.First(v => v.VehicleType == VehicleType.SEDAN).NumbersAvailable);
			Assert.Equal(1, result.First(v => v.VehicleType == VehicleType.SUV).NumbersAvailable);
		}

		[Fact]
		public async void Given_Both_Dates_Fall_Beyond_Reservations_Then_All__Matching_Vehicles_Are_Returned_With_Correct_Count()
		{
			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SEDAN },
				new Vehicle { VehicleType = VehicleType.SUV },
				new Vehicle { VehicleType = VehicleType.SUV },
				new Vehicle { VehicleType = VehicleType.VAN }
			});

			_mockVehicleRepository.Setup(repo => repo.GetVehiclesReservationsAsync()).ReturnsAsync(new List<VehicleReservation>
			{
				//AddDays(3) to AddDays(7)
				new VehicleReservation { VehicleType = VehicleType.SEDAN, PickupOn = DateTime.Now.AddDays(3), ReturnOn = DateTime.Now.AddDays(7) },
				//AddDays(4) to AddDays(8)
				new VehicleReservation { VehicleType = VehicleType.SUV, PickupOn = DateTime.Now.AddDays(4), ReturnOn = DateTime.Now.AddDays(8) }
			});

			//AddDays(9) to AddDays(11)
			var result = await sut.GetAvailableVehiclesAsync(new VehicleAvailabilityQuery { VehicleTypes = [VehicleType.SEDAN, VehicleType.SUV], PickupOn = DateTime.Now.AddDays(9), ReturnOn = DateTime.Now.AddDays(11) });
			Assert.Equal(2, result.Count());
			Assert.Equal(2, result.First(v => v.VehicleType == VehicleType.SEDAN).NumbersAvailable);
			Assert.Equal(2, result.First(v => v.VehicleType == VehicleType.SUV).NumbersAvailable);
		}

		[Fact]
		public async void Given_Vehicle_NonAvailable_Then_ReserveVehicleAsync_Throws_RequestedVehicleNotAvailableException()
		{
			_mockVehicleRepository.Setup(repo => repo.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
									.Returns<Func<Task>>(operation => operation());

			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>());						

			await Assert.ThrowsAsync<RequestedVehicleNotAvailableException>(() => sut.ReserveVehicleAsync(new VehicleReservationRequest()));
		}

		[Fact]
		public async void Given_Vehicle_Available_For_Reservation_Then_ReserveVehicleAsync_Calls_Repo_To_Add_Reservation()
		{
			_mockVehicleRepository.Setup(repo => repo.ExecuteInTransactionAsync(It.IsAny<Func<Task>>()))
									.Returns<Func<Task>>(operation => operation());

			_mockVehicleRepository.Setup(repo => repo.GetAvailableVehiclesAsync()).ReturnsAsync(new List<Vehicle>
			{
				new Vehicle { VehicleType = VehicleType.SEDAN }
			});

			_mockVehicleRepository.Setup(repo => repo.GetVehiclesReservationsAsync()).ReturnsAsync(new List<VehicleReservation>());

			await sut.ReserveVehicleAsync(new VehicleReservationRequest { VehicleType = VehicleType.SEDAN });

			_mockVehicleRepository.Verify(repo => repo.AddVehicleReservationAsync(It.IsAny<VehicleReservation>()), Times.Once);
		}
	}
}
