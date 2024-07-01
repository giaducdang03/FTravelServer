using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTravel.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableServiceTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ServiceTicket",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ServiceTicket");
        }
    }
}
