using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Note_oriousWebApp.API.Migrations
{
    /// <inheritdoc />
    public partial class AddReminderAndArchiveToNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchive",
                table: "Notes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Reminder",
                table: "Notes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchive",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Reminder",
                table: "Notes");
        }
    }
}
