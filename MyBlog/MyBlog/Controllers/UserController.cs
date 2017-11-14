using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;

namespace MyBlog.Controllers
{
    public class UserController : Controller
    {
        private readonly MyBlogContext _context;

        private IHostingEnvironment hostingEnv;

        public UserController(MyBlogContext context, IHostingEnvironment env)
        {
            _context = context;
            this.hostingEnv = env;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.mb_user.ToListAsync());
        }

        //登录页面 GET：User/Login
        public async Task<IActionResult> Login()
        {
            return View();
        }
        //登录验证 POST:User/LoginCheck
        public async Task<IActionResult> LoginCheck(string User_id,string User_pwd)
        {
            //验证用户
            var mb_user = await _context.mb_user.
                SingleOrDefaultAsync(m => m.User_id == User_id && m.User_pwd == User_pwd);
            if(mb_user == null)
            {
                return NotFound();
            }

            //用户标识
             var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, User_id));

            identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
            //用户签到
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
          
           
            //重定向     
            return RedirectToAction("Index", "Home");

        }
        //退出登录 GET：User/Logout
        public async Task<IActionResult> Logout()
        {
            //注销登录
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_user = await _context.mb_user
                .SingleOrDefaultAsync(m => m.User_id == id);
            if (mb_user == null)
            {
                return NotFound();
            }

            return View(mb_user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,User_id,User_name,Fans_num,Blog_num,Comment_num,User_pwd,Avatar")] mb_user mb_user)
        {
            if (ModelState.IsValid)
            {
                mb_user.Avatar = "../../userhead/1.png";
                _context.Add(mb_user);
                await _context.SaveChangesAsync();

                //用户标识
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, mb_user.User_id));

                identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                //用户签到
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Home");
            }
            return View(mb_user);
           
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_user = await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == id);
            if (mb_user == null)
            {
                return NotFound();
            }
            return View(mb_user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("User_id,User_name,Fans_num,Blog_num,Comment_num,User_pwd,Avatar")] mb_user mb_user)
        {
            if (id != mb_user.User_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mb_user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mb_userExists(mb_user.User_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mb_user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_user = await _context.mb_user
                .SingleOrDefaultAsync(m => m.User_id == id);
            if (mb_user == null)
            {
                return NotFound();
            }

            return View(mb_user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var mb_user = await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == id);
            _context.mb_user.Remove(mb_user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool mb_userExists(string id)
        {
            return _context.mb_user.Any(e => e.User_id == id);
        }
       //上传头像函数
        public async Task<IActionResult> Uploadavatar(List<IFormFile> files)
        {
            string tempname = "";

            foreach (var file in files)
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
                filename = hostingEnv.WebRootPath + $@"\userhead\{filename1}";

                //当前用户的头像路径更改
                var mb_user = await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == User.Identity.Name);

                mb_user.Avatar = $@"../../userhead/" + tempname; ;
                try
                {
                    _context.Update(mb_user);
                    await _context.SaveChangesAsync();
                }
                catch { }
                //用户头像储存到ViewData
                ViewData["avatar"] = mb_user.Avatar;

                //在wwwroot的upload文件夹下，写入图片
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    //清楚缓冲区数据
                    fs.Flush();

                }
            }

            return RedirectToAction("Index", "Blog");
        }
        
    }
}
