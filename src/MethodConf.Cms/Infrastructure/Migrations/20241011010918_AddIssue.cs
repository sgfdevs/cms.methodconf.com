using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MethodConf.Cms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConferenceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 10000, nullable: false),
                    Resolution = table.Column<string>(type: "TEXT", maxLength: 10000, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");
        }
    }
}
