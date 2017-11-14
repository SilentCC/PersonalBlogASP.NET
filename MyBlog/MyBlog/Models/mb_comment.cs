using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    //评论表
    public class mb_comment
    {
        //评论ID 主键
        [Key]
        public int ID {get;set;}
        //评论用户A，发布者
        public string User_publish { get; set; }
        //评论用户B，被评论这
        public string User_accept { get; set; }
        //发布日期
        public DateTime Create_time { get; set; }
        //发布内容
        public string content { get; set; }
        //评论的博客id
        public int blog_id { get; set;}
        //评论的评论id
        public int comment_id { get; set; }

    }
}
