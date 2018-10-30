using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Client retrieval parameters.
	/// </summary>
	public class ClientAccountRetrievalParams
	{
		/// <summary>
		/// Create new instance of the class.
		/// </summary>
		public ClientAccountRetrievalParams()
		{
		}

		/// <summary>
		/// Client account identifier.
		/// </summary>
		[Required]
		public int ClientId { get; set; }
	}
}