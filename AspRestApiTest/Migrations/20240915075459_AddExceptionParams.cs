using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspRestApiTest.Migrations
{
    public partial class AddExceptionParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BodyParameters",
                table: "ExceptionJournals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QueryParameters",
                table: "ExceptionJournals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyParameters",
                table: "ExceptionJournals");

            migrationBuilder.DropColumn(
                name: "QueryParameters",
                table: "ExceptionJournals");
        }
    }
}
