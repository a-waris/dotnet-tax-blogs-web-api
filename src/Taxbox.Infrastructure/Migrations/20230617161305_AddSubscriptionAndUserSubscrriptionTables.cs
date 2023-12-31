﻿using System;
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
                    ValidityPeriodType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    CancellationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    
                    // add a constraint to ensure that either subscription start date and subscription end date are both null or both not null
                    table.CheckConstraint("CK_UserSubscriptions_SubscriptionStartDate_SubscriptionEndDate", "((SubscriptionStartDate IS NULL AND SubscriptionEndDate IS NULL) OR (SubscriptionStartDate IS NOT NULL AND SubscriptionEndDate IS NOT NULL))");
                    
                    // add a constraint to ensure that next billing date is not null if subscription start date and subscription end date are both not null
                    table.CheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionStartDate_SubscriptionEndDate", "((NextBillingDate IS NULL AND SubscriptionStartDate IS NULL AND SubscriptionEndDate IS NULL) OR (NextBillingDate IS NOT NULL AND SubscriptionStartDate IS NOT NULL AND SubscriptionEndDate IS NOT NULL))");
                    
                    // add a constraint to ensure that next billing date is greater than subscription end date 
                    table.CheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionEndDate", "((NextBillingDate IS NULL AND SubscriptionEndDate IS NULL) OR (NextBillingDate IS NOT NULL AND SubscriptionEndDate IS NOT NULL AND NextBillingDate > SubscriptionEndDate))");
                    
                    // add a constraint to ensure that subscription end date is greater than subscription start date
                    table.CheckConstraint("CK_UserSubscriptions_SubscriptionEndDate_SubscriptionStartDate", "((SubscriptionEndDate IS NULL AND SubscriptionStartDate IS NULL) OR (SubscriptionEndDate IS NOT NULL AND SubscriptionStartDate IS NOT NULL AND SubscriptionEndDate > SubscriptionStartDate))");
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
                columns: new[] { "Id", "Name", "Status", "Description", "Currency",  "Amount", "ValidityPeriod", "ValidityPeriodType", "VAT" },
                values: new object[] { new Guid("b1b9b6a0-0b9a-4b1e-9b0a-0b9a4b1e9b0b"), "AED 390 + 5% VAT", "Active", "AED 390 + 5% VAT", "aed", (decimal)390, 30, "Days",(decimal)0.05  }
            );      
                
            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "Name", "Status", "Description", "Currency",  "Amount", "ValidityPeriod", "ValidityPeriodType", "VAT" },
                values: new object[] { new Guid("b1b9b6a0-0b9a-4b1e-9b0a-0b9a4b1e9b0c"), "AED 590 + 5% VAT", "Active", "AED 590 + 5% VAT", "aed", (decimal)590, 30, "Days", (decimal)0.05 }
            );
            
            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "Name", "Status", "Description", "Currency",  "Amount", "ValidityPeriod", "ValidityPeriodType", "VAT" },
                values: new object[] { new Guid("b1b9b6a0-0b9a-4b1e-9b0a-0b9a4b1e9b0d"), "Free", "Active", "Free Subscription", "aed", (decimal)0, 30, "Days",
                    (decimal)0 }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // remove all constraints
            migrationBuilder.DropUniqueConstraint("AK_UserSubscriptions_UserId_IsActive", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_SubscriptionStartDate_SubscriptionEndDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionEndDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_NextBillingDate_SubscriptionStartDate_SubscriptionEndDate", "UserSubscriptions");
            migrationBuilder.DropCheckConstraint("CK_UserSubscriptions_SubscriptionEndDate_SubscriptionStartDate", "UserSubscriptions");
            
            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
