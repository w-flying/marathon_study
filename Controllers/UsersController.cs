using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Marathon.Data;
using marathon.Models;
using Marathon.Views;
using System.Linq.Expressions;
using Marathon.Models;
using Microsoft.Extensions.Hosting;

namespace Marathon.Controllers
{
    public class UsersController : Controller
    {
        private readonly MarathonContext _context;
    
        public UsersController(MarathonContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> GetImageAsync(string username)
        {
            var user = await _context.User
    .FirstOrDefaultAsync(m => m.UserName == username);

            if (user == null) return null;
            if (user.Image == null) return null;
            return new FileContentResult(user.Image, "image/jpeg");
        }
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        public async    Task<List<Check>> Search(string id)
        {
          var rows=from c in _context.Set<Check>()  where c.UserName == id  select c;
            var rows_lst=rows.ToList<Check>();
            var marathon= await _context.Marathonentity.ToListAsync();
            for (int i = 0; i < rows_lst.Count; i++)
            {
                rows_lst[i].Marathonentity = marathon.FirstOrDefault(n => n.MarathonId == rows_lst[i].MarathonId);
            }
            return rows_lst;
        }
        public  List<Post> Search_post(string id,List<Post> rows)
        {
            List<Post> posts = new List<Post>();
            foreach(var item in rows)
            {
                if(item.User_id==id) posts.Add(item);
            }
   
            return posts;
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserName == id);
            var posts= await _context.Post.ToListAsync();
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.checks=Search(id);
            ViewBag.posts=Search_post(id,posts);
			ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>());
            HttpContext.Session.SetString("User", id);
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>());
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,PassWord,Name,Gender,Age,PhoneNumber,Name")] User user, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    byte[] avatarData = new byte[image.Length];
                    await image.CopyToAsync(new MemoryStream(avatarData));
                    user.Image = avatarData;
                }
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>());
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>());
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,PassWord,Name,Gender,Age,PhoneNumber")] User user, IFormFile image)
        {


            if (id != user.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    byte[] avatarData = new byte[image.Length];
                    await image.CopyToAsync(new MemoryStream(avatarData));
                    user.Image = avatarData;
                }
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>());
                    return RedirectToAction(nameof(Index));
            }
			return View(user);
        }
        //用户编辑
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(string id, [Bind("UserName,PassWord,Name,Gender,Age,PhoneNumber")] User user, IFormFile image)
        {


            if (id != user.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    byte[] avatarData = new byte[image.Length];
                    await image.CopyToAsync(new MemoryStream(avatarData));
                    user.Image = avatarData;
                }
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>());
            }
            return RedirectToAction("Details", "Users",new{id=id });
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.UserName == id);
        }
    }
}
