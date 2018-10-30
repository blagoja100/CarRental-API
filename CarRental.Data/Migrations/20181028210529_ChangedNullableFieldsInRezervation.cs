using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace CarRental.Data.Migrations
{
	public partial class ChangedNullableFieldsInRezervation : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<DateTime>(
				name: "ReturnDate",
				table: "Rezervations",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldNullable: true);

			migrationBuilder.AlterColumn<DateTime>(
				name: "PickUpDate",
				table: "Rezervations",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldNullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<DateTime>(
				name: "ReturnDate",
				table: "Rezervations",
				nullable: true,
				oldClrType: typeof(DateTime));

			migrationBuilder.AlterColumn<DateTime>(
				name: "PickUpDate",
				table: "Rezervations",
				nullable: true,
				oldClrType: typeof(DateTime));
		}
	}
}