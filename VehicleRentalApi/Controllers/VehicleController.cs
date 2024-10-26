using VehicleRentalApi.Contracts;
using VehicleRentalApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace VehicleRentalApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VehicleController : ControllerBase
	{
		private readonly IRentalService _rentalService;
		private readonly IValidator _validator;

		public VehicleController(IRentalService rentalService, IValidator validator)
		{
			_rentalService = rentalService;
			_validator = validator;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VehicleAvailabilityResponse>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromQuery] VehicleAvailabilityQuery vehicleAvailabilityQuery)
		{
			var result = await _rentalService.GetVehiclesAsync(vehicleAvailabilityQuery);

			if (result.Any() == false) { return NotFound(); }

			return Ok(result);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Post([FromBody] VehicleReservationRequest vehicleReservationRequest)
		{
			var validationResult = _validator.Validate(vehicleReservationRequest);

			if (validationResult.IsValid == false) { return BadRequest(validationResult.Errors); }

			try
			{
				await _rentalService.ReserveVehicleAsync(vehicleReservationRequest);
			}
			catch (RequestedVehicleNotAvailableException ex)
			{
				return NotFound(ex.Message);
			}

			return Ok("Vehicle reserved for requested dates");
		}
	}
}
