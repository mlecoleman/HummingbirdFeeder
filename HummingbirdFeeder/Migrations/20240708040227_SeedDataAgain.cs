using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HummingbirdFeeder.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Feeders",
                table: "Feeders");

            migrationBuilder.RenameTable(
                name: "Feeders",
                newName: "Feeder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feeder",
                table: "Feeder",
                column: "FeederId");

            migrationBuilder.InsertData(
                table: "Feeder",
                columns: new[] { "FeederId", "Zipcode" },
                values: new object[] { 1, 40204 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Feeder",
                table: "Feeder");

            migrationBuilder.DeleteData(
                table: "Feeder",
                keyColumn: "FeederId",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "Feeder",
                newName: "Feeders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feeders",
                table: "Feeders",
                column: "FeederId");
        }
    }
}
