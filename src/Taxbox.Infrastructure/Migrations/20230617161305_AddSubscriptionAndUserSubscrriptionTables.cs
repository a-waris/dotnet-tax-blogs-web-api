using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taxbox.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionAndUserSubscrriptionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidityPeriod = table.Column<int>(type: "int", nullable: false),
                    ValidityPeriodType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextBillingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AutoRenewal = table.Column<bool>(type: "bit", nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrialStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrialEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    
                    // add a constraint to ensure that a user can only have one active subscription at a time
                    table.UniqueConstraint("AK_UserSubscriptions_UserId_IsActive", x => new { x.UserId, x.IsActive });
                    
                    // add a constraint to ensure that either trial start date and trial end date are both null or both not null
                    table.CheckConstraint("CK_UserSubscriptions_TrialStartDate_TrialEndDate", "((TrialStartDate IS NULL AND TrialEndDate IS NULL) OR (TrialStartDate IS NOT NULL AND TrialEndDate IS NOT NULL))");
                    
                    // add a constraint to ensure that either subscription start date and subscription end date are both null or both not null
                    table.CheckConstraint("CK_UserSubscriptions_SubscriptionStartDate_SubscriptionEndDate", "((SubscriptionStartDate IS NULL AND SubscriptionEndDate IS NULL) OR (SubscriptionStartDate IS NOT NULL AND SubscriptionEndDate IS NOT NULL))");
                    
                    // add a constraint to ensure that next billing date is not null if subscription start date and subscription end date are both not null
                    table.CheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionEndDate", "((NextBillingDate IS NULL AND SubscriptionEndDate IS NULL) OR (NextBillingDate IS NOT NULL AND SubscriptionEndDate IS NOT NULL))");
                    
                    // add a constraint to ensure that next billing date is greater than subscription start date 
                    table.CheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionStartDate", "((NextBillingDate IS NULL AND SubscriptionStartDate IS NULL) OR (NextBillingDate IS NOT NULL AND SubscriptionStartDate IS NOT NULL AND NextBillingDate > SubscriptionStartDate))");
                    
                    // add a constraint to ensure that subscription end date is greater than subscription start date
                    table.CheckConstraint("CK_UserSubscriptions_SubscriptionEndDate_SubscriptionStartDate", "((SubscriptionEndDate IS NULL AND SubscriptionStartDate IS NULL) OR (SubscriptionEndDate IS NOT NULL AND SubscriptionStartDate IS NOT NULL AND SubscriptionEndDate > SubscriptionStartDate))");
                    
                    // add a constraint to ensure that trial end date is greater than trial start date
                    table.CheckConstraint("CK_UserSubscriptions_TrialEndDate_TrialStartDate", "((TrialEndDate IS NULL AND TrialStartDate IS NULL) OR (TrialEndDate IS NOT NULL AND TrialStartDate IS NOT NULL AND TrialEndDate > TrialStartDate))");
                    
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_SubscriptionId",
                table: "UserSubscriptions",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_UserId",
                table: "UserSubscriptions",
                column: "UserId");
            
             // seed data into subscriptions table - Standard, Gold, Platinum
            
            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "Name", "Status", "Description", "Currency",  "Amount", "ValidityPeriod", "ValidityPeriodType", },
                values: new object[]  { new Guid("b1b9b6a0-0b9a-4b1e-9b0a-0b9a4b1e9b0a"), "Standard", "Active", "Standard Subscription", "usd", (decimal)10, 30, "Days" }
            );      
                
            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "Name", "Status", "Description", "Currency",  "Amount", "ValidityPeriod", "ValidityPeriodType", },
                values: new object[] { new Guid("b1b9b6a0-0b9a-4b1e-9b0a-0b9a4b1e9b0b"), "Gold", "Active", "Gold Subscription", "usd", (decimal)20, 6, "Months" }
            );      
                
            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "Name", "Status", "Description", "Currency",  "Amount", "ValidityPeriod", "ValidityPeriodType", },
                values: new object[] { new Guid("b1b9b6a0-0b9a-4b1e-9b0a-0b9a4b1e9b0c"), "Platinum", "Active", "Platinum Subscription", "usd", (decimal)30, 12, "Months" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remove all constraints
            migrationBuilder.DropUniqueConstraint("AK_UserSubscriptions_UserId_IsActive", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_TrialStartDate_TrialEndDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_SubscriptionStartDate_SubscriptionEndDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionEndDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionStartDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_SubscriptionEndDate_SubscriptionStartDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_TrialEndDate_TrialStartDate", "UserSubscriptions");
            
            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
