using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiYVien.Migrations
{
    /// <inheritdoc />
    public partial class CategoryService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryServiceId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_CategoryServiceId",
                table: "Services",
                column: "CategoryServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_CategoryServices_CategoryServiceId",
                table: "Services",
                column: "CategoryServiceId",
                principalTable: "CategoryServices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_CategoryServices_CategoryServiceId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_CategoryServiceId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CategoryServiceId",
                table: "Services");
        }
    }
}
