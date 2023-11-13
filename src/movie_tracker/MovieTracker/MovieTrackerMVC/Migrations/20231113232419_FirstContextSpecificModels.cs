using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTrackerMVC.Migrations
{
    /// <inheritdoc />
    public partial class FirstContextSpecificModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMediaNotes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserMediaId = table.Column<long>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMediaNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMediaStatus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMediaStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMedia",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    MediaId = table.Column<long>(type: "INTEGER", nullable: false),
                    UserMediaNotesId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserMediaStatusId = table.Column<long>(type: "INTEGER", nullable: true),
                    Score = table.Column<float>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedia", x => new { x.UserId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_UserMedia_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMedia_UserMediaNotes_UserMediaNotesId",
                        column: x => x.UserMediaNotesId,
                        principalTable: "UserMediaNotes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserMedia_UserMediaStatus_UserMediaStatusId",
                        column: x => x.UserMediaStatusId,
                        principalTable: "UserMediaStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_MediaId",
                table: "UserMedia",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_UserMediaNotesId",
                table: "UserMedia",
                column: "UserMediaNotesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMedia_UserMediaStatusId",
                table: "UserMedia",
                column: "UserMediaStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMedia");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "UserMediaNotes");

            migrationBuilder.DropTable(
                name: "UserMediaStatus");
        }
    }
}
