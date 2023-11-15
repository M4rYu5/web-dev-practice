using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTrackerMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedNextOptionInUserMediaStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserMediaStatus",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[] { -85L, -8388480, "Next" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMediaStatus",
                keyColumn: "Id",
                keyValue: -85L);
        }
    }
}
