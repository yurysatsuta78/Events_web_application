using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipantDb_Events_EventId",
                table: "EventParticipantDb");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipantDb_Participants_ParticipantId",
                table: "EventParticipantDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipantDb",
                table: "EventParticipantDb");

            migrationBuilder.DropIndex(
                name: "IX_EventParticipantDb_EventId",
                table: "EventParticipantDb");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EventParticipantDb");

            migrationBuilder.RenameTable(
                name: "EventParticipantDb",
                newName: "EventParticipants");

            migrationBuilder.RenameIndex(
                name: "IX_EventParticipantDb_ParticipantId",
                table: "EventParticipants",
                newName: "IX_EventParticipants_ParticipantId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventRegistrationDate",
                table: "EventParticipants",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipants",
                table: "EventParticipants",
                columns: new[] { "EventId", "ParticipantId" });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantRoles",
                columns: table => new
                {
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantRoles", x => new { x.ParticipantId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ParticipantRoles_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Participant" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantRoles_RoleId",
                table: "ParticipantRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Events_EventId",
                table: "EventParticipants",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipants_Participants_ParticipantId",
                table: "EventParticipants",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Events_EventId",
                table: "EventParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_EventParticipants_Participants_ParticipantId",
                table: "EventParticipants");

            migrationBuilder.DropTable(
                name: "ParticipantRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventParticipants",
                table: "EventParticipants");

            migrationBuilder.RenameTable(
                name: "EventParticipants",
                newName: "EventParticipantDb");

            migrationBuilder.RenameIndex(
                name: "IX_EventParticipants_ParticipantId",
                table: "EventParticipantDb",
                newName: "IX_EventParticipantDb_ParticipantId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EventRegistrationDate",
                table: "EventParticipantDb",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "EventParticipantDb",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventParticipantDb",
                table: "EventParticipantDb",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EventParticipantDb_EventId",
                table: "EventParticipantDb",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipantDb_Events_EventId",
                table: "EventParticipantDb",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventParticipantDb_Participants_ParticipantId",
                table: "EventParticipantDb",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
