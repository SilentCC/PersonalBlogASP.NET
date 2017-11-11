using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;

namespace MyBlog.Models
{
    public class MyBlogContext : DbContext
    {
        public MyBlogContext (DbContextOptions<MyBlogContext> options)
            : base(options)
        {
        }

       

        public DbSet<MyBlog.Models.mb_blog> mb_blog { get; set; }

       

        public DbSet<MyBlog.Models.mb_user> mb_user { get; set; }
    }
}
