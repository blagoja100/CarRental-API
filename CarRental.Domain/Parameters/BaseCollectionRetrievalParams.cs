using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Base class for paged collection retrieval params
	/// </summary>
	public abstract class BaseCollectionRetrievalParams
	{
		/// <summary>
		/// Creates a new, empty model.
		/// </summary>
		public BaseCollectionRetrievalParams()
		{
			// Used by MVC
		}

		/// <summary>
		/// Start index to return items from. Default is 0.
		/// </summary>
		public int StartIndex { get; set; }

		/// <summary>
		/// Max Count of items to return. Default is 0 (return all items).
		/// </summary>
		public int MaxItems { get; set; }
	}
}
