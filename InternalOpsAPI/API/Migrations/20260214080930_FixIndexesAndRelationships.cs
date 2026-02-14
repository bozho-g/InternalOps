using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class FixIndexesAndRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_AspNetUsers_UserId",
                table: "RequestComments");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_AspNetUsers_UserId",
                table: "RequestComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_AspNetUsers_UserId",
                table: "RequestComments");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_AspNetUsers_UserId",
                table: "RequestComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
