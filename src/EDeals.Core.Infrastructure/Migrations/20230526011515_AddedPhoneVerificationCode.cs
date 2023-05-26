using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDeals.Core.Infrastructure.Migrations
{
    public partial class AddedPhoneVerificationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DigitCode",
                table: "AspNetUsers",
                type: "varchar(6)",
                maxLength: 6,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResendAvailableAfter",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DigitCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResendAvailableAfter",
                table: "AspNetUsers");
        }
    }
}
