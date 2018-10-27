using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain
{
	/// <summary>
	/// Contains types user in the business logic.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Car types enum.
		/// </summary>
		public enum CarTypeEnum
		{
			/// <summary>
			/// Standard car type, like sedan or hatchback
			/// </summary>
			Standard = 1,
			/// <summary>
			/// Famility type, like MPV, SUV
			/// </summary>
			Family = 2,
			/// <summary>
			/// Prestiege type, like Sports car or high class SUV.
			/// </summary>
			Prestiege = 3
		}
	}
}
