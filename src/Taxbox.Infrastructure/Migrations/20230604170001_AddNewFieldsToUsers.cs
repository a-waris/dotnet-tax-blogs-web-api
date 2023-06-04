using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Taxbox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                nullable: false, 
                type: "nvarchar(254)",
                maxLength: 254
                );

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                nullable: false,
                type: "nvarchar(254)",
                maxLength: 254
                );

            migrationBuilder.AddColumn<string>(
                name: "DisplayPicture",
                table: "Users",
                nullable: true,
                type: "nvarchar(2048)",
                maxLength: 2048
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");
            
            migrationBuilder.DropColumn(
                name: "DisplayPicture",
                table: "Users");
        }
    }
}
