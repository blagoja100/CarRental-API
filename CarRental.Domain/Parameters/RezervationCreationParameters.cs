using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRental.Domain.Constants;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Rezervation creation parameters.
	/// </summary>
	public class RezervationCreationParameters : RezervationBaseParameters
	{
		/// <summary>
		/// Client account model.
		/// </summary>
		public ClientAccountCreationParams ClientAccount { get; set; }
	}
}
