using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MethodConf.Cms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionFeedbackEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SessionFeedback",
                type: "TEXT",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "SessionFeedback");
        }
    }
}
