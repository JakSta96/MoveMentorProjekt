using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoveMentor.Data.Migrations
{
    public partial class AddCommentToTrening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "SportType");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Trening",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Trening");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "SportType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
