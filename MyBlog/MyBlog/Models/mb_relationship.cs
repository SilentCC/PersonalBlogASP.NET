using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    //用户之间关系表
    //用户A是用户B的粉丝
    public class mb_relationship
    {
        //主键
        [Key]
        public string ID { get; set; }
        //用户A的id
        public string UserA_id { get; set; }
        //用户B的id
        public string UserB_id { get; set; }
    }
}
