using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class entitiesupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Projects_ProjectId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_ProjectId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "Profile_ProfilePicture",
                table: "Users",
                newName: "AboutMe_ProfilePicture");

            migrationBuilder.RenameColumn(
                name: "Profile_LastName",
                table: "Users",
                newName: "AboutMe_LastName");

            migrationBuilder.RenameColumn(
                name: "Profile_Headline",
                table: "Users",
                newName: "AboutMe_Headline");

            migrationBuilder.RenameColumn(
                name: "Profile_FirstName",
                table: "Users",
                newName: "AboutMe_FirstName");

            migrationBuilder.RenameColumn(
                name: "Profile_Country",
                table: "Users",
                newName: "AboutMe_Country");

            migrationBuilder.RenameColumn(
                name: "Profile_City",
                table: "Users",
                newName: "AboutMe_City");

            migrationBuilder.RenameColumn(
                name: "Profile_Bio",
                table: "Users",
                newName: "AboutMe_Bio");

            migrationBuilder.AddColumn<List<string>>(
                name: "Skills",
                table: "Projects",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "AboutMe_ProfilePicture",
                table: "Users",
                newName: "Profile_ProfilePicture");

            migrationBuilder.RenameColumn(
                name: "AboutMe_LastName",
                table: "Users",
                newName: "Profile_LastName");

            migrationBuilder.RenameColumn(
                name: "AboutMe_Headline",
                table: "Users",
                newName: "Profile_Headline");

            migrationBuilder.RenameColumn(
                name: "AboutMe_FirstName",
                table: "Users",
                newName: "Profile_FirstName");

            migrationBuilder.RenameColumn(
                name: "AboutMe_Country",
                table: "Users",
                newName: "Profile_Country");

            migrationBuilder.RenameColumn(
                name: "AboutMe_City",
                table: "Users",
                newName: "Profile_City");

            migrationBuilder.RenameColumn(
                name: "AboutMe_Bio",
                table: "Users",
                newName: "Profile_Bio");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Skills",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ProjectId",
                table: "Skills",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Projects_ProjectId",
                table: "Skills",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
