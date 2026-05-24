using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventsTicketingSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StartsAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalSeats = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    HolderName = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ReservedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Name", "StartsAt", "TotalSeats" },
                values: new object[] { 1, "Live Coding Lounge – Friday Night", new DateTime(2026, 1, 17, 19, 0, 0, 0, DateTimeKind.Utc), 50 });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "EventId", "HolderName", "ReservedAt", "Status" },
                values: new object[,]
                {
                    { 1, 1, null, null, "Available" },
                    { 2, 1, null, null, "Available" },
                    { 3, 1, null, null, "Available" },
                    { 4, 1, null, null, "Available" },
                    { 5, 1, null, null, "Available" },
                    { 6, 1, null, null, "Available" },
                    { 7, 1, null, null, "Available" },
                    { 8, 1, null, null, "Available" },
                    { 9, 1, null, null, "Available" },
                    { 10, 1, null, null, "Available" },
                    { 11, 1, null, null, "Available" },
                    { 12, 1, null, null, "Available" },
                    { 13, 1, null, null, "Available" },
                    { 14, 1, null, null, "Available" },
                    { 15, 1, null, null, "Available" },
                    { 16, 1, null, null, "Available" },
                    { 17, 1, null, null, "Available" },
                    { 18, 1, null, null, "Available" },
                    { 19, 1, null, null, "Available" },
                    { 20, 1, null, null, "Available" },
                    { 21, 1, null, null, "Available" },
                    { 22, 1, null, null, "Available" },
                    { 23, 1, null, null, "Available" },
                    { 24, 1, null, null, "Available" },
                    { 25, 1, null, null, "Available" },
                    { 26, 1, null, null, "Available" },
                    { 27, 1, null, null, "Available" },
                    { 28, 1, null, null, "Available" },
                    { 29, 1, null, null, "Available" },
                    { 30, 1, null, null, "Available" },
                    { 31, 1, null, null, "Available" },
                    { 32, 1, null, null, "Available" },
                    { 33, 1, null, null, "Available" },
                    { 34, 1, null, null, "Available" },
                    { 35, 1, null, null, "Available" },
                    { 36, 1, null, null, "Available" },
                    { 37, 1, null, null, "Available" },
                    { 38, 1, null, null, "Available" },
                    { 39, 1, null, null, "Available" },
                    { 40, 1, null, null, "Available" },
                    { 41, 1, null, null, "Available" },
                    { 42, 1, null, null, "Available" },
                    { 43, 1, null, null, "Available" },
                    { 44, 1, null, null, "Available" },
                    { 45, 1, null, null, "Available" },
                    { 46, 1, null, null, "Available" },
                    { 47, 1, null, null, "Available" },
                    { 48, 1, null, null, "Available" },
                    { 49, 1, null, null, "Available" },
                    { 50, 1, null, null, "Available" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
