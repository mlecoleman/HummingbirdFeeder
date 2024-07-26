using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HummingbirdFeeder.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feeder",
                columns: table => new
                {
                    FeederId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FeederName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Zipcode = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    LastChangeDate = table.Column<int>(type: "INTEGER", maxLength: 8, nullable: false),
                    ChangeFeeder = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeder", x => x.FeederId);
                });

            migrationBuilder.InsertData(
                table: "Feeder",
                columns: new[] { "FeederId", "ChangeFeeder", "FeederName", "LastChangeDate", "Zipcode" },
                values: new object[] { 1, true, "My Feeder", 20240725, "40204" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feeder");
        }
    }
}
