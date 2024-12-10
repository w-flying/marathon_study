using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Marathon.Data;
using marathon.Models;
using NuGet.Protocol;
using System.Collections;
using System.Text.Json;

namespace Marathon.Controllers
{
    public class ChecksController : Controller
    {
        private readonly MarathonContext _context;

        public ChecksController(MarathonContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> UserIndex()
        {
            var marathonContext = _context.Marathonentity;
            return View(await marathonContext.ToListAsync());
        }

        public IActionResult CreateUser()
        {
            ViewBag.Health = new SelectList(Enum.GetValues(typeof(Health)).Cast<Health>());
            ViewBag.MarathonClass = new SelectList(Enum.GetValues(typeof(MarathonClass)).Cast<MarathonClass>());
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName");
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser([Bind("Id,UserName,MarathonId,Health,MarathonClass,Experience,CheckState")] Check check)
        {
            if (check.CheckState == null) check.CheckState = "未审核";
            if (ModelState.IsValid)
            {
                if (await NameandmarathonAsync(check.UserName, check.MarathonId))
                {
                    return Content("<script>alert('不允许重复报名');parent.location.href='/Checks/Index'</script>", "text/html", System.Text.Encoding.GetEncoding("GB2312"));
                }
                _context.Add(check);
                await _context.SaveChangesAsync();
                return Content("<script>alert('报名成功');parent.location.href='/Checks/UserIndex'</script>", "text/html", System.Text.Encoding.GetEncoding("GB2312"));
            }
            ViewBag.Health = new SelectList(Enum.GetValues(typeof(Health)).Cast<Health>());
            ViewBag.MarathonClass = new SelectList(Enum.GetValues(typeof(MarathonClass)).Cast<MarathonClass>());
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName", check.MarathonId);
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName", check.UserName);
            return View(check);
        }


        // GET: Checks
        public async Task<IActionResult> Index()
        {
            var marathonContext = _context.Check.Include(c => c.Marathonentity).Include(c => c.User);
            return View(await marathonContext.ToListAsync());
        }

        // GET: Checks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Check
                .Include(c => c.Marathonentity)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (check == null)
            {
                return NotFound();
            }

            return View(check);
        }

        // GET: Checks/Create
        public IActionResult Create()
        {
            ViewBag.Health = new SelectList(Enum.GetValues(typeof(Health)).Cast<Health>());
            ViewBag.MarathonClass = new SelectList(Enum.GetValues(typeof(MarathonClass)).Cast<MarathonClass>());
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName");
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName");
            return View();
        }

        private async Task<bool> NameandmarathonAsync(string username, int marethonid)
        {
            var usercheck = await _context.Check.FirstOrDefaultAsync(ss => ss.UserName == username && ss.MarathonId == marethonid);
            return usercheck != null;
        }//不允许重复报名

        // POST: Checks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,MarathonId,Health,MarathonClass,Experience,CheckState")] Check check)
        {
            if (check.CheckState == null) check.CheckState = "未审核";
            if (ModelState.IsValid)
            {
                if (await NameandmarathonAsync(check.UserName, check.MarathonId))
                {
                    return Content("<script>alert('不允许重复报名');parent.location.href='/Checks/Index'</script>", "text/html", System.Text.Encoding.GetEncoding("GB2312"));
                }
                _context.Add(check);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Health = new SelectList(Enum.GetValues(typeof(Health)).Cast<Health>());
            ViewBag.MarathonClass = new SelectList(Enum.GetValues(typeof(MarathonClass)).Cast<MarathonClass>());
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName", check.MarathonId);
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName", check.UserName);
            return View(check);
        }

        // GET: Checks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Check.FindAsync(id);
            if (check == null)
            {
                return NotFound();
            }
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName", check.MarathonId);
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName", check.UserName);
            return View(check);
        }



        // POST: Checks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,MarathonId,Health,MarathonClass,Experience,CheckState")] Check check)
        {
            if (id != check.Id)
            {
                return NotFound();
            }
            var temp = check.CheckState;
            check = await _context.Check.Include(c => c.Marathonentity).Include(c => c.User).FirstOrDefaultAsync(m => m.Id == id);
            check.CheckState = temp;

            if (check != null)
            {
                try
                {
                    _context.Update(check);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckExists(check.Id))
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
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName", check.MarathonId);
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName", check.UserName);
            return View(check);
        }

        // GET: Checks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Check
                .Include(c => c.Marathonentity)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (check == null)
            {
                return NotFound();
            }

            return View(check);
        }

        // POST: Checks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var check = await _context.Check.FindAsync(id);
            if (check != null)
            {
                _context.Check.Remove(check);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckExists(int id)
        {
            return _context.Check.Any(e => e.Id == id);
        }

        public MarathonContext Get_context()
        {
            return _context;
        }

        [HttpPost]
        public async Task<IActionResult> DbContextTransaction([FromBody] List<string[]> checks)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    foreach (var checkData in checks)
                    {
                        var marathonName = checkData[2];
                        var userName = checkData[1];  

                        // 查找现有的Check对象  
                        var row = await _context.Check
                            .Include(c => c.Marathonentity)
                            .Include(c => c.User)
                            .FirstOrDefaultAsync(ss => ss.Marathonentity.MarathonName == marathonName && ss.User.Name == userName);
                            row.CheckState = checkData[6];
                        _context.Update(row);
                    }

                    // 只在所有操作完成后保存更改  
                    await _context.SaveChangesAsync();

                    // 提交事务  
                    transaction.Commit();
                    return RedirectToAction(nameof(Index));

                }
            }
            catch (Exception ex)
            {
                // 捕获异常并回滚事务  
                Console.WriteLine("失败了: " + ex.Message);
                return RedirectToAction("Error", new { message = "处理请求时出错" }); // 或者返回适当的错误响应  
            }

        }
    }
}
