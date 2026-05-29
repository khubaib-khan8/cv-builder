using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVBuilderAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                table: "CVs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateName",
                table: "CVs");
        }
    }
}
