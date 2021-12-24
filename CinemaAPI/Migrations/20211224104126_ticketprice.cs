using Microsoft.EntityFrameworkCore.Migrations;

namespace CinemaAPI.Migrations
{
    public partial class ticketprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TicketPrice",
                table: "Movies",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Movies");
        }
    }
}
