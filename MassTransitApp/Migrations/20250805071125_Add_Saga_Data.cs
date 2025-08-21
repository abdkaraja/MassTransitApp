using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MassTransitApp.Migrations
{
    /// <inheritdoc />
    public partial class Add_Saga_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsletterOnboardingSagaData",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentState = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    SubscriberId = table.Column<Guid>(type: "uuid", nullable: false),
                    WelcomeEmailSent = table.Column<bool>(type: "boolean", nullable: false),
                    FollowUpEmailSent = table.Column<bool>(type: "boolean", nullable: false),
                    OnboardingCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsletterOnboardingSagaData", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsletterOnboardingSagaData");
        }
    }
}
