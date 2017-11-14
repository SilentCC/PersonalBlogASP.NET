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
    public class RelationController : Controller
    {
        private readonly MyBlogContext _context;

        public RelationController(MyBlogContext context)
        {
            _context = context;
        }

        // GET: Relation
        public async Task<IActionResult> Index()
        {
            return View(await _context.mb_relationship.ToListAsync());
        }

        // GET: Relation/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationship = await _context.mb_relationship
                .SingleOrDefaultAsync(m => m.ID == id);
            if (relationship == null)
            {
                return NotFound();
            }

            return View(relationship);
        }

        // GET: Relation/Create
        public IActionResult Create()
        {
            return View();
        }
        //订阅事件
        public async Task<IActionResult> Subscribe([Bind("ID,UserA_id,UserB_id")] mb_relationship relationship,string userid,string blogid)
        {
           
             
                _context.Add(relationship);
                await _context.SaveChangesAsync();

                 //被订阅者粉丝数量加1
                var  mb_user = await _context.mb_user.SingleOrDefaultAsync(m => m.User_id == userid);
                mb_user.Fans_num++;

                try
                {
                     _context.Update(relationship);
                    await _context.SaveChangesAsync();
                 }
                catch { }


                return RedirectToAction("Details","blog",new {id = blogid, createid =userid});
            

        }

        // POST: Relation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserA_id,UserB_id")] mb_relationship relationship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relationship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Relation/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationship = await _context.mb_relationship.SingleOrDefaultAsync(m => m.ID == id);
            if (relationship == null)
            {
                return NotFound();
            }
            return View(relationship);
        }

        // POST: Relation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,UserA_id,UserB_id")] mb_relationship relationship)
        {
            if (id != relationship.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelationshipExists(relationship.ID))
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
            return View(relationship);
        }

        // GET: Relation/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relationship = await _context.mb_relationship
                .SingleOrDefaultAsync(m => m.ID == id);
            if (relationship == null)
            {
                return NotFound();
            }

            return View(relationship);
        }

        // POST: Relation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var relationship = await _context.mb_relationship.SingleOrDefaultAsync(m => m.ID == id);
            _context.mb_relationship.Remove(relationship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RelationshipExists(string id)
        {
            return _context.mb_relationship.Any(e => e.ID == id);
        }
    }
}
