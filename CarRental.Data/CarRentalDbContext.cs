using CarRental.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace CarRental.Data
{
	public class CarRentalDbContext : DbContext
	{
		public CarRentalDbContext()
		{
		}

		public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["CarRentalConnectionString"].ConnectionString);
			}

			base.OnConfiguring(optionsBuilder);
		}

		public virtual DbSet<ClientAccount> ClientAccounts { get; set; }
		public virtual DbSet<Rezervation> Rezervations { get; set; }
	}
}