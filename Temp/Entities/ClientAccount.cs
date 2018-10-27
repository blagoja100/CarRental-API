using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
