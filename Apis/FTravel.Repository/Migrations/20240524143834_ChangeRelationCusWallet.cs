using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTravel.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationCusWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallet_CustomerId",
                table: "Wallet");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CustomerId",
                table: "Wallet",
                column: "CustomerId",
                unique: true,
                filter: "[CustomerId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallet_CustomerId",
                table: "Wallet");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_CustomerId",
                table: "Wallet",
                column: "CustomerId");
        }
    }
}
