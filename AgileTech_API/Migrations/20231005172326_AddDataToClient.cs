using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgileTech_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "Id", "Created", "Email", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 5, 13, 23, 26, 465, DateTimeKind.Local).AddTicks(3087), "eduard@gmail.com", "Eduard Name" },
                    { 2, new DateTime(2023, 10, 5, 13, 23, 26, 465, DateTimeKind.Local).AddTicks(3138), "alis@gmail.com", "Alis Navarrete" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "clients",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
