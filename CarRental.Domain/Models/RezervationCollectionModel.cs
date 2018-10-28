using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
	/// <summary>
	/// Rezervation collection model.
	/// </summary>
	public class RezervationCollectionModel : BaseCollectionModel<RezervationModel>
	{
		/// <summary>
		/// Creates new instance of the rezervation collection model.
		/// </summary>
		public RezervationCollectionModel()
		{
		}

		/// <summary>
		/// Creates new instance of the rezervation collection model.
		/// </summary>
		/// <param name="rezervationModels">Collection of rezervation models.</param>
		public RezervationCollectionModel(IEnumerable<RezervationModel> rezervationModels) : base(rezervationModels)
		{
		}

		/// <summary>
		/// Creates new instance of the rezervation collection model.
		/// </summary>
		/// <param name="rezervationModels">Collection of rezervation models.</param>
		/// <param name="startIndex">Starting index of the splice.</param>
		/// <param name="spliceCount">Number of elements per splice.</param>
		public RezervationCollectionModel(IEnumerable<RezervationModel> rezervationModels, int startIndex, int spliceCount) : base(rezervationModels, startIndex, spliceCount)
		{
		}
	}
}
