using CarRental.Data.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarRental.Data
{
	public class CarRentalDbContext : DbContext
	{
		public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
		{
		}

		public DbSet<ClientAccount> ClientAccounts { get; set; }
		public DbSet<Rezervation> Rezervations { get; set; }
	}
}
