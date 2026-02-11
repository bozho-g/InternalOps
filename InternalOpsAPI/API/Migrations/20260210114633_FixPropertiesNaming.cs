using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class FixPropertiesNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_ApprovedById",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "Requests",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedOn",
                table: "Requests",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Requests",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "ApprovedById",
                table: "Requests",
                newName: "HandledById");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_ApprovedById",
                table: "Requests",
                newName: "IX_Requests_HandledById");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "RequestComments",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "RequestedById",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_HandledById",
                table: "Requests",
                column: "HandledById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_HandledById",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Requests",
                newName: "UpdatedOn");

            migrationBuilder.RenameColumn(
                name: "HandledById",
                table: "Requests",
                newName: "ApprovedById");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Requests",
                newName: "DeletedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Requests",
                newName: "CreatedOn");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_HandledById",
                table: "Requests",
                newName: "IX_Requests_ApprovedById");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RequestComments",
                newName: "CreatedOn");

            migrationBuilder.AlterColumn<string>(
                name: "RequestedById",
                table: "Requests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_ApprovedById",
                table: "Requests",
                column: "ApprovedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
