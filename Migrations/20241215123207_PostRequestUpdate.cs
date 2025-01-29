using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApps.Migrations
{
    /// <inheritdoc />
    public partial class PostRequestUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PostRequests",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PostRequests_UserId",
                table: "PostRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostRequests_AspNetUsers_UserId",
                table: "PostRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostRequests_AspNetUsers_UserId",
                table: "PostRequests");

            migrationBuilder.DropIndex(
                name: "IX_PostRequests_UserId",
                table: "PostRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostRequests");
        }
    }
}
