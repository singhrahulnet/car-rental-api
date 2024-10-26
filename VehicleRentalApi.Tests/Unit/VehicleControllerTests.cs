using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using VehicleRentalApi.Contracts;
using VehicleRentalApi.Controllers;
using VehicleRentalApi.Domain;

namespace VehicleRentalApi.Tests.Unit
{
	public class VehicleControllerTests
	{
		private readonly Mock<IRentalService> _mockRentalService;
		private readonly Mock<IValidator> _mockValidator;
		private readonly Mock<ValidationResult> _mockValidatioResult;

		private readonly VehicleController sut;

		public VehicleControllerTests()
		{
			_mockRentalService = new Mock<IRentalService>();
			_mockValidator = new Mock<IValidator>();
            _mockValidatioResult = new Mock<ValidationResult>();
			sut = new VehicleController(_mockRentalService.Object, _mockValidator.Object);
		}

		[Fact]
		public async Task Given_Get_Returns_NoVehicles_Then_Response_Is_NotFound()
		{
			var vehicleAvailabilityQuery = new VehicleAvailabilityQuery();
			_mockRentalService.Setup(x => x.GetAvailableVehiclesAsync(vehicleAvailabilityQuery))
				.ReturnsAsync(new List<VehicleAvailabilityResponse>());

			var result = await sut.Get(vehicleAvailabilityQuery);

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public async Task Given_Get_Returns_FewVehicles_Then_Response_Is_Ok_With_Available_Vehicles()
		{
			var vehicleAvailabilityQuery = new VehicleAvailabilityQuery();
			var vehicles = new List<VehicleAvailabilityResponse>
			{
				new VehicleAvailabilityResponse(),
				new VehicleAvailabilityResponse()
			};
			_mockRentalService.Setup(x => x.GetAvailableVehiclesAsync(vehicleAvailabilityQuery))
				.ReturnsAsync(vehicles);

			var result = await sut.Get(vehicleAvailabilityQuery);

			var okResult = Assert.IsType<OkObjectResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<VehicleAvailabilityResponse>>(okResult.Value);
			Assert.Equal(vehicles, model);
		}

		[Fact]
		public async Task Given_Model_Is_Invalid_Then_POST_Response_Is_BadRequest()
		{
			var vehicleReservationRequest = new VehicleReservationRequest();

			_mockValidatioResult.Setup(x => x.IsValid).Returns(false);
			_mockValidator.Setup(x => x.Validate(vehicleReservationRequest))
				.Returns(_mockValidatioResult.Object);

			var result = await sut.Post(vehicleReservationRequest);

			Assert.IsType<BadRequestObjectResult>(result);
		}

        [Fact]
		public async Task Given_RentalService_Throws_RequestedVehicleNotAvailableException_Then_POST_Response_Is_NotFound()
		{
            var vehicleReservationRequest = new VehicleReservationRequest();
            _mockValidatioResult.Setup(x => x.IsValid).Returns(true);
            _mockValidator.Setup(x => x.Validate(vehicleReservationRequest))
                .Returns(_mockValidatioResult.Object);

            _mockRentalService.Setup(x => x.ReserveVehicleAsync(vehicleReservationRequest))
                .ThrowsAsync(new RequestedVehicleNotAvailableException("Vehicle not available"));   
            
            var result = await sut.Post(vehicleReservationRequest);
            
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
		public async Task Given_Valid_Request_And_Vehicle_Is_Available_Then_POST_Response_Is_Ok()
		{
            var vehicleReservationRequest = new VehicleReservationRequest();
            _mockValidatioResult.Setup(x => x.IsValid).Returns(true);
            _mockValidator.Setup(x => x.Validate(vehicleReservationRequest))
                .Returns(_mockValidatioResult.Object);

             _mockRentalService.Setup(x => x.ReserveVehicleAsync(vehicleReservationRequest))
                .Returns(Task.CompletedTask);   
            
            var result = await sut.Post(vehicleReservationRequest);
            
            Assert.IsType<OkObjectResult>(result);
        }
	}
}