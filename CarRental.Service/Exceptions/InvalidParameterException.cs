using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Service.Exceptions
{
	/// <summary>
	/// Handles the case when the parameters provided to the service method is invlaid.
	/// </summary>
	public class InvalidParameterException : Exception
	{
		/// <summary>
		/// Instantiates a new InvlaidParameter exception.
		/// </summary>
		/// <param name="message"></param>
		public InvalidParameterException(string message) : base(message)
		{			
		}

		public override string Message => $"Invalid parameter: {base.Message}";

		/// <summary>
		/// Instantiates a new InvlaidParameter exception.
		/// </summary>
		/// <param name="message"></param>
		public InvalidParameterException(string message, Exception e) : base(message, e) { }
	}
}
