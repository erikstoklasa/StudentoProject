using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentoMainProject.Migrations
{
    public partial class Logging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroupEnrollments_Students_StudentId",
                table: "StudentGroupEnrollments");

            migrationBuilder.CreateTable(
                name: "LogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAuthId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventParam = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogItems", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroupEnrollments_Students_StudentId",
                table: "StudentGroupEnrollments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroupEnrollments_Students_StudentId",
                table: "StudentGroupEnrollments");

            migrationBuilder.DropTable(
                name: "LogItems");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroupEnrollments_Students_StudentId",
                table: "StudentGroupEnrollments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
