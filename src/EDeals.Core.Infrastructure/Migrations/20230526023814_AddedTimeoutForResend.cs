using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDeals.Core.Infrastructure.Migrations
{
    public partial class AddedTimeoutForResend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResendAvailableAfter",
                table: "AspNetUsers",
                newName: "ResendTokenAvailableAfter");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResendCodeAvailableAfter",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResendCodeAvailableAfter",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ResendTokenAvailableAfter",
                table: "AspNetUsers",
                newName: "ResendAvailableAfter");
        }
    }
}
