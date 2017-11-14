using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class mb_user
    {
        [Key]
        public string ID { get; set; }
        //用户id， 主键
        [StringLength(60,MinimumLength =3)]
        [Required]
        public string User_id { get; set; }
        //用户姓名
        public string User_name { get; set; }
        //粉丝数量
        public int Fans_num { get; set; }
        //博客数量
        public int Blog_num { get; set; }
        //评论条数
        public int Comment_num { get; set; }
        //用户密码
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string User_pwd { get; set; }
        //用户头像路径
        public string Avatar { get; set; }


    }
}
