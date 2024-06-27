using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FTravel.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabelNoti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Notificat__UserI__48CFD27E",
                table: "Notification");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Notification",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Notificat__UserI__48CFD27E",
                table: "Notification",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Notificat__UserI__48CFD27E",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Notification");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notification",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Notification",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddForeignKey(
                name: "FK__Notificat__UserI__48CFD27E",
                table: "Notification",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
