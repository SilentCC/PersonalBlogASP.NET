using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyBlog.Migrations
{
    public partial class mb_blog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mb_blog",
                columns: table => new
                {
                    Blog_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Blog_tag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Blog_title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Classify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Create_id = table.Column<int>(type: "int", nullable: false),
                    Create_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PageViews = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mb_blog", x => x.Blog_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mb_blog");
        }
    }
}
