using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MethodConf.Cms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionFeedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SpeakerRating = table.Column<int>(type: "INTEGER", nullable: false),
                    ContentRating = table.Column<int>(type: "INTEGER", nullable: false),
                    VenueRating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", maxLength: 5000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionFeedback", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionFeedback");
        }
    }
}
