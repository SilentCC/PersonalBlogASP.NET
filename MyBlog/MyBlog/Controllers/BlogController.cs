using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly MyBlogContext _context;

        public BlogController(MyBlogContext context)
        {
            _context = context;
        }

        // GET: mb_blog
        //必须当前用户角色为User才可以访问  
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index(string sortOrder,string searchString,string currentFilter,int? page)
        {
            //博客总页面，显示的是当前用户的博客列表
            //根据sortOrder ，选择排序的方式

            //保存当前的排序方式
            ViewData["CurrentSort"] = sortOrder;
            ViewData["PageViewsSort"] = String.IsNullOrEmpty(sortOrder) ? "page_desc" : "";
            ViewData["CreateTimeSort"] = sortOrder == "Time" ? "time_desc" : "Time";

            

            //如果分页期间，改变搜索字符串，页将重置为1
            if(searchString!=null)
            {
                page = 1;
            }
            else
            {
                //分页的时候，保存搜索的关键字，
                searchString = currentFilter;
            }

            //当前搜索关键字
            ViewData["CurrentFilter"] = searchString;

            var mb_blog = from b in _context.mb_blog
                          select b;
            //当前用户的所有博客哦~不是所有的博客
            mb_blog = mb_blog.Where(b => b.Create_id == User.Identity.Name);

            //关键字搜索
            if(!String.IsNullOrEmpty(searchString))
            {
                mb_blog = mb_blog.Where(b => b.Blog_title.Contains(searchString)
                || b.Content.Contains(searchString));
            }
            //排序方式
            switch(sortOrder)
            {
                case "page_desc":
                    mb_blog = mb_blog.OrderByDescending(b => b.PageViews);
                    break;
                case "Time":
                    mb_blog = mb_blog.OrderBy(b => b.Create_time);
                    break;
                case "time_desc":
                    mb_blog = mb_blog.OrderByDescending(b => b.Create_time);
                    break;
                default:
                    mb_blog = mb_blog.OrderBy(b => b.PageViews);
                    break;

            }

            //用ViewData 传递，当前用户的粉丝数量和博客数量,以及头像路径
            var mb_user =await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == User.Identity.Name);
            ViewData["blog_num"] = mb_user.Blog_num;
            ViewData["fans_num"] = mb_user.Fans_num;
            ViewData["avatar"] = mb_user.Avatar;



            //将当前用户的所有粉丝列出来

           
            var realation = from b in _context.mb_relationship  select b;
            realation = realation.Where(m => m.UserB_id == User.Identity.Name);

            List<mb_relationship> listrelation = await realation.AsNoTracking().ToListAsync();

            List<mb_user> listfans = new List<mb_user>();
            
            foreach (mb_relationship item in listrelation)
            {
                listfans.Add(await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == item.UserA_id));

            }
            ViewBag.DV = listfans;

            int pageSize = 3;
            //将博客查询转换为支持分页的集合类型中的博客单页
            return View(await PaginatedList<mb_blog>.CreateAsync(mb_blog.AsNoTracking()
                , page ?? 1, pageSize));
        }

        // GET: mb_blog/Details/5
        public async Task<IActionResult> Details(int? id, string createid)
        {
            if (id == null)
            {
                return NotFound();
            }
            BlogDetails blogDetails = new BlogDetails();
            //获取博客信息
            var mb_blog = await _context.mb_blog
                .SingleOrDefaultAsync(m => m.Blog_id == id);
            //获取用户信息
            var mb_user = await _context.mb_user.
                SingleOrDefaultAsync(m => m.User_id == createid);
            //将博客信息，和用户信息都传入blogDetails
            blogDetails.blog = mb_blog;
            blogDetails.user = mb_user;

            //搜索用户id=creatid的博客
            var blog = from b in _context.mb_blog select b;
            blog = blog.Where(b => b.Create_id == createid);

            blogDetails.listblog = await blog.AsNoTracking().ToListAsync();


            if (mb_blog == null)
            {
                return NotFound();
            }

            //每次有用户访问了Details,都意味着这篇博客的访问数量+1
            mb_blog.PageViews++;
            try { 
            _context.Update(mb_blog);
            await _context.SaveChangesAsync();
            }
            catch { }

            //当当前用户和博客创建者已经是订阅关系了
            var mb_relation = _context.mb_relationship.SingleOrDefaultAsync(m => m.UserA_id == User.Identity.Name && m.UserB_id == mb_blog.Create_id);
            if(mb_relation.Result!=null)
            {
                ViewData["IsRelation"] = "true";
            }

            //获取所有该博客的评论
            var comment = from b in _context.mb_comment select b;
            comment = comment.Where(m => m.blog_id == id&&m.comment_id==0);
            
            List<mb_comment> mb_comment = await comment.AsNoTracking().ToListAsync();
            List<CommentDetails> allcomment = new List<CommentDetails>();

            foreach(var item in mb_comment)
            {
                mb_user user = await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == item.User_publish);
                allcomment.Add(new CommentDetails(item,user));
            }
            foreach(var item in allcomment)
            {
                await dfs(item);
            }
            
            ViewBag.CO = allcomment;
           
            return View(blogDetails);
        }
        //利用深度优先搜索，将数据库中相应的搜索查询出来并封装
        public async Task dfs( CommentDetails comment)
        {
            //查询所有当前评论的子评论
            var comment2 = from b in _context.mb_comment select b;
            comment2 = comment2.Where(m => m.comment_id == comment.comment.ID);
            List<mb_comment> mb_comment = await comment2.AsNoTracking().ToListAsync();

            foreach(var item in mb_comment)
            {
                mb_user user =await _context.mb_user.SingleOrDefaultAsync(b => b.User_id == item.User_publish);
                CommentDetails m = new CommentDetails(item,user);
                comment.listnum++;
                comment.listson.Add(m);
                await dfs(comment.listson[comment.listnum-1]);
            }


        }

        // GET: mb_blog/Create
        //必须当前用户角色为User才可以访问  
        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: mb_blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //必须当前用户角色为User才可以访问  
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create([Bind("Blog_id,Blog_title,Create_time,Create_id,Blog_tag,Classify,Content,PageViews")] mb_blog mb_blog)
        {
            if (ModelState.IsValid)
            {
                //博客创建者
                mb_blog.Create_id = User.Identity.Name;
                //博客创建时间
                mb_blog.Create_time = DateTime.Now;
                mb_blog.PageViews = 0;
                _context.Add(mb_blog);

                //每创建一个博客，博客数加1
                var mb_user = await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == mb_blog.Create_id);
                mb_user.Blog_num++;
                try
                {
                    _context.Update(mb_user);
                    await _context.SaveChangesAsync();
                }
                catch { }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mb_blog);
        }

        // GET: mb_blog/Edit/5
        //必须当前用户角色为User才可以访问  
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_blog = await _context.mb_blog.SingleOrDefaultAsync(m => m.Blog_id == id);
            if (mb_blog == null)
            {
                return NotFound();
            }
            return View(mb_blog);
        }

        // POST: mb_blog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //必须当前用户角色为User才可以访问  
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int id, [Bind("Blog_id,Blog_title,Create_time,Create_id,Blog_tag,Classify,Content,PageViews")] mb_blog mb_blog)
        {
            if (id != mb_blog.Blog_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mb_blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mb_blogExists(mb_blog.Blog_id))
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
            return View(mb_blog);
        }

        // GET: mb_blog/Delete/5
        //必须当前用户角色为User才可以访问  
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_blog = await _context.mb_blog
                .SingleOrDefaultAsync(m => m.Blog_id == id);
            if (mb_blog == null)
            {
                return NotFound();
            }

            return View(mb_blog);
        }

        // POST: mb_blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //必须当前用户角色为User才可以访问  
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mb_blog = await _context.mb_blog.SingleOrDefaultAsync(m => m.Blog_id == id);
            _context.mb_blog.Remove(mb_blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool mb_blogExists(int id)
        {
            return _context.mb_blog.Any(e => e.Blog_id == id);
        }

        //新建一个来，封装blog和user,
        public class BlogDetails
        {
            public mb_blog blog { get; set; }

            public mb_user user { get; set; }
            //详情页的文章推荐
            public List<mb_blog> listblog { get; set; }

        }

        //新建一个类，用来封装评论
        public class CommentDetails
        {
            //评论者
            public mb_user user;
            //评论内容
            public mb_comment comment { get; set; }
            //子评论的个数
            public int listnum { get; set; }
            //子评论的内容
            public List<CommentDetails> listson { get; set; }

            public CommentDetails(mb_comment comment,mb_user user)
            {
                this.user = user;
                this.listson = new List<CommentDetails>();
                this.comment = comment;
            }
            
        }

    }
}
