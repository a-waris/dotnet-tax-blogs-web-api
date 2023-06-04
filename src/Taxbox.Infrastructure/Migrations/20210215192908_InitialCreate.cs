using System;
using Microsoft.EntityFrameworkCore.Migrations;
using BC = BCrypt.Net.BCrypt;

namespace Taxbox.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName= table.Column<string>(name: "FirstName",nullable: false, type: "nvarchar(254)", maxLength: 254),
                    LastName=table.Column<string>(name: "LastName",nullable: false,type: "nvarchar(254)", maxLength: 254),
                    DisplayPicture=table.Column<string>( name: "DisplayPicture", nullable: true, type: "nvarchar(2048)", maxLength: 2048)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role", "FirstName", "LastName" },
                values: new object[] { new Guid("687d9fd5-2752-4a96-93d5-0f33a49913c6"), "admin@taxbox.com", BC.HashPassword("adminpassword"), "Admin", "Admin", "User" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role", "FirstName", "LastName" },
                values: new object[] { new Guid("6648c89f-e894-42bb-94f0-8fd1059c86b4"), "user@taxbox.com", BC.HashPassword("userpassword"), "User" , "Normal", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Users");
        }
    }
}
