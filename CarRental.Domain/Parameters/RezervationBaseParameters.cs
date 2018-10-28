using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRental.Domain.Constants;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Rezervation base parameters.
	/// </summary>
	public class RezervationBaseParameters
	{
		/// <summary>
		/// Car type, Standard, Family, Prestiege.
		/// </summary>
		public CarTypeEnum CarType { get; set; }

		/// <summary>
		/// Car plate number.
		/// </summary>
		public string CarPlateNumber { get; set; }

		/// <summary>
		/// Date of car pick up.
		/// </summary>
		public DateTime PickUpDate { get; set; }

		/// <summary>
		/// Date of car return.
		/// </summary>
		public DateTime ReturnDate { get; set; }		
	}
}
