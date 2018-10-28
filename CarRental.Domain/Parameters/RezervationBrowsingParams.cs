using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Rezervation retrieval parameters.
	/// </summary>
	public class RezervationBrowsingParams : BaseCollectionRetrievalParams
	{
		/// <summary>
		/// Retrieve rezervations for Client with name.
		/// </summary>
		public string ClientFullName { get; set; }
		/// <summary>
		/// Retrieve rezervations for Client with email.
		/// </summary>
		public string ClientEmail { get; set; }
		/// <summary>
		/// Retrieve rezervations for Client with phone number.
		/// </summary>
		public string ClientPhone { get; set; }

		/// <summary>
		/// Retrieve rezervations from PickUp date.
		/// </summary>
		public DateTime? PickUpDateFrom { get; set; }

		/// <summary>
		/// Retrieve rezervations to PickUp date.
		/// </summary>
		public DateTime? PickUpDateTo { get; set; }

		/// <summary>
		/// Retrieve rezervations where the car is picked up
		/// </summary>
		public bool IsPickedUp { get; set; }

		/// <summary>
		/// Retrieve cancelled rezervations.
		/// </summary>
		public bool IsCancelled { get; set; }

		/// <summary>
		/// Retrieve completed rezervations.
		/// </summary>
		public bool IsReturned { get; set; }

		/// <summary>
		/// Retrieve booked rezervations.
		/// </summary>
		public bool IsBooked { get; set; }
	}
}
