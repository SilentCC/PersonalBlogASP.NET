using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBlog.Migrations
{
    public partial class updateuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_mb_user",
                table: "mb_user");

            migrationBuilder.AlterColumn<string>(
                name: "User_id",
                table: "mb_user",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ID",
                table: "mb_user",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mb_user",
                table: "mb_user",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_mb_user",
                table: "mb_user");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "mb_user");

            migrationBuilder.AlterColumn<string>(
                name: "User_id",
                table: "mb_user",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_mb_user",
                table: "mb_user",
                column: "User_id");
        }
    }
}
