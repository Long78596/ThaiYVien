using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiYVien.Migrations
{
    /// <inheritdoc />
    public partial class databaseAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppoitmentModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SlotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlotTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cancelled = table.Column<bool>(type: "bit", nullable: false),
                    Payment = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppoitmentModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppoitmentModel_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppoitmentModel_LocationModel_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppoitmentModel_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppoitmentModel_LocationId",
                table: "AppoitmentModel",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppoitmentModel_ServiceId",
                table: "AppoitmentModel",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppoitmentModel_UserId",
                table: "AppoitmentModel",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppoitmentModel");

            migrationBuilder.DropTable(
                name: "LocationModel");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }
    }
}
