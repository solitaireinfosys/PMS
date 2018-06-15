using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PMS.APP.Migrations
{
    public partial class db10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "Projects",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "Milestones",
                newName: "Notes");

            migrationBuilder.AddColumn<decimal>(
                name: "RecievedAmount",
                table: "Milestones",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecievedAmount",
                table: "Milestones");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Projects",
                newName: "Feedback");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Milestones",
                newName: "Feedback");
        }
    }
}
