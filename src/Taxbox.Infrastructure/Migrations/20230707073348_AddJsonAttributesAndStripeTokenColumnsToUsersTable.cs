using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxbox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJsonAttributesAndStripeTokenColumnsToUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetadataJson",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCustomerToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetadataJson",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StripeCustomerToken",
                table: "Users");
        }
    }
}
