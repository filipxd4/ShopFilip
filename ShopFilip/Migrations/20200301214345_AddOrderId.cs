using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopFilip.Migrations
{
    public partial class AddOrderId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Login",
                newName: "Town");

            migrationBuilder.RenameColumn(
                name: "ConfirmPassword",
                table: "Login",
                newName: "Street");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Login",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Login",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Login");

            migrationBuilder.RenameColumn(
                name: "Town",
                table: "Login",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Login",
                newName: "ConfirmPassword");
        }
    }
}
