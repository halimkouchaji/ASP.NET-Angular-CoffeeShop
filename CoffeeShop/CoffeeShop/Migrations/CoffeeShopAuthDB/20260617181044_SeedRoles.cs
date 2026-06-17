using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShop.API.Migrations.CoffeeShopAuthDB
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f2504e0-4f89-41d3-9a0c-0305e82c3301",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-9f8e-4a7b-b6c5-1234567890ab",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "User", "USER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f2504e0-4f89-41d3-9a0c-0305e82c3301",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Reader", "READER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1b2c3d4-9f8e-4a7b-b6c5-1234567890ab",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Writer", "WRITER" });
        }
    }
}
