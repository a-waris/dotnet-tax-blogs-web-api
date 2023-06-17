using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Taxbox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionAndPaymentMethodTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier"),
                    Name = table.Column<string>(type: "nvarchar(max)"),
                    Status = table.Column<string>(type: "nvarchar(max)"),
                    Description = table.Column<string>(type: "nvarchar(max)"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier"),
                    ValidityPeriod = table.Column<int>(type: "int"),
                    ValidityPeriodType = table.Column<string>(type: "nvarchar(max)"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    
                    table.ForeignKey(
                        name: "FK_Subscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });
            
            
            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier"),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier"),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime2"),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2"),
                    TrialStartDate= table.Column<DateTime>(type: "datetime2"),
                    TrialEndDate= table.Column<DateTime>(type: "datetime2"),
                    NextBillingDate= table.Column<DateTime>(type: "datetime2"),
                    IsActive = table.Column<bool>(type: "bit"),
                    AutoRenewal = table.Column<bool>(type: "bit"),
                    CouponCode = table.Column<string>(type: "nvarchar(max)"),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)"),
                    CancellationDate = table.Column<DateTime>(type: "datetime2"),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier"),
                },
                
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptions", x => x.Id);
                    
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id");
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSubscriptions");
            
            migrationBuilder.DropTable(
                name: "Subscriptions");

        }
    }
}
