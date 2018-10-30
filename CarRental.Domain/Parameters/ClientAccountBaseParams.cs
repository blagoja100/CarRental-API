using System.ComponentModel.DataAnnotations;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Client account base parameters
	/// </summary>
	public class ClientAccountBaseParams
	{
		/// <summary>
		/// Client's full name.
		/// </summary>
		[Required]
		public string FullName { get; set; }

		/// <summary>
		/// Client's email.
		/// </summary>
		[Required]
		public string Email { get; set; }

		/// <summary>
		/// Client's phone number.
		/// </summary>
		[Required]
		public string Phone { get; set; }
	}
}