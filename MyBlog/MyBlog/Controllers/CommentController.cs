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
    public class CommentController : Controller
    {
        private readonly MyBlogContext _context;

        public CommentController(MyBlogContext context)
        {
            _context = context;
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            return View(await _context.mb_comment.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_comment = await _context.mb_comment
                .SingleOrDefaultAsync(m => m.ID == id);
            if (mb_comment == null)
            {
                return NotFound();
            }

            return View(mb_comment);
        }

        // GET: Comment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //添加评论
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,User_publish,User_accept,Create_time,content,blog_id,comment_id")] mb_comment mb_comment,string bid,string createid)
        {
            if (ModelState.IsValid)
            {
                mb_comment.User_publish = User.Identity.Name;
                mb_comment.Create_time = DateTime.Now;

                _context.Add(mb_comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Blog",new { id =bid,createid =createid} );
            }
            return View(mb_comment);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_comment = await _context.mb_comment.SingleOrDefaultAsync(m => m.ID == id);
            if (mb_comment == null)
            {
                return NotFound();
            }
            return View(mb_comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,User_publish,User_accept,Create_time,content,blog_id,comment_id")] mb_comment mb_comment)
        {
            if (id != mb_comment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mb_comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!mb_commentExists(mb_comment.ID))
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
            return View(mb_comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mb_comment = await _context.mb_comment
                .SingleOrDefaultAsync(m => m.ID == id);
            if (mb_comment == null)
            {
                return NotFound();
            }

            return View(mb_comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mb_comment = await _context.mb_comment.SingleOrDefaultAsync(m => m.ID == id);
            _context.mb_comment.Remove(mb_comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool mb_commentExists(int id)
        {
            return _context.mb_comment.Any(e => e.ID == id);
        }
    }
}
