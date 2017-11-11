﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using MyBlog.Models;
using System;

namespace MyBlog.Migrations
{
    [DbContext(typeof(MyBlogContext))]
    [Migration("20171111031003_updatemb_blog")]
    partial class updatemb_blog
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyBlog.Models.mb_blog", b =>
                {
                    b.Property<int>("Blog_id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Blog_tag");

                    b.Property<string>("Blog_title");

                    b.Property<string>("Classify");

                    b.Property<string>("Content");

                    b.Property<string>("Create_id");

                    b.Property<DateTime>("Create_time");

                    b.Property<int>("PageViews");

                    b.HasKey("Blog_id");

                    b.ToTable("mb_blog");
                });
#pragma warning restore 612, 618
        }
    }
}