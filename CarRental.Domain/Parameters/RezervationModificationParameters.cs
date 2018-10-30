using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Rezervation modification parameters.
	/// </summary>
	public class RezervationModificationParameters : RezervationBaseParameters
	{
		/// <summary>
		/// Rezervation identifier.
		/// </summary>
		[Required]
		public int RezervationId { get; set; }

		/// <summary>
		/// Rate under which the cancelation fee is
		/// </summary>
		public decimal? CancelationFeeRate { get; set; }

		/// <summary>
		/// Client account model.
		/// </summary>
		public ClientAccountModificationParams ClientAccount { get; set; }
	}
}