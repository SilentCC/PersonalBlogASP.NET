using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBlog.Migrations
{
    public partial class updaterelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship");

            migrationBuilder.RenameTable(
                name: "Relationship",
                newName: "mb_relationship");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mb_relationship",
                table: "mb_relationship",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_mb_relationship",
                table: "mb_relationship");

            migrationBuilder.RenameTable(
                name: "mb_relationship",
                newName: "Relationship");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relationship",
                table: "Relationship",
                column: "ID");
        }
    }
}
