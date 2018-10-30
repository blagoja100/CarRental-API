using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Client account creation parameters used in the API.
	/// </summary>
	public class ClientAccountModificationParams : ClientAccountBaseParams
	{
		/// <summary>
		/// Client account identifier.
		/// </summary>
		[Required]
		public int ClientId { get; set; }
	}
}