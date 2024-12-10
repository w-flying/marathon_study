using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Marathon.Data;
using marathon.Models;

namespace Marathon.Controllers
{
    public class EnrollsController : Controller
    {
        private readonly MarathonContext _context;

        public EnrollsController(MarathonContext context)
        {
            _context = context;
        }

        // GET: Enrolls
        public async Task<IActionResult> Index()
        {
            var marathonContext = _context.Enroll.Include(e => e.Marathonentity).Include(e => e.User);
            return View(await marathonContext.ToListAsync());
        }

        // GET: Enrolls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enroll = await _context.Enroll
                .Include(e => e.Marathonentity)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enroll == null)
            {
                return NotFound();
            }

            return View(enroll);
        }

        // GET: Enrolls/Create
        public IActionResult Create()
        {
            ViewBag.Health = new SelectList(Enum.GetValues(typeof(Health)).Cast<Health>());
            ViewBag.MarathonClass = new SelectList(Enum.GetValues(typeof(MarathonClass)).Cast<MarathonClass>());
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName");
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName");
            return View();
        }

        // POST: Enrolls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,MarathonId,Health,MarathonClass,Experience")] Enroll enroll)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enroll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Health = new SelectList(Enum.GetValues(typeof(Health)).Cast<Health>());
            ViewBag.MarathonClass = new SelectList(Enum.GetValues(typeof(MarathonClass)).Cast<MarathonClass>());
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName", enroll.MarathonId);
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName", enroll.UserName);
            return View(enroll);
        }

        // GET: Enrolls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enroll = await _context.Enroll.FindAsync(id);
            if (enroll == null)
            {
                return NotFound();
            }
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName", enroll.MarathonId);
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName", enroll.UserName);
            return View(enroll);
        }

        // POST: Enrolls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,MarathonId,Health,MarathonClass,Experience")] Enroll enroll)
        {
            if (id != enroll.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enroll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollExists(enroll.Id))
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
            ViewData["MarathonId"] = new SelectList(_context.Marathonentity, "MarathonId", "MarathonName", enroll.MarathonId);
            ViewData["UserName"] = new SelectList(_context.User, "UserName", "UserName", enroll.UserName);
            return View(enroll);
        }

        // GET: Enrolls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enroll = await _context.Enroll
                .Include(e => e.Marathonentity)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enroll == null)
            {
                return NotFound();
            }

            return View(enroll);
        }

        // POST: Enrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enroll = await _context.Enroll.FindAsync(id);
            if (enroll != null)
            {
                _context.Enroll.Remove(enroll);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollExists(int id)
        {
            return _context.Enroll.Any(e => e.Id == id);
        }
    }
}
