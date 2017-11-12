using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    //博客 实体
    public class mb_blog
    {
        //博客ID 唯一标示符 声明主键
        [Key]
        public int Blog_id { get; set; }
        //博客的标题
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Blog_title { get; set; }
        //博客的创建时间
        public DateTime Create_time { get; set; }
        //博客的创建者ID
        public string Create_id { get; set; }
        //博客的标签
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Blog_tag { get; set; }
        //博客分类
        [Required]
        public string Classify { get; set; }
        //博客内容
        public string Content { get; set; }
        //博客浏览量
        public int PageViews { get; set; }
        

    }
}
