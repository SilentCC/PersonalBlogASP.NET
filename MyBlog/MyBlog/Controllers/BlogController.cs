using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;


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

            int pageSize = 3;
            //将博客查询转换为支持分页的集合类型中的博客单页
            return View(await PaginatedList<mb_blog>.CreateAsync(mb_blog.AsNoTracking()
                , page ?? 1, pageSize));
        }

        // GET: mb_blog/Details/5
        public async Task<IActionResult> Details(int? id,string createid)
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

            blogDetails.listblog =await blog.AsNoTracking().ToListAsync();
           

            if (mb_blog == null)
            {
                return NotFound();
            }

            return View(blogDetails);
        }

        // GET: mb_blog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: mb_blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Blog_id,Blog_title,Create_time,Create_id,Blog_tag,Classify,Content,PageViews")] mb_blog mb_blog)
        {
            if (ModelState.IsValid)
            {
                mb_blog.Create_id = User.Identity.Name;
                mb_blog.Create_time = DateTime.Now;
                mb_blog.PageViews = 0;
                _context.Add(mb_blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mb_blog);
        }

        // GET: mb_blog/Edit/5
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

    }
}
