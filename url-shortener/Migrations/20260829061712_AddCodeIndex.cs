using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_playground.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Links_Code",
                table: "Links",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Links_Code",
                table: "Links");
        }
    }
}
