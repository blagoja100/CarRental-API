using CarRental.Domain.Models;
using CarRental.Domain.Parameters;
using CarRental.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarRental.Api.Controllers
{
	/// <summary>
	/// Controller responsible for the creation of new car reservations, rezervation modification, cancelling etc.
	/// </summary>
	public class RezervationController : ApiController
    {
		private readonly IRezervationService rezervationService;

		/// <summary>
		/// Creates a new instance of the rezervation controller.
		/// </summary>
		/// <param name="rezervationService"></param>
		public RezervationController(IRezervationService rezervationService)
		{
			this.rezervationService = rezervationService;
		}

		/// <summary>
		/// Get the client account information.
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		[HttpPost]
		public RezervationModel BookRezervation(RezervationCreationParameters parameters)
		{
			return this.rezervationService.CreateBooking(parameters);
		}
		
		[HttpPost]
		public bool PickUpCar(RezervationRetrievalParams parameters)
		{
			return this.rezervationService.PickUpCar(parameters.RezervationId);
		}

		[HttpPost]
		public bool ReturnCar(RezervationRetrievalParams parameters)
		{
			return this.rezervationService.ReturnCar(parameters.RezervationId);
		}

		[HttpPost]		
		public ICollection<RezervationModel> GetAllRezervations(RezervationBrowsingParams parameters)
		{
			return this.rezervationService.FindRezervations(parameters);
		}
	}
}
