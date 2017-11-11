using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBlog.Migrations
{
    public partial class mb_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mb_user",
                columns: table => new
                {
                    User_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Blog_num = table.Column<int>(type: "int", nullable: false),
                    Comment_num = table.Column<int>(type: "int", nullable: false),
                    Fans_num = table.Column<int>(type: "int", nullable: false),
                    User_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_pwd = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mb_user", x => x.User_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mb_user");
        }
    }
}
