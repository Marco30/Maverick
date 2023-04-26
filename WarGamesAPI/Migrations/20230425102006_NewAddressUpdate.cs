using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewAddressUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_UserId",
                table: "Address");

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Address");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "UserId");

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "UserId", "Attention", "CareOf", "City", "Country", "Municipality", "Street", "ZipCode" },
                values: new object[] { 5, null, null, "Stockholm", "Sweden", null, "Röntgenvägen 5 lgh 1410", "14152" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Address",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "Attention", "CareOf", "City", "Country", "Municipality", "Street", "UserId", "ZipCode" },
                values: new object[] { 1, null, null, "Stockholm", "Sweden", null, "Röntgenvägen 5 lgh 1410", 5, "14152" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5,
                column: "AddressId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserId",
                table: "Address",
                column: "UserId",
                unique: true);
        }
    }
}
