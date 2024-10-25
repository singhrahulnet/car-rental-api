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
		public IEnumerable<string> Get([FromQuery] VehicleAvailabilityQuery vehicleAvailabilityQuery)
		{
			return new string[] { "value1", "value2" };
		}

		[HttpPost]
		public void Post([FromBody] VehicleReservationRequest vehicleReservationRequest)
		{
		}
	}
}
