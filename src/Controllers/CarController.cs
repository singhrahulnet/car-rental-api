using CarRentalApi.Contracts;
using CarRentalApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarController : ControllerBase
	{
		private readonly IRentalService _rentalService;
		private readonly IValidator _validator;

		public CarController(IRentalService rentalService, IValidator validator)
        {
			_rentalService = rentalService;
			_validator = validator;
        }

        [HttpGet]
		public IEnumerable<string> Get([FromQuery] CarAvailabilityQuery carAvailabilityQuery)
		{
			return new string[] { "value1", "value2" };
		}

		[HttpPost]
		public void Post([FromBody] CarReservation carReservation)
		{
		}
	}
}
