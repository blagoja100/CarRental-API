using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Client retrieval parameters.
	/// </summary>
	public class ClientAccountRetrievalParams
	{
		/// <summary>
		/// Client account identifier.
		/// </summary>
		public int ClientId { get; set; }
	}
}
