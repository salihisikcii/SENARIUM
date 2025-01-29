using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApps.Migrations
{
    /// <inheritdoc />
    public partial class sh256update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SHA256Hash",
                table: "Posts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SHA256Hash",
                table: "Posts");
        }
    }
}
