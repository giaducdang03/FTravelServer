using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTravel.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "City",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "City");
        }
    }
}
