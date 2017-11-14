using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBlog.Migrations
{
    public partial class updatecomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mb_comment",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User_accept = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_publish = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    blog_id = table.Column<int>(type: "int", nullable: false),
                    comment_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mb_comment", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mb_comment");
        }
    }
}
