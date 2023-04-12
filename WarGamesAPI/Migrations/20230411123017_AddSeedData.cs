using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarGamesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Answer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Address",
                columns: new[] { "Id", "Attention", "CareOf", "City", "Country", "Municipality", "Street", "UserId", "ZipCode" },
                values: new object[] { 1, null, null, "Stockholm", "Sweden", null, "Röntgenvägen 5 lgh 1410", 5, "14152" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "AddressId", "AgreeMarketing", "Email", "FirstName", "FullName", "Gender", "LastName", "MobilePhoneNumber", "Password", "PhoneNumber", "ProfileImage", "SocialSecurityNumber", "SubscribeToEmailNotification" },
                values: new object[] { 5, 1, true, "khaled@khaled.se", "Khaled", "Khaled Abo", "Man", "Abo", null, "123456", null, null, "198507119595", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Address",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Answer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
