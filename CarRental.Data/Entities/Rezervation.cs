using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Data.Entities
{
	/// <summary>
	/// Rezervation entity. Holding the information of each client's rezervation.
	/// </summary>
	public class Rezervation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RezervationId { get; set; }

		public int ClientId { get; set; }

		[ForeignKey("ClientId")]
		public virtual ClientAccount ClientAccount { get; set; }

		public int CarType { get; set; }

		public string CarPlateNumber { get; set; }

		public DateTime PickUpDate { get; set; }

		public DateTime ReturnDate { get; set; }

		public decimal DepositFee { get; set; }

		public decimal? CancellationFee { get; set; }

		public decimal? CancelationFeeRate { get; set; }

		public decimal RentaltFee { get; set; }

		public bool IsPickedUp { get; set; }

		public bool IsCancelled { get; set; }

		public bool IsReturned { get; set; }
	}
}