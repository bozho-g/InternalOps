using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Requests_IsDeleted",
                table: "Requests",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestType",
                table: "Requests",
                column: "RequestType");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Status",
                table: "Requests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Status_UpdatedAt",
                table: "Requests",
                columns: new[] { "Status", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UpdatedAt",
                table: "Requests",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Requests_IsDeleted",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestType",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_Status",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_Status_UpdatedAt",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_UpdatedAt",
                table: "Requests");
        }
    }
}
