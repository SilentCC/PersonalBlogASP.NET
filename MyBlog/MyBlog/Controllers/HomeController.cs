using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment hostingEnv;

        //依赖注入MyBlogContext
        private MyBlogContext _context;
        public HomeController(IHostingEnvironment env,MyBlogContext context)
        {
            this._context = context;
            this.hostingEnv = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.mb_blog.ToListAsync());
            
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Kindeditor 编辑器图片上传处理函数
        public IActionResult KindeditorPicUpload(IList<IFormFile> imgFile, string dir)
        {
            //返回结果，为json格式
            PicUploadResponse rspJson = new PicUploadResponse() { error = 0, url = "/upload/" };
            long size = 0;
            /*var filename = "/upload/image/";
            foreach (var file in imgFile)
            {
                 filename +=file.FileName.Trim('"');

                FileStream fs = System.IO.File.Create(filename);
                file.CopyTo(fs);
                fs.Flush();
            }
            rspJson.url = filename;
            return Json(rspJson);*/
            string tempname = "";

            foreach (var file in imgFile)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName.ToString()
                                .Trim('"');
                //图片后缀名
                var extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                //图片重新命名
                var filename1 = System.Guid.NewGuid().ToString() + extname;

                tempname = filename1;
                //获取当前项目的路径
                var path = hostingEnv.WebRootPath;
                //完整的图片储存路径
                filename = hostingEnv.WebRootPath + $@"\upload\{filename1}";
                size += file.Length;
                //在wwwroot的upload文件夹下，写入图片
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    //清楚缓冲区数据
                    fs.Flush();
                    
                }
            }
            rspJson.error = 0;
            rspJson.url = $@"../../upload/" + tempname;
            return Json(rspJson);
        }
      
    }

    public class PicUploadResponse
    {
        public int error { get; set; }
        public string url { get; set; }
    }


}


