using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Service.Exceptions
{
	public class NotFoundException : Exception
	{
		/// <summary>
		/// Instantiates a new NotFoundException exception. Hadles the cases when the searched item is not found in the database.
		/// </summary>
		/// <param name="message"></param>
		public NotFoundException(string message) : base(message) { }

		/// <summary>
		/// Instantiates a new NotFoundException exception. Hadles the cases when the searched item is not found in the database.
		/// </summary>
		/// <param name="message"></param>
		public NotFoundException(string message, Exception e) : base(message, e) { }
	}
}
