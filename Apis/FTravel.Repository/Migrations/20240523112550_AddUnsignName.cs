using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTravel.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUnsignName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DOB",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignFullName",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignName",
                table: "Trip",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignName",
                table: "Station",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignName",
                table: "Service",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignName",
                table: "Route",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignFullName",
                table: "Customer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignName",
                table: "City",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnsignName",
                table: "BusCompany",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnsignFullName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UnsignName",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "UnsignName",
                table: "Station");

            migrationBuilder.DropColumn(
                name: "UnsignName",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "UnsignName",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "UnsignFullName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UnsignName",
                table: "City");

            migrationBuilder.DropColumn(
                name: "UnsignName",
                table: "BusCompany");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DOB",
                table: "User",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
