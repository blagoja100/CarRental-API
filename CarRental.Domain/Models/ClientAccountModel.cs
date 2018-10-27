using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
	/// <summary>
	/// Represents the client account model.
	/// </summary>
	public class ClientAccountModel
	{
		/// <summary>
		/// Client account identifier.
		/// </summary>
		public int ClientId { get; set; }
		/// <summary>
		/// Client's full name.
		/// </summary>
		public string FullName { get; set; }
		/// <summary>
		/// Client's email.
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// Client's phone number.
		/// </summary>
		public string Phone { get; set; }
	}
}
