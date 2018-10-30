using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Data.Entities
{
	/// <summary>
	/// Contains the clients information.
	/// </summary>
	public class ClientAccount
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ClientId { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }
	}
}