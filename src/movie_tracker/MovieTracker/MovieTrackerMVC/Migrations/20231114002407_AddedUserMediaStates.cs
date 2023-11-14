using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieTrackerMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserMediaStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserMediaStatus",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[,]
                {
                    { -100L, -16744448, "Now" },
                    { -90L, -16776961, "Finished" },
                    { -80L, -8388480, "Interested" },
                    { -70L, -256, "On Hold" },
                    { -60L, -65536, "Dropped" },
                    { -50L, -8355712, "Uninterested" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserMediaStatus",
                keyColumn: "Id",
                keyValue: -100L);

            migrationBuilder.DeleteData(
                table: "UserMediaStatus",
                keyColumn: "Id",
                keyValue: -90L);

            migrationBuilder.DeleteData(
                table: "UserMediaStatus",
                keyColumn: "Id",
                keyValue: -80L);

            migrationBuilder.DeleteData(
                table: "UserMediaStatus",
                keyColumn: "Id",
                keyValue: -70L);

            migrationBuilder.DeleteData(
                table: "UserMediaStatus",
                keyColumn: "Id",
                keyValue: -60L);

            migrationBuilder.DeleteData(
                table: "UserMediaStatus",
                keyColumn: "Id",
                keyValue: -50L);
        }
    }
}
